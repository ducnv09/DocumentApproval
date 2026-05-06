using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// KHU VỰC 1: ĐĂNG KÝ DỊCH VỤ (Khai báo trước khi Build)
// ============================================================

builder.Services.AddControllers();
// .NET 8 equivalent for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Đăng ký Database Context (Sử dụng MySQL)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BE.Infrastructure.Persistence.Contexts.ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// 2. Mở CORS để App Mobile (VueJS) có thể gọi được API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMobileApp", policy =>
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// 3. Đăng ký cấu hình bảo mật JWT
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
// LƯU Ý: THỨ TỰ Ở ĐÂY RẤT QUAN TRỌNG, KHÔNG ĐẢO LỘN
// ============================================================

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();

// 4. Kích hoạt CORS (Phải đứng trước Authentication)
app.UseCors("AllowMobileApp");

// 5. Kích hoạt Authentication (Xác thực xem user là ai - Bắt buộc phải có)
app.UseAuthentication();

// 6. Kích hoạt Authorization (Kiểm tra xem user có quyền làm gì không)
app.UseAuthorization();

app.MapControllers();

app.Run();
