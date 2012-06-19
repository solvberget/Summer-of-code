using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;

//using Microsoft.Ajax.Utilities;


namespace Solvberget.JsCombine
{
    class Program
    {
        private static string _projectDir;
        private static string _jsFolder;
        private static string _scriptsFolder;
        private static string _pluginsFolder;
        private static string _combinedJsFile;
        private static string _cssFolder;
        private static string _combinedPluginsFile;


        /// <summary>
        /// Usage for running once then watching for file changes: JsCombine.exe  [project dir]
        /// Usage for running just once: JsCombine.exe [project dir] /once
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Pass project directory as argument");
                Environment.ExitCode = 1;
                return;
            }


            _projectDir = args[0];


            bool watch = true;


            if (args.Length > 1)
            {
                watch = args[1] != "/once";
            }


            Console.WriteLine("_projectDir: " + _projectDir);


            _jsFolder = Path.Combine(_projectDir, "js");


            Console.WriteLine("_jsFolder: " + _jsFolder);


            _cssFolder = Path.Combine(_projectDir, "css");


            Console.WriteLine("_cssFolder: " + _cssFolder);


            _scriptsFolder = Path.Combine(_jsFolder, "scripts");


            _pluginsFolder = Path.Combine(_jsFolder, "plugins");


            _combinedJsFile = Path.Combine(_jsFolder, "script.js");
            _combinedPluginsFile = Path.Combine(_jsFolder, "plugins.js");


            ThreadPool.QueueUserWorkItem(BeginWatching);


            Console.WriteLine(DateTime.Now.ToShortTimeString() + ": Started JsCombine, triggering initial update of combined js files and running tests.");


            lock (SyncLock)
            {
                Combine();
                Minify();
                RunTests();
            }


            Console.WriteLine(DateTime.Now.ToShortTimeString() + ": Watching for file changes...");


