import { useDispatch } from "react-redux";
import { setFilters } from "../../../../redux/filtersSlice";
import { useForm } from "react-hook-form";
import { FilterFormValues } from "../../../../interfaces/interfaces";
import { setCurrentPageNumber } from "../../../../redux/interactionsSlice";

export const useFilter = () => {
  const { register, handleSubmit, setValue, watch, reset } = useForm<FilterFormValues>({
    defaultValues: {
      startDate: "",
      endDate: "",
      interactionType: "",
      outcome: "",
    },
  });

  const formValues = watch();
  const dispatch = useDispatch();

  const handleFilterChange = () => {
    dispatch(setFilters(formValues));
    dispatch(setCurrentPageNumber(1));
  };

  const clearFilters = () => {
      dispatch(setFilters({
          startDate: '',
          endDate: '',
          interactionType: '',
          outcome: '',
      }));
      dispatch(setCurrentPageNumber(1));
  };

    return {
      handleSubmit,
      setValue,
      reset,
      register,
      handleFilterChange,
      formValues,
      clearFilters
    }
}