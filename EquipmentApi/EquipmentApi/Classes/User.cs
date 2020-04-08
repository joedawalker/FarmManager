using System.Text.Json.Serialization;

namespace EquipmentApi.Classes
{
    public class User
    {
        [JsonPropertyName( "id" )]
        public int Id { get; set; }

        [JsonPropertyName( "first_name" )]
        public string FirstName { get; set; }

        [JsonPropertyName( "last_name" )]
        public string LastName { get; set; }

        [JsonPropertyName( "email" )]
        public string Email { get; set; }

        [JsonPropertyName( "password" )]
        public string Password { get; set; }
    }
}
