using FeatureFlag.API.Controllers.Infra.Attributes;
using FeatureFlag.API.Infra;
using FeatureFlag.Repositorio.Infra;
using FeatureFlag.Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(options =>
{
    var configuration = builder.Configuration;
    options.Filters.Add(new FeatureFlagAuthorizeAttribute(configuration));
});

var mongoConnectionString = builder.Configuration.GetConnectionString("MongoConnection") ?? "mongodb://localhost:27017";
var mongoDatabaseName = builder.Configuration["MongoDatabaseName"] ?? "feature-flag";
builder.Services.ConfigurarBanco(mongoConnectionString, mongoDatabaseName);

builder.Services.ConfigurarRepositorios();
builder.Services.ConfigurarServicos();
builder.Services.ConfigurarServicosAplicacao();
builder.Services.ConfigurarAutoMapper();
builder.Services.ConfigurarJsonOptions();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();  

app.MapControllers();

app.Run();
