using Currency_Converter.Domain.Interfaces;

namespace Currency_Converter.Domain.Users.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByEmail(string email);
    }
}