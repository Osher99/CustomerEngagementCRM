import { useQuery, useQueryClient } from '@tanstack/react-query';
import axios from 'axios';
import { useDispatch } from 'react-redux';
import { setInteractions, setFilteredInteractions } from '../redux/interactionsSlice';
import { Filters } from '../interfaces/types';
import { PaginatedResult, CustomerInteraction } from '../interfaces/interfaces';
import { useEffect } from 'react';

const fetchInteractions = async (pageNumber: number, pageSize?: number, filters?: Filters) => {
    const { data } = await axios.get(`${import.meta.env.REACT_APP_API_URL || 'https://localhost:7095'}/api/interactions`, {
      params: {
        pageNumber: pageNumber,
        pageSize: pageSize,
        startDate: filters?.startDate,
        endDate: filters?.endDate,
        interactionType: filters?.interactionType,
        outcome: filters?.outcome,
      },
    });
    return data;
  };

export const useInteractions = (pageNumber: number, pageSize?: number, filters?: Filters) => {
  const dispatch = useDispatch();

  const query = useQuery({
      queryKey: ['interactions', pageNumber, pageSize, filters],
      queryFn: () => fetchInteractions(pageNumber, pageSize, filters),
      placeholderData: (previousData) => previousData,
      staleTime: 1000 * 60 * 5,
  });

  useEffect(() => {
      if (!query.data) return;
      if (filters && Object.values(filters).some(val => val)) {
          dispatch(setFilteredInteractions({ filters, pageNumber, interactions: query.data }));
      } else {
          dispatch(setFilteredInteractions({ filters: undefined, pageNumber: undefined, interactions: undefined }));
          dispatch(setInteractions(query.data));
      }
  }, [query.data, filters, pageNumber, dispatch]); 

  return query;
};