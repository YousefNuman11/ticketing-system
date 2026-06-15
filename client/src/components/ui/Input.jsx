export default function Input({ label, error, className = '', ...props }) {
  return (
    <div className={className}>
      {label && <label className="form-label">{label}</label>}
      <input className="form-input" {...props} />
      {error && <p className="mt-1 text-xs text-error-500">{error}</p>}
    </div>
  );
}
