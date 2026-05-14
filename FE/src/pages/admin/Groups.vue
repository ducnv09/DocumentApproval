<template>
  <div class="space-y-4">
    <!-- Tabs: Phòng ban / Chức vụ -->
    <div class="flex bg-white rounded-xl border border-slate-200 p-1">
      <button
        v-for="tab in tabs"
        :key="tab.key"
        class="flex-1 py-2 text-sm font-semibold rounded-lg border-none cursor-pointer transition-all"
        :class="activeTab === tab.key ? 'bg-blue-500 text-white shadow-sm' : 'bg-transparent text-slate-500 hover:text-slate-700'"
        @click="activeTab = tab.key"
      >
        <i :class="tab.icon" class="mr-1.5 text-xs" />
        {{ tab.label }}
      </button>
    </div>

    <!-- ==================== TAB: Phòng Ban ==================== -->
    <template v-if="activeTab === 'groups'">
      <div class="flex items-center justify-between">
        <p class="text-xs text-slate-400">{{ groups.length }} phòng ban</p>
        <button
          class="flex items-center gap-1.5 bg-blue-500 text-white text-sm font-semibold px-3.5 py-2 rounded-lg border-none cursor-pointer hover:bg-blue-600 active:scale-95"
          @click="openGroupDialog()"
        >
          <i class="pi pi-plus text-xs" /> Thêm
        </button>
      </div>

      <div v-if="isLoading" class="flex justify-center py-12">
        <i class="pi pi-spin pi-spinner text-2xl text-blue-500" />
      </div>

      <div v-else class="flex flex-col gap-2.5">
        <div v-if="groups.length === 0" class="text-center py-12 text-slate-400 text-sm">
          <i class="pi pi-sitemap text-3xl mb-2 block" />
          Chưa có phòng ban nào
        </div>

        <div
          v-for="group in groups"
          :key="group.id"
          class="bg-white rounded-xl px-4 py-3.5 shadow-sm border border-slate-200"
        >
          <div class="flex items-center justify-between">
            <div class="flex items-center gap-3">
              <div class="w-10 h-10 rounded-lg bg-indigo-50 flex items-center justify-center shrink-0">
                <i class="pi pi-sitemap text-indigo-500" />
              </div>
              <div>
                <p class="font-semibold text-sm text-slate-800">{{ group.name }}</p>
                <p class="text-xs text-slate-400 mt-0.5">Mã: {{ group.code }}</p>
              </div>
            </div>
            <button
              class="text-slate-400 hover:text-blue-500 transition-colors cursor-pointer bg-transparent border-none p-1.5"
              @click="openAssignDialog(group)"
              title="Gán nhân sự"
            >
              <i class="pi pi-user-plus text-sm" />
            </button>
          </div>
        </div>
      </div>
    </template>

    <!-- ==================== TAB: Chức Vụ ==================== -->
    <template v-if="activeTab === 'positions'">
      <div class="flex items-center justify-between">
        <p class="text-xs text-slate-400">{{ positions.length }} chức vụ</p>
        <button
          class="flex items-center gap-1.5 bg-blue-500 text-white text-sm font-semibold px-3.5 py-2 rounded-lg border-none cursor-pointer hover:bg-blue-600 active:scale-95"
          @click="openPositionDialog()"
        >
          <i class="pi pi-plus text-xs" /> Thêm
        </button>
      </div>

      <div v-if="isLoading" class="flex justify-center py-12">
        <i class="pi pi-spin pi-spinner text-2xl text-blue-500" />
      </div>

      <div v-else class="flex flex-col gap-2.5">
        <div v-if="positions.length === 0" class="text-center py-12 text-slate-400 text-sm">
          <i class="pi pi-id-card text-3xl mb-2 block" />
          Chưa có chức vụ nào
        </div>

        <div
          v-for="pos in positions"
          :key="pos.id"
          class="bg-white rounded-xl px-4 py-3.5 shadow-sm border border-slate-200 flex items-center gap-3"
        >
          <div class="w-10 h-10 rounded-lg bg-amber-50 flex items-center justify-center shrink-0">
            <i class="pi pi-id-card text-amber-500" />
          </div>
          <p class="font-semibold text-sm text-slate-800">{{ pos.name }}</p>
        </div>
      </div>
    </template>

    <!-- ==================== DIALOG: Tạo Phòng Ban ==================== -->
    <div v-if="showGroupDialog" class="fixed inset-0 z-[100] flex items-end sm:items-center justify-center">
      <div class="absolute inset-0 bg-black/50" @click="showGroupDialog = false" />
      <div class="relative bg-white w-full sm:max-w-md sm:rounded-2xl rounded-t-2xl p-6 z-10">
        <div class="flex items-center justify-between mb-5">
          <h3 class="text-base font-bold text-slate-800">Thêm phòng ban</h3>
          <button class="text-slate-400 hover:text-slate-600 bg-transparent border-none cursor-pointer" @click="showGroupDialog = false">
            <i class="pi pi-times" />
          </button>
        </div>

        <form class="flex flex-col gap-4" @submit.prevent="handleCreateGroup">
          <div class="flex flex-col gap-1.5">
            <label class="text-xs font-medium text-slate-600">Tên phòng ban *</label>
            <input v-model="groupForm.name" type="text" required class="w-full px-3 py-2.5 border border-slate-200 rounded-lg text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-500/10" />
          </div>
          <div class="flex flex-col gap-1.5">
            <label class="text-xs font-medium text-slate-600">Mã phòng ban *</label>
            <input v-model="groupForm.code" type="text" required class="w-full px-3 py-2.5 border border-slate-200 rounded-lg text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-500/10" placeholder="VD: PB-KT" />
          </div>

          <button
            type="submit"
            class="w-full py-2.5 bg-blue-500 text-white text-sm font-semibold rounded-lg border-none cursor-pointer hover:bg-blue-600 disabled:opacity-50"
            :disabled="isSaving"
          >
            {{ isSaving ? 'Đang lưu...' : 'Tạo phòng ban' }}
          </button>
        </form>
      </div>
    </div>

    <!-- ==================== DIALOG: Tạo Chức Vụ ==================== -->
    <div v-if="showPositionDialog" class="fixed inset-0 z-[100] flex items-end sm:items-center justify-center">
      <div class="absolute inset-0 bg-black/50" @click="showPositionDialog = false" />
      <div class="relative bg-white w-full sm:max-w-md sm:rounded-2xl rounded-t-2xl p-6 z-10">
        <div class="flex items-center justify-between mb-5">
          <h3 class="text-base font-bold text-slate-800">Thêm chức vụ</h3>
          <button class="text-slate-400 hover:text-slate-600 bg-transparent border-none cursor-pointer" @click="showPositionDialog = false">
            <i class="pi pi-times" />
          </button>
        </div>

        <form class="flex flex-col gap-4" @submit.prevent="handleCreatePosition">
          <div class="flex flex-col gap-1.5">
            <label class="text-xs font-medium text-slate-600">Tên chức vụ *</label>
            <input v-model="positionForm.name" type="text" required class="w-full px-3 py-2.5 border border-slate-200 rounded-lg text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-500/10" placeholder="VD: Trưởng phòng" />
          </div>

          <button
            type="submit"
            class="w-full py-2.5 bg-blue-500 text-white text-sm font-semibold rounded-lg border-none cursor-pointer hover:bg-blue-600 disabled:opacity-50"
            :disabled="isSaving"
          >
            {{ isSaving ? 'Đang lưu...' : 'Tạo chức vụ' }}
          </button>
        </form>
      </div>
    </div>

    <!-- ==================== DIALOG: Gán nhân sự vào phòng ban ==================== -->
    <div v-if="showAssignDialog" class="fixed inset-0 z-[100] flex items-end sm:items-center justify-center">
      <div class="absolute inset-0 bg-black/50" @click="showAssignDialog = false" />
      <div class="relative bg-white w-full sm:max-w-md sm:rounded-2xl rounded-t-2xl p-6 z-10">
        <div class="flex items-center justify-between mb-5">
          <div>
            <h3 class="text-base font-bold text-slate-800">Gán nhân sự</h3>
            <p class="text-xs text-slate-400 mt-0.5">{{ assigningGroup?.name }}</p>
          </div>
          <button class="text-slate-400 hover:text-slate-600 bg-transparent border-none cursor-pointer" @click="showAssignDialog = false">
            <i class="pi pi-times" />
          </button>
        </div>

        <form class="flex flex-col gap-4" @submit.prevent="handleAssign">
          <div class="flex flex-col gap-1.5">
            <label class="text-xs font-medium text-slate-600">Người dùng *</label>
            <select v-model="assignForm.userId" required class="w-full px-3 py-2.5 border border-slate-200 rounded-lg text-sm outline-none focus:border-blue-500 bg-white">
              <option value="" disabled>-- Chọn người dùng --</option>
              <option v-for="u in allUsers" :key="u.id" :value="u.id">{{ u.fullName }} ({{ u.username }})</option>
            </select>
          </div>
          <div class="flex flex-col gap-1.5">
            <label class="text-xs font-medium text-slate-600">Chức vụ *</label>
            <select v-model="assignForm.positionId" required class="w-full px-3 py-2.5 border border-slate-200 rounded-lg text-sm outline-none focus:border-blue-500 bg-white">
              <option value="" disabled>-- Chọn chức vụ --</option>
              <option v-for="p in positions" :key="p.id" :value="p.id">{{ p.name }}</option>
            </select>
          </div>

          <button
            type="submit"
            class="w-full py-2.5 bg-emerald-500 text-white text-sm font-semibold rounded-lg border-none cursor-pointer hover:bg-emerald-600 disabled:opacity-50"
            :disabled="isSaving"
          >
            {{ isSaving ? 'Đang gán...' : 'Gán vào phòng ban' }}
          </button>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import {
  getGroups, createGroup,
  getPositions, createPosition,
  getUsers, assignUserToGroup,
} from '../../features/admin/admin.api'
import type { GroupDto, CreateGroupDto, PositionDto, CreatePositionDto, UserDto, AssignUserGroupDto } from '../../features/admin/admin.types'
import { toastService } from '../../utils/toastService'

