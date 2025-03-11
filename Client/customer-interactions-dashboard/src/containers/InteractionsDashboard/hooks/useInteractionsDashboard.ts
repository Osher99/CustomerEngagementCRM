import { useEffect, useMemo } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useInteractions } from "../../../hooks/useInteractions";
import { RootState } from "../../../redux/store";
import { createFilteredKey } from "../../../utils";
import { setCurrentPageNumber } from "../../../redux/interactionsSlice";

export const useInteractionsDashboard = () => {
    const { interactions, loading, error, filtered, currentPageNumber } = useSelector((state: RootState) => state.interactions);
    const filters = useSelector((state: any) => state.filters);
    const dispatch = useDispatch();
    
    const { data, isLoading, isError, refetch } = useInteractions(currentPageNumber, 20, filters);

    useEffect(() => {
        if (data) {
            dispatch({ type: "interactions/setInteractions", payload: data });
        }
    }, [data, dispatch]);

    const onSetPrevPage = () => {
        dispatch(setCurrentPageNumber(currentPageNumber-1));
    };

    const onSetNextPage = () => {
        dispatch(setCurrentPageNumber(currentPageNumber+1));
    };

    const getFiltersKey = useMemo(() => {
        return createFilteredKey(filters, currentPageNumber);
    }, [filters, currentPageNumber]);

    const getCurrentPaginatedResult = useMemo(() => {
        if (!filtered || !interactions) return undefined;
        if (!filtered[getFiltersKey] || !filtered[getFiltersKey][currentPageNumber]) {
            return interactions[currentPageNumber] || undefined;
        }
        return filtered[getFiltersKey][currentPageNumber];
    }, [filtered, interactions, getFiltersKey, currentPageNumber]);

    
    return {
        loading,
        error,
        currentPageNumber,
        isLoading,
        isError,
        onSetPrevPage,
        onSetNextPage,
        paginatedResult: getCurrentPaginatedResult
    };
};