            while (watch)
            {
                Ev.WaitOne(int.MaxValue);
            }
        }


        private static void Minify()
        {
            MinifyCss();
            //MinifyJs();
        }


        //private static void MinifyJs()
        //{
        //    var scriptsBody = File.ReadAllText(_combinedJsFile);
        //    var pluginsBody = File.ReadAllText(_combinedPluginsFile);


        //    var minifier = new Minifier();
        //    var codeSettings = new CodeSettings
        //    {
        //        CollapseToLiteral = false
        //    };


        //    var minifiedScriptPath = Path.Combine(_jsFolder, "script.min.js");
        //    var minifiedPluginsPath = Path.Combine(_jsFolder, "plugins.min.js");


        //    File.WriteAllText(minifiedScriptPath, minifier.MinifyJavaScript(scriptsBody, codeSettings));
        //    File.WriteAllText(minifiedPluginsPath, minifier.MinifyJavaScript(pluginsBody, codeSettings));
        //}


        private static void MinifyCss()
        {
            var files = Directory.GetFiles(_cssFolder);
            foreach (var file in files)
            {
                if (file.EndsWith(".min.css"))
                {
                    continue;
                }


                var cssBody = File.ReadAllText(file);


                cssBody = Regex.Replace(cssBody, @"[a-zA-Z]+#", "#");
                cssBody = Regex.Replace(cssBody, @"[\n\r]+\s*", string.Empty);
                cssBody = Regex.Replace(cssBody, @"\s+", " ");
                cssBody = Regex.Replace(cssBody, @"\s?([:,;{}])\s?", "$1");
                cssBody = cssBody.Replace(";}", "}");
                cssBody = Regex.Replace(cssBody, @"([\s:]0)(px|pt|%|em)", "$1");


                // Remove comments from CSS
                cssBody = Regex.Replace(cssBody, @"/\*[\d\D]*?\*/", string.Empty);


                var fileNameWithoutExt = Path.GetFileNameWithoutExtension(file);
                var minifiedFilePath = Path.Combine(_cssFolder, fileNameWithoutExt + ".min.css");


                File.WriteAllText(minifiedFilePath, cssBody);
            }
        }


        private static readonly AutoResetEvent Ev = new AutoResetEvent(false);


        private static void BeginWatching(object state)
        {
            var watcher = new FileSystemWatcher(_jsFolder)
            {
                IncludeSubdirectories = true,
                Filter = "*.js"
            };


            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Deleted += OnChanged;
            watcher.Renamed += OnChanged;


            watcher.EnableRaisingEvents = true;
        }


        private static readonly object SyncLock = new object();




        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            var path = Path.GetDirectoryName(e.FullPath);


            if ((path == _scriptsFolder || path == _pluginsFolder) && !e.Name.EndsWith("___jb_old___"))
            {
                Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + e.Name + " triggered update of combined js files and test run.");


                lock (SyncLock)
                {
                    Combine();
                    RunTests();
                }
            }
            else if (path != null && (path.Contains("test") && !e.Name.EndsWith("___jb_old___")))
            {
                Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + e.Name + " triggered test run.");


                lock (SyncLock)
                {
                    RunTests();
                }
            }


            Ev.Set();
        }


        private static void RunTests()
        {
            //var startInfo = new ProcessStartInfo("C:\\Program Files (x86)\\nodejs\\node.exe");
            //startInfo.Arguments = "\"C:\\Program Files (x86)\\nodejs\\node_modules\\mocha\\bin\\mocha\" -r ./script -r ../lib/globals/should -R xunit > test-results.txt";


            var startInfo = new ProcessStartInfo(Path.Combine(_projectDir, "lib\\runmocha.bat")) { WorkingDirectory = _jsFolder, UseShellExecute = false };


            Console.WriteLine(DateTime.Now.ToShortTimeString() + ": Running tests...");
            Console.WriteLine("------------------------------------------------------------------------------");


            var p = Process.Start(startInfo);
            p.WaitForExit();


            try
            {
                PrintTestResults();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to parse test-results.xml: " + ex.Message);
            }
        }


        private static void PrintTestResults()
        {
            var contents = File.ReadAllText(Path.Combine(_jsFolder, "test-results.txt"));


            var xmlStart = contents.IndexOf("<testsuite", StringComparison.Ordinal);


            if (xmlStart > 0)
            {
                var log = contents.Substring(0, xmlStart);


                Console.WriteLine("console.log() output:");
                Console.WriteLine(log);
            }


            var xml = xmlStart > 0 ? contents.Substring(xmlStart) : contents;


            var results = XDocument.Parse(xml);


            File.WriteAllText(Path.Combine(_jsFolder, "test-results.xml"), xml);


            foreach (var suite in results.Descendants("testsuite"))
            {
                var name = suite.Attribute("name").Value;
                var tests = suite.Attribute("tests").Value;
                var failures = suite.Attribute("failures").Value;
                var errors = suite.Attribute("errors").Value;
                var skip = suite.Attribute("skip").Value;
                var time = suite.Attribute("time").Value;


                Console.WriteLine("{0} | tests: {1} | failures: {2} | errors: {3} | skipped: {4} | time: {5}s", name, tests, failures, errors, skip, time);
                Console.WriteLine("===============================================================================================================");


                foreach (var testCase in suite.Descendants("testcase"))
                {
                    if (testCase.IsEmpty) continue;


                    var tClassname = testCase.Attribute("classname").Value;
                    var tName = testCase.Attribute("name").Value;
                    var tTime = testCase.Attribute("time").Value;


                    Console.WriteLine("FAILED: {0} | {1} | time: {2}s", tClassname, tName, tTime);


                    foreach (var failure in testCase.Descendants("failure"))
                    {
                        Console.WriteLine(failure.Value);
                    }


                    Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
                }
            }
        }


        private static void Combine()
        {
            Combine(_scriptsFolder, _combinedJsFile);
            Combine(_pluginsFolder, _combinedPluginsFile);
        }


        private static void Combine(string scriptsFolder, string combinedJsFile)
        {
            if (File.Exists(combinedJsFile)) File.Delete(combinedJsFile);


            File.WriteAllText(combinedJsFile, String.Empty);


            foreach (var file in Directory.GetFiles(scriptsFolder).OrderBy(s => s))
            {
                if (file.EndsWith(".js"))
                {
                    File.AppendAllText(combinedJsFile, Environment.NewLine + Environment.NewLine + "//// " + file + " //////////////////////////////////////" + Environment.NewLine + Environment.NewLine);
                    File.AppendAllText(combinedJsFile, File.ReadAllText(file));
                }
            }
        }
    }
}
