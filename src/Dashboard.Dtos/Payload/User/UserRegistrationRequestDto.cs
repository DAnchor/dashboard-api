using System.Text.Json.Serialization;

namespace Dashboard.Dtos.Payload.User;

public record class UserRegistrationRequestDto
(
    [property: JsonPropertyName("age")]
    int Age,
    [property: JsonPropertyName("address")]
    string Address,
    [property: JsonPropertyName("firstName")]
    string FirstName,
    [property: JsonPropertyName("lastName")]
    string LastName,
    [property: JsonPropertyName("passsword")]
    string Password
);