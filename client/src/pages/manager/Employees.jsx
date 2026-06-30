import { useEffect, useState } from 'react';
import { getEmployees, createEmployee, toggleUserStatus } from '../../api/manager';
import Spinner from '../../components/ui/Spinner.jsx';
import Alert from '../../components/ui/Alert.jsx';
import Modal from '../../components/ui/Modal.jsx';
import Pagination from '../../components/ui/Pagination.jsx';
import { useTheme } from '../../context/ThemeContext.jsx';
import { useLanguage } from '../../context/LanguageContext.jsx';

const emptyForm = { fullName: '', email: '', mobileNumber: '', password: '', address: '' };

export default function Employees() {
  const { colors } = useTheme();
  const { t, isRTL } = useLanguage();

  const [items, setItems] = useState([]);
  const [pagination, setPagination] = useState(null);
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [open, setOpen] = useState(false);
  const [form, setForm] = useState(emptyForm);
  const [saving, setSaving] = useState(false);

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
    try {
      const { items, pagination } = await getEmployees({
        pageNumber: page,
        pageSize: 10,
        search: debouncedSearch || undefined,
      });
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
  }, [page, debouncedSearch]);

  const set = (k) => (e) => setForm({ ...form, [k]: e.target.value });

  const create = async (e) => {
    e.preventDefault();
    setSaving(true);
    setError('');
    try {
      await createEmployee(form);
      setOpen(false);
      setForm(emptyForm);
      setPage(1);
      await load();
    } catch (err) {
      setError(err.message);
    } finally {
      setSaving(false);
    }
  };

  const toggle = async (userId) => {
    try {
      await toggleUserStatus(userId);
      await load();
    } catch (err) {
      setError(err.message);
    }
  };

  const inputStyle = {
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
  };

  return (
    <div dir={isRTL ? 'rtl' : 'ltr'} style={{ padding: '24px', backgroundColor: colors.bgPage, minHeight: '100vh' }}>

      {/* HEADER */}
      <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', marginBottom: '24px' }}>
        <div>
          <p style={{ fontSize: 11, fontWeight: 600, letterSpacing: '0.07em', textTransform: 'uppercase', color: colors.textMuted, marginBottom: 4 }}>
            {t('appName')}
          </p>
          <h1 style={{ fontSize: 22, fontWeight: 800, color: colors.textPrimary, margin: 0 }}>
            {t('employeesTitle')}
          </h1>
          <p style={{ fontSize: 13, color: colors.textSecondary, margin: 0, marginTop: 2 }}>
            {t('employeesSubtitle')}
          </p>
        </div>
        <button
          onClick={() => setOpen(true)}
          style={{
            background: colors.brandGradient,
            color: '#fff',
            border: 'none',
            borderRadius: 12,
            padding: '10px 20px',
            fontSize: 14,
            fontWeight: 600,
            cursor: 'pointer',
            display: 'flex',
            alignItems: 'center',
            gap: 6,
            boxShadow: `0 4px 14px ${colors.brandShadow}`,
            transition: 'transform 0.2s, box-shadow 0.2s',
          }}
          onMouseEnter={e => { e.currentTarget.style.transform = 'translateY(-2px)'; }}
          onMouseLeave={e => { e.currentTarget.style.transform = 'translateY(0)'; }}
        >
          + {t('addEmployee')}
        </button>
      </div>

      {error && <Alert>{error}</Alert>}

      {/* SEARCH BOX */}
      <div style={{ marginBottom: 16 }}>
        <input
          type="text"
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          placeholder={t('search for a specific employee') || 'Search by name, email, or location...'}
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
            {t('allEmployees')}
          </p>
          <span style={{
            background: colors.pillBg,
            color: colors.pillText,
            fontSize: 12,
            fontWeight: 600,
            padding: '3px 10px',
            borderRadius: 99,
          }}>
            {pagination?.totalCount ?? items.length} {t('members')}
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
                  {[t('fullName'), t('email'), t('mobile'), t('status'), t('action')].map((col) => (
                    <th
                      key={col}
                      style={{
                        padding: '12px 24px',
                        textAlign: isRTL ? 'right' : 'left',
                        fontSize: 11,
                        fontWeight: 600,
                        textTransform: 'uppercase',
                        letterSpacing: '0.06em',
                        color: colors.textMuted,
                        borderBottom: `1px solid ${colors.border}`,
                      }}
                    >
                      {col}
                    </th>
                  ))}
                </tr>
              </thead>
              <tbody>
                {items.length === 0 ? (
                  <tr>
                    <td colSpan={5} style={{ padding: '48px', textAlign: 'center', color: colors.textMuted, fontSize: 14 }}>
                      {t('noEmployeesFound')}
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
                            width: 36,
                            height: 36,
                            borderRadius: '50%',
                            background: 'linear-gradient(135deg, #1e40af, #3b82f6)',
                            display: 'flex',
                            alignItems: 'center',
                            justifyContent: 'center',
                            color: '#fff',
                            fontSize: 13,
                            fontWeight: 700,
                            flexShrink: 0,
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

                      {/* MOBILE */}
                      <td style={{ padding: '14px 24px', fontSize: 13, color: colors.textSecondary }}>
                        {u.mobileNumber || '—'}
                      </td>

                      {/* STATUS BADGE */}
                      <td style={{ padding: '14px 24px' }}>
                        <span style={{
                          display: 'inline-flex',
                          alignItems: 'center',
                          gap: 5,
                          padding: '4px 12px',
                          borderRadius: 99,
                          fontSize: 12,
                          fontWeight: 600,
                          background: u.isActive ? colors.successBg : colors.dangerBg,
                          color: u.isActive ? colors.successText : colors.dangerText,
                        }}>
                          <span style={{
                            width: 6,
                            height: 6,
                            borderRadius: '50%',
                            background: u.isActive ? colors.successDot : colors.dangerDot,
                            display: 'inline-block',
                          }} />
                          {u.isActive ? t('active') : t('inactive')}
                        </span>
                      </td>

                      {/* ACTION BUTTON */}
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

            <div style={{ padding: '12px 24px', borderTop: `1px solid ${colors.border}` }}>
              <Pagination pagination={pagination} onPageChange={setPage} />
            </div>
          </>
        )}
      </div>

      {/* ADD EMPLOYEE MODAL */}
      <Modal open={open} title={t('addSupportEmployee')} onClose={() => setOpen(false)}>
        <form onSubmit={create} style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 16 }}>
          <div style={{ gridColumn: '1 / -1' }}>
            <label style={{ fontSize: 12, fontWeight: 600, color: colors.textSecondary, display: 'block', marginBottom: 6 }}>
              {t('fullName')}
            </label>
            <input value={form.fullName} onChange={set('fullName')} required style={inputStyle} />
          </div>
          <div>
            <label style={{ fontSize: 12, fontWeight: 600, color: colors.textSecondary, display: 'block', marginBottom: 6 }}>
              {t('email')}
            </label>
            <input type="email" value={form.email} onChange={set('email')} required style={inputStyle} />
          </div>
          <div>
            <label style={{ fontSize: 12, fontWeight: 600, color: colors.textSecondary, display: 'block', marginBottom: 6 }}>
              {t('mobileNumber')}
            </label>
            <input value={form.mobileNumber} onChange={set('mobileNumber')} required style={inputStyle} />
          </div>
          <div style={{ gridColumn: '1 / -1' }}>
            <label style={{ fontSize: 12, fontWeight: 600, color: colors.textSecondary, display: 'block', marginBottom: 6 }}>
              {t('address')}
            </label>
            <input value={form.address} onChange={set('address')} style={inputStyle} />
          </div>
          <div style={{ gridColumn: '1 / -1' }}>
            <label style={{ fontSize: 12, fontWeight: 600, color: colors.textSecondary, display: 'block', marginBottom: 6 }}>
              {t('password')}
            </label>
            <input type="password" value={form.password} onChange={set('password')} required style={inputStyle} />
          </div>
          <div style={{ gridColumn: '1 / -1', display: 'flex', justifyContent: 'flex-end', gap: 10, marginTop: 4 }}>
            <button
              type="button"
              onClick={() => setOpen(false)}
              style={{
                padding: '9px 18px',
                borderRadius: 10,
                border: `1px solid ${colors.borderInput}`,
                background: colors.bgCard,
                color: colors.textSecondary,
                fontSize: 14,
                fontWeight: 600,
                cursor: 'pointer',
              }}
            >
              {t('cancel')}
            </button>
            <button
              type="submit"
              disabled={saving}
              style={{
                padding: '9px 20px',
                borderRadius: 10,
                border: 'none',
                background: saving ? '#93c5fd' : colors.brandGradient,
                color: '#fff',
                fontSize: 14,
                fontWeight: 600,
                cursor: saving ? 'not-allowed' : 'pointer',
                boxShadow: `0 4px 12px ${colors.brandShadow}`,
              }}
            >
              {saving ? t('saving') : t('createEmployee')}
            </button>
          </div>
        </form>
      </Modal>
    </div>
  );
}