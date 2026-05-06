using System;

namespace BE.Domain.Entities
{
    public class UserGroup
    {
        public Guid UserId { get; private set; }
        public Guid GroupId { get; private set; }
        public Guid? PositionId { get; private set; }

        protected UserGroup() { }

        public UserGroup(Guid userId, Guid groupId, Guid? positionId)
        {
            UserId = userId;
            GroupId = groupId;
            PositionId = positionId;
        }

        public void UpdatePosition(Guid? positionId)
        {
            PositionId = positionId;
        }
    }
}
