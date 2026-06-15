import { Link } from 'react-router-dom';

export default function NotFound() {
  return (
    <div className="flex h-screen flex-col items-center justify-center gap-4 text-center">
      <h1 className="text-5xl font-bold text-brand-500">404</h1>
      <p className="text-gray-500">The page you are looking for does not exist.</p>
      <Link to="/" className="rounded-lg bg-brand-500 px-5 py-2.5 text-sm font-medium text-white">
        Back home
      </Link>
    </div>
  );
}
