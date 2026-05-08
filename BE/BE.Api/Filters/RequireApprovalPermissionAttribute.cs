using BE.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BE.Api.Filters
{
    public class RequireApprovalPermissionAttribute : TypeFilterAttribute
    {
        public RequireApprovalPermissionAttribute() : base(typeof(ApprovalPermissionFilter))
        {
        }
    }

    public class ApprovalPermissionFilter : IAsyncAuthorizationFilter
    {
        private readonly ApplicationDbContext _dbContext;

        public ApprovalPermissionFilter(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user.Identity?.IsAuthenticated != true)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Lấy DocumentId từ Route hoặc Body
            if (!context.RouteData.Values.TryGetValue("id", out var docIdObj) || !Guid.TryParse(docIdObj?.ToString(), out Guid documentId))
            {
                // Nếu không có trong route, có thể check trong body nếu cần, nhưng thường là Route
                return;
            }

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                context.Result = new ForbidResult();
                return;
            }

            // Kiểm tra: Tờ trình có đang chờ duyệt không?
            var document = await _dbContext.Documents
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == documentId);

            if (document == null)
            {
                context.Result = new NotFoundObjectResult(new { message = "Không tìm thấy tờ trình." });
                return;
            }

            if (document.Status != BE.Domain.Enums.DocumentStatus.Pending)
            {
                context.Result = new BadRequestObjectResult(new { message = "Tờ trình không ở trạng thái chờ duyệt." });
                return;
            }

            // Kiểm tra: User có thuộc đúng Nhóm/Phòng ban được chỉ định ở bước hiện tại không?
            // Logic: Tìm Approval đang ở trạng thái Pending cho Document này
            var currentApproval = await _dbContext.Approvals
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.DocumentId == documentId && a.ActionType == BE.Domain.Enums.ActionType.Pending);

            if (currentApproval == null)
            {
                context.Result = new BadRequestObjectResult(new { message = "Tờ trình hiện không có luồng duyệt khả dụng." });
                return;
            }

            // Kiểm tra User có nằm trong Group của bước duyệt này không
            var isUserInGroup = await _dbContext.UserGroups
                .AnyAsync(ug => ug.UserId == userId && ug.GroupId == currentApproval.GroupId);

            if (!isUserInGroup)
            {
                context.Result = new ObjectResult(new { message = "Bạn không có quyền thực hiện hành động này trên tờ trình này." }) 
                { 
                    StatusCode = StatusCodes.Status403Forbidden 
                };
            }
        }
    }
}
