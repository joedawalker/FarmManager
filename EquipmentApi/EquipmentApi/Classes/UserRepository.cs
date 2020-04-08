using EquipmentApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EquipmentApi.Classes
{
	public class UserRepository : IUserRepository
	{
		private readonly EquipmentApiContext _equipmentContext;

		public UserRepository( EquipmentApiContext equipmentContext )
		{
			_equipmentContext = equipmentContext;
		}

		public async Task<List<User>> GetUsersAsync()
		{
			return await _equipmentContext.Users.ToListAsync().ConfigureAwait( false );
		}

		public async Task<User> GetUserAsync( int id )
		{
			return await _equipmentContext.Users.FirstOrDefaultAsync( u => u.Id == id ).ConfigureAwait( false );
		}

		public async Task<User> GetUserAsync( string email )
		{
			return await _equipmentContext.Users.FirstOrDefaultAsync( u => u.Email == email ).ConfigureAwait( false );
		}

		public async Task<bool> CreateUserAsync( User user )
		{
			try
			{
				_equipmentContext.Users.Add( user );

				await _equipmentContext.SaveChangesAsync().ConfigureAwait( false );

				return true;
			}
			catch
			{
				return false;
			}
		}

		public async Task<bool> UpdateUserAsync( User user )
		{
			try
			{
				User existingUser = await _equipmentContext.Users.FirstOrDefaultAsync( u => u.Id == user.Id).ConfigureAwait( false );
				
				if ( existingUser != null )
				{
					existingUser.FirstName = user.FirstName;
					existingUser.LastName = user.LastName;
					existingUser.Email = user.Email;
					existingUser.Password = user.Password;

					_equipmentContext.Users.Update( existingUser );
					await _equipmentContext.SaveChangesAsync().ConfigureAwait( false );
				}

				return true;
			}
			catch
			{
				return false;
			}
		}

		public async Task<bool> DeleteUserAsync( int id )
		{
			try
			{
				User user = await GetUserAsync( id ).ConfigureAwait( false );

				if ( user != null )
				{
					_equipmentContext.Users.Remove( user );
				}

				await _equipmentContext.SaveChangesAsync().ConfigureAwait( false );

				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
