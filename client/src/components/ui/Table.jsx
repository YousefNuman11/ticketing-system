export function Table({ columns, children }) {
  return (
    <div className="overflow-x-auto rounded-2xl border border-gray-200 bg-white shadow-theme-sm">
      <table className="min-w-full text-left text-sm">
        <thead className="border-b border-gray-200 bg-gray-50 text-xs font-medium uppercase tracking-wide text-gray-500">
          <tr>
            {columns.map((c) => (
              <th key={c} className="px-5 py-3.5">{c}</th>
            ))}
          </tr>
        </thead>
        <tbody className="divide-y divide-gray-100">{children}</tbody>
      </table>
    </div>
  );
}

export function EmptyRow({ colSpan, message = 'No records found' }) {
  return (
    <tr>
      <td colSpan={colSpan} className="px-5 py-10 text-center text-gray-400">
        {message}
      </td>
    </tr>
  );
}
