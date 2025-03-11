import { createSlice, PayloadAction } from "@reduxjs/toolkit";

interface FiltersState {
  startDate: string;
  endDate: string;
  interactionType: string;
  outcome: string;
}

const initialState: FiltersState = {
  startDate: "",
  endDate: "",
  interactionType: "",
  outcome: "",
};

const filtersSlice = createSlice({
  name: "filters",
  initialState,
  reducers: {
    setFilters: (state, action: PayloadAction<FiltersState>) => {
      return { ...state, ...action.payload };
    },
    resetFilters: () => initialState,
  },
});

export const { setFilters, resetFilters } = filtersSlice.actions;
export default filtersSlice.reducer;