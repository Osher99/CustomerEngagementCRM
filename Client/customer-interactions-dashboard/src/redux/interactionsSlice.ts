import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { CustomerInteraction, PaginatedResult } from '../interfaces/interfaces';
import { createFilteredKey } from '../utils';
import { Filters } from '../interfaces/types';

interface InteractionsState {
    interactions: {
      [pageNumber: number]: PaginatedResult<CustomerInteraction>;
    };
    filtered: {
      [filterKey: string]: { [pageNumber: number]: PaginatedResult<CustomerInteraction> };
    };
    currentPageNumber: number;
    loading: boolean;
    error: string | null;
  }
  
  const initialState: InteractionsState = {
    interactions: {},
    filtered: {},
    currentPageNumber: 1,
    loading: false,
    error: null,
  };

const interactionsSlice = createSlice({
  name: 'interactions',
  initialState,
  reducers: {
    setInteractions: (state, action: PayloadAction<PaginatedResult<CustomerInteraction>>) => {
        const { currentPage } = action.payload;
        state.interactions[currentPage] = action.payload;
      },
      setFilteredInteractions: (
        state,
        action: PayloadAction<{ filters?: Filters; pageNumber?: number; interactions?: PaginatedResult<CustomerInteraction> }>
      ) => {
        const { filters, pageNumber, interactions } = action.payload;
      
        if (!filters || !pageNumber || !interactions) {
          state.filtered = {};
          return;
        }
      
        const key = createFilteredKey(filters, pageNumber);
        if (!state.filtered[key]) {
          state.filtered[key] = {};
        }
        state.filtered[key][pageNumber] = interactions;
      },
    setLoading: (state, action: PayloadAction<boolean>) => {
      state.loading = action.payload;
    },
    setError: (state, action: PayloadAction<string>) => {
      state.error = action.payload;
    },
    setCurrentPageNumber: (state, action: PayloadAction<number>) => {
      state.currentPageNumber = action.payload;
    },
  },
});

export const { setInteractions, setLoading, setError, setFilteredInteractions, setCurrentPageNumber } = interactionsSlice.actions;

export default interactionsSlice.reducer;