using System;
using System.Linq;
using Solvberget.Core.DTOs;
using Solvberget.Domain.Aleph;
using Solvberget.Domain.Documents;
using Solvberget.Domain.Lists;

namespace Solvberget.Nancy.Mapping
{
    public static class DtoMaps
    {
        public static LibrarylistDto Map(LibraryList list, IRepository documents = null)
        {
            if (null != documents)
            {
                return new LibrarylistDto
                {
                    Id = list.Id,
                    Name = list.Name,
                    Documents = list.Documents.Count > 0
                        ? list.Documents.Select(Map).ToList()
                        : list.DocumentNumbers.Keys.Select(dn => Map(documents.GetDocument(dn, true))).ToList()
                };
            }

            return new LibrarylistDto
            {
                Id = list.Id,
                Name = list.Name
            };
        }
        
        public static DocumentDto Map(Document document)
        {
            DocumentDto dto;

            if (document is Book)
            {
                var book = (Book)document;
                var bookDto = new BookDto();
                dto = bookDto;
                bookDto.Classification = book.ClassificationNr;
            }
            else
            {
                dto = new DocumentDto(); // todo other types
            }

            dto.Id = document.DocumentNumber;
            dto.Type = document.DocType;
            dto.Title = document.Title;
            dto.SubTitle = document.CompressedSubTitle;
            dto.Availability = MapAvailability(document);

            return dto;
        }

        private static DocumentAvailabilityDto MapAvailability(Document document)
        {
            if (null == document.AvailabilityInfo) return null;

            var availability = document.AvailabilityInfo.FirstOrDefault();

            if (null == availability) return null;

            return new DocumentAvailabilityDto
            {
                Branch = availability.Branch,
                AvailableCount = availability.AvailableCount,
                TotalCount = availability.TotalCount,

                Department = availability.Department.Aggregate((acc, dep) =>
                {
                    if (String.IsNullOrEmpty(acc)) return dep;
                    return acc + " - " + dep;
                }),

                Collection = availability.PlacementCode,
                Location = document.LocationCode,
                EstimatedAvailableDate = availability.EarliestAvailableDateFormatted
            };
        }
    }
}
