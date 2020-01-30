using System;
using System.Collections.Generic;
using System.Linq;

namespace UserService
{
    public class UserManager : IUserManager
    {
        private List<User> _seedUsers = new List<User>{
                new User
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Smith",
                    Email = "john@john.com",
                    Password = "ASDF23$12*jkl"
                },
                new User
                {
                    Id = 2,
                    FirstName = "Johnny",
                    LastName = "Smith",
                    Email = "johnny@john.com",
                    Password = "johnnyPa$sw0rd"
                }
            };

        public List<User> GetUsers()
        {
            return _seedUsers;
        }

        public User GetUser( int id )
        {
            return _seedUsers?.FirstOrDefault( u => u.Id == id );
        }

        public User GetUser( string email )
        {
            return _seedUsers?.FirstOrDefault( u => u.Email.Equals( email, StringComparison.InvariantCultureIgnoreCase ) );
        }

        public List<UserValidationFailureType> CreateUser( string firstName, string lastName, string email, string password )
        {
            List<UserValidationFailureType> validationFailures = new List<UserValidationFailureType>();

            if ( string.IsNullOrWhiteSpace( firstName ) )
            {
                validationFailures.Add( UserValidationFailureType.FirstNameMissing );
            }

            if ( string.IsNullOrWhiteSpace( lastName ) )
            {
                validationFailures.Add( UserValidationFailureType.LastNameMissing );
            }

            if ( string.IsNullOrWhiteSpace( email ) )
            {
                validationFailures.Add( UserValidationFailureType.EmailMissing );
            }

            if ( string.IsNullOrWhiteSpace( password ) )
            {
                validationFailures.Add( UserValidationFailureType.PasswordMissing );
            }

            if ( !validationFailures.Any() )
            {
                _seedUsers.Add( new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Password = password
                } );
            }

            return validationFailures;
        }
    }
}
