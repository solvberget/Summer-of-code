using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solvberget.Domain.DTO
{
    public class Notification
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Content{ get; set; }
        public string DocumentTitle { get; set; }
        public string DocumentNumber { get; set; }
       

    }
}
