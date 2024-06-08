using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server;
using Server.Data;
using Server.JwtFeatures;
using Server.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Recupera del fichero "~/appsettings.json" la cadena de conexión "DefaultConnection" para ser utilizada en nuestro contexto de base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
// Servicio que crea tanto la tabla de "AspNetRoles" como la tabla de "AspNetUsers"
builder.Services.AddIdentity<Usuario, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>();

// Servicio que proporciona a la clase "JwtHandler" como dependencia en el servidor
builder.Services.AddScoped<JwtHandler>();
// Servicio que configura Jwt en el servidor
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["JwtIssuer"],
            ValidAudience = jwtSettings["JwtAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.GetValue<string>("JwtSecurityKey")!))
        };
    });

// Servicio que configura AutoMapper con el constructor de la clase "MappingProfile"
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Servicio que condigura CORS en el servidor
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: "MyPolicy",
        configurePolicy: builder =>
        {
            builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Cuando se encuentra en un entorno de desarrollo crea la base de datos
    var db = app.Services
        .CreateScope()
        .ServiceProvider.GetService<ApplicationDbContext>();
    db?.Database.EnsureCreated();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Habilita la autenticación en el servidor
// Obligatorio ponerlo antes de "app.UseAuthorization();"
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors(policyName: "MyPolicy");

app.Run();
