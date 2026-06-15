import { useEffect, useState } from 'react';
import { getTicketStatus, getTopEmployees, getTicketTrend } from '../../api/dashboard';
import Card from '../../components/ui/Card.jsx';
import PageHeader from '../../components/ui/PageHeader.jsx';
import Spinner from '../../components/ui/Spinner.jsx';
import Alert from '../../components/ui/Alert.jsx';
import StatusChart from '../../components/charts/StatusChart.jsx';
import TrendChart from '../../components/charts/TrendChart.jsx';
import TopEmployeesChart from '../../components/charts/TopEmployeesChart.jsx';

export default function Dashboard() {
  const [status, setStatus] = useState([]);
  const [top, setTop] = useState([]);
  const [trend, setTrend] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    (async () => {
      try {
        const [s, t, tr] = await Promise.all([
          getTicketStatus(),
          getTopEmployees(),
          getTicketTrend(),
        ]);
        setStatus(s || []);
        setTop(t || []);
        setTrend(tr || []);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    })();
  }, []);

  if (loading) return <Spinner />;

  const total = status.reduce((sum, s) => sum + (s.count || 0), 0);
  const resolved = status.find((s) => s.status === 'Resolved')?.count || 0;
  const open = status
    .filter((s) => ['New', 'Assigned', 'InProgress'].includes(s.status))
    .reduce((sum, s) => sum + (s.count || 0), 0);

  const stats = [
    { label: 'Total tickets', value: total },
    { label: 'Open tickets', value: open },
    { label: 'Resolved', value: resolved },
    { label: 'Status types', value: status.length },
  ];

  return (
    <div>
      <PageHeader title="Dashboard" subtitle="Overview of support activity" />
      <Alert>{error}</Alert>

      <div className="mb-6 grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-4">
        {stats.map((s) => (
          <Card key={s.label}>
            <p className="text-sm text-gray-500">{s.label}</p>
            <p className="mt-2 text-3xl font-bold text-gray-800">{s.value}</p>
          </Card>
        ))}
      </div>

      <div className="grid grid-cols-1 gap-6 lg:grid-cols-2">
        <Card>
          <h3 className="mb-4 text-sm font-semibold text-gray-800">Tickets by status</h3>
          <StatusChart data={status} />
        </Card>
        <Card>
          <h3 className="mb-4 text-sm font-semibold text-gray-800">Most productive employees</h3>
          <TopEmployeesChart data={top} />
        </Card>
        <Card className="lg:col-span-2">
          <h3 className="mb-4 text-sm font-semibold text-gray-800">Tickets over time</h3>
          <TrendChart data={trend} />
        </Card>
      </div>
    </div>
  );
}
