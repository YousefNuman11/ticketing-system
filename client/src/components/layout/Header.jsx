import { useState } from 'react';
import { useAuth } from '../../context/AuthContext.jsx';
import { useTheme } from '../../context/ThemeContext.jsx';
import { useLanguage } from '../../context/LanguageContext.jsx';

export default function Header({ onToggleSidebar }) {
  const { user, logout } = useAuth();
  const { colors } = useTheme();
  const { isRTL } = useLanguage();
  const [menu, setMenu] = useState(false);

  return (
    <header
      className="sticky top-0 z-30 flex h-16 items-center justify-between px-3 sm:px-4 lg:px-6"
      style={{
        backgroundColor: colors.bgCard,
        borderBottom: `1px solid ${colors.border}`,
      }}
    >
      <button
        onClick={onToggleSidebar}
        className="flex h-9 w-9 items-center justify-center rounded-lg lg:hidden"
        style={{ border: `1px solid ${colors.borderInput}`, color: colors.textSecondary }}
        aria-label="Toggle sidebar"
      >
        <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round">
          <line x1="3" y1="6" x2="21" y2="6" />
          <line x1="3" y1="12" x2="21" y2="12" />
          <line x1="3" y1="18" x2="21" y2="18" />
        </svg>
      </button>

      {/* Sits at "end" of row: right in English, left in Arabic */}
      <div className="flex items-center gap-3" style={{ marginInlineStart: 'auto' }}>
        <div className="relative">
          <button
            onClick={() => setMenu((v) => !v)}
            dir="ltr"
            className="flex items-center gap-2 rounded-lg px-1.5 py-1.5 sm:px-2"
            style={{ transition: 'background-color 0.15s' }}
            onMouseEnter={(e) => { e.currentTarget.style.backgroundColor = colors.bgCardHover; }}
            onMouseLeave={(e) => { e.currentTarget.style.backgroundColor = 'transparent'; }}
          >
            {/* Avatar - forced first via dir="ltr", always on the left of this pair */}
            <span
              className="flex h-9 w-9 shrink-0 items-center justify-center rounded-full text-sm font-semibold"
              style={{ backgroundColor: colors.pillBg, color: colors.pillText }}
            >
              {user?.fullName?.charAt(0)?.toUpperCase() || 'U'}
            </span>

            {/* Text - always on the right of the avatar */}
            <span className="hidden sm:block" style={{ textAlign: 'left' }}>
              <span className="block max-w-[140px] truncate text-sm font-medium" style={{ color: colors.textPrimary }}>
                {user?.fullName}
              </span>
              <span className="block max-w-[140px] truncate text-xs" style={{ color: colors.textMuted }}>
                {user?.email}
              </span>
            </span>
          </button>

          {menu && (
            <div
              className="absolute mt-2 w-56 max-w-[90vw] rounded-lg py-1"
              style={{
                backgroundColor: colors.bgCard,
                border: `1px solid ${colors.border}`,
                boxShadow: '0 4px 16px rgba(0,0,0,0.12)',
                insetInlineEnd: 0,
              }}
            >
              <div
                className="px-4 py-2 text-xs"
                style={{ borderBottom: `1px solid ${colors.borderLight}`, color: colors.textMuted }}
              >
                Signed in as {user?.role}
              </div>
              <button
                onClick={logout}
                className="block w-full px-4 py-2 text-sm"
                style={{ color: colors.dangerText, textAlign: isRTL ? 'right' : 'left' }}
                onMouseEnter={(e) => { e.currentTarget.style.backgroundColor = colors.bgCardHover; }}
                onMouseLeave={(e) => { e.currentTarget.style.backgroundColor = 'transparent'; }}
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