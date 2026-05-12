import { apiGet, apiPost } from '../../api/axiosClient'
import type { DocumentDto } from './document.types'

// ============================================================
// DOCUMENT API - Các endpoint quản lý tờ trình
// (Chuẩn bị sẵn - sẽ hoàn thiện khi backend có Controller)
// ============================================================

const BASE = '/documents'

/** GET /api/documents - Lấy danh sách tờ trình */
export function getDocuments(): Promise<DocumentDto[]> {
  return apiGet<DocumentDto[]>(BASE)
}

/** GET /api/documents/:id - Lấy chi tiết tờ trình */
export function getDocumentById(id: string): Promise<DocumentDto> {
  return apiGet<DocumentDto>(`${BASE}/${id}`)
}

/** POST /api/documents - Tạo tờ trình mới */
export function createDocument(data: Partial<DocumentDto>): Promise<DocumentDto> {
  return apiPost<DocumentDto>(BASE, data)
}
