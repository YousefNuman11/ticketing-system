import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { getMyTickets } from '../../api/tickets';
import { shortId } from '../../utils/format';
import PageHeader from '../../components/ui/PageHeader.jsx';
import Spinner from '../../components/ui/Spinner.jsx';
import Alert from '../../components/ui/Alert.jsx';
import Button from '../../components/ui/Button.jsx';
import { Table, EmptyRow } from '../../components/ui/Table.jsx';
import Pagination from '../../components/ui/Pagination.jsx';
import StatusBadge from '../../components/ui/StatusBadge.jsx';

export default function MyTickets() {
  const navigate = useNavigate();
  const [items, setItems] = useState([]);
  const [pagination, setPagination] = useState(null);
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    (async () => {
      setLoading(true);
      try {
        const { items, pagination } = await getMyTickets({ pageNumber: page, pageSize: 10 });
        setItems(items);
        setPagination(pagination);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    })();
  }, [page]);

  return (
    <div>
      <PageHeader
        title="My Tickets"
        subtitle="Your submitted support tickets"
        action={<Button onClick={() => navigate('/client/new')}>+ New ticket</Button>}
      />
      <Alert>{error}</Alert>
      {loading ? (
        <Spinner />
      ) : (
        <>
          <Table columns={['Ticket', 'Title', 'Status', 'Action']}>
            {items.length === 0 ? (
              <EmptyRow colSpan={4} message="You have no tickets yet." />
            ) : (
              items.map((t) => (
                <tr key={t.id} className="hover:bg-gray-50">
                  <td className="px-5 py-4 font-mono text-xs text-gray-500">#{shortId(t.id)}</td>
                  <td className="px-5 py-4 font-medium text-gray-800">{t.title}</td>
                  <td className="px-5 py-4"><StatusBadge status={t.status} /></td>
                  <td className="px-5 py-4">
                    <Button variant="secondary" onClick={() => navigate(`/client/tickets/${t.id}`)}>
                      View
                    </Button>
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
