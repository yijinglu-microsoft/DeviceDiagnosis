using Microsoft.Extensions.FileProviders;
using System.Net;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Setting CORS to allow localhost:3000 for Microsoft Chat Copilot
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Listen(IPAddress.Loopback, 7147, listenOptions =>
    {
        listenOptions.UseHttps("C:\\Users\\yijinglu\\AppData\\Local\\mkcert\\localhost.pfx");
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

_ = builder.Logging.ClearProviders();
_ = builder.Logging.AddConsole();

// Service Bindings
var app = builder.Build();

// Add CORS middleware before routing (and explicitly before UseEndpoints)
app.UseCors("AllowLocalhost3000");

// Add static files (manifest etc)
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, ".well-known")),
    RequestPath = "/.well-known"
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
