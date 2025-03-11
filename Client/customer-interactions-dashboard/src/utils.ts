import { Filters } from "./interfaces/types";

export const createFilteredKey = (filters?: Filters, pageNumber?: number) => {
    return `${filters?.startDate}-${filters?.endDate}-${filters?.interactionType}-${filters?.outcome}-${pageNumber}`;
};

export const isValueValid = (val: string | null | undefined): boolean => {
    return typeof val === 'string' && val.trim() !== '';
};