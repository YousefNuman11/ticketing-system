import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getTicketDetails, getEmployees, assignTicket } from '../../api/manager';
import { shortId } from '../../utils/format';
import PageHeader from '../../components/ui/PageHeader.jsx';
import Card from '../../components/ui/Card.jsx';
import Spinner from '../../components/ui/Spinner.jsx';
import Alert from '../../components/ui/Alert.jsx';
import StatusBadge from '../../components/ui/StatusBadge.jsx';
import Select from '../../components/ui/Select.jsx';
import Button from '../../components/ui/Button.jsx';
import CommentsPanel from '../../components/tickets/CommentsPanel.jsx';
import AttachmentsPanel from '../../components/tickets/AttachmentsPanel.jsx';

export default function TicketDetails() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [ticket, setTicket] = useState(null);
  const [employees, setEmployees] = useState([]);
  const [employeeId, setEmployeeId] = useState('');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [message, setMessage] = useState('');
  const [saving, setSaving] = useState(false);

  const load = async () => {
    setLoading(true);
    try {
      const [t, emps] = await Promise.all([
        getTicketDetails(id),
        getEmployees({ pageNumber: 1, pageSize: 100 }),
      ]);
      setTicket(t);
      setEmployees(emps.items);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    load();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [id]);

  const assign = async () => {
    if (!employeeId) return;
    setSaving(true);
    setError('');
    setMessage('');
    try {
      await assignTicket(id, employeeId);
      setMessage('Ticket assigned successfully.');
      await load();
    } catch (err) {
      setError(err.message);
    } finally {
      setSaving(false);
    }
  };

  if (loading) return <Spinner />;
  if (!ticket) return <Alert>{error || 'Ticket not found'}</Alert>;

  const assigned = Boolean(ticket.assignedEmployeeId);

  return (
    <div>
      <PageHeader
        title={`Ticket #${shortId(ticket.id)}`}
        action={<Button variant="secondary" onClick={() => navigate('/manager/tickets')}>Back</Button>}
      />
      <Alert>{error}</Alert>
      <Alert type="success">{message}</Alert>

      <div className="grid grid-cols-1 gap-6 lg:grid-cols-3">
        <div className="space-y-6 lg:col-span-2">
          <Card>
            <div className="mb-3 flex items-center justify-between">
              <h2 className="text-lg font-semibold text-gray-800">{ticket.title}</h2>
              <StatusBadge status={ticket.status} />
            </div>
            <p className="text-sm text-gray-600">{ticket.description}</p>
          </Card>
          <Card><CommentsPanel ticketId={id} canComment={false} /></Card>
          <Card><AttachmentsPanel ticketId={id} canUpload={false} /></Card>
        </div>

        <div className="space-y-6">
          <Card>
            <h3 className="mb-4 text-sm font-semibold text-gray-800">Assignment</h3>
            {assigned ? (
              <p className="text-sm text-gray-600">
                Assigned to employee{' '}
                <span className="font-mono text-xs">#{shortId(ticket.assignedEmployeeId)}</span>.
                Once assigned, a ticket cannot be reassigned.
              </p>
            ) : (
              <div className="space-y-3">
                <Select
                  label="Assign to employee"
                  value={employeeId}
                  onChange={(e) => setEmployeeId(e.target.value)}
                >
                  <option value="">Select an employee</option>
                  {employees.map((emp) => (
                    <option key={emp.id} value={emp.id}>
                      {emp.fullName} ({emp.email})
                    </option>
                  ))}
                </Select>
                <Button onClick={assign} disabled={saving || !employeeId} className="w-full">
                  {saving ? 'Assigning...' : 'Assign ticket'}
                </Button>
              </div>
            )}
          </Card>
        </div>
      </div>
    </div>
  );
}
