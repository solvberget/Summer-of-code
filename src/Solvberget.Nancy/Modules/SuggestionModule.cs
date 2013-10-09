using Nancy;

using Solvberget.Domain.Abstract;

namespace Solvberget.Nancy.Modules
{
    public class SuggestionModule : NancyModule
    {
        public SuggestionModule(ISuggestionDictionary suggestionDictionary) : base("/suggestions")
        {
            Get["/lookup"] = _ => suggestionDictionary.Lookup(Request.Query.q);

            Get["/"] = _ => suggestionDictionary.SuggestionList();
        }
    }
}