using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solvberget.Domain.DTO
{
    public class AvailabilityInformation
    {

        public static readonly IEnumerable<string> BranchesToHandle = new List<string> { "Hovedbibl.", "Madla" };

        public string Branch { get; private set; }
        public IEnumerable<string> Department { get; private set; }
        public string PlacementCode { get; private set; }
        public int TotalCount { get; private set; }
        public int AvailableCount { get; private set; }
        private DateTime? EarliestAvailableDate { get; set; }
        public string EarliestAvailableDateFormatted { get; private set; }
        public string RecKeyIfAvailable { get; private set; } //For later use with reservartion

        public static AvailabilityInformation GenerateInfoFor(Document doc, string branch, IEnumerable<DocumentItem> docItems)
        {
            var availabilityInformation = new AvailabilityInformation();
            availabilityInformation.FillProperties(doc, branch, docItems);
            return availabilityInformation.Branch != null ? availabilityInformation : null;
        }

        private void FillProperties(Document doc, string branch, IEnumerable<DocumentItem> docItems)
        {

            var items = docItems.Select(x => x).Where(x => x.Branch.Equals(branch) && x.IsReservable).ToList();

            if (items.Any())
            {
                Branch = branch;
                Department = items.Where(x => x.Department != null).Select(x => x.Department).Distinct();
                PlacementCode = items.FirstOrDefault().PlacementCode;

                TotalCount = items.Count();
                AvailableCount = items.Count(x => x.LoanStatus == null && !x.OnHold);

                if (AvailableCount == 0)
                {
                    var dueDates = items.Where(x => x.LoanDueDate != null).Select(x => x.LoanDueDate.Value);
                    var earliestDueDate = dueDates.OrderBy(x => x).FirstOrDefault();
                    EarliestAvailableDate = items.All(x => x.OnHold) ? earliestDueDate.AddDays(doc.StandardLoanTime) : earliestDueDate;
                    EarliestAvailableDateFormatted = earliestDueDate.ToShortDateString();
                }
                else
                {
                    EarliestAvailableDateFormatted = "";
                }
            }

        }

    }
}
