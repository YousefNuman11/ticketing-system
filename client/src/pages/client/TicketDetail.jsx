import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getTicketById, closeTicket } from '../../api/tickets';
import { shortId } from '../../utils/format';
import PageHeader from '../../components/ui/PageHeader.jsx';
import Card from '../../components/ui/Card.jsx';
import Spinner from '../../components/ui/Spinner.jsx';
import Alert from '../../components/ui/Alert.jsx';
import Button from '../../components/ui/Button.jsx';
import StatusBadge from '../../components/ui/StatusBadge.jsx';
import CommentsPanel from '../../components/tickets/CommentsPanel.jsx';
import AttachmentsPanel from '../../components/tickets/AttachmentsPanel.jsx';

export default function TicketDetail() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [ticket, setTicket] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [message, setMessage] = useState('');

  const load = async () => {
    setLoading(true);
    try {
      setTicket(await getTicketById(id));
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

  const close = async () => {
    setError('');
    setMessage('');
    try {
      await closeTicket(id);
      setMessage('Ticket closed. Thank you for confirming!');
      await load();
    } catch (err) {
      setError(err.message);
    }
  };

  if (loading) return <Spinner />;
  if (!ticket) return <Alert>{error || 'Ticket not found'}</Alert>;

  const canEdit = ticket.status === 'New';
  const canClose = ticket.status === 'Resolved';

  return (
    <div>
      <PageHeader
        title={`Ticket #${shortId(ticket.id)}`}
        action={
          <div className="flex gap-2">
            {canEdit && (
              <Button variant="secondary" onClick={() => navigate(`/client/tickets/${id}/edit`)}>
                Edit
              </Button>
            )}
            {canClose && <Button variant="success" onClick={close}>Close ticket</Button>}
            <Button variant="ghost" onClick={() => navigate('/client')}>Back</Button>
          </div>
        }
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
          <Card><CommentsPanel ticketId={id} canComment /></Card>
        </div>
        <div>
          <Card><AttachmentsPanel ticketId={id} canUpload /></Card>
        </div>
      </div>
    </div>
  );
}
