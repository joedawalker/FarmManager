using System.Text.Json.Serialization;

namespace EquipmentApi.Models
{
    public class ErrorModel : BaseResponseModel
    {
        public ErrorModel( string message, string hint )
        {
            IsSuccess = false;
            Message = $"Error: {message}";
            Hint = hint;
        }

        [JsonPropertyName( "hint" )]
        public string Hint { get; set; }
    }
}