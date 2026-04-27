using BE.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Api.Data
{
    //Nó kế thừa từ DbContext vì DbContext là lớp gốc mà EF Core cung cấp để quản lý
    //kết nối, truy vấn, lưu thay đổi, và ánh xạ các bảng trong database sang các class trong code.
    //Nếu không kế thừa DbContext, bạn sẽ không có những tính
    //năng này(ví dụ: SaveChanges(), OnModelCreating(), quản lý transaction…).
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Khai báo các bảng trong Database
        public DbSet<User> Users { get; set; }
    }
}
