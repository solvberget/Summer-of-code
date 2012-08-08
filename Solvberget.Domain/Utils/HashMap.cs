using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Solvberget.Domain.Utils
{
    public class HashMap : Dictionary<string, string>
    {

        public new void Add(string key, string value)
        {
            this.Add(value);
        }

        public void Add(string value)
        {
            if(!this.Contains(value))
            base.Add(value, value);
        }

        public bool Contains(string value)
        {
            return (this.ContainsKey(value));
        }
    
    }
}
