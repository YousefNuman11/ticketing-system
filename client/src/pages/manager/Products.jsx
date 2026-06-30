import { useEffect, useState } from 'react';
import { getProducts, createProduct } from '../../api/products';
import Spinner from '../../components/ui/Spinner.jsx';
import Modal from '../../components/ui/Modal.jsx';
import Pagination from '../../components/ui/Pagination.jsx';
import { useTheme } from '../../context/ThemeContext.jsx';
import { useLanguage } from '../../context/LanguageContext.jsx';

const emptyForm = { name: '', description: '' };

export default function Products() {
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
      const { items, pagination } = await getProducts({
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
      await createProduct(form);
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
      <div style={{ display: 'flex', alignItems: 'flex-start', justifyContent: 'space-between', marginBottom: 24 }}>
        <div>
          <p style={{
            fontSize: 11, fontWeight: 600, letterSpacing: '0.07em',
            textTransform: 'uppercase', color: colors.textMuted, marginBottom: 4,
          }}>
            {t('appName')}
          </p>
          <h1 style={{ fontSize: 22, fontWeight: 800, color: colors.textPrimary, margin: 0 }}>
            {t('productsTitle')}
          </h1>
          <p style={{ fontSize: 13, color: colors.textSecondary, margin: 0, marginTop: 2 }}>
            {t('productsSubtitle')}
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
          + {t('addProduct')}
        </button>
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
          placeholder={t('searchProductsPlaceholder') || 'Search by product name...'}
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
            {t('allProducts')}
          </p>
          <span style={{
            background: colors.pillBg, color: colors.pillText,
            fontSize: 12, fontWeight: 600,
            padding: '3px 10px', borderRadius: 99,
          }}>
            {pagination?.totalCount ?? items.length} {t('productsLabel')}
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
                  {[t('product'), t('description'), t('status')].map((col) => (
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
                    <td colSpan={3} style={{ padding: '48px', textAlign: 'center' }}>
                      <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', gap: 8 }}>
                        <div style={{
                          width: 48, height: 48, borderRadius: 12,
                          background: colors.pillBg,
                          display: 'flex', alignItems: 'center', justifyContent: 'center',
                          fontSize: 22,
                        }}>
                          📦
                        </div>
                        <p style={{ fontSize: 14, fontWeight: 600, color: colors.textPrimary, margin: 0 }}>
                          {t('noProductsYet')}
                        </p>
                        <p style={{ fontSize: 13, color: colors.textMuted, margin: 0 }}>
                          {t('addProductHint')}
                        </p>
                      </div>
                    </td>
                  </tr>
                ) : (
                  items.map((p, idx) => (
                    <tr
                      key={p.id}
                      style={{
                        borderBottom: idx < items.length - 1 ? `1px solid ${colors.borderLight}` : 'none',
                        transition: 'background 0.15s',
                      }}
                      onMouseEnter={e => e.currentTarget.style.background = colors.bgCardHover}
                      onMouseLeave={e => e.currentTarget.style.background = 'transparent'}
                    >
                      {/* PRODUCT NAME + ICON */}
                      <td style={{ padding: '14px 24px' }}>
                        <div style={{ display: 'flex', alignItems: 'center', gap: 10 }}>
                          <div style={{
                            width: 36, height: 36, borderRadius: 10,
                            background: 'linear-gradient(135deg, #059669, #34d399)',
                            display: 'flex', alignItems: 'center', justifyContent: 'center',
                            color: '#fff', fontSize: 15, fontWeight: 700, flexShrink: 0,
                          }}>
                            {p.name?.charAt(0)?.toUpperCase() || 'P'}
                          </div>
                          <span style={{ fontWeight: 600, color: colors.textPrimary, fontSize: 14 }}>
                            {p.name}
                          </span>
                        </div>
                      </td>

                      {/* DESCRIPTION */}
                      <td style={{ padding: '14px 24px', fontSize: 13, color: colors.textSecondary, maxWidth: 360 }}>
                        <span style={{
                          display: '-webkit-box',
                          WebkitLineClamp: 1,
                          WebkitBoxOrient: 'vertical',
                          overflow: 'hidden',
                        }}>
                          {p.description || '—'}
                        </span>
                      </td>

                      {/* STATUS */}
                      <td style={{ padding: '14px 24px' }}>
                        <span style={{
                          display: 'inline-flex', alignItems: 'center', gap: 5,
                          padding: '4px 12px', borderRadius: 99,
                          fontSize: 12, fontWeight: 600,
                          background: p.isActive ? colors.successBg : colors.bgSecondary,
                          color: p.isActive ? colors.successText : colors.textSecondary,
                        }}>
                          <span style={{
                            width: 6, height: 6, borderRadius: '50%',
                            background: p.isActive ? colors.successDot : colors.textMuted,
                          }} />
                          {p.isActive ? t('active') : t('inactive')}
                        </span>
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

      {/* ADD PRODUCT MODAL */}
      <Modal open={open} title={t('addProductModal')} onClose={() => setOpen(false)}>
        <form onSubmit={create} style={{ display: 'flex', flexDirection: 'column', gap: 16 }}>

          <div>
            <label style={{ fontSize: 12, fontWeight: 600, color: colors.textSecondary, display: 'block', marginBottom: 6 }}>
              {t('productName')}
            </label>
            <input
              value={form.name}
              onChange={set('name')}
              required
              placeholder="e.g. Help Desk Pro"
              style={inputStyle}
            />
          </div>

          <div>
            <label style={{ fontSize: 12, fontWeight: 600, color: colors.textSecondary, display: 'block', marginBottom: 6 }}>
              {t('description')}
            </label>
            <textarea
              value={form.description}
              onChange={set('description')}
              rows={3}
              placeholder="Brief description of this product..."
              style={{ ...inputStyle, resize: 'vertical', fontFamily: 'inherit' }}
            />
          </div>

          <div style={{ display: 'flex', justifyContent: 'flex-end', gap: 10, marginTop: 4 }}>
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
                boxShadow: saving ? 'none' : `0 4px 12px ${colors.brandShadow}`,
              }}
            >
              {saving ? t('saving') : t('createProduct')}
            </button>
          </div>
        </form>
      </Modal>
    </div>
  );
}