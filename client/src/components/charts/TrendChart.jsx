import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';
import { formatDate } from '../../utils/format';

export default function TrendChart({ data }) {
  const chartData = (data || []).map((d) => ({ date: formatDate(d.date), count: d.count }));
  if (!chartData.length) return <p className="py-10 text-center text-gray-400">No data</p>;
  return (
    <ResponsiveContainer width="100%" height={260}>
      <LineChart data={chartData}>
        <CartesianGrid strokeDasharray="3 3" stroke="#f2f4f7" />
        <XAxis dataKey="date" tick={{ fontSize: 12, fill: '#667085' }} />
        <YAxis allowDecimals={false} tick={{ fontSize: 12, fill: '#667085' }} />
        <Tooltip />
        <Line type="monotone" dataKey="count" stroke="#465fff" strokeWidth={2} dot={{ r: 3 }} />
      </LineChart>
    </ResponsiveContainer>
  );
}
