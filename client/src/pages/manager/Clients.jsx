import { useEffect, useState } from 'react';
import { getClientsWithTickets, toggleUserStatus } from '../../api/manager';
import Spinner from '../../components/ui/Spinner.jsx';
import { useTheme } from '../../context/ThemeContext.jsx';
import { useLanguage } from '../../context/LanguageContext.jsx';

export default function Clients() {
  const { colors } = useTheme();
  const { t, isRTL } = useLanguage();

  const [items, setItems] = useState([]);
  const [pagination, setPagination] = useState(null);
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  const [search, setSearch] = useState('');
  const [debouncedSearch, setDebouncedSearch] = useState('');

  // Debounce the search box ~400ms so we're not firing a request every keystroke
  useEffect(() => {
    const handle = setTimeout(() => setDebouncedSearch(search), 400);
    return () => clearTimeout(handle);
  }, [search]);

  // Reset to page 1 whenever the search term changes, so we don't land on an empty page
  useEffect(() => {
    setPage(1);
  }, [debouncedSearch]);

  const load = async () => {
    setLoading(true);
    setError('');
    try {
      const res = await getClientsWithTickets({
        pageNumber: page,
        pageSize: 10,
        search: debouncedSearch || undefined,
      });
      setItems(res.items);
      setPagination(res.pagination);
    } catch (err) {
      setError(err.message || 'Something went wrong');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    load();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [page, debouncedSearch]);

  const toggle = async (id) => {
    try {
      await toggleUserStatus(id);
      await load();
    } catch (err) {
      setError(err.message || 'Failed to update user');
    }
  };

  const totalPages = pagination ? Math.ceil(pagination.totalCount / 10) : 1;

  return (
    <div dir={isRTL ? 'rtl' : 'ltr'} style={{ padding: '24px', backgroundColor: colors.bgPage, minHeight: '100vh' }}>

      {/* HEADER */}
      <div style={{ marginBottom: 24 }}>
        <p style={{
          fontSize: 11, fontWeight: 600, letterSpacing: '0.07em',
          textTransform: 'uppercase', color: colors.textMuted, marginBottom: 4,
        }}>
          {t('appName')}
        </p>
        <h1 style={{ fontSize: 22, fontWeight: 800, color: colors.textPrimary, margin: 0 }}>
          {t('clientsTitle')}
        </h1>
        <p style={{ fontSize: 13, color: colors.textSecondary, margin: 0, marginTop: 2 }}>
          {t('clientsSubtitle')}
        </p>
      </div>

      {/* ERROR */}
      {error && (
        <div style={{
          background: colors.dangerBg, color: colors.dangerText, borderRadius: 12,
          padding: '12px 16px', fontSize: 13, marginBottom: 16,
        }}>
          {error}
        </div>
      )}

      {/* SEARCH BOX */}
      <div style={{ marginBottom: 16 }}>
        <input
          type="text"
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          placeholder={t('search for a specific client') || 'Search by name, email, or location...'}
          style={{
            width: '100%',
            maxWidth: 420,
            padding: '10px 14px',
            borderRadius: 10,
            border: `1px solid ${colors.borderInput}`,
            fontSize: 14,
            fontWeight: 500,
            color: colors.textPrimary,
            background: colors.inputBg,
            outline: 'none',
            boxSizing: 'border-box',
          }}
        />
      </div>

      {/* TABLE CARD */}
      <div style={{
        backgroundColor: colors.bgCard,
        borderRadius: 16,
        border: `1px solid ${colors.border}`,
        overflow: 'hidden',
      }}>

        {/* CARD HEADER */}
        <div style={{
          padding: '16px 24px',
          borderBottom: `1px solid ${colors.border}`,
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'space-between',
        }}>
          <p style={{ fontSize: 15, fontWeight: 700, color: colors.textPrimary, margin: 0 }}>
            {t('allClients')}
          </p>
          <span style={{
            background: colors.pillBg, color: colors.pillText,
            fontSize: 12, fontWeight: 600,
            padding: '3px 10px', borderRadius: 99,
          }}>
            {pagination?.totalCount ?? items.length} {t('clientsLabel')}
          </span>
        </div>

        {loading ? (
          <div style={{ padding: '48px', display: 'flex', justifyContent: 'center' }}>
            <Spinner />
          </div>
        ) : (
          <>
            <table style={{ width: '100%', borderCollapse: 'collapse' }}>
              <thead>
                <tr style={{ background: colors.tableHeaderBg }}>
                  {[t('client'), t('email'), t('ticketsLabel'), t('status'), t('action')].map((col) => (
                    <th key={col} style={{
                      padding: '12px 24px',
                      textAlign: isRTL ? 'right' : 'left',
                      fontSize: 11,
                      fontWeight: 600,
                      textTransform: 'uppercase',
                      letterSpacing: '0.06em',
                      color: colors.textMuted,
                      borderBottom: `1px solid ${colors.border}`,
                    }}>
                      {col}
                    </th>
                  ))}
                </tr>
              </thead>
              <tbody>
                {items.length === 0 ? (
                  <tr>
                    <td colSpan={5} style={{ padding: '48px', textAlign: 'center', color: colors.textMuted, fontSize: 14 }}>
                      {t('noClientsFound')}
                    </td>
                  </tr>
                ) : (
                  items.map((u, idx) => (
                    <tr
                      key={u.id}
                      style={{
                        borderBottom: idx < items.length - 1 ? `1px solid ${colors.borderLight}` : 'none',
                        transition: 'background 0.15s',
                      }}
                      onMouseEnter={e => e.currentTarget.style.background = colors.bgCardHover}
                      onMouseLeave={e => e.currentTarget.style.background = 'transparent'}
                    >
                      {/* NAME + AVATAR */}
                      <td style={{ padding: '14px 24px' }}>
                        <div style={{ display: 'flex', alignItems: 'center', gap: 10 }}>
                          <div style={{
                            width: 36, height: 36, borderRadius: '50%',
                            background: 'linear-gradient(135deg, #7c3aed, #a78bfa)',
                            display: 'flex', alignItems: 'center', justifyContent: 'center',
                            color: '#fff', fontSize: 13, fontWeight: 700, flexShrink: 0,
                          }}>
                            {u.fullName?.charAt(0)?.toUpperCase() || '?'}
                          </div>
                          <span style={{ fontWeight: 600, color: colors.textPrimary, fontSize: 14 }}>
                            {u.fullName}
                          </span>
                        </div>
                      </td>

                      {/* EMAIL */}
                      <td style={{ padding: '14px 24px', fontSize: 13, color: colors.textSecondary }}>
                        {u.email}
                      </td>

                      {/* TICKETS COUNT */}
                      <td style={{ padding: '14px 24px' }}>
                        <span style={{
                          display: 'inline-flex', alignItems: 'center', gap: 5,
                          padding: '4px 12px', borderRadius: 99,
                          fontSize: 12, fontWeight: 600,
                          background: colors.pillBg, color: colors.pillText,
                        }}>
                          {u.tickets?.length || 0} {t('ticketsLabel')}
                        </span>
                      </td>

                      {/* STATUS */}
                      <td style={{ padding: '14px 24px' }}>
                        <span style={{
                          display: 'inline-flex', alignItems: 'center', gap: 5,
                          padding: '4px 12px', borderRadius: 99,
                          fontSize: 12, fontWeight: 600,
                          background: u.isActive ? colors.successBg : colors.dangerBg,
                          color: u.isActive ? colors.successText : colors.dangerText,
                        }}>
                          <span style={{
                            width: 6, height: 6, borderRadius: '50%',
                            background: u.isActive ? colors.successDot : colors.dangerDot,
                          }} />
                          {u.isActive ? t('active') : t('inactive')}
                        </span>
                      </td>

                      {/* ACTION */}
                      <td style={{ padding: '14px 24px' }}>
                        <button
                          onClick={() => toggle(u.id)}
                          style={{
                            padding: '6px 14px',
                            borderRadius: 8,
                            border: 'none',
                            fontSize: 12,
                            fontWeight: 600,
                            cursor: 'pointer',
                            transition: 'opacity 0.2s',
                            background: u.isActive
                              ? 'linear-gradient(135deg, #dc2626, #f87171)'
                              : 'linear-gradient(135deg, #059669, #34d399)',
                            color: '#fff',
                            boxShadow: u.isActive
                              ? '0 2px 8px rgba(220,38,38,0.25)'
                              : '0 2px 8px rgba(5,150,105,0.25)',
                          }}
                          onMouseEnter={e => e.currentTarget.style.opacity = '0.85'}
                          onMouseLeave={e => e.currentTarget.style.opacity = '1'}
                        >
                          {u.isActive ? t('deactivate') : t('activate')}
                        </button>
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>

            {/* PAGINATION */}
            <div style={{
              padding: '14px 24px',
              borderTop: `1px solid ${colors.border}`,
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'space-between',
            }}>
              <span style={{ fontSize: 12, color: colors.textMuted }}>
                {t('page')} {page} {t('of')} {totalPages}
              </span>
              <div style={{ display: 'flex', gap: 8 }}>
                <button
                  onClick={() => setPage((p) => Math.max(1, p - 1))}
                  disabled={page === 1}
                  style={{
                    padding: '6px 14px',
                    borderRadius: 8,
                    border: `1px solid ${colors.borderInput}`,
                    background: page === 1 ? colors.bgSecondary : colors.bgCard,
                    color: page === 1 ? colors.textMuted : colors.textPrimary,
                    fontSize: 13,
                    fontWeight: 600,
                    cursor: page === 1 ? 'not-allowed' : 'pointer',
                    transition: 'background 0.15s',
                  }}
                >
                  {isRTL ? `${t('prev')} →` : `← ${t('prev')}`}
                </button>
                {Array.from({ length: totalPages }, (_, i) => i + 1)
                  .filter(p => p === 1 || p === totalPages || Math.abs(p - page) <= 1)
                  .reduce((acc, p, i, arr) => {
                    if (i > 0 && p - arr[i - 1] > 1) acc.push('...');
                    acc.push(p);
                    return acc;
                  }, [])
                  .map((p, i) =>
                    p === '...' ? (
                      <span key={`dots-${i}`} style={{ padding: '6px 4px', color: colors.textMuted, fontSize: 13 }}>…</span>
                    ) : (
                      <button
                        key={p}
                        onClick={() => setPage(p)}
                        style={{
                          padding: '6px 12px',
                          borderRadius: 8,
                          border: `1px solid ${colors.borderInput}`,
                          background: page === p ? colors.brandGradient : colors.bgCard,
                          color: page === p ? '#fff' : colors.textPrimary,
                          fontSize: 13,
                          fontWeight: 600,
                          cursor: 'pointer',
                          boxShadow: page === p ? `0 2px 8px ${colors.brandShadow}` : 'none',
                        }}
                      >
                        {p}
                      </button>
                    )
                  )}
                <button
                  onClick={() => setPage((p) => Math.min(totalPages, p + 1))}
                  disabled={page === totalPages}
                  style={{
                    padding: '6px 14px',
                    borderRadius: 8,
                    border: `1px solid ${colors.borderInput}`,
                    background: page === totalPages ? colors.bgSecondary : colors.bgCard,
                    color: page === totalPages ? colors.textMuted : colors.textPrimary,
                    fontSize: 13,
                    fontWeight: 600,
                    cursor: page === totalPages ? 'not-allowed' : 'pointer',
                  }}
                >
                  {isRTL ? `← ${t('next')}` : `${t('next')} →`}
                </button>
              </div>
            </div>
          </>
        )}
      </div>
    </div>
  );
}