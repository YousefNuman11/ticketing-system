import { createContext, useContext, useState, useEffect } from 'react';

const ThemeContext = createContext(null);

// Color tokens for both modes. Components read these instead of hardcoded hex.
const LIGHT = {
  mode: 'light',
  bgPage: '#f0f4ff',
  bgCard: '#ffffff',
  bgCardHover: '#f8fafc',
  bgSecondary: '#f8fafc',
  border: 'rgba(0,0,0,0.07)',
  borderLight: 'rgba(0,0,0,0.05)',
  borderInput: 'rgba(0,0,0,0.1)',
  textPrimary: '#1e293b',
  textSecondary: '#64748b',
  textMuted: '#94a3b8',
  inputBg: '#f8fafc',
  pillBg: '#f0f4ff',
  pillText: '#1e40af',
  brandPrimary: '#1e40af',
  brandSecondary: '#3b82f6',
  brandGradient: 'linear-gradient(135deg, #1e40af 0%, #3b82f6 100%)',
  brandShadow: 'rgba(30,64,175,0.3)',
  successBg: '#dcfce7',
  successText: '#15803d',
  successDot: '#16a34a',
  dangerBg: '#fee2e2',
  dangerText: '#b91c1c',
  dangerDot: '#dc2626',
  tableHeaderBg: '#f8fafc',
};

const DARK = {
  mode: 'dark',
  bgPage: '#0f1117',
  bgCard: '#171a23',
  bgCardHover: '#1e222c',
  bgSecondary: '#1c2030',
  border: 'rgba(255,255,255,0.08)',
  borderLight: 'rgba(255,255,255,0.06)',
  borderInput: 'rgba(255,255,255,0.12)',
  textPrimary: '#e8eaf0',
  textSecondary: '#9aa3b8',
  textMuted: '#6b7384',
  inputBg: '#1c2030',
  pillBg: '#1e2640',
  pillText: '#7fa8ff',
  brandPrimary: '#3b6cf0',
  brandSecondary: '#5e8bf5',
  brandGradient: 'linear-gradient(135deg, #3b6cf0 0%, #5e8bf5 100%)',
  brandShadow: 'rgba(59,108,240,0.35)',
  successBg: '#0f2a1c',
  successText: '#4ade80',
  successDot: '#22c55e',
  dangerBg: '#2a1414',
  dangerText: '#f87171',
  dangerDot: '#ef4444',
  tableHeaderBg: '#1c2030',
};

export function ThemeProvider({ children }) {
  const [mode, setMode] = useState(() => {
    try {
      return localStorage.getItem('theme') || 'light';
    } catch {
      return 'light';
    }
  });

  useEffect(() => {
    try {
      localStorage.setItem('theme', mode);
    } catch {
      // ignore storage errors (e.g. private browsing)
    }
    document.documentElement.setAttribute('data-theme', mode);
  }, [mode]);

  const toggleTheme = () => setMode((m) => (m === 'light' ? 'dark' : 'light'));
  const setTheme = (m) => setMode(m === 'dark' ? 'dark' : 'light');

  const colors = mode === 'dark' ? DARK : LIGHT;

  return (
    <ThemeContext.Provider value={{ mode, colors, toggleTheme, setTheme }}>
      {children}
    </ThemeContext.Provider>
  );
}

export function useTheme() {
  const ctx = useContext(ThemeContext);
  if (!ctx) {
    throw new Error('useTheme must be used within a ThemeProvider');
  }
  return ctx;
}