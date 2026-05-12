// ============================================================
// Document DTOs - Chuẩn bị sẵn theo DB Schema
// ============================================================

export type DocumentStatus = 'Draft' | 'Pending' | 'Approved' | 'Rejected'
export type WorkflowType = 'SEQUENTIAL' | 'PARALLEL'

export interface DocumentDto {
  id: string
  creatorId: string
  groupId: string
  docTypeId: string
  title: string
  content: string
  attachmentUrl: string | null
  status: DocumentStatus
  createdAt: string
  updatedAt: string
}

export interface WorkflowDto {
  id: string
  name: string
  type: WorkflowType
  isActive: boolean
}

export interface StepDto {
  id: string
  workflowId: string
  stepOrder: number
  groupId: string
  positionId: string | null
  approvalCount: number
}

export interface DocTypeDto {
  id: string
  workflowId: string
  name: string
  description: string
}
