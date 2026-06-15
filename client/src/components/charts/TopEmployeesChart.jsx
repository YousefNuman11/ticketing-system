import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';

export default function TopEmployeesChart({ data }) {
  const chartData = (data || []).map((d) => ({
    name: d.employeeName,
    resolved: d.resolvedCount,
  }));
  if (!chartData.length) return <p className="py-10 text-center text-gray-400">No data</p>;
  return (
    <ResponsiveContainer width="100%" height={260}>
      <BarChart data={chartData}>
        <CartesianGrid strokeDasharray="3 3" stroke="#f2f4f7" />
        <XAxis dataKey="name" tick={{ fontSize: 12, fill: '#667085' }} />
        <YAxis allowDecimals={false} tick={{ fontSize: 12, fill: '#667085' }} />
        <Tooltip />
        <Bar dataKey="resolved" fill="#465fff" radius={[6, 6, 0, 0]} />
      </BarChart>
    </ResponsiveContainer>
  );
}
