// src/components/Filter/Button.tsx
import React from 'react';

interface ButtonProps {
  onClick?: () => void;
  label: string;
  className?: string;
  disabled?: boolean;
  type?: "submit" | "reset" | "button" | undefined;
}

const Button: React.FC<ButtonProps> = ({ onClick, label, className, disabled, type }) => {
  const defaultClassName = "w-full bg-blue-500 text-white py-2 rounded-md hover:bg-blue-600 transition duration-300";
  return (
    <button
      onClick={() => onClick?.()}
      className={className ? className : defaultClassName}
      disabled={disabled}
      type={type}
    >
      {label}
    </button>
  );
};

export default Button;