// ---- Tab ----
const tabs = [
  { key: 'groups', label: 'Phòng ban', icon: 'pi pi-sitemap' },
  { key: 'positions', label: 'Chức vụ', icon: 'pi pi-id-card' },
] as const

const activeTab = ref<'groups' | 'positions'>('groups')

// ---- Data ----
const groups = ref<GroupDto[]>([])
const positions = ref<PositionDto[]>([])
const allUsers = ref<UserDto[]>([])
const isLoading = ref(false)
const isSaving = ref(false)

// ---- Dialogs ----
const showGroupDialog = ref(false)
const showPositionDialog = ref(false)
const showAssignDialog = ref(false)
const assigningGroup = ref<GroupDto | null>(null)

const groupForm = ref<CreateGroupDto>({ name: '', code: '' })
const positionForm = ref<CreatePositionDto>({ name: '' })
const assignForm = ref<AssignUserGroupDto>({ userId: '', groupId: '', positionId: '' })

// ---- Load ----
async function loadAll() {
  isLoading.value = true
  try {
    const [g, p, u] = await Promise.all([getGroups(), getPositions(), getUsers()])
    groups.value = g
    positions.value = p
    allUsers.value = u
  } catch {
    // Interceptor đã toast lỗi
  } finally {
    isLoading.value = false
  }
}

