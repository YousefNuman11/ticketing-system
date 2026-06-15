import { useEffect, useState } from 'react';
import { getProducts, createProduct } from '../../api/products';
import PageHeader from '../../components/ui/PageHeader.jsx';
import Spinner from '../../components/ui/Spinner.jsx';
import Alert from '../../components/ui/Alert.jsx';
import Button from '../../components/ui/Button.jsx';
import Input from '../../components/ui/Input.jsx';
import Textarea from '../../components/ui/Textarea.jsx';
import Modal from '../../components/ui/Modal.jsx';
import { Table, EmptyRow } from '../../components/ui/Table.jsx';
import Pagination from '../../components/ui/Pagination.jsx';

const emptyForm = { name: '', description: '' };

export default function Products() {
  const [items, setItems] = useState([]);
  const [pagination, setPagination] = useState(null);
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [open, setOpen] = useState(false);
  const [form, setForm] = useState(emptyForm);
  const [saving, setSaving] = useState(false);

  const load = async () => {
    setLoading(true);
    try {
      const { items, pagination } = await getProducts({ pageNumber: page, pageSize: 10 });
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
  }, [page]);

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

  return (
    <div>
      <PageHeader
        title="Products"
        subtitle="Products clients can raise tickets against"
        action={<Button onClick={() => setOpen(true)}>+ Add product</Button>}
      />
      <Alert>{error}</Alert>
      {loading ? (
        <Spinner />
      ) : (
        <>
          <Table columns={['Name', 'Description', 'Status']}>
            {items.length === 0 ? (
              <EmptyRow colSpan={3} message="No products yet. Add one to let clients open tickets." />
            ) : (
              items.map((p) => (
                <tr key={p.id} className="hover:bg-gray-50">
                  <td className="px-5 py-4 font-medium text-gray-800">{p.name}</td>
                  <td className="px-5 py-4 text-gray-500">{p.description}</td>
                  <td className="px-5 py-4">
                    <span className={`inline-flex rounded-full px-2.5 py-0.5 text-xs font-medium ${p.isActive ? 'bg-success-50 text-success-700' : 'bg-gray-100 text-gray-600'}`}>
                      {p.isActive ? 'Active' : 'Inactive'}
                    </span>
                  </td>
                </tr>
              ))
            )}
          </Table>
          <Pagination pagination={pagination} onPageChange={setPage} />
        </>
      )}

      <Modal open={open} title="Add product" onClose={() => setOpen(false)}>
        <form onSubmit={create} className="space-y-4">
          <Input label="Name" value={form.name} onChange={set('name')} required />
          <Textarea label="Description" value={form.description} onChange={set('description')} rows={3} />
          <div className="flex justify-end gap-2">
            <Button variant="secondary" type="button" onClick={() => setOpen(false)}>Cancel</Button>
            <Button type="submit" disabled={saving}>{saving ? 'Saving...' : 'Create'}</Button>
          </div>
        </form>
      </Modal>
    </div>
  );
}
