export default function Pagination({ pagination, onPageChange }) {
  if (!pagination || pagination.totalPages <= 1) return null;
  const { pageNumber, totalPages, totalCount } = pagination;
  return (
    <div className="mt-4 flex items-center justify-between text-sm text-gray-500">
      <span>
        Page {pageNumber} of {totalPages} · {totalCount} items
      </span>
      <div className="flex gap-2">
        <button
          onClick={() => onPageChange(pageNumber - 1)}
          disabled={pageNumber <= 1}
          className="rounded-lg border border-gray-300 px-3 py-1.5 disabled:opacity-40"
        >
          Previous
        </button>
        <button
          onClick={() => onPageChange(pageNumber + 1)}
          disabled={pageNumber >= totalPages}
          className="rounded-lg border border-gray-300 px-3 py-1.5 disabled:opacity-40"
        >
          Next
        </button>
      </div>
    </div>
  );
}
