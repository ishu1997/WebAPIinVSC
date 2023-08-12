using WebAPIinVSC.Models;
using Microsoft.EntityFrameworkCore;
using WebAPIinVSC.Handler;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

var _auth = builder.Configuration.GetValue<string>("JwtSettings:SecurityKey");
builder.Services.AddAuthentication(Item =>
{
    Item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    Item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(Item =>
{
    Item.RequireHttpsMetadata = true;
    Item.SaveToken = true;
    Item.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_auth)),
        ValidateAudience = false,
        ValidateIssuer = false,
        ClockSkew = TimeSpan.Zero // this parameter is used when the token expiration is less than 5 mins
    };
}

);

builder.Services.AddDbContext<LearnDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("constring"));
});

// dependency registered here
builder.Services.AddScoped<IproductRepo, ProductRepo>();

// need to configure JWT services ->  to use it in our constructors
var _jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(_jwtSettings);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
