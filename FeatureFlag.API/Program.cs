using FeatureFlag.API.Infra;
using FeatureFlag.Repositorio.Infra;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.ConfigurarBanco("mongodb://localhost:27017", "feature-flag");
builder.Services.ConfigurarRepositorios();
builder.Services.ConfigurarServicos();
DocumentsRegistration.Registrar();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
