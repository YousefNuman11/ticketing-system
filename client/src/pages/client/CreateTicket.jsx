import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { createTicket, uploadAttachment } from '../../api/tickets';
import { getProducts } from '../../api/products';
import PageHeader from '../../components/ui/PageHeader.jsx';
import Card from '../../components/ui/Card.jsx';
import Input from '../../components/ui/Input.jsx';
import Textarea from '../../components/ui/Textarea.jsx';
import Select from '../../components/ui/Select.jsx';
import Button from '../../components/ui/Button.jsx';
import Alert from '../../components/ui/Alert.jsx';

export default function CreateTicket() {
  const navigate = useNavigate();
  const [products, setProducts] = useState([]);
  const [form, setForm] = useState({ title: '', description: '', productId: '' });
  const [file, setFile] = useState(null);
  const [error, setError] = useState('');
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    (async () => {
      try {
        const { items } = await getProducts({ pageNumber: 1, pageSize: 100 });
        setProducts(items);
      } catch (err) {
        setError(err.message);
      }
    })();
  }, []);

  const set = (k) => (e) => setForm({ ...form, [k]: e.target.value });

  const submit = async (e) => {
    e.preventDefault();
    setSaving(true);
    setError('');
    try {
      const ticket = await createTicket(form);
      if (file && ticket?.id) {
        await uploadAttachment(ticket.id, file);
      }
      navigate(`/client/tickets/${ticket.id}`);
    } catch (err) {
      setError(err.message);
    } finally {
      setSaving(false);
    }
  };

  return (
    <div className="mx-auto max-w-2xl">
      <PageHeader title="New Ticket" subtitle="Describe the issue you are facing" />
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
          <div>
            <label className="form-label">Attachment (optional)</label>
            <input
              type="file"
              onChange={(e) => setFile(e.target.files?.[0] || null)}
              className="text-sm text-gray-600 file:mr-3 file:rounded-lg file:border-0 file:bg-brand-50 file:px-3 file:py-2 file:text-sm file:text-brand-600"
            />
          </div>
          <div className="flex justify-end gap-2">
            <Button variant="secondary" type="button" onClick={() => navigate('/client')}>Cancel</Button>
            <Button type="submit" disabled={saving}>{saving ? 'Submitting...' : 'Submit ticket'}</Button>
          </div>
        </form>
      </Card>
    </div>
  );
}
