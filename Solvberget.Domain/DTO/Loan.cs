using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Solvberget.Domain.DTO
{
    public class Loan
    {

        public string DocumentNumber { get; set; }
        public string LoanTitle { get; set;  }
        public string SubLibrary { get; set; }
        public string LoanCatalogerName { get; set; }
        public string OriginalDueDate { get; set; }
        public string ItemStatus { get; set; }
        public string ReturnedHour { get; set; }
        public string ReturnedDate { get; set; }
        public string LoanDate { get; set; }
        public string LoanHour { get; set; }
        public string Material { get; set; }
        public string Status { get; set; }
        public string DueDate { get; set; }
        public string DueHour { get; set; }

    }

}
