using Solvberget.Domain.Documents;
using Solvberget.Domain.Lists;
using Solvberget.Domain.Users;

namespace Solvberget.Domain.Favorites
{
    public class Favorite
    {
        public UserInfo User { get; set; }
        public Document Document { get; set; }
    }
}