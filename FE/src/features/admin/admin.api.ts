import { apiGet, apiPost, apiPut, apiDelete } from '../../api/axiosClient'
import type {
  UserDto,
  CreateUserDto,
  UpdateUserDto,
  GroupDto,
  CreateGroupDto,
  PositionDto,
  CreatePositionDto,
  AssignUserGroupDto,
} from './admin.types'

// ============================================================
// ADMIN API - Các endpoint quản trị (chỉ Admin)
// Backend: AdminController [Route("api/[controller]")]
// ============================================================

const BASE = '/admin'

// ---- Users ----

/** GET /api/admin/users */
export function getUsers(): Promise<UserDto[]> {
  return apiGet<UserDto[]>(`${BASE}/users`)
}

/** POST /api/admin/users */
export function createUser(dto: CreateUserDto): Promise<UserDto> {
  return apiPost<UserDto>(`${BASE}/users`, dto)
}

/** PUT /api/admin/users/:id */
export function updateUser(id: string, dto: UpdateUserDto): Promise<void> {
  return apiPut(`${BASE}/users/${id}`, dto)
}

/** DELETE /api/admin/users/:id */
export function deleteUser(id: string): Promise<void> {
  return apiDelete(`${BASE}/users/${id}`)
}

// ---- Groups ----

/** GET /api/admin/groups */
export function getGroups(): Promise<GroupDto[]> {
  return apiGet<GroupDto[]>(`${BASE}/groups`)
}

/** POST /api/admin/groups */
export function createGroup(dto: CreateGroupDto): Promise<GroupDto> {
  return apiPost<GroupDto>(`${BASE}/groups`, dto)
}

// ---- Positions ----

/** GET /api/admin/positions */
export function getPositions(): Promise<PositionDto[]> {
  return apiGet<PositionDto[]>(`${BASE}/positions`)
}

/** POST /api/admin/positions */
export function createPosition(dto: CreatePositionDto): Promise<PositionDto> {
  return apiPost<PositionDto>(`${BASE}/positions`, dto)
}

// ---- Assign User to Group ----

/** POST /api/admin/assign-group */
export function assignUserToGroup(dto: AssignUserGroupDto): Promise<void> {
  return apiPost(`${BASE}/assign-group`, dto)
}
