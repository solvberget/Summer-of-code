using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solvberget.Domain.DTO
{
    
    public class Organization
    {
        
        public string Name { get; set; }
        public string UnderOrganization { get; set; }
        public string Role { get; set; }
        public string FurtherExplanation { get; set; }
        public string ReferencedPublication { get; set; }

    }

}
