using Serilog;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

//Add logging config 
var loggingConfig = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile
    (
        path: "appsettings.json", 
        optional: false, 
        reloadOnChange: true //Allows for dynamic reloading on setting change
    )
    .Build();

//Adding logging 
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(loggingConfig, sectionName: "Serilog")
    .CreateLogger();

//Use Serilog instead of basic logging
builder.Host.UseSerilog(); 

// Add services to the container.
builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("mssqlconnection");
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging(); //Use Serilog request logging middleware

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
