using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using WebAPIinVSC.Models;

namespace WebAPIinVSC.Handler;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{

    public readonly LearnDbContext learnDbContext;
    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> option, ILoggerFactory loggerFactory, UrlEncoder urlEncoder, ISystemClock systemClock, LearnDbContext _learnDB)
    : base(option, loggerFactory, urlEncoder, systemClock)
    {
        learnDbContext = _learnDB;
    }

    protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.Fail("No header found");
        }

        var _headerValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
        var bytes = Convert.FromBase64String(_headerValue.Parameter);
        string credentials = Encoding.UTF8.GetString(bytes);

        if (!string.IsNullOrEmpty(credentials))
        {
            string[] array = credentials.Split(':');
            string userName = array[0];
            string password = array[1];

            var user = this.learnDbContext.Users.FirstOrDefault(User => User.UserName == userName && User.Password == password);
            if (user == null)
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            // in case of success when have to generate a ticket 
            var claim = new[] { new Claim(ClaimTypes.Name, userName) };
            var identity = new ClaimsIdentity(claim, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);

        }




        return AuthenticateResult.Fail("unauthorised");
    }
}
