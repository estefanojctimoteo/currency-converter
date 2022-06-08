using System.Linq;
using Currency_Converter.Domain.Users;
using Currency_Converter.Domain.Users.Repository;
using Currency_Converter.Infra.Data.Context;

namespace Currency_Converter.Infra.Data.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ConversionContext context) : base(context)
        {
        }
        public User GetByEmail(string email)
        {
            var ret = Db.User.Where(e => e.Email == email);
            return ret.FirstOrDefault();
        }
    }
}