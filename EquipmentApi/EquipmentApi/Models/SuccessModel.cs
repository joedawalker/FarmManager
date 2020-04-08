namespace EquipmentApi.Models
{
	public class SuccessModel : BaseResponseModel
	{
		public SuccessModel( string message )
		{
			IsSuccess = true;
			Message = message;
		}
	}
}
