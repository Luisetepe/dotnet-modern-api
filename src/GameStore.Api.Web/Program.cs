using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder();

builder.Services.AddFastEndpoints().SwaggerDocument();

var app = builder.Build();
app.UseFastEndpoints().UseSwaggerGen();

app.Run();
