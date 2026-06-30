import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getTicketById, updateTicket } from '../../api/tickets';
import { getProducts } from '../../api/products';
import Spinner from '../../components/ui/Spinner.jsx';
import { useTheme } from '../../context/ThemeContext.jsx';
import { useLanguage } from '../../context/LanguageContext.jsx';

export default function EditTicket() {
  const { id } = useParams();
  const navigate = useNavigate();
  const { colors } = useTheme();
  const { t, isRTL } = useLanguage();

  const [products, setProducts] = useState([]);
  const [form, setForm] = useState({ title: '', description: '', productId: '' });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    (async () => {
      try {
        const [tk, prods] = await Promise.all([
          getTicketById(id),
          getProducts({ pageNumber: 1, pageSize: 100 }),
        ]);
        setForm({ title: tk.title, description: tk.description, productId: tk.productId });
        setProducts(prods.items);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    })();
  }, [id]);

  const set = (k) => (e) => setForm({ ...form, [k]: e.target.value });

  const submit = async (e) => {
    e.preventDefault();
    setSaving(true);
    setError('');
    try {
      await updateTicket(id, form);
      navigate(`/client/tickets/${id}`);
    } catch (err) {
      setError(err.message);
    } finally {
      setSaving(false);
    }
  };

  const inputStyle = {
    width: '100%',
    padding: '10px 12px',
    borderRadius: 10,
    border: `1px solid ${colors.borderInput}`,
    fontSize: 13,
    fontWeight: 500,
    color: colors.textPrimary,
    background: colors.inputBg,
    outline: 'none',
    boxSizing: 'border-box',
    fontFamily: 'inherit',
  };

  const labelStyle = {
    fontSize: 12,
    fontWeight: 600,
    color: colors.textSecondary,
    display: 'block',
    marginBottom: 6,
  };

  if (loading) {
    return (
      <div style={{ height: '80vh', display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
        <Spinner />
      </div>
    );
  }

  return (
    <div dir={isRTL ? 'rtl' : 'ltr'} style={{ padding: '24px', backgroundColor: colors.bgPage, minHeight: '100vh' }}>
      <div style={{ maxWidth: 640, margin: '0 auto' }}>

        {/* HEADER */}
        <div style={{ marginBottom: 24 }}>
          <p style={{
            fontSize: 11, fontWeight: 600, letterSpacing: '0.07em',
            textTransform: 'uppercase', color: colors.textMuted, marginBottom: 4,
          }}>
            {t('appName')}
          </p>
          <h1 style={{ fontSize: 22, fontWeight: 800, color: colors.textPrimary, margin: 0 }}>
            {t('editTicketTitle')}
          </h1>
          <p style={{ fontSize: 13, color: colors.textSecondary, margin: 0, marginTop: 2 }}>
            {t('editTicketSubtitle')}
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

        {/* FORM CARD */}
        <div style={{
          backgroundColor: colors.bgCard,
          borderRadius: 16,
          border: `1px solid ${colors.border}`,
          padding: '28px',
        }}>
          <form onSubmit={submit} style={{ display: 'flex', flexDirection: 'column', gap: 18 }}>

            {/* PRODUCT */}
            <div>
              <label style={labelStyle}>{t('product')}</label>
              <select
                value={form.productId}
                onChange={set('productId')}
                required
                style={{ ...inputStyle, cursor: 'pointer' }}
              >
                <option value="">{t('selectProduct')}</option>
                {products.map((p) => (
                  <option key={p.id} value={p.id}>{p.name}</option>
                ))}
              </select>
            </div>

            {/* TITLE */}
            <div>
              <label style={labelStyle}>{t('title')}</label>
              <input
                value={form.title}
                onChange={set('title')}
                required
                style={inputStyle}
              />
            </div>

            {/* DESCRIPTION */}
            <div>
              <label style={labelStyle}>{t('problemDescription')}</label>
              <textarea
                value={form.description}
                onChange={set('description')}
                rows={6}
                required
                style={{ ...inputStyle, resize: 'vertical' }}
              />
            </div>

            {/* ACTIONS */}
            <div style={{ display: 'flex', justifyContent: 'flex-end', gap: 10, marginTop: 8 }}>
              <button
                type="button"
                onClick={() => navigate(`/client/tickets/${id}`)}
                style={{
                  padding: '10px 20px',
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
                  padding: '10px 22px',
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
                {saving ? t('saving') : t('saveChanges')}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}