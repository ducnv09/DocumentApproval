using BE.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BE.Infrastructure.Persistence.Contexts
{
    // thiết lập kết nối với db
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<DocType> DocTypes { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Approval> Approvals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // tự động "quét" (scan) toàn bộ project (Assembly) hiện tại để tìm những class nào đang kế thừa từ interface IEntityTypeConfiguration<T>
            // sau khi đã tìm thấy thì nó sẽ tự động gọi phương thức Configure(modelBuilder) trong mỗi class config
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
