import { useEffect, useState } from 'react';
import { getClientsWithTickets, toggleUserStatus } from '../../api/manager';
import PageHeader from '../../components/ui/PageHeader.jsx';
import Spinner from '../../components/ui/Spinner.jsx';
import Alert from '../../components/ui/Alert.jsx';
import Button from '../../components/ui/Button.jsx';
import { Table, EmptyRow } from '../../components/ui/Table.jsx';
import Pagination from '../../components/ui/Pagination.jsx';

export default function Clients() {
  const [items, setItems] = useState([]);
  const [pagination, setPagination] = useState(null);
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  const load = async () => {
    setLoading(true);
    try {
      const { items, pagination } = await getClientsWithTickets({ pageNumber: page, pageSize: 10 });
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

  const toggle = async (id) => {
    try {
      await toggleUserStatus(id);
      await load();
    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <div>
      <PageHeader title="Clients" subtitle="External clients and their tickets" />
      <Alert>{error}</Alert>

      {loading ? (
        <Spinner />
      ) : (
        <>
          <Table columns={['Name', 'Email', 'Tickets', 'Action']}>
            {items.length === 0 ? (
              <EmptyRow colSpan={4} />
            ) : (
              items.map((c) => (
                <tr key={c.id} className="hover:bg-gray-50">
                  <td className="px-5 py-4 font-medium text-gray-800">{c.fullName}</td>
                  <td className="px-5 py-4 text-gray-500">{c.email}</td>
                  <td className="px-5 py-4">
                    <span className="inline-flex rounded-full bg-brand-50 px-2.5 py-0.5 text-xs font-medium text-brand-600">
                      {c.tickets?.length || 0} tickets
                    </span>
                  </td>
                  <td className="px-5 py-4">
                    <Button variant="danger" onClick={() => toggle(c.id)}>Deactivate</Button>
                  </td>
                </tr>
              ))
            )}
          </Table>
          <Pagination pagination={pagination} onPageChange={setPage} />
        </>
      )}
    </div>
  );
}
