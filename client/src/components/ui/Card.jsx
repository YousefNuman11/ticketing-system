export default function Card({ children, className = '' }) {
  return (
    <div className={`rounded-2xl border border-gray-200 bg-white p-5 shadow-theme-sm ${className}`}>
      {children}
    </div>
  );
}
