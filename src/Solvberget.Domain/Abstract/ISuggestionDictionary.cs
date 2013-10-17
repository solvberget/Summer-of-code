namespace Solvberget.Domain.Abstract
{
    public interface ISuggestionDictionary
    {
        string Lookup(string value);
        string[] SuggestionList();
    }
}