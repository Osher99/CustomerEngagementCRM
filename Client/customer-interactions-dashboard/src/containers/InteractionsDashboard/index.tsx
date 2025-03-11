// src/components/InteractionsDashboard.tsx
import React from 'react';
import { useInteractionsDashboard } from './hooks/useInteractionsDashboard';
import FilterRow from './FilterRow';
import PaginationRow from './PaginationRow';
import Chart from './Chart';
import { InteractionPieChart } from './PieChart';

const InteractionsDashboard: React.FC = () => {
  const {
    loading, error, currentPageNumber, onSetNextPage, onSetPrevPage, paginatedResult
}  = useInteractionsDashboard();

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div className="w-full flex flex-col home">
        <main className="flex-1 flex flex-col items-center justify-center bg-gray-100 p-4">
        <div className="w-full flex justify-center dashboard">
            <FilterRow />
            <Chart />
            <hr/>
            <InteractionPieChart />
            <PaginationRow page={currentPageNumber} onNext={() => onSetNextPage()} onPrev={onSetPrevPage} paginatedResult={paginatedResult}  />
        </div>
        </main>
    </div>
  );
};

export default InteractionsDashboard;
