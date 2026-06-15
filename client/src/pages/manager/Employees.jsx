import { useEffect, useState } from 'react';
import { getEmployees, createEmployee, toggleUserStatus } from '../../api/manager';
import PageHeader from '../../components/ui/PageHeader.jsx';
import Spinner from '../../components/ui/Spinner.jsx';
import Alert from '../../components/ui/Alert.jsx';
import Button from '../../components/ui/Button.jsx';
import Input from '../../components/ui/Input.jsx';
import Modal from '../../components/ui/Modal.jsx';
import { Table, EmptyRow } from '../../components/ui/Table.jsx';
import Pagination from '../../components/ui/Pagination.jsx';

const emptyForm = { fullName: '', email: '', mobileNumber: '', password: '', address: '' };

export default function Employees() {
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
      const { items, pagination } = await getEmployees({ pageNumber: page, pageSize: 10 });
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

  return (
    <div>
      <PageHeader
        title="Support Employees"
        subtitle="Manage your support team"
        action={<Button onClick={() => setOpen(true)}>+ Add employee</Button>}
      />
      <Alert>{error}</Alert>

      {loading ? (
        <Spinner />
      ) : (
        <>
          <Table columns={['Name', 'Email', 'Mobile', 'Status', 'Action']}>
            {items.length === 0 ? (
              <EmptyRow colSpan={5} />
            ) : (
              items.map((u) => (
                <tr key={u.id} className="hover:bg-gray-50">
                  <td className="px-5 py-4 font-medium text-gray-800">{u.fullName}</td>
                  <td className="px-5 py-4 text-gray-500">{u.email}</td>
                  <td className="px-5 py-4 text-gray-500">{u.mobileNumber || '-'}</td>
                  <td className="px-5 py-4">
                    <span className={`inline-flex rounded-full px-2.5 py-0.5 text-xs font-medium ${u.isActive ? 'bg-success-50 text-success-700' : 'bg-error-50 text-error-700'}`}>
                      {u.isActive ? 'Active' : 'Inactive'}
                    </span>
                  </td>
                  <td className="px-5 py-4">
                    <Button variant={u.isActive ? 'danger' : 'success'} onClick={() => toggle(u.id)}>
                      {u.isActive ? 'Deactivate' : 'Activate'}
                    </Button>
                  </td>
                </tr>
              ))
            )}
          </Table>
          <Pagination pagination={pagination} onPageChange={setPage} />
        </>
      )}

      <Modal open={open} title="Add support employee" onClose={() => setOpen(false)}>
        <form onSubmit={create} className="grid grid-cols-1 gap-4 sm:grid-cols-2">
          <Input label="Full name" value={form.fullName} onChange={set('fullName')} required className="sm:col-span-2" />
          <Input label="Email" type="email" value={form.email} onChange={set('email')} required />
          <Input label="Mobile number" value={form.mobileNumber} onChange={set('mobileNumber')} required />
          <Input label="Address" value={form.address} onChange={set('address')} className="sm:col-span-2" />
          <Input label="Password" type="password" value={form.password} onChange={set('password')} required className="sm:col-span-2" />
          <div className="flex justify-end gap-2 sm:col-span-2">
            <Button variant="secondary" type="button" onClick={() => setOpen(false)}>Cancel</Button>
            <Button type="submit" disabled={saving}>{saving ? 'Saving...' : 'Create'}</Button>
          </div>
        </form>
      </Modal>
    </div>
  );
}
