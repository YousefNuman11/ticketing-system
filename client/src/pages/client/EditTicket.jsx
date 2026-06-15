import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getTicketById, updateTicket } from '../../api/tickets';
import { getProducts } from '../../api/products';
import PageHeader from '../../components/ui/PageHeader.jsx';
import Card from '../../components/ui/Card.jsx';
import Input from '../../components/ui/Input.jsx';
import Textarea from '../../components/ui/Textarea.jsx';
import Select from '../../components/ui/Select.jsx';
import Button from '../../components/ui/Button.jsx';
import Alert from '../../components/ui/Alert.jsx';
import Spinner from '../../components/ui/Spinner.jsx';

export default function EditTicket() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [products, setProducts] = useState([]);
  const [form, setForm] = useState({ title: '', description: '', productId: '' });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    (async () => {
      try {
        const [t, prods] = await Promise.all([
          getTicketById(id),
          getProducts({ pageNumber: 1, pageSize: 100 }),
        ]);
        setForm({ title: t.title, description: t.description, productId: t.productId });
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

  if (loading) return <Spinner />;

  return (
    <div className="mx-auto max-w-2xl">
      <PageHeader title="Edit Ticket" subtitle="Only new tickets can be edited" />
      <Card>
        <Alert>{error}</Alert>
        <form onSubmit={submit} className="space-y-4">
          <Select label="Product" value={form.productId} onChange={set('productId')} required>
            <option value="">Select a product</option>
            {products.map((p) => (
              <option key={p.id} value={p.id}>{p.name}</option>
            ))}
          </Select>
          <Input label="Title" value={form.title} onChange={set('title')} required />
          <Textarea label="Problem description" value={form.description} onChange={set('description')} rows={6} required />
          <div className="flex justify-end gap-2">
            <Button variant="secondary" type="button" onClick={() => navigate(`/client/tickets/${id}`)}>Cancel</Button>
            <Button type="submit" disabled={saving}>{saving ? 'Saving...' : 'Save changes'}</Button>
          </div>
        </form>
      </Card>
    </div>
  );
}
