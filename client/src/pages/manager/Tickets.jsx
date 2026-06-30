import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { getAllTickets } from '../../api/manager';
import { TICKET_STATUS } from '../../utils/constants';
import { shortId } from '../../utils/format';
import Spinner from '../../components/ui/Spinner.jsx';
import Pagination from '../../components/ui/Pagination.jsx';
import { useTheme } from '../../context/ThemeContext.jsx';
import { useLanguage } from '../../context/LanguageContext.jsx';

const STATUS_STYLES_LIGHT = {
  New:        { bg: '#eff6ff', color: '#1d4ed8', dot: '#3b82f6' },
  Assigned:   { bg: '#f0f4ff', color: '#4338ca', dot: '#6366f1' },
  InProgress: { bg: '#fefce8', color: '#a16207', dot: '#eab308' },
  Resolved:   { bg: '#dcfce7', color: '#15803d', dot: '#16a34a' },
  Closed:     { bg: '#f1f5f9', color: '#475569', dot: '#94a3b8' },
};

const STATUS_STYLES_DARK = {
  New:        { bg: '#15233f', color: '#7fa8ff', dot: '#3b82f6' },
  Assigned:   { bg: '#1e2140', color: '#a5a8f5', dot: '#6366f1' },
  InProgress: { bg: '#3a2f0c', color: '#facc15', dot: '#eab308' },
  Resolved:   { bg: '#0f2a1c', color: '#4ade80', dot: '#22c55e' },
  Closed:     { bg: '#1f242e', color: '#94a3b8', dot: '#6b7384' },
};

function StatusPill({ status, mode }) {
  const styles = mode === 'dark' ? STATUS_STYLES_DARK : STATUS_STYLES_LIGHT;
  const s = styles[status] || styles.Closed;
  return (
    <span style={{
      display: 'inline-flex',
      alignItems: 'center',
      gap: 5,
      padding: '4px 12px',
      borderRadius: 99,
      fontSize: 12,
      fontWeight: 600,
      background: s.bg,
      color: s.color,
    }}>
      <span style={{ width: 6, height: 6, borderRadius: '50%', background: s.dot, display: 'inline-block' }} />
      {status}
    </span>
  );
}

