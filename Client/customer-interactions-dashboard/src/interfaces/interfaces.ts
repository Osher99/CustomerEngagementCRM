export interface CustomerInteraction {
    id: string;
    customerName: string;
    interactionType: string;
    interactionDate: string;
    outcome: string;
    notes?: string;
  }

  export interface PaginatedResult<T> {
    items: T[];
    totalPages: number;
    currentPage: number;
    pageSize: number;
    totalItems: number;
  }

  
 export interface FilterFormValues {
    startDate: string;
    endDate: string;
    interactionType: string;
    outcome: string;
  }