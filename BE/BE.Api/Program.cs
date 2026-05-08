using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BE.Api.Middlewares;
using BE.Api.Extensions;
using BE.Application.DTOs;
using BE.Application.DTOs.Auth;
using BE.Application.DTOs.Admin;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// KHU VỰC 1: ĐĂNG KÝ DỊCH VỤ (Khai báo trước khi Build)
// ============================================================

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Bind cấu hình từ appsettings.json vào JwtOptions
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

// Tối ưu hóa JSON Serialization bằng Source Generator (Chaining modular contexts)
// Thay vì dùng cơ chế Reflection chậm chạp để parse JSON lúc runtime, bạn đang đăng ký sẵn các JsonContext. 
// Việc này giúp API serialize/deserialize các DTO nhanh hơn rất nhiều và tiêu tốn ít RAM hơn.
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, SharedJsonContext.Default);
    options.SerializerOptions.TypeInfoResolverChain.Insert(1, AuthJsonContext.Default);
    options.SerializerOptions.TypeInfoResolverChain.Insert(2, AdminJsonContext.Default);
});

// 1. Đăng ký Database Context (Sử dụng MySQL)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BE.Infrastructure.Persistence.Contexts.ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// 1.1 Đăng ký Services & Repositories thông qua Extensions
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

// 2. Mở CORS để App Mobile (VueJS) có thể gọi được API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMobileApp", policy =>
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// 2.1 Cấu hình Policy phân quyền
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("IsAdmin", "true"));
});

// 3. Đăng ký Global Exception Handler (.NET 8)
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// 4. Đăng ký cấu hình bảo mật JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "ChuoiMacDinhNeuQuenCauHinh123456789!!!";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

var app = builder.Build();

// ============================================================
// KHU VỰC 2: CẤU HÌNH PIPELINE (Luồng chạy của Request)
// ============================================================

// Global Exception Handling (.NET 8 - Phải đặt ở đầu Pipeline)
app.UseExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowMobileApp");

app.UseAuthentication();
app.UseActiveUserCheck();
app.UseAuthorization();

app.MapControllers();

// Seed Database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BE.Infrastructure.Persistence.Contexts.ApplicationDbContext>();
    await BE.Infrastructure.Persistence.DbInitializer.SeedAsync(context);
}

app.Run();
