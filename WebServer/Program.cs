using DataLayer;
using Mapster;
using WebServer.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<IDataService, DataService>();
builder.Services.AddSingleton(new Hashing());
// Add services to the container.
builder.Services.AddMvcCore();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();
