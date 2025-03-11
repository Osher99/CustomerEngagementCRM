import { configureStore } from '@reduxjs/toolkit';
import interactionsReducer from './interactionsSlice';
import filtersReducer from './filtersSlice';

const store = configureStore({
    reducer: {
      interactions: interactionsReducer,
      filters: filtersReducer
    },
  });

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
export default store;