import React from "react";
import Button from "../../../components/Button";
import { CustomerInteraction, PaginatedResult } from "../../../interfaces/interfaces";

interface PaginationProps {
  page: number;
  paginatedResult: PaginatedResult<CustomerInteraction> | undefined;
  onNext: () => void;
  onPrev: () => void;
}

const PaginationRow: React.FC<PaginationProps> = ({
  page,
  paginatedResult,
  onNext,
  onPrev,
}) => {
  return (
    <div className="mt-4 text-center">
      <hr className="my-2" />
      <div className="page-data text-lg font-semibold">
        <span>עמוד {page} מתוך {paginatedResult?.totalPages}</span>
        <br />
        {paginatedResult && paginatedResult?.items?.length > 0 && (
          <span>מציג {paginatedResult?.items?.length} תוצאות מתוך {paginatedResult?.totalItems} סה"כ</span>
        )}
      </div>
      <hr className="my-2" />
      <div className="flex justify-center gap-4 mt-2">
      <Button
            className="px-4 py-2 border rounded bg-gray-200 hover:bg-gray-300 disabled:opacity-50"
            disabled={page === 1}
            onClick={onPrev}
            label="הקודם"
        />
      <Button
          className="px-4 py-2 border rounded bg-blue-500 text-white hover:bg-blue-600 disabled:opacity-50"
          disabled={page === paginatedResult?.totalPages}
            onClick={onNext}
            label="הבא"
        />
      </div>
    </div>
  );
};

export default PaginationRow;