// ============================================================
// Approval DTOs - Lịch sử & hành động phê duyệt
// ============================================================

export type ApprovalAction = 'PENDING' | 'APPROVED' | 'REJECTED'

export interface ApprovalDto {
  id: string
  documentId: string
  stepId: string
  groupId: string
  approverId: string | null
  actionType: ApprovalAction
  reason: string | null
  signatureData: string | null
  actionAt: string | null
}