export default function Tickets() {
  const navigate = useNavigate();
  const { colors, mode } = useTheme();
  const { t, isRTL } = useLanguage();

  const [items, setItems] = useState([]);
  const [pagination, setPagination] = useState(null);
  const [status, setStatus] = useState('');
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  const load = async () => {
    setLoading(true);
    setError('');
    try {
      const params = { pageNumber: page, pageSize: 10 };
      if (status) params.status = status;
      const { items, pagination } = await getAllTickets(params);
      setItems(items);
      setPagination(pagination);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    load();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [page, status]);

  return (
    <div dir={isRTL ? 'rtl' : 'ltr'} style={{ padding: '24px', backgroundColor: colors.bgPage, minHeight: '100vh' }}>

      {/* HEADER */}
      <div style={{ marginBottom: '24px' }}>
        <p style={{
          fontSize: 11, fontWeight: 600, letterSpacing: '0.07em',
          textTransform: 'uppercase', color: colors.textMuted, marginBottom: 4,
        }}>
          {t('appName')}
        </p>
        <h1 style={{ fontSize: 22, fontWeight: 800, color: colors.textPrimary, margin: 0 }}>
          {t('allTicketsTitle')}
        </h1>
        <p style={{ fontSize: 13, color: colors.textSecondary, margin: 0, marginTop: 2 }}>
          {t('allTicketsSubtitle')}
        </p>
      </div>

      {error && (
        <div style={{
          background: colors.dangerBg, color: colors.dangerText, borderRadius: 12,
          padding: '12px 16px', fontSize: 13, marginBottom: 16,
        }}>
          {error}
        </div>
      )}

      {/* FILTER BAR */}
      <div style={{
        backgroundColor: colors.bgCard,
        borderRadius: 16,
        border: `1px solid ${colors.border}`,
        padding: '16px 24px',
        marginBottom: 16,
        display: 'flex',
        alignItems: 'center',
        gap: 12,
      }}>
        <span style={{ fontSize: 13, fontWeight: 600, color: colors.textSecondary }}>{t('filterByStatus')}</span>
        <select
          value={status}
          onChange={(e) => { setPage(1); setStatus(e.target.value); }}
          style={{
            padding: '8px 14px',
            borderRadius: 10,
            border: `1px solid ${colors.borderInput}`,
            fontSize: 13,
            fontWeight: 500,
            color: colors.textPrimary,
            background: colors.inputBg,
            cursor: 'pointer',
            outline: 'none',
            minWidth: 180,
          }}
        >
          <option value="">{t('allStatuses')}</option>
          {TICKET_STATUS.map((s) => (
            <option key={s} value={s}>{s}</option>
          ))}
        </select>

        {status && (
          <button
            onClick={() => { setPage(1); setStatus(''); }}
            style={{
              padding: '7px 14px',
              borderRadius: 10,
              border: `1px solid ${colors.borderInput}`,
              background: colors.bgCard,
              color: colors.textSecondary,
              fontSize: 13,
              fontWeight: 500,
              cursor: 'pointer',
            }}
          >
            {t('clear')}
          </button>
        )}

        <span style={{
          marginLeft: isRTL ? 0 : 'auto',
          marginRight: isRTL ? 'auto' : 0,
          background: colors.pillBg,
          color: colors.pillText,
          fontSize: 12,
          fontWeight: 600,
          padding: '3px 10px',
          borderRadius: 99,
        }}>
          {pagination?.totalCount ?? items.length} {t('ticketsLabel')}
        </span>
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
        }}>
          <p style={{ fontSize: 15, fontWeight: 700, color: colors.textPrimary, margin: 0 }}>
            {t('ticketList')}
          </p>
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
                  {[t('ticket'), t('title'), t('status'), t('assignedTo'), t('action')].map((col) => (
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
                      {t('noTicketsFound')}
                    </td>
                  </tr>
                ) : (
                  items.map((tk, idx) => (
                    <tr
                      key={tk.id}
                      style={{
                        borderBottom: idx < items.length - 1 ? `1px solid ${colors.borderLight}` : 'none',
                        transition: 'background 0.15s',
                      }}
                      onMouseEnter={e => e.currentTarget.style.background = colors.bgCardHover}
                      onMouseLeave={e => e.currentTarget.style.background = 'transparent'}
                    >
                      {/* TICKET ID */}
                      <td style={{ padding: '14px 24px' }}>
                        <span style={{
                          fontFamily: 'monospace',
                          fontSize: 12,
                          fontWeight: 700,
                          color: colors.pillText,
                          background: colors.pillBg,
                          padding: '3px 8px',
                          borderRadius: 6,
                        }}>
                          #{shortId(tk.id)}
                        </span>
                      </td>

                      {/* TITLE */}
                      <td style={{ padding: '14px 24px', fontSize: 14, fontWeight: 600, color: colors.textPrimary, maxWidth: 280 }}>
                        <span style={{
                          display: '-webkit-box',
                          WebkitLineClamp: 1,
                          WebkitBoxOrient: 'vertical',
                          overflow: 'hidden',
                        }}>
                          {tk.title}
                        </span>
                      </td>

                      {/* STATUS */}
                      <td style={{ padding: '14px 24px' }}>
                        <StatusPill status={tk.status} mode={mode} />
                      </td>

                      {/* ASSIGNED */}
                      <td style={{ padding: '14px 24px' }}>
                        {tk.assignedEmployeeId ? (
                          <span style={{
                            fontSize: 13,
                            fontWeight: 500,
                            color: colors.textPrimary,
                            display: 'flex',
                            alignItems: 'center',
                            gap: 6,
                          }}>
                            <span style={{
                              width: 24,
                              height: 24,
                              borderRadius: '50%',
                              background: 'linear-gradient(135deg, #7c3aed, #a78bfa)',
                              display: 'inline-flex',
                              alignItems: 'center',
                              justifyContent: 'center',
                              color: '#fff',
                              fontSize: 10,
                              fontWeight: 700,
                            }}>
                              E
                            </span>
                            {shortId(tk.assignedEmployeeId)}
                          </span>
                        ) : (
                          <span style={{
                            fontSize: 12,
                            fontWeight: 500,
                            color: colors.textMuted,
                            background: colors.bgSecondary,
                            padding: '3px 10px',
                            borderRadius: 99,
                          }}>
                            {t('unassigned')}
                          </span>
                        )}
                      </td>

                      {/* ACTION */}
                      <td style={{ padding: '14px 24px' }}>
                        <button
                          onClick={() => navigate(`/manager/tickets/${tk.id}`)}
                          style={{
                            padding: '6px 16px',
                            borderRadius: 8,
                            border: 'none',
                            background: colors.brandGradient,
                            color: '#fff',
                            fontSize: 12,
                            fontWeight: 600,
                            cursor: 'pointer',
                            boxShadow: `0 2px 8px ${colors.brandShadow}`,
                            transition: 'opacity 0.2s',
                          }}
                          onMouseEnter={e => e.currentTarget.style.opacity = '0.85'}
                          onMouseLeave={e => e.currentTarget.style.opacity = '1'}
                        >
                          {t('view')}
                        </button>
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>

            <div style={{ padding: '12px 24px', borderTop: `1px solid ${colors.border}` }}>
              <Pagination pagination={pagination} onPageChange={setPage} />
            </div>
          </>
        )}
      </div>
    </div>
  );
}