using KeyCloakTest.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeyCloakTest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController(IConfiguration configuration) : ControllerBase
{
    [Authorize]
    [HttpGet("[action]")]
    public IActionResult Test() => Ok("ok!");

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(string Username, string Password)
    {
        var options = configuration.GetSection("Keycloak").Get<KeycloakOptions>();
        var tokenUrl = $"{options.Authority}/protocol/openid-connect/token";

        using var client = new HttpClient();
        var data = new Dictionary<string, string>
        {
            ["grant_type"] = "password",
            ["client_id"] = options.ClientId,
            ["username"] = Username,
            ["password"] = Password,
            ["scope"] = "openid"
        };

        var response = await client.PostAsync(tokenUrl, new FormUrlEncodedContent(data));
        var json = await response.Content.ReadAsStringAsync();

        return Content(json, "application/json");
    }


    //[HttpGet("[action]")]
    //public async Task<IActionResult> Callback([FromQuery] string code)
    //{
    //    if (string.IsNullOrEmpty(code)) return BadRequest("Missing authorization code.");
    //    var options = configuration.GetSection("Keycloak").Get<KeycloakOptions>();

    //    using var client = new HttpClient();
    //    var tokenUrl = $"{options.Authority}/protocol/openid-connect/token";

    //    var data = new Dictionary<string, string>
    //    {
    //        ["grant_type"] = "authorization_code",
    //        ["client_id"] = options.ClientId,
    //        ["code"] = code,
    //        ["redirect_uri"] = options.RedirectUri
    //    };

    //    var response = await client.PostAsync(tokenUrl, new FormUrlEncodedContent(data));
    //    var json = await response.Content.ReadAsStringAsync();
    //    return Ok(json);
    //}
}