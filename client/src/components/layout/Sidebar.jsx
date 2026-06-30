import { NavLink } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext.jsx';
import { useLanguage } from '../../context/LanguageContext.jsx';
import { useTheme } from '../../context/ThemeContext.jsx';
import { ROLES } from '../../utils/constants';

const NAV = {
  [ROLES.MANAGER]: [
    { to: '/manager', labelKey: 'navDashboard', end: true },
    { to: '/manager/tickets', labelKey: 'navAllTickets' },
    { to: '/manager/employees', labelKey: 'navEmployees' },
    { to: '/manager/clients', labelKey: 'navClients' },
    { to: '/manager/products', labelKey: 'navProducts' },
  ],
  [ROLES.CLIENT]: [
    { to: '/client', labelKey: 'navMyTickets', end: true },
    { to: '/client/new', labelKey: 'navNewTicket' },
  ],
  [ROLES.EMPLOYEE]: [
    { to: '/employee', labelKey: 'navAssignedTickets', end: true },
  ],
};

function NavItem({ to, end, onClose, colors, children }) {
  return (
    <NavLink
      to={to}
      end={end}
      onClick={onClose}
      style={({ isActive }) => ({
        display: 'block',
        borderRadius: 8,
        padding: '10px 12px',
        fontSize: 14,
        fontWeight: 500,
        transition: 'background-color 0.15s, color 0.15s',
        backgroundColor: isActive ? colors.pillBg : 'transparent',
        color: isActive ? colors.pillText : colors.textSecondary,
      })}
      onMouseEnter={(e) => {
        if (e.currentTarget.style.backgroundColor === 'transparent') {
          e.currentTarget.style.backgroundColor = colors.bgCardHover;
        }
      }}
      onMouseLeave={(e) => {
        if (e.currentTarget.getAttribute('aria-current') !== 'page') {
          e.currentTarget.style.backgroundColor = 'transparent';
        }
      }}
    >
      {children}
    </NavLink>
  );
}

export default function Sidebar({ open, onClose }) {
  const { user } = useAuth();
  const { t, isRTL } = useLanguage();
  const { colors } = useTheme();
  const items = NAV[user?.role] || [];

  return (
    <aside
      dir={isRTL ? 'rtl' : 'ltr'}
      className={`fixed left-0 top-0 z-40 flex h-screen w-[260px] max-w-[80vw] shrink-0 flex-col transition-transform duration-300 lg:relative lg:w-72 lg:max-w-none lg:translate-x-0 ${
        open ? 'translate-x-0' : '-translate-x-full'
      }`}
      style={{
        backgroundColor: colors.bgCard,
        borderRight: `1px solid ${colors.border}`,
      }}
    >
      <div className="flex items-center gap-2 px-5 py-5 lg:px-6 lg:py-6">
        <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-lg bg-brand-500 text-lg font-bold text-white">
          T
        </div>
        <div className="min-w-0">
          <p className="truncate text-sm font-semibold" style={{ color: colors.textPrimary }}>
            {t('appName')}
          </p>
          <p className="truncate text-xs" style={{ color: colors.textMuted }}>
            {user?.role} portal
          </p>
        </div>
      </div>

      <nav className="flex-1 space-y-1 overflow-y-auto px-3 lg:px-4">
        <p
          className="mb-2 px-2 text-xs font-medium uppercase tracking-wide"
          style={{ color: colors.textMuted }}
        >
          {t('menu')}
        </p>
        {items.map((item) => (
          <NavItem key={item.to} to={item.to} end={item.end} onClose={onClose} colors={colors}>
            {t(item.labelKey)}
          </NavItem>
        ))}

        {/* SETTINGS LINK - available to all roles */}
        <div className="my-2 border-t" style={{ borderColor: colors.border }} />
        <NavItem to="/settings" onClose={onClose} colors={colors}>
          ⚙️ {t('navSettings')}
        </NavItem>
      </nav>
    </aside>
  );
}