// ---- Group CRUD ----
function openGroupDialog() {
  groupForm.value = { name: '', code: '' }
  showGroupDialog.value = true
}

async function handleCreateGroup() {
  isSaving.value = true
  try {
    await createGroup(groupForm.value)
    toastService.success('Đã tạo phòng ban mới.')
    showGroupDialog.value = false
    await loadAll()
  } catch { /* interceptor */ } finally { isSaving.value = false }
}

// ---- Position CRUD ----
function openPositionDialog() {
  positionForm.value = { name: '' }
  showPositionDialog.value = true
}

async function handleCreatePosition() {
  isSaving.value = true
  try {
    await createPosition(positionForm.value)
    toastService.success('Đã tạo chức vụ mới.')
    showPositionDialog.value = false
    await loadAll()
  } catch { /* interceptor */ } finally { isSaving.value = false }
}

// ---- Assign User to Group ----
function openAssignDialog(group: GroupDto) {
  assigningGroup.value = group
  assignForm.value = { userId: '', groupId: group.id, positionId: '' }
  showAssignDialog.value = true
}

async function handleAssign() {
  isSaving.value = true
  try {
    await assignUserToGroup(assignForm.value)
    toastService.success(`Đã gán nhân sự vào ${assigningGroup.value?.name}.`)
    showAssignDialog.value = false
  } catch { /* interceptor */ } finally { isSaving.value = false }
}

onMounted(loadAll)
</script>
