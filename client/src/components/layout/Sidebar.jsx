import { NavLink } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext.jsx';
import { ROLES } from '../../utils/constants';

const NAV = {
  [ROLES.MANAGER]: [
    { to: '/manager', label: 'Dashboard', end: true },
    { to: '/manager/tickets', label: 'All Tickets' },
    { to: '/manager/employees', label: 'Employees' },
    { to: '/manager/clients', label: 'Clients' },
    { to: '/manager/products', label: 'Products' },
  ],
  [ROLES.CLIENT]: [
    { to: '/client', label: 'My Tickets', end: true },
    { to: '/client/new', label: 'New Ticket' },
  ],
  [ROLES.EMPLOYEE]: [
    { to: '/employee', label: 'Assigned Tickets', end: true },
  ],
};

export default function Sidebar({ open }) {
  const { user } = useAuth();
  const items = NAV[user?.role] || [];

  return (
    <aside
      className={`fixed left-0 top-0 z-40 flex h-screen w-72 flex-col border-r border-gray-200 bg-white transition-transform duration-300 lg:static lg:translate-x-0 ${
        open ? 'translate-x-0' : '-translate-x-full'
      }`}
    >
      <div className="flex items-center gap-2 px-6 py-6">
        <div className="flex h-9 w-9 items-center justify-center rounded-lg bg-brand-500 text-lg font-bold text-white">
          T
        </div>
        <div>
          <p className="text-sm font-semibold text-gray-800">Support Desk</p>
          <p className="text-xs text-gray-400">{user?.role} portal</p>
        </div>
      </div>

      <nav className="flex-1 space-y-1 px-4">
        <p className="mb-2 px-2 text-xs font-medium uppercase tracking-wide text-gray-400">
          Menu
        </p>
        {items.map((item) => (
          <NavLink
            key={item.to}
            to={item.to}
            end={item.end}
            className={({ isActive }) =>
              `block rounded-lg px-3 py-2.5 text-sm font-medium transition ${
                isActive
                  ? 'bg-brand-50 text-brand-600'
                  : 'text-gray-600 hover:bg-gray-50'
              }`
            }
          >
            {item.label}
          </NavLink>
        ))}
      </nav>
    </aside>
  );
}
