import { useState } from 'react';
import { useAuth } from '../../context/AuthContext.jsx';

export default function Header({ onToggleSidebar }) {
  const { user, logout } = useAuth();
  const [menu, setMenu] = useState(false);

  return (
    <header className="sticky top-0 z-30 flex h-16 items-center justify-between border-b border-gray-200 bg-white px-4 lg:px-6">
      <button
        onClick={onToggleSidebar}
        className="rounded-lg border border-gray-200 p-2 text-gray-600 lg:hidden"
        aria-label="Toggle sidebar"
      >
        ☰
      </button>

      <div className="ml-auto flex items-center gap-3">
        <div className="relative">
          <button
            onClick={() => setMenu((v) => !v)}
            className="flex items-center gap-2 rounded-lg px-2 py-1.5 hover:bg-gray-50"
          >
            <span className="flex h-9 w-9 items-center justify-center rounded-full bg-brand-100 text-sm font-semibold text-brand-600">
              {user?.fullName?.charAt(0)?.toUpperCase() || 'U'}
            </span>
            <span className="hidden text-left sm:block">
              <span className="block text-sm font-medium text-gray-800">
                {user?.fullName}
              </span>
              <span className="block text-xs text-gray-400">{user?.email}</span>
            </span>
          </button>
          {menu && (
            <div className="absolute right-0 mt-2 w-48 rounded-lg border border-gray-200 bg-white py-1 shadow-theme-md">
              <div className="border-b border-gray-100 px-4 py-2 text-xs text-gray-400">
                Signed in as {user?.role}
              </div>
              <button
                onClick={logout}
                className="block w-full px-4 py-2 text-left text-sm text-error-600 hover:bg-gray-50"
              >
                Sign out
              </button>
            </div>
          )}
        </div>
      </div>
    </header>
  );
}
