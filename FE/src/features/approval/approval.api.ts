import { apiGet, apiPost } from '../../api/axiosClient'
import type { ApprovalDto } from './approval.types'

// ============================================================
// APPROVAL API - Các endpoint phê duyệt tờ trình
// (Chuẩn bị sẵn - sẽ hoàn thiện khi backend có Controller)
// ============================================================

const BASE = '/approvals'

/** GET /api/approvals/document/:documentId - Lấy lịch sử phê duyệt */
export function getApprovalsByDocument(documentId: string): Promise<ApprovalDto[]> {
  return apiGet<ApprovalDto[]>(`${BASE}/document/${documentId}`)
}

/** POST /api/approvals/:id/approve - Duyệt tờ trình */
export function approveDocument(approvalId: string, signatureData?: string): Promise<void> {
  return apiPost(`${BASE}/${approvalId}/approve`, { signatureData })
}

/** POST /api/approvals/:id/reject - Từ chối tờ trình */
export function rejectDocument(approvalId: string, reason: string): Promise<void> {
  return apiPost(`${BASE}/${approvalId}/reject`, { reason })
}
