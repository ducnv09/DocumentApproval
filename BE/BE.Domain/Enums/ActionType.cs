using System;
using System.Collections.Generic;
using System.Text;

namespace BE.Domain.Enums
{
    public enum ActionType
    {
        Pending, // Hệ thống tự động sinh ra bản ghi Approvals mang trạng thái này để "chờ" người dùng vào thao tác
        Approved, // Người dùng đã bấm nút [Đồng ý].
        Rejected, // Người dùng đã bấm nút [Từ chối].
        Canceled, // Bị hủy bỏ do luồng bị từ chối
    }
}
