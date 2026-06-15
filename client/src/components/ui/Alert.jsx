export default function Alert({ type = 'error', children }) {
  if (!children) return null;
  const styles = {
    error: 'border-error-100 bg-error-50 text-error-700',
    success: 'border-success-100 bg-success-50 text-success-700',
    info: 'border-brand-100 bg-brand-50 text-brand-700',
  };
  return (
    <div className={`mb-4 rounded-lg border px-4 py-3 text-sm ${styles[type]}`}>
      {children}
    </div>
  );
}
