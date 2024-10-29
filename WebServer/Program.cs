using DataLayer;
using Mapster;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<IDataService, DataService>();

// Add services to the container.
builder.Services.AddMvcCore();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
