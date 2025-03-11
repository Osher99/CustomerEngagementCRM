import { useMemo } from "react";
import { useSelector } from "react-redux";
import { createFilteredKey } from "../../../../utils";
import { RootState } from "../../../../redux/store";

export const useInteractionTypesData = (dataType: "types" | "outcomes" = "outcomes") => {
    const { interactions, filtered } = useSelector((state: RootState) => state.interactions);
    const filters = useSelector((state: RootState) => state.filters);
    const filterKey = createFilteredKey(filters, 1);
    
    const interactionData = useMemo(() => {
        const typeCounts: Record<string, number> = {};
        const outcomeCounts: Record<string, number> = {};

        const dataSource = filtered[filterKey] || interactions;

        Object.values(dataSource).forEach(page => {
            page.items.forEach((interaction: { interactionType: string | number; outcome: string | number }) => {
                typeCounts[interaction.interactionType] =
                    (typeCounts[interaction.interactionType] || 0) + 1;

                outcomeCounts[interaction.outcome] =
                    (outcomeCounts[interaction.outcome] || 0) + 1;
            });
        });

        const types = Object.entries(typeCounts).map(([type, count]) => ({
            label: `Type: ${type}`,
            name: type,
            value: count,
        }));

        const outcomes = Object.entries(outcomeCounts).map(([outcome, count]) => ({
            label: `Outcome: ${outcome}`,
            name: outcome,
            value: count,
        }));

        return dataType === "types" ? types : outcomes;
    }, [interactions, filtered, filterKey, dataType]);

    return interactionData;
};
