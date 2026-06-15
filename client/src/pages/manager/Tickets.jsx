import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { getAllTickets } from '../../api/manager';
import { TICKET_STATUS } from '../../utils/constants';
import { shortId } from '../../utils/format';
import PageHeader from '../../components/ui/PageHeader.jsx';
import Card from '../../components/ui/Card.jsx';
import Select from '../../components/ui/Select.jsx';
import Spinner from '../../components/ui/Spinner.jsx';
import Alert from '../../components/ui/Alert.jsx';
import { Table, EmptyRow } from '../../components/ui/Table.jsx';
import Pagination from '../../components/ui/Pagination.jsx';
import StatusBadge from '../../components/ui/StatusBadge.jsx';
import Button from '../../components/ui/Button.jsx';

export default function Tickets() {
  const navigate = useNavigate();
  const [items, setItems] = useState([]);
  const [pagination, setPagination] = useState(null);
  const [status, setStatus] = useState('');
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  const load = async () => {
    setLoading(true);
    setError('');
    try {
      const params = { pageNumber: page, pageSize: 10 };
      if (status) params.status = status;
      const { items, pagination } = await getAllTickets(params);
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
  }, [page, status]);

  return (
    <div>
      <PageHeader title="All Tickets" subtitle="Track and assign support tickets" />
      <Alert>{error}</Alert>
      <Card className="mb-4">
        <div className="flex flex-wrap items-end gap-4">
          <Select
            label="Filter by status"
            value={status}
            onChange={(e) => { setPage(1); setStatus(e.target.value); }}
            className="w-56"
          >
            <option value="">All statuses</option>
            {TICKET_STATUS.map((s) => (
              <option key={s} value={s}>{s}</option>
            ))}
          </Select>
        </div>
      </Card>

      {loading ? (
        <Spinner />
      ) : (
        <>
          <Table columns={['Ticket', 'Title', 'Status', 'Assigned', 'Action']}>
            {items.length === 0 ? (
              <EmptyRow colSpan={5} />
            ) : (
              items.map((t) => (
                <tr key={t.id} className="hover:bg-gray-50">
                  <td className="px-5 py-4 font-mono text-xs text-gray-500">#{shortId(t.id)}</td>
                  <td className="px-5 py-4 font-medium text-gray-800">{t.title}</td>
                  <td className="px-5 py-4"><StatusBadge status={t.status} /></td>
                  <td className="px-5 py-4 text-gray-500">
                    {t.assignedEmployeeId ? shortId(t.assignedEmployeeId) : 'Unassigned'}
                  </td>
                  <td className="px-5 py-4">
                    <Button variant="secondary" onClick={() => navigate(`/manager/tickets/${t.id}`)}>
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
