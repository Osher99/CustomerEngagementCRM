// src/components/Filter/Filter.tsx
import React from 'react';
import DatePicker from '../../../components/DatePicker';
import SelectType from '../../../components/SelectType';
import Button from '../../../components/Button';
import { useFilter } from './hooks/useFilter';


const Filter: React.FC = () => {
  const { handleSubmit, formValues, handleFilterChange, setValue, register, reset, clearFilters } = useFilter();

  return (
    <form
      onSubmit={handleSubmit(() => handleFilterChange())}
      className="bg-white p-6 rounded-lg shadow-md w-full max-w-lg mx-auto filter-row"
    >
      <DatePicker
        label="Start Date"
        value={formValues.startDate}
        onChange={(value) => setValue("startDate", value)}
      />
      <DatePicker
        label="End Date"
        value={formValues.endDate}
        onChange={(value) => setValue("endDate", value)}
      />
      <SelectType
        value={formValues.interactionType}
        onChange={(value) => setValue("interactionType", value)}
      />
      <input
        type="text"
        placeholder="Outcome"
        value={formValues.outcome}
        {...register("outcome")}
        className="border border-black rounded-lg shadow-sm focus:ring-2 focus:ring-blue-500 focus:border-blue-500 text-gray-700 px-3 py-2 h-50%  mt-2 text-center"
        />
      <div className='max-w-lg rounded-lg'>
        <Button type="submit" label="Apply Filters" className="w-full bg-blue-500 text-white py-2 px-6 rounded-md hover:bg-blue-600 transition duration-300 shadow-md" />
        <hr />
        <Button 
        type="button"
        className="w-full bg-gray-400 text-white py-2 px-6 rounded-md hover:bg-gray-500 transition duration-300 shadow-md"
        onClick={() => {
          reset();
          clearFilters();
        }} label="Clear Filters" />
      </div>
    </form>
  );
};

export default Filter;