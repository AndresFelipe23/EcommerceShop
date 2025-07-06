using Ecommerce.BD;
using Ecommerce.BD.Interfaces;
using Ecommerce.BD.Repositorios;
using Ecommerce.PRC.Interfaces;
using Ecommerce.PRC.Services;
using Ecommerce.PRC.Servicios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Text;
using Ecommerce.API.Models;
using Ecommerce.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar zona horaria para Colombia
TimeZoneInfo colombiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
AppContext.SetSwitch("System.Globalization.InvariantGlobalization", false);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Ecommerce Ropa API",
        Version = "v1",
        Description = "API para gestión de ecommerce de ropa",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Desarrollador",
            Email = "desarrollador@ecommerce.com"
        }
    });

    // Configurar autenticación JWT en Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    // Agregar descripciones de los endpoints
});

// CONEXION A BD
ConexionBD.CadenaConexion = builder.Configuration.GetConnectionString("DefaultConnection");

// =========================================
// REGISTRO DE REPOSITORIOS
// =========================================
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioRoleRepository, UsuarioRoleRepository>();
builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<ITallaRepository, TallaRepository>();
builder.Services.AddScoped<ITemporadaRepository, TemporadaRepository>();
builder.Services.AddScoped<IPromocionRepository, PromocionRepository>();
builder.Services.AddScoped<IColorRepository, ColorRepository>();
builder.Services.AddScoped<ICuponRepository, CuponRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICuponesUsuarioRepository, CuponesUsuarioRepository>();
builder.Services.AddScoped<IDireccionRepository, DireccionRepository>();
builder.Services.AddScoped<ICarritoRepository, CarritoRepository>();
builder.Services.AddScoped<ICarritoItemRepository, CarritoItemRepository>();
builder.Services.AddScoped<IInventarioRepository, InventarioRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IPedidoDetalleRepository, PedidoDetalleRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoImagenRepository, ProductoImagenRepository>();
builder.Services.AddScoped<IProductoTallaColorRepository, ProductoTallaColorRepository>();

// =========================================
// REGISTRO DE SERVICIOS
// =========================================
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioRoleService, UsuarioRoleService>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<ITallaService, TallaService>();
builder.Services.AddScoped<ITemporadaService, TemporadaService>();
builder.Services.AddScoped<IPromocionService, PromocionService>();
builder.Services.AddScoped<IColorService, ColorService>();
builder.Services.AddScoped<ICuponService, CuponService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<ICuponesUsuarioService, CuponesUsuarioService>();
builder.Services.AddScoped<IDireccionService, DireccionService>();
builder.Services.AddScoped<ICarritoService, CarritoService>();
builder.Services.AddScoped<ICarritoItemService, CarritoItemService>();
builder.Services.AddScoped<IInventarioService, InventarioService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IPedidoDetalleService, PedidoDetalleService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IProductoImagenService, ProductoImagenService>();
builder.Services.AddScoped<IProductoTallaColorService, ProductoTallaColorService>();

// =========================================
// CONTEXTO DAPPER
// =========================================
builder.Services.AddSingleton<DapperContext>();

// =========================================
// CONFIGURACIÓN JWT
// =========================================
// Configurar JwtSettings como singleton
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);

// Registrar el servicio JWT
builder.Services.AddScoped<IJwtService, JwtService>();

// Configurar autenticación JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings?.SecretKey ?? "DefaultKey")),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings?.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings?.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") 
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

// Agregar autenticación antes de autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
