using Framework;
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

        public Tuple<User, List<UserValidationFailureType>> GetUser( int id )
        {
            List<UserValidationFailureType> validationFailures = new List<UserValidationFailureType>();

            if ( ValidationHelper.IsValidId( id ) )
            {
                validationFailures.Add( UserValidationFailureType.InvalidId );
                return Tuple.Create( new User(), validationFailures );
            }

            return Tuple.Create( _seedUsers?.FirstOrDefault( u => u.Id == id ), validationFailures );
        }

        public User GetUser( string email )
        {
            return _seedUsers?.FirstOrDefault( u => u.Email.Equals( email, StringComparison.InvariantCultureIgnoreCase ) );
        }

        public List<UserValidationFailureType> CreateUser( User user )
        {
            List<UserValidationFailureType> validationFailures = ValidateUser( user );

            if ( !validationFailures.Any() )
            {
                _seedUsers.Add( user );
            }

            return validationFailures;
        }

        public List<UserValidationFailureType> UpdateUser( User user )
        {
            List<UserValidationFailureType> validationFailures = new List<UserValidationFailureType>();

            if ( ValidationHelper.IsValidId( user.Id ) )
            {
                validationFailures.Add( UserValidationFailureType.InvalidId );
                return validationFailures;
            }

            validationFailures.AddRange( ValidateUser( user ) );

            if ( !validationFailures.Any() )
            {
                User existingUser = _seedUsers.FirstOrDefault( u => u.Id == user.Id );

                if ( existingUser != null )
                {
                    _seedUsers.Remove( existingUser );
                    _seedUsers.Add( user );
                }
                else
                {
                    validationFailures.Add( UserValidationFailureType.UserNotFound );
                }
            }

            return validationFailures;
        }

        public List<UserValidationFailureType> DeleteUser( int id )
        {
            List<UserValidationFailureType> validationFailures = new List<UserValidationFailureType>();
            try
            {
                if ( !ValidationHelper.IsValidId( id ) )
                {
                    validationFailures.Add( UserValidationFailureType.InvalidId );
                }

                User existingUser = _seedUsers.FirstOrDefault( u => u.Id == id );

                if ( existingUser != null )
                {
                    _seedUsers.Remove( existingUser );
                }

                return validationFailures;
            }
            catch
            {
                validationFailures.Add( UserValidationFailureType.GenericError );
                return validationFailures;
            }
        }

        public List<UserValidationFailureType> ValidateUser( User user )
        {
            List<UserValidationFailureType> validationFailures = new List<UserValidationFailureType>();

            if ( string.IsNullOrWhiteSpace( user.FirstName ) )
            {
                validationFailures.Add( UserValidationFailureType.FirstNameMissing );
            }

            if ( string.IsNullOrWhiteSpace( user.LastName ) )
            {
                validationFailures.Add( UserValidationFailureType.LastNameMissing );
            }

            if ( string.IsNullOrWhiteSpace( user.Email ) )
            {
                validationFailures.Add( UserValidationFailureType.EmailMissing );
            }

            if ( string.IsNullOrWhiteSpace( user.Password ) )
            {
                validationFailures.Add( UserValidationFailureType.PasswordMissing );
            }

            return validationFailures;
        }
    }
}
