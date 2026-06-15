import { useEffect, useState } from 'react';
import { getComments, addComment } from '../../api/tickets';
import { formatDate } from '../../utils/format';
import Button from '../ui/Button.jsx';
import Alert from '../ui/Alert.jsx';

export default function CommentsPanel({ ticketId, canComment = true }) {
  const [comments, setComments] = useState([]);
  const [text, setText] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const load = async () => {
    try {
      const { items } = await getComments(ticketId, { pageNumber: 1, pageSize: 50 });
      setComments(items);
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
    if (!text.trim()) return;
    setLoading(true);
    setError('');
    try {
      await addComment(ticketId, text.trim());
      setText('');
      await load();
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h3 className="mb-3 text-sm font-semibold text-gray-800">Comments</h3>
      <Alert>{error}</Alert>
      <div className="mb-4 space-y-3">
        {comments.length === 0 && (
          <p className="text-sm text-gray-400">No comments yet.</p>
        )}
        {comments.map((c) => (
          <div key={c.id} className="rounded-lg border border-gray-100 bg-gray-50 p-3">
            <p className="text-sm text-gray-700">{c.text}</p>
            <p className="mt-1 text-xs text-gray-400">{formatDate(c.createdAt)}</p>
          </div>
        ))}
      </div>
      {canComment && (
        <form onSubmit={submit} className="flex gap-2">
          <input
            value={text}
            onChange={(e) => setText(e.target.value)}
            placeholder="Write a comment..."
            className="form-input flex-1"
          />
          <Button type="submit" disabled={loading}>
            {loading ? '...' : 'Send'}
          </Button>
        </form>
      )}
    </div>
  );
}
