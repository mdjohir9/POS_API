using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using POS_API.Entities;
using POS_API.Implementation;
using POS_API.Repository;

var builder = WebApplication.CreateBuilder(args);

// =========================================================================
// 1. SERVICES CONFIGURATION (All services must be registered BEFORE Build())
// =========================================================================

// CORS Configuration
var MyAllowOrigins = "MyAllowOrigins";
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowOrigins,
        policy =>
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// Controllers and Caching
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddMemoryCache();

// Session Management
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// HTTP Context Accessor
builder.Services.AddHttpContextAccessor();

// Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

// HTTP Client
builder.Services.AddHttpClient();

// Swagger / OpenAPI Configuration
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(option =>
//{
//    option.SwaggerDoc("v1", new OpenApiInfo { Title = "POS API", Version = "v1" });
//    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        In = ParameterLocation.Header,
//        Description = "Please enter a valid JWT token",
//        Name = "Authorization",
//        Type = SecuritySchemeType.Http,
//        BearerFormat = "JWT",
//        Scheme = "Bearer"
//    });

//    option.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer"
//                }
//            },
//            Array.Empty<string>()
//        }
//    });
//});

// JWT Authentication Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
        };
    });

// Custom Dependency Injection (Repositories and Unit of Work)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

// =========================================================================
// 2. APPLICATION PIPELINE CONFIGURATION (Middleware execution order)
// =========================================================================

var app = builder.Build();

// Enable Swagger UI in both Development and Production (Adjust if restricted in Production)
//app.UseSwagger();
//app.UseSwaggerUI(options =>
//{
//    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Loan API v1");
//    options.RoutePrefix = "swagger"; // Standard URL: https://localhost:<port>/swagger
//});

app.UseHttpsRedirection();

app.UseCors(MyAllowOrigins);

// Static Files Setup
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
    }
});
app.UseStaticFiles();

// Session and Routing
app.UseSession();
app.UseRouting();

// Custom Timestamp Middleware
app.UseMiddleware<TimestampMiddleware>();

// Custom No-Cache Headers Middleware
app.Use(async (context, next) =>
{
    context.Response.Headers.Remove("Cache-Control");
    context.Response.Headers.Remove("Pragma");
    context.Response.Headers.Remove("Expires");

    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";

    await next();
});

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Map Endpoints
app.MapControllers();

app.Run();