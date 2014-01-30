using Nancy;
using Solvberget.Domain.Search;

namespace Solvberget.Nancy.Modules
{
    public class SuggestionModule : NancyModule
    {
        public SuggestionModule(ISuggestionDictionary suggestionDictionary) : base("/suggestions")
        {
            Get["/"] = _ => suggestionDictionary.SuggestionList();

            Get["/lookup"] = _ => suggestionDictionary.Lookup(Request.Query.q);
        }
    }
}