import { useEffect, useState, useRef } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getTicketDetails, getEmployees, assignTicket } from '../../api/manager';
import { shortId } from '../../utils/format';
import Spinner from '../../components/ui/Spinner.jsx';
import CommentsPanel from '../../components/tickets/CommentsPanel.jsx';
import AttachmentsPanel from '../../components/tickets/AttachmentsPanel.jsx';
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
      display: 'inline-flex', alignItems: 'center', gap: 5,
      padding: '5px 14px', borderRadius: 99, fontSize: 12, fontWeight: 600,
      background: s.bg, color: s.color,
    }}>
      <span style={{ width: 6, height: 6, borderRadius: '50%', background: s.dot }} />
      {status}
    </span>
  );
}

function InfoRow({ label, value, colors }) {
  return (
    <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', padding: '10px 0', borderBottom: `1px solid ${colors.borderLight}` }}>
      <span style={{ fontSize: 12, fontWeight: 600, textTransform: 'uppercase', letterSpacing: '0.05em', color: colors.textMuted }}>{label}</span>
      <span style={{ fontSize: 13, fontWeight: 600, color: colors.textPrimary }}>{value}</span>
    </div>
  );
}

// Searchable employee picker — debounced Lucene search via getEmployees({ search }).
// Keeps the same employeeId/onSelect contract the assign button already relies on.
function EmployeeSearchPicker({ colors, t, value, onSelect }) {
  const [query, setQuery] = useState('');
  const [debouncedQuery, setDebouncedQuery] = useState('');
  const [results, setResults] = useState([]);
  const [loading, setLoading] = useState(false);
  const [open, setOpen] = useState(false);
  const [selectedLabel, setSelectedLabel] = useState('');
  const containerRef = useRef(null);

  // Debounce ~400ms before hitting the search endpoint
  useEffect(() => {
    const handle = setTimeout(() => setDebouncedQuery(query), 400);
    return () => clearTimeout(handle);
  }, [query]);

  // Fetch matching employees whenever the debounced query changes
  useEffect(() => {
    let active = true;

    const run = async () => {
      if (!debouncedQuery.trim()) {
        setResults([]);
        return;
      }
      setLoading(true);
      try {
        const { items } = await getEmployees({
          pageNumber: 1,
          pageSize: 20,
          search: debouncedQuery,
        });
        if (active) setResults(items);
      } catch {
        if (active) setResults([]);
      } finally {
        if (active) setLoading(false);
      }
    };

    run();
    return () => { active = false; };
  }, [debouncedQuery]);

  // Close the dropdown when clicking outside
  useEffect(() => {
    const handleClick = (e) => {
      if (containerRef.current && !containerRef.current.contains(e.target)) {
        setOpen(false);
      }
    };
    document.addEventListener('mousedown', handleClick);
    return () => document.removeEventListener('mousedown', handleClick);
  }, []);

  const pick = (emp) => {
    onSelect(emp.id);
    setSelectedLabel(`${emp.fullName} (${emp.email})`);
    setQuery('');
    setResults([]);
    setOpen(false);
  };

  const clearSelection = () => {
    onSelect('');
    setSelectedLabel('');
  };

  return (
    <div ref={containerRef} style={{ position: 'relative' }}>
      {value && selectedLabel ? (
        <div style={{
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'space-between',
          padding: '9px 12px',
          borderRadius: 10,
          border: `1px solid ${colors.borderInput}`,
          background: colors.pillBg,
          fontSize: 13,
          fontWeight: 600,
          color: colors.pillText,
        }}>
          <span>{selectedLabel}</span>
          <button
            onClick={clearSelection}
            style={{
              border: 'none',
              background: 'transparent',
              color: colors.textMuted,
              cursor: 'pointer',
              fontSize: 13,
              fontWeight: 700,
              padding: '0 0 0 8px',
            }}
            type="button"
          >
            ×
          </button>
        </div>
      ) : (
        <input
          type="text"
          value={query}
          onChange={(e) => {
            setQuery(e.target.value);
            setOpen(true);
          }}
          onFocus={() => setOpen(true)}
          placeholder={t('search for employee') || 'Search by name, email, or location...'}
          style={{
            width: '100%',
            padding: '9px 12px',
            borderRadius: 10,
            border: `1px solid ${colors.borderInput}`,
            fontSize: 13,
            fontWeight: 500,
            color: colors.textPrimary,
            background: colors.inputBg,
            outline: 'none',
            boxSizing: 'border-box',
          }}
        />
      )}

      {open && !value && (query.trim() || loading) && (
        <div style={{
          position: 'absolute',
          top: 'calc(100% + 6px)',
          left: 0,
          right: 0,
          background: colors.bgCard,
          border: `1px solid ${colors.border}`,
          borderRadius: 10,
          boxShadow: '0 8px 24px rgba(0,0,0,0.12)',
          maxHeight: 220,
          overflowY: 'auto',
          zIndex: 20,
        }}>
          {loading ? (
            <div style={{ padding: '12px 14px', fontSize: 13, color: colors.textMuted }}>
              {t('searching') || 'Searching...'}
            </div>
          ) : results.length === 0 ? (
            <div style={{ padding: '12px 14px', fontSize: 13, color: colors.textMuted }}>
              {t('noEmployeesFound') || 'No employees found'}
            </div>
          ) : (
            results.map((emp) => (
              <div
                key={emp.id}
                onClick={() => pick(emp)}
                style={{
                  padding: '10px 14px',
                  cursor: 'pointer',
                  borderBottom: `1px solid ${colors.borderLight}`,
                  transition: 'background 0.1s',
                }}
                onMouseEnter={e => e.currentTarget.style.background = colors.bgCardHover}
                onMouseLeave={e => e.currentTarget.style.background = 'transparent'}
              >
                <div style={{ fontSize: 13, fontWeight: 600, color: colors.textPrimary }}>
                  {emp.fullName}
                </div>
                <div style={{ fontSize: 12, color: colors.textSecondary }}>
                  {emp.email}{emp.address ? ` · ${emp.address}` : ''}
                </div>
              </div>
            ))
          )}
        </div>
      )}
    </div>
  );
}

