using AoiCryptoAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMongoDB(Environment.GetEnvironmentVariable("CONNECTION_STRING"), "AoiCrypto");

builder.Services.AddRepositories();

builder.Services.AddHttpClient();
builder.Services.AddServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8081";
builder.WebHost.UseUrls($"http://*:{port}");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
