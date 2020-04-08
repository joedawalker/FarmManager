using System.Collections.Generic;
using System.Threading.Tasks;

namespace EquipmentApi.Classes
{
	public interface IUserRepository
	{
		Task<bool> CreateUserAsync( User user );
		Task<bool> DeleteUserAsync( int id );
		Task<User> GetUserAsync( int id );
		Task<User> GetUserAsync( string email );
		Task<List<User>> GetUsersAsync();
		Task<bool> UpdateUserAsync( User user );
	}
}