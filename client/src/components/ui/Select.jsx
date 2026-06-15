export default function Select({ label, children, className = '', ...props }) {
  return (
    <div className={className}>
      {label && <label className="form-label">{label}</label>}
      <select className="form-input appearance-none" {...props}>
        {children}
      </select>
    </div>
  );
}
