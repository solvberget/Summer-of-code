using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Solvberget.Domain.Utils
{
    public static class MarcUtils
    {
        public static string GetVarfield(IEnumerable<XElement> nodes, string id, string subfieldLabel)
        {
            var varfield =
                nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals(id)).Elements("subfield");
            return
                varfield.Where(x => ((string)x.Attribute("label")).Equals(subfieldLabel)).Select(x => x.Value).FirstOrDefault();
        }

        public static IEnumerable<string> GetVarfieldAsList(IEnumerable<XElement> nodes, string id,
                                                               string subfieldLabel)
        {
            var varfield =
                nodes.Elements("varfield").Where(x => ((string)x.Attribute("id")).Equals(id)).Elements("subfield");
            return varfield.Where(x => ((string)x.Attribute("label")).Equals(subfieldLabel)).Select(x => x.Value).ToList();
        }
    }
}
