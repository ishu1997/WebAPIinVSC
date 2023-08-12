using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using WebAPIinVSC.Models;


namespace WebAPIinVSC.Controllers;

[ApiController]
[Route("[controller]")]
public class userController : ControllerBase
{


    private readonly LearnDbContext learnDbContext;
    private readonly JwtSettings jwtSettings;

    public userController(LearnDbContext _learnDbContext, IOptions<JwtSettings> options)
    {
        learnDbContext = _learnDbContext;
        jwtSettings = options.Value;
    }

    [HttpPost("Authenticate")]
    public IActionResult Authenticate([FromBody] UserCred userCred)
    {
        Console.WriteLine(userCred.Name);

        foreach (var us in learnDbContext.Users)
        {
            Console.WriteLine(us.UserName + " " + us.Password);
        }
        var user = learnDbContext.Users.FirstOrDefault(x => x.UserName == userCred.Name && x.Password == userCred.Password);
        if (user == null)
        {
            return Unauthorized();
        }

        // here we generate our token
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(this.jwtSettings.SecurityKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, user.UserName) }),
            Expires = DateTime.Now.AddSeconds(20),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        string finalToken = tokenHandler.WriteToken(token);
        return Ok(finalToken);
    }
}