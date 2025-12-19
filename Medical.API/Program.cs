using Medical.Application.Interfaces;
using Medical.Application.Services;
using Medical.Domain.Interfaces;
using Medical.Infrastructure.Persistence;
using Medical.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------------------------------------------------
// 1. CONFIGURACIÓN DE BASE DE DATOS (MySQL)
// -------------------------------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// -------------------------------------------------------------------------
// 2. INYECCIÓN DE DEPENDENCIAS (Arquitectura Hexagonal)
// -------------------------------------------------------------------------
// Repositorios
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

// Servicios
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

// -------------------------------------------------------------------------
// 3. CORS (Habilitar conexión con React)
// -------------------------------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// -------------------------------------------------------------------------
// 4. CONTROLADORES Y SWAGGER (Documentación)
// -------------------------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sistema de Gestión de Citas Médicas",
        Version = "v1.0",
        Description = "API RESTful desarrollada con Arquitectura Hexagonal para la gestión de clínicas, doctores y citas de pacientes."
        // Se eliminó la sección de contacto por privacidad
    });
});

var app = builder.Build();

// -------------------------------------------------------------------------
// 5. PIPELINE HTTP
// -------------------------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sistema Citas Médicas v1");
        c.DocumentTitle = "Portal API - Citas Médicas";
    });
}

app.UseHttpsRedirection();

// Importante: CORS debe ir antes de Authorization
app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();

app.Run();