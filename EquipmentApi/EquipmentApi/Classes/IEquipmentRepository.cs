using System.Collections.Generic;
using System.Threading.Tasks;

namespace EquipmentApi.Classes
{
    public interface IEquipmentRepository
    {
        Task<List<EquipmentItem>> GetEquipmentAsync();
        Task<EquipmentItem> GetEquipmentItemAsync( int id );
        Task<bool> CreateEquipmentItemAsync( EquipmentItem equipmentItem );
        Task<bool> UpdateEquipmentItemAsync( EquipmentItem equipmentItem );
        Task<bool> DeleteEquipmentItemAsync( int id );
        Task<bool> DeleteMaintenanceItemAsync( int id );
    }
}