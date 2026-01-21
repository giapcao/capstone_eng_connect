using Serilog;
using EngConnect.Presentation;
using System.Text;

// Set UTF-8 Encoding for console output
Console.OutputEncoding = Encoding.UTF8;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog(Log.Logger);

builder.Configure();
var app = builder.Build();

app.Configure();
app.Run();