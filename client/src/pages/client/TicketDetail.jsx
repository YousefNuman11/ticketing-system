import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getTicketById, closeTicket } from '../../api/tickets';
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

export default function TicketDetail() {
  const { id } = useParams();
  const navigate = useNavigate();
  const { colors, mode } = useTheme();
  const { t, isRTL } = useLanguage();

  const [ticket, setTicket] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [message, setMessage] = useState('');

  const load = async () => {
    setLoading(true);
    try {
      setTicket(await getTicketById(id));
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

  const close = async () => {
    setError('');
    setMessage('');
    try {
      await closeTicket(id);
      setMessage('Ticket closed. Thank you for confirming!');
      await load();
    } catch (err) {
      setError(err.message);
    }
  };

  if (loading) {
    return (
      <div style={{ height: '80vh', display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
        <Spinner />
      </div>
    );
  }

  if (!ticket) {
    return (
      <div style={{ padding: 24, background: colors.dangerBg, color: colors.dangerText, borderRadius: 12, margin: 24 }}>
        {error || 'Ticket not found'}
      </div>
    );
  }

  const canEdit = ticket.status === 'New';
  const canClose = ticket.status === 'Resolved';

  return (
    <div dir={isRTL ? 'rtl' : 'ltr'} style={{ padding: '24px', backgroundColor: colors.bgPage, minHeight: '100vh' }}>

      {/* HEADER */}
      <div style={{ display: 'flex', alignItems: 'flex-start', justifyContent: 'space-between', marginBottom: 24, flexWrap: 'wrap', gap: 12 }}>
        <div>
          <p style={{
            fontSize: 11, fontWeight: 600, letterSpacing: '0.07em',
            textTransform: 'uppercase', color: colors.textMuted, marginBottom: 4,
          }}>
            {t('ticketsDetails')}
          </p>
          <h1 style={{ fontSize: 22, fontWeight: 800, color: colors.textPrimary, margin: 0 }}>
            {t('ticket')} <span style={{ fontFamily: 'monospace', color: colors.pillText }}>#{shortId(ticket.id)}</span>
          </h1>
        </div>

        <div style={{ display: 'flex', gap: 10, flexWrap: 'wrap' }}>
          {canEdit && (
            <button
              onClick={() => navigate(`/client/tickets/${id}/edit`)}
              style={{
                padding: '9px 18px', borderRadius: 10, border: `1px solid ${colors.borderInput}`,
                background: colors.bgCard, color: colors.textPrimary, fontSize: 13, fontWeight: 600, cursor: 'pointer',
              }}
            >
              {t('edit')}
            </button>
          )}
          {canClose && (
            <button
              onClick={close}
              style={{
                padding: '9px 18px', borderRadius: 10, border: 'none',
                background: 'linear-gradient(135deg, #059669, #34d399)',
                color: '#fff', fontSize: 13, fontWeight: 600, cursor: 'pointer',
                boxShadow: '0 4px 12px rgba(5,150,105,0.3)',
              }}
            >
              {t('closeTicket')}
            </button>
          )}
          <button
            onClick={() => navigate('/client')}
            style={{
              padding: '9px 18px', borderRadius: 10, border: `1px solid ${colors.borderInput}`,
              background: colors.bgCard, color: colors.textSecondary, fontSize: 13, fontWeight: 600, cursor: 'pointer',
            }}
          >
            {isRTL ? `${t('back')} →` : `← ${t('back')}`}
          </button>
        </div>
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
              <CommentsPanel ticketId={id} canComment />
            </div>
          </div>
        </div>

        {/* RIGHT COLUMN */}
        <div style={{ display: 'flex', flexDirection: 'column', gap: 20 }}>
          <div style={{ background: colors.bgCard, borderRadius: 16, border: `1px solid ${colors.border}`, overflow: 'hidden' }}>
            <div style={{ padding: '16px 24px', borderBottom: `1px solid ${colors.border}` }}>
              <p style={{ fontSize: 15, fontWeight: 700, color: colors.textPrimary, margin: 0 }}>{t('attachments')}</p>
            </div>
            <div style={{ padding: '20px 24px' }}>
              <AttachmentsPanel ticketId={id} canUpload />
            </div>
          </div>
        </div>

      </div>
    </div>
  );
}