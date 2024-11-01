using Microsoft.AspNetCore.OData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers().AddOData();
// Ceci est nécessaire pour les API controllers!
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS configuration - Nécessaire pour le frontend React
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty; // Ceci fait que Swagger s'ouvre à la racine
    });
    
}

app.UseHttpsRedirection();

// Activer CORS avant le routing
app.UseCors("AllowAll");

app.UseAuthorization(); // Important pour la sécurité future

app.MapControllers();

app.Run();