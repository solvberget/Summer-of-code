namespace Solvberget.Domain.Search
{
    public interface ISuggestionDictionary
    {
        string Lookup(string value);
        string[] SuggestionList();
    }
}