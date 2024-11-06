using DataLayer;
using Mapster;
using WebServer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<IDataService, DataService>();
builder.Services.AddSingleton(new Hashing());
builder.Services.AddAuthorization();
// Add services to the container.
builder.Services.AddControllers(); // <-- Add this line
var secret = "this is my custom Secret key for authnetication";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
        ClockSkew = TimeSpan.Zero
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseHttpsRedirection();


app.MapControllers();
app.UseAuthorization();
app.Run();
