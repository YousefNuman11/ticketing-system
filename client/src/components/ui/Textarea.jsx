export default function Textarea({ label, className = '', rows = 4, ...props }) {
  return (
    <div className={className}>
      {label && <label className="form-label">{label}</label>}
      <textarea
        rows={rows}
        className="w-full rounded-lg border border-gray-300 bg-white px-4 py-2.5 text-sm text-gray-800 shadow-theme-sm outline-none transition focus:border-brand-400 focus:ring-2 focus:ring-brand-100"
        {...props}
      />
    </div>
  );
}
