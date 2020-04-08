using System.Text.Json.Serialization;

namespace EquipmentApi.Models
{
	public abstract class BaseResponseModel
	{
		[JsonPropertyName( "is_success" )]
		public bool IsSuccess { get; set; }

		[JsonPropertyName( "message" )]
		public string Message { get; set; }
	}
}
