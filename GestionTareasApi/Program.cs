using Microsoft.EntityFrameworkCore;
using GestionTareasApi.Data;
using System.Text.Json.Serialization;
using GestionTareasApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Serializar enums como strings en las respuestas y recibir cadenas en los requests
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Registrar DbContext con SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar HttpClient y Servicio para la API externa
builder.Services.AddHttpClient<TareasExternasService>(client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
    client.Timeout = TimeSpan.FromSeconds(15); // Límite de tiempo controlado
});

// Registrar Servicio de Análisis de Sentimiento (ML.NET) como Singleton
builder.Services.AddSingleton<SentimentAnalysisService>();

var app = builder.Build();

// Auto-migrar la base de datos al arrancar la aplicación
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
