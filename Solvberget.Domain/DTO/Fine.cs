using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class Fine
    {
        public string Date { get; set; }
        public string Status { get; set; }
        public char CreditDebit { get; set; }
        public double Sum { get; set; }
        public string Description { get; set; }
        public string DocumentTitle { get; set; }
        public string DocumentNumber { get; set; }

    }
}
