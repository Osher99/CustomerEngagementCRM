// src/components/Filter/Input.tsx
import React from 'react';

interface InputProps {
  value: string;
  label: string;
  onChange: (value: string) => void;
  rest?: any
}

const Input: React.FC<InputProps> = ({ value, onChange, label, rest }) => {
  return (
    <div className="mb-4">
      <label className="block text-sm font-semibold text-gray-700">{label}</label>
      <input
        type="text"
        value={value}
        onChange={(e) => onChange(e.target.value)}
        className="mt-2 p-2 border border-gray-300 rounded-md w-full"
        {...rest}
      />
    </div>
  );
};

export default Input;