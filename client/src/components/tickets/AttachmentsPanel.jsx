import { useEffect, useState } from 'react';
import { getAttachments, uploadAttachment } from '../../api/tickets';
import Button from '../ui/Button.jsx';
import Alert from '../ui/Alert.jsx';

export default function AttachmentsPanel({ ticketId, canUpload = true }) {
  const [items, setItems] = useState([]);
  const [file, setFile] = useState(null);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const load = async () => {
    try {
      const { items } = await getAttachments(ticketId, { pageNumber: 1, pageSize: 50 });
      setItems(items);
    } catch (err) {
      setError(err.message);
    }
  };

  useEffect(() => {
    load();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [ticketId]);

  const submit = async (e) => {
    e.preventDefault();
    if (!file) return;
    setLoading(true);
    setError('');
    try {
      await uploadAttachment(ticketId, file);
      setFile(null);
      e.target.reset();
      await load();
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h3 className="mb-3 text-sm font-semibold text-gray-800">Attachments</h3>
      <Alert>{error}</Alert>
      <ul className="mb-4 space-y-2">
        {items.length === 0 && <li className="text-sm text-gray-400">No attachments.</li>}
        {items.map((a) => (
          <li key={a.id} className="flex items-center gap-2 rounded-lg border border-gray-100 bg-gray-50 px-3 py-2 text-sm text-gray-700">
            <span>📎</span> {a.fileName}
          </li>
        ))}
      </ul>
      {canUpload && (
        <form onSubmit={submit} className="flex items-center gap-2">
          <input
            type="file"
            onChange={(e) => setFile(e.target.files?.[0] || null)}
            className="text-sm text-gray-600 file:mr-3 file:rounded-lg file:border-0 file:bg-brand-50 file:px-3 file:py-2 file:text-sm file:text-brand-600"
          />
          <Button type="submit" variant="secondary" disabled={loading || !file}>
            {loading ? '...' : 'Upload'}
          </Button>
        </form>
      )}
    </div>
  );
}
