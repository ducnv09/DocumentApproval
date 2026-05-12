import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { DocumentDto, DocumentStatus } from '../features/document/document.types'

// ============================================================
// DOCUMENT STORE - Cache danh sách tờ trình đang xem
// ============================================================
export const useDocumentStore = defineStore('document', () => {
  // ---- State ----
  const documents = ref<DocumentDto[]>([])
  const currentDocument = ref<DocumentDto | null>(null)
  const filterStatus = ref<DocumentStatus | 'All'>('All')

  // ---- Getters ----
  const filteredDocuments = computed(() => {
    if (filterStatus.value === 'All') return documents.value
    return documents.value.filter(doc => doc.status === filterStatus.value)
  })

  const documentCount = computed(() => documents.value.length)

  const pendingCount = computed(() => 
    documents.value.filter(doc => doc.status === 'Pending').length
  )

  // ---- Actions ----

  function setDocuments(docs: DocumentDto[]): void {
    documents.value = docs
  }

  function setCurrentDocument(doc: DocumentDto | null): void {
    currentDocument.value = doc
  }

  function setFilterStatus(status: DocumentStatus | 'All'): void {
    filterStatus.value = status
  }

  function updateDocumentInList(updatedDoc: DocumentDto): void {
    const index = documents.value.findIndex(d => d.id === updatedDoc.id)
    if (index !== -1) {
      documents.value[index] = updatedDoc
    }
  }

  function removeDocumentFromList(docId: string): void {
    documents.value = documents.value.filter(d => d.id !== docId)
  }

  function clearDocuments(): void {
    documents.value = []
    currentDocument.value = null
  }

  return {
    // State
    documents,
    currentDocument,
    filterStatus,
    // Getters
    filteredDocuments,
    documentCount,
    pendingCount,
    // Actions
    setDocuments,
    setCurrentDocument,
    setFilterStatus,
    updateDocumentInList,
    removeDocumentFromList,
    clearDocuments,
  }
})