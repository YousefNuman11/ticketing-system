import { useTheme } from '../context/ThemeContext.jsx';
import { useLanguage } from '../context/LanguageContext.jsx';

function ToggleSwitch({ checked, onChange, colors }) {
  return (
    <button
      onClick={onChange}
      style={{
        width: 48,
        height: 28,
        borderRadius: 99,
        border: 'none',
        background: checked ? colors.brandGradient : colors.borderInput,
        position: 'relative',
        cursor: 'pointer',
        padding: 0,
        transition: 'background 0.2s',
        flexShrink: 0,
      }}
      role="switch"
      aria-checked={checked}
    >
      <span
        style={{
          position: 'absolute',
          top: 3,
          left: checked ? 23 : 3,
          width: 22,
          height: 22,
          borderRadius: '50%',
          background: '#fff',
          transition: 'left 0.2s',
          boxShadow: '0 1px 3px rgba(0,0,0,0.25)',
        }}
      />
    </button>
  );
}

export default function Settings() {
  const { mode, toggleTheme, colors } = useTheme();
  const { lang, setLang, isRTL, t } = useLanguage();

  const cardStyle = {
    backgroundColor: colors.bgCard,
    borderRadius: 16,
    border: `1px solid ${colors.border}`,
    overflow: 'hidden',
  };

  const rowStyle = {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'space-between',
    padding: '18px 24px',
    gap: 16,
  };

  return (
    <div
      dir={isRTL ? 'rtl' : 'ltr'}
      style={{ padding: '24px', backgroundColor: colors.bgPage, minHeight: '100vh' }}
    >
      <div style={{ maxWidth: 680, margin: '0 auto' }}>

        {/* HEADER */}
        <div style={{ marginBottom: 24 }}>
          <p style={{
            fontSize: 11, fontWeight: 600, letterSpacing: '0.07em',
            textTransform: 'uppercase', color: colors.textMuted, marginBottom: 4,
          }}>
            {t('appName')}
          </p>
          <h1 style={{ fontSize: 22, fontWeight: 800, color: colors.textPrimary, margin: 0 }}>
            {t('settingsTitle')}
          </h1>
          <p style={{ fontSize: 13, color: colors.textSecondary, margin: 0, marginTop: 2 }}>
            {t('settingsSubtitle')}
          </p>
        </div>

        {/* APPEARANCE CARD */}
        <div style={{ ...cardStyle, marginBottom: 20 }}>
          <div style={{
            padding: '16px 24px',
            borderBottom: `1px solid ${colors.border}`,
          }}>
            <p style={{ fontSize: 15, fontWeight: 700, color: colors.textPrimary, margin: 0 }}>
              {t('appearance')}
            </p>
          </div>

          <div style={rowStyle}>
            <div>
              <p style={{ fontSize: 14, fontWeight: 600, color: colors.textPrimary, margin: 0, marginBottom: 2 }}>
                {t('darkMode')}
              </p>
              <p style={{ fontSize: 12, color: colors.textSecondary, margin: 0 }}>
                {t('darkModeDesc')}
              </p>
            </div>
            <ToggleSwitch checked={mode === 'dark'} onChange={toggleTheme} colors={colors} />
          </div>
        </div>

        {/* LANGUAGE CARD */}
        <div style={cardStyle}>
          <div style={{
            padding: '16px 24px',
            borderBottom: `1px solid ${colors.border}`,
          }}>
            <p style={{ fontSize: 15, fontWeight: 700, color: colors.textPrimary, margin: 0 }}>
              {t('language')}
            </p>
          </div>

          <div style={rowStyle}>
            <div>
              <p style={{ fontSize: 14, fontWeight: 600, color: colors.textPrimary, margin: 0, marginBottom: 2 }}>
                {t('language')}
              </p>
              <p style={{ fontSize: 12, color: colors.textSecondary, margin: 0 }}>
                {t('languageDesc')}
              </p>
            </div>

            <div style={{ display: 'flex', gap: 8, flexShrink: 0 }}>
              <button
                onClick={() => setLang('en')}
                style={{
                  padding: '8px 16px',
                  borderRadius: 10,
                  border: `1px solid ${lang === 'en' ? 'transparent' : colors.borderInput}`,
                  background: lang === 'en' ? colors.brandGradient : colors.bgCard,
                  color: lang === 'en' ? '#fff' : colors.textPrimary,
                  fontSize: 13,
                  fontWeight: 600,
                  cursor: 'pointer',
                }}
              >
                {t('english')}
              </button>
              <button
                onClick={() => setLang('ar')}
                style={{
                  padding: '8px 16px',
                  borderRadius: 10,
                  border: `1px solid ${lang === 'ar' ? 'transparent' : colors.borderInput}`,
                  background: lang === 'ar' ? colors.brandGradient : colors.bgCard,
                  color: lang === 'ar' ? '#fff' : colors.textPrimary,
                  fontSize: 13,
                  fontWeight: 600,
                  cursor: 'pointer',
                }}
              >
                {t('arabic')}
              </button>
            </div>
          </div>
        </div>

      </div>
    </div>
  );
}