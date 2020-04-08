using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EquipmentApi.Classes
{
    public interface IUserManager
    {
        Task<List<UserValidationFailureType>> CreateUserAsync( User user );
        Task<List<UserValidationFailureType>> DeleteUserAsync( int id );
        Task<Tuple<User, List<UserValidationFailureType>>> GetUserAsync( int id );
        Task<Tuple<User, List<UserValidationFailureType>>> GetUserAsync( string email );
        Task<List<User>> GetUsersAsync();
        Task<List<UserValidationFailureType>> UpdateUserAsync( User user );
    }
}
