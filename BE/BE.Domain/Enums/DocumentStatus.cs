using System;
using System.Collections.Generic;
using System.Text;

namespace BE.Domain.Enums
{
    public enum DocumentStatus
    {
        Draft, // Trạng thái khi người dùng vừa tạo tờ trình nhưng chưa bấm gửi duyệt.
        Pending, // Tờ trình đã được gửi và đang nằm trong luồng chờ duyệt.
        Approved, // Tờ trình đã đi đến bước cuối cùng và được phê duyệt hoàn toàn
        Rejected, // Tờ trình bị một người trong nhóm duyệt từ chối, đóng băng quy trình.
    }
}
