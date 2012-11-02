using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solvberget.Domain.DTO
{
    public class AvailabilityInformation
    {

        public static readonly IEnumerable<string> BranchesToHandle = new List<string> { "Hovedbibl.", "Madla" };
        public static readonly int AveragePickupTimeInDays = 4;

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
                    if (dueDates.Any())
                    {
                        var earliestDueDate = dueDates.OrderBy(x => x).FirstOrDefault();
                        
                        if (!items.Any(x => x.NoRequests > 0))
                        {
                            if (earliestDueDate.CompareTo(DateTime.Now) < 0)
                            {
                                // The due date has passed, but the document is not handed in yet. Set to next day.
                                EarliestAvailableDateFormatted = DateTime.Now.AddDays(1).ToShortDateString();
                            }
                            else
                            {
                                EarliestAvailableDateFormatted = earliestDueDate.ToShortDateString();
                            }

                        }
                        else
                        {
                            var totalNumberOfReservations = items.Sum(x => x.NoRequests);
                            var calculation1 = (totalNumberOfReservations * (doc.StandardLoanTime + AveragePickupTimeInDays));
                            // Below for added days: if it is required to round up the result of dividing m by n 
                            // (where m and n are integers), one should compute (m+n-1)/n
                            // Source: Number Conversion, Roland Backhouse, 2001
                            var calculation2 = ( calculation1 + TotalCount - 1 ) / TotalCount;
                            
                            if (TotalCount == 1)
                                EarliestAvailableDateFormatted = earliestDueDate.AddDays(calculation2).ToShortDateString();
                            else
                                EarliestAvailableDateFormatted = earliestDueDate.AddDays(calculation2 + doc.StandardLoanTime).ToShortDateString();
                        
                        }
                    }
                    else
                    {
                        EarliestAvailableDateFormatted = "Ukjent";
;                    }
                }
                else
                {
                    EarliestAvailableDateFormatted = "";
                }
            }

        }

    }
}
