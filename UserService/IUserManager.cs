using System.Collections.Generic;

namespace UserService
{
    public interface IUserManager
    {
        List<UserValidationFailureType> CreateUser( string firstName, string lastName, string email, string password );
        User GetUser( int id );
        User GetUser( string email );
        List<User> GetUsers();
    }
}