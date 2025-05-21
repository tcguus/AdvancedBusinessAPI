using Microsoft.EntityFrameworkCore;
using AdvancedBusinessAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o DbContext com Oracle
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// Adiciona suporte a controllers
builder.Services.AddControllers();

// Adiciona Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();