export default function TicketDetails() {
  const { id } = useParams();
  const navigate = useNavigate();
  const { colors, mode } = useTheme();
  const { t, isRTL } = useLanguage();

  const [ticket, setTicket] = useState(null);
  const [employeeId, setEmployeeId] = useState('');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [message, setMessage] = useState('');
  const [saving, setSaving] = useState(false);

  const load = async () => {
    setLoading(true);
    try {
      const tk = await getTicketDetails(id);
      setTicket(tk);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    load();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [id]);

  const assign = async () => {
    if (!employeeId) return;
    setSaving(true);
    setError('');
    setMessage('');
    try {
      await assignTicket(id, employeeId);
      setMessage('Ticket assigned successfully.');
      setEmployeeId('');
      await load();
    } catch (err) {
      setError(err.message);
    } finally {
      setSaving(false);
    }
  };

  if (loading) return (
    <div style={{ height: '80vh', display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
      <Spinner />
    </div>
  );

  if (!ticket) return (
    <div style={{ padding: 24, background: colors.dangerBg, color: colors.dangerText, borderRadius: 12, margin: 24 }}>
      {error || 'Ticket not found'}
    </div>
  );

  const assigned = Boolean(ticket.assignedEmployeeId);

  return (
    <div dir={isRTL ? 'rtl' : 'ltr'} style={{ padding: '24px', backgroundColor: colors.bgPage, minHeight: '100vh' }}>

      {/* HEADER */}
      <div style={{ display: 'flex', alignItems: 'flex-start', justifyContent: 'space-between', marginBottom: 24, flexWrap: 'wrap', gap: 12 }}>
        <div>
          <p style={{ fontSize: 11, fontWeight: 600, letterSpacing: '0.07em', textTransform: 'uppercase', color: colors.textMuted, marginBottom: 4 }}>
            {t('ticketsDetails')}
          </p>
          <h1 style={{ fontSize: 22, fontWeight: 800, color: colors.textPrimary, margin: 0 }}>
            {t('ticket')} <span style={{ fontFamily: 'monospace', color: colors.pillText }}>#{shortId(ticket.id)}</span>
          </h1>
        </div>
        <button
          onClick={() => navigate('/manager/tickets')}
          style={{
            padding: '9px 18px',
            borderRadius: 10,
            border: `1px solid ${colors.borderInput}`,
            background: colors.bgCard,
            color: colors.textSecondary,
            fontSize: 13,
            fontWeight: 600,
            cursor: 'pointer',
            display: 'flex',
            alignItems: 'center',
            gap: 6,
            transition: 'background 0.15s',
          }}
          onMouseEnter={e => e.currentTarget.style.background = colors.bgCardHover}
          onMouseLeave={e => e.currentTarget.style.background = colors.bgCard}
        >
          {isRTL ? `${t('backToTickets')} →` : `← ${t('backToTickets')}`}
        </button>
      </div>

      {/* ALERTS */}
      {error && (
        <div style={{ background: colors.dangerBg, color: colors.dangerText, borderRadius: 12, padding: '12px 16px', fontSize: 13, marginBottom: 16 }}>
          {error}
        </div>
      )}
      {message && (
        <div style={{ background: colors.successBg, color: colors.successText, borderRadius: 12, padding: '12px 16px', fontSize: 13, marginBottom: 16 }}>
          {message}
        </div>
      )}

      {/* MAIN GRID */}
      <div style={{ display: 'grid', gridTemplateColumns: '1fr 340px', gap: 20, alignItems: 'start' }}>

        {/* LEFT COLUMN */}
        <div style={{ display: 'flex', flexDirection: 'column', gap: 20 }}>

          {/* TICKET INFO CARD */}
          <div style={{ background: colors.bgCard, borderRadius: 16, border: `1px solid ${colors.border}`, overflow: 'hidden' }}>
            <div style={{ padding: '20px 24px', borderBottom: `1px solid ${colors.border}`, display: 'flex', alignItems: 'center', justifyContent: 'space-between', flexWrap: 'wrap', gap: 10 }}>
              <h2 style={{ fontSize: 16, fontWeight: 700, color: colors.textPrimary, margin: 0 }}>
                {ticket.title}
              </h2>
              <StatusPill status={ticket.status} mode={mode} />
            </div>
            <div style={{ padding: '20px 24px' }}>
              <p style={{ fontSize: 13, fontWeight: 600, textTransform: 'uppercase', letterSpacing: '0.05em', color: colors.textMuted, marginBottom: 8 }}>
                {t('description')}
              </p>
              <p style={{ fontSize: 14, color: colors.textSecondary, lineHeight: 1.7, margin: 0 }}>
                {ticket.description || t('noDescription')}
              </p>
            </div>
          </div>

          {/* COMMENTS CARD */}
          <div style={{ background: colors.bgCard, borderRadius: 16, border: `1px solid ${colors.border}`, overflow: 'hidden' }}>
            <div style={{ padding: '16px 24px', borderBottom: `1px solid ${colors.border}` }}>
              <p style={{ fontSize: 15, fontWeight: 700, color: colors.textPrimary, margin: 0 }}>{t('comments')}</p>
            </div>
            <div style={{ padding: '20px 24px' }}>
              <CommentsPanel ticketId={id} canComment={false} />
            </div>
          </div>

          {/* ATTACHMENTS CARD */}
          <div style={{ background: colors.bgCard, borderRadius: 16, border: `1px solid ${colors.border}`, overflow: 'hidden' }}>
            <div style={{ padding: '16px 24px', borderBottom: `1px solid ${colors.border}` }}>
              <p style={{ fontSize: 15, fontWeight: 700, color: colors.textPrimary, margin: 0 }}>{t('attachments')}</p>
            </div>
            <div style={{ padding: '20px 24px' }}>
              <AttachmentsPanel ticketId={id} canUpload={false} />
            </div>
          </div>
        </div>

        {/* RIGHT COLUMN */}
        <div style={{ display: 'flex', flexDirection: 'column', gap: 20 }}>

          {/* TICKET META CARD */}
          <div style={{ background: colors.bgCard, borderRadius: 16, border: `1px solid ${colors.border}`, padding: '20px 24px' }}>
            <p style={{ fontSize: 11, fontWeight: 600, textTransform: 'uppercase', letterSpacing: '0.07em', color: colors.textMuted, marginBottom: 12 }}>
              {t('ticketInfo')}
            </p>
            <InfoRow
              colors={colors}
              label={t('id')}
              value={<span style={{ fontFamily: 'monospace', color: colors.pillText, background: colors.pillBg, padding: '2px 8px', borderRadius: 6 }}>#{shortId(ticket.id)}</span>}
            />
            <InfoRow colors={colors} label={t('status')} value={<StatusPill status={ticket.status} mode={mode} />} />
            <InfoRow
              colors={colors}
              label={t('assigned')}
              value={
                assigned
                  ? <span style={{ display: 'flex', alignItems: 'center', gap: 6 }}>
                      <span style={{ width: 22, height: 22, borderRadius: '50%', background: 'linear-gradient(135deg,#7c3aed,#a78bfa)', display: 'inline-flex', alignItems: 'center', justifyContent: 'center', color: '#fff', fontSize: 10, fontWeight: 700 }}>E</span>
                      {shortId(ticket.assignedEmployeeId)}
                    </span>
                  : <span style={{ fontSize: 12, color: colors.textMuted, background: colors.bgSecondary, padding: '3px 10px', borderRadius: 99, fontWeight: 500 }}>{t('unassigned')}</span>
              }
            />
          </div>

          {/* ASSIGNMENT CARD */}
          <div style={{ background: colors.bgCard, borderRadius: 16, border: `1px solid ${colors.border}`, padding: '20px 24px' }}>
            <p style={{ fontSize: 11, fontWeight: 600, textTransform: 'uppercase', letterSpacing: '0.07em', color: colors.textMuted, marginBottom: 12 }}>
              {t('assignment')}
            </p>

            {assigned ? (
              <div style={{ background: colors.pillBg, borderRadius: 12, padding: '14px 16px' }}>
                <p style={{ fontSize: 13, color: colors.pillText, fontWeight: 600, margin: 0, marginBottom: 4 }}>
                  {t('alreadyAssigned')}
                </p>
                <p style={{ fontSize: 12, color: colors.textSecondary, margin: 0, lineHeight: 1.6 }}>
                  {t('assignedToInfo')}{' '}
                  <span style={{ fontFamily: 'monospace', fontWeight: 700, color: colors.pillText }}>
                    #{shortId(ticket.assignedEmployeeId)}
                  </span>.
                  {' '}{t('cannotReassign')}
                </p>
              </div>
            ) : (
              <div style={{ display: 'flex', flexDirection: 'column', gap: 12 }}>
                <div>
                  <label style={{ fontSize: 12, fontWeight: 600, color: colors.textSecondary, display: 'block', marginBottom: 6 }}>
                    {t('assignToEmployee')}
                  </label>
                  <EmployeeSearchPicker
                    colors={colors}
                    t={t}
                    value={employeeId}
                    onSelect={setEmployeeId}
                  />
                </div>

                <button
                  onClick={assign}
                  disabled={saving || !employeeId}
                  style={{
                    width: '100%',
                    padding: '10px',
                    borderRadius: 10,
                    border: 'none',
                    background: saving || !employeeId ? '#93c5fd' : colors.brandGradient,
                    color: '#fff',
                    fontSize: 14,
                    fontWeight: 600,
                    cursor: saving || !employeeId ? 'not-allowed' : 'pointer',
                    boxShadow: saving || !employeeId ? 'none' : `0 4px 12px ${colors.brandShadow}`,
                    transition: 'opacity 0.2s',
                  }}
                  onMouseEnter={e => { if (!saving && employeeId) e.currentTarget.style.opacity = '0.88'; }}
                  onMouseLeave={e => e.currentTarget.style.opacity = '1'}
                >
                  {saving ? t('assigning') : t('assignTicket')}
                </button>
              </div>
            )}
          </div>

        </div>
      </div>
    </div>
  );
}