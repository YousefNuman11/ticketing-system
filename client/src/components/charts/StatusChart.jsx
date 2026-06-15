import { PieChart, Pie, Cell, ResponsiveContainer, Legend, Tooltip } from 'recharts';

const COLORS = ['#465fff', '#f79009', '#7592ff', '#12b76a', '#98a2b3'];

export default function StatusChart({ data }) {
  const chartData = (data || []).map((d) => ({ name: d.status, value: d.count }));
  if (!chartData.length) return <p className="py-10 text-center text-gray-400">No data</p>;
  return (
    <ResponsiveContainer width="100%" height={260}>
      <PieChart>
        <Pie data={chartData} dataKey="value" nameKey="name" innerRadius={55} outerRadius={90} paddingAngle={2}>
          {chartData.map((_, i) => (
            <Cell key={i} fill={COLORS[i % COLORS.length]} />
          ))}
        </Pie>
        <Tooltip />
        <Legend />
      </PieChart>
    </ResponsiveContainer>
  );
}
