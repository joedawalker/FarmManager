using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EquipmentApi.Classes
{
	public class UserManager : IUserManager
	{
		private readonly IUserRepository _userRepository;

		public UserManager( IUserRepository userRepository )
		{
			_userRepository = userRepository;
		}

		public async Task<List<User>> GetUsersAsync()
		{
			return await _userRepository.GetUsersAsync().ConfigureAwait( false );
		}

		public async Task<Tuple<User, List<UserValidationFailureType>>> GetUserAsync( int id )
		{
			List<UserValidationFailureType> validationFailures = new List<UserValidationFailureType>();
			User user = null;

			if ( !ValidationHelper.IsValidId( id ) )
			{
				validationFailures.Add( UserValidationFailureType.InvalidId );
				return Tuple.Create( user, validationFailures );
			}

			user = await _userRepository.GetUserAsync( id ).ConfigureAwait( false );

			if ( user == null )
			{
				validationFailures.Add( UserValidationFailureType.UserNotFound );
			}

			return Tuple.Create( user, validationFailures );
		}

		public async Task<Tuple<User, List<UserValidationFailureType>>> GetUserAsync( string email )
		{
			List<UserValidationFailureType> validationFailures = new List<UserValidationFailureType>();
			User user = null;

			if ( string.IsNullOrWhiteSpace( email ) )
			{
				validationFailures.Add( UserValidationFailureType.EmailBadFormat );

				return Tuple.Create( user, validationFailures );
			}

			user = await _userRepository.GetUserAsync( email ).ConfigureAwait( false );

			if ( user == null )
			{
				validationFailures.Add( UserValidationFailureType.UserNotFound );
			}

			return Tuple.Create( user, validationFailures );
		}

		public async Task<List<UserValidationFailureType>> CreateUserAsync( User user )
		{
			List<UserValidationFailureType> validationFailures = ValidateUser( user );

			if ( !validationFailures.Any() )
			{
				List<User> existingUsers = await GetUsersAsync().ConfigureAwait( false );

				if ( existingUsers.FirstOrDefault( u => u.Email.Equals( user.Email, StringComparison.InvariantCultureIgnoreCase ) ) == null )
				{
					if ( await _userRepository.CreateUserAsync( user ).ConfigureAwait( false ) )
					{
						return validationFailures;
					}

					validationFailures.Add( UserValidationFailureType.GenericError );
				}
				else
				{
					validationFailures.Add( UserValidationFailureType.EmailAlreadyInUse );
				}
			}

			return validationFailures;
		}

		public async Task<List<UserValidationFailureType>> UpdateUserAsync( User user )
		{
			List<UserValidationFailureType> validationFailures = new List<UserValidationFailureType>();
			validationFailures.AddRange( ValidateUser( user ) );

			if ( user != null && !ValidationHelper.IsValidId( user.Id ) )
			{
				validationFailures.Add( UserValidationFailureType.InvalidId );
				return validationFailures;
			}

			if ( !validationFailures.Any() )
			{
				List<User> existingUsers = await GetUsersAsync().ConfigureAwait( false );

				if ( existingUsers.FirstOrDefault( u => u.Id == user.Id ) == null )
				{
					validationFailures.Add( UserValidationFailureType.UserNotFound );
				}
				else
				{
					if ( !await _userRepository.UpdateUserAsync( user ).ConfigureAwait( false ) )
					{
						validationFailures.Add( UserValidationFailureType.GenericError );
					}
				}
			}

			return validationFailures;
		}

		public async Task<List<UserValidationFailureType>> DeleteUserAsync( int id )
		{
			List<UserValidationFailureType> validationFailures = new List<UserValidationFailureType>();

			try
			{
				if ( !ValidationHelper.IsValidId( id ) )
				{
					validationFailures.Add( UserValidationFailureType.InvalidId );
				}

				if ( !await _userRepository.DeleteUserAsync( id ).ConfigureAwait( false ) )
				{
					validationFailures.Add( UserValidationFailureType.GenericError );
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

			if ( user == null )
			{
				validationFailures.Add( UserValidationFailureType.UserIsNull );
				return validationFailures;
			}

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
