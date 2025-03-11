import { useMemo } from "react";
import { useSelector } from "react-redux";
import { createFilteredKey } from "../../../../utils";
import { RootState } from "../../../../redux/store";

export const useChart = () => {
  const { interactions, filtered, currentPageNumber } = useSelector((state: RootState) => state.interactions);
  const filters = useSelector((state: RootState) => state.filters);

  const filteredKey = createFilteredKey(filters, currentPageNumber);
  const relevantData = filtered[filteredKey] 
    ? Object.values(filtered[filteredKey])
        .filter(item => item.currentPage === currentPageNumber)
        .flatMap(page => page.items)
    : Object.values(interactions)
        .filter(item => item.currentPage === currentPageNumber)
        .flatMap(page => page.items);

  return useMemo(() => {
    const groupedData: Record<string, { count: number; notes: string[] }> = {};

    relevantData.forEach(({ interactionDate, notes }) => {
      const date = new Date(interactionDate).toISOString().split("T")[0];
      if (!groupedData[date]) {
        groupedData[date] = { count: 0, notes: [] };
      }
      groupedData[date].count += 1;
      if (notes) groupedData[date].notes.push(notes);
    });

    return Object.entries(groupedData).map(([date, { count, notes }]) => ({ date, count, notes }));
  }, [relevantData]);
};
