// ============================================================
// Admin DTOs - Khớp với BE.Application.DTOs.Admin.AdminDtos.cs
// ============================================================

export interface UserDto {
  id: string
  username: string
  fullName: string
  email: string
  isAdmin: boolean
  isActive: boolean
}

export interface CreateUserDto {
  username: string
  password: string
  fullName: string
  email: string
  isAdmin: boolean
}

export interface UpdateUserDto {
  fullName: string
  email: string
  isActive: boolean
}

export interface GroupDto {
  id: string
  name: string
  code: string
}

export interface CreateGroupDto {
  name: string
  code: string
}

export interface PositionDto {
  id: string
  name: string
}

export interface CreatePositionDto {
  name: string
}

export interface AssignUserGroupDto {
  userId: string
  groupId: string
  positionId: string
}
