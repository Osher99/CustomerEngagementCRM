import React from 'react';

interface SelectTypeProps {
  value: string;
  onChange: (value: string) => void;
}

const SelectType: React.FC<SelectTypeProps> = ({ value, onChange }) => {
  return (
    <div className="mb-4">
      <label className="block text-sm font-semibold text-gray-700">Interaction Type</label>
      <select
        value={value}
        onChange={(e) => onChange(e.target.value)}
        className="mt-2 p-2 border border-gray-300 rounded-md w-full"
      >
        <option value="">All Types</option>
        <option value="call">Call</option>
        <option value="email">Email</option>
      </select>
    </div>
  );
};

export default SelectType;