using System;
using System.Collections.Generic;

namespace UserService
{
    public interface IUserManager
    {
        List<UserValidationFailureType> CreateUser( User user );
        List<UserValidationFailureType> DeleteUser( int id );
        Tuple<User, List<UserValidationFailureType>> GetUser( int id );
        User GetUser( string email );
        List<User> GetUsers();
        List<UserValidationFailureType> UpdateUser( User user );
    }
}