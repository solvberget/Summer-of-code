using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Implementation
{
    public class AvailabilityRepository
    {
        public static AvailabilityInformation GenerateInfoFor(Document doc, string branch, IEnumerable<DocumentItem> docItems)
        {
            var availabilityInformation = FillProperties(doc, branch, docItems);
            return availabilityInformation.Branch != null ? availabilityInformation : null;
        }

        public static List<AvailabilityInformation> GenerateLocationAndAvailabilityInfo(IEnumerable<DocumentItem> docItems, Document doc)
        {
            var items = docItems.ToList();
            if (!items.Any()) return null;
            var availabilityinfo = new List<AvailabilityInformation>();

            foreach (var availabilityInfo in AvailabilityInformation.BranchesToHandle.Select(branch => AvailabilityRepository.GenerateInfoFor(doc, branch, items)).Where(availabilityInfo => availabilityInfo != null))
            {
                availabilityinfo.Add(availabilityInfo);
            }
            return availabilityinfo;
        }

        private static AvailabilityInformation FillProperties(Document doc, string branch, IEnumerable<DocumentItem> docItems)
        {
            var availability = new AvailabilityInformation();
            var items = docItems.Select(x => x).Where(x => x.Branch.Equals(branch) && x.IsReservable).ToList();

            if (items.Any())
            {
                availability.Branch = branch;
                availability.Department = items.Where(x => x.Department != null).Select(x => x.Department).Distinct();
                availability.PlacementCode = items.FirstOrDefault().PlacementCode;

                availability.TotalCount = items.Count();
                availability.AvailableCount = items.Count(x => x.LoanStatus == null && !x.OnHold);

                if (availability.AvailableCount == 0)
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
                                availability.EarliestAvailableDateFormatted = DateTime.Now.AddDays(1).ToShortDateString();
                            }
                            else
                            {
                                availability.EarliestAvailableDateFormatted = earliestDueDate.ToShortDateString();
                            }
                        }
                        else
                        {
                            var totalNumberOfReservations = items.Sum(x => x.NoRequests);
                            var calculation1 = (totalNumberOfReservations * (doc.StandardLoanTime + AvailabilityInformation.AveragePickupTimeInDays));
                            // Below for added days: if it is required to round up the result of dividing m by n 
                            // (where m and n are integers), one should compute (m+n-1)/n
                            // Source: Number Conversion, Roland Backhouse, 2001
                            var calculation2 = (calculation1 + availability.TotalCount - 1) / availability.TotalCount;

                            if (availability.TotalCount == 1)
                                availability.EarliestAvailableDateFormatted = earliestDueDate.AddDays(calculation2).ToShortDateString();
                            else
                                availability.EarliestAvailableDateFormatted = earliestDueDate.AddDays(calculation2 + doc.StandardLoanTime).ToShortDateString();

                        }
                    }
                    else
                    {
                        availability.EarliestAvailableDateFormatted = "Ukjent";
                        ;
                    }
                }
                else
                {
                    availability.EarliestAvailableDateFormatted = "";
                }
            }
            return availability;
        }
    }
}
