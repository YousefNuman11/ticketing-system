import { useEffect, useState } from 'react';
import { Box, Paper, Typography, Alert, Stack } from '@mui/material';

import AssignmentTurnedInIcon from '@mui/icons-material/AssignmentTurnedIn';
import HourglassTopIcon from '@mui/icons-material/HourglassTop';
import DoneAllIcon from '@mui/icons-material/DoneAll';
import CategoryIcon from '@mui/icons-material/Category';

import {
  getTicketStatus,
  getTopEmployees,
  getTicketTrend,
} from '../../api/dashboard';

import StatusChart from '../../components/charts/StatusChart.jsx';
import TrendChart from '../../components/charts/TrendChart.jsx';
import TopEmployeesChart from '../../components/charts/TopEmployeesChart.jsx';

import { useTheme } from '../../context/ThemeContext.jsx';
import { useLanguage } from '../../context/LanguageContext.jsx';

const KPI_GRADIENTS = [
  { light: 'linear-gradient(135deg, #1e40af 0%, #3b82f6 100%)', dark: 'linear-gradient(135deg, #1e3a8a 0%, #2563eb 100%)' },
  { light: 'linear-gradient(135deg, #d97706 0%, #f59e0b 100%)', dark: 'linear-gradient(135deg, #92400e 0%, #d97706 100%)' },
  { light: 'linear-gradient(135deg, #059669 0%, #34d399 100%)', dark: 'linear-gradient(135deg, #065f46 0%, #059669 100%)' },
  { light: 'linear-gradient(135deg, #7c3aed 0%, #a78bfa 100%)', dark: 'linear-gradient(135deg, #5b21b6 0%, #7c3aed 100%)' },
];

export default function Dashboard() {
  const { colors, mode } = useTheme();
  const { t, isRTL } = useLanguage();

  const [status, setStatus] = useState([]);
  const [top, setTop] = useState([]);
  const [trend, setTrend] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  const [animatedTotal, setAnimatedTotal] = useState(0);
  const [animatedOpen, setAnimatedOpen] = useState(0);
  const [animatedResolved, setAnimatedResolved] = useState(0);
  const [animatedTypes, setAnimatedTypes] = useState(0);

  useEffect(() => {
    (async () => {
      try {
        const [s, t2, tr] = await Promise.all([
          getTicketStatus(),
          getTopEmployees(),
          getTicketTrend(),
        ]);
        setStatus(s || []);
        setTop(t2 || []);
        setTrend(tr || []);
      } catch (err) {
        setError(err.message || 'Failed to load dashboard');
      } finally {
        setLoading(false);
      }
    })();
  }, []);

  const total = status.reduce((sum, s) => sum + (s.count || 0), 0);
  const resolved = status.find((s) => s.status === 'Resolved')?.count || 0;
  const open = status
    .filter((s) => ['New', 'Assigned', 'InProgress'].includes(s.status))
    .reduce((sum, s) => sum + (s.count || 0), 0);
  const types = status.length;

  useEffect(() => {
    const duration = 800;
    const steps = 30;
    let i = 0;
    const interval = setInterval(() => {
      i++;
      setAnimatedTotal(Math.round((total * i) / steps));
      setAnimatedOpen(Math.round((open * i) / steps));
      setAnimatedResolved(Math.round((resolved * i) / steps));
      setAnimatedTypes(Math.round((types * i) / steps));
      if (i >= steps) clearInterval(interval);
    }, duration / steps);
    return () => clearInterval(interval);
  }, [total, open, resolved, types]);

  const stats = [
    { label: t('totalTickets'), value: animatedTotal, icon: <AssignmentTurnedInIcon sx={{ color: '#fff', fontSize: 28 }} /> },
    { label: t('openTickets'), value: animatedOpen, icon: <HourglassTopIcon sx={{ color: '#fff', fontSize: 28 }} /> },
    { label: t('resolved'), value: animatedResolved, icon: <DoneAllIcon sx={{ color: '#fff', fontSize: 28 }} /> },
    { label: t('statusTypes'), value: animatedTypes, icon: <CategoryIcon sx={{ color: '#fff', fontSize: 28 }} /> },
  ];

  if (loading) {
    return (
      <Box sx={{ height: '80vh', display: 'flex', justifyContent: 'center', alignItems: 'center', backgroundColor: colors.bgPage }}>
        <Typography sx={{ color: colors.textSecondary }}>{t('loading')}</Typography>
      </Box>
    );
  }

  return (
    <Box dir={isRTL ? 'rtl' : 'ltr'} sx={{ p: 3, backgroundColor: colors.bgPage, minHeight: '100vh' }}>

      {error && (
        <Alert severity="error" sx={{ mb: 3, borderRadius: 3 }}>
          {error}
        </Alert>
      )}

      {/* SECTION LABEL */}
      <Typography
        sx={{
          fontSize: 11,
          fontWeight: 600,
          letterSpacing: '0.07em',
          textTransform: 'uppercase',
          color: colors.textMuted,
          mb: 1.5,
        }}
      >
        {t('overview')}
      </Typography>

      {/* KPI CARDS */}
      <Box
        sx={{
          display: 'grid',
          gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))',
          gap: 2,
          mb: 3,
        }}
      >
        {stats.map((s, i) => (
          <Box
            key={s.label}
            sx={{
              background: mode === 'dark' ? KPI_GRADIENTS[i].dark : KPI_GRADIENTS[i].light,
              borderRadius: 4,
              p: 3,
              position: 'relative',
              overflow: 'hidden',
              transition: 'transform 0.2s, box-shadow 0.2s',
              '&:hover': {
                transform: 'translateY(-4px)',
                boxShadow: '0 12px 32px rgba(0,0,0,0.25)',
              },
            }}
          >
            <Box
              sx={{
                position: 'absolute',
                right: isRTL ? 'auto' : -16,
                left: isRTL ? -16 : 'auto',
                bottom: -16,
                width: 80,
                height: 80,
                borderRadius: '50%',
                background: 'rgba(255,255,255,0.1)',
              }}
            />
            <Box
              sx={{
                width: 44,
                height: 44,
                borderRadius: 2.5,
                background: 'rgba(255,255,255,0.18)',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                mb: 2,
              }}
            >
              {s.icon}
            </Box>
            <Typography
              sx={{
                fontSize: 11,
                fontWeight: 600,
                letterSpacing: '0.05em',
                textTransform: 'uppercase',
                color: 'rgba(255,255,255,0.8)',
                mb: 0.5,
              }}
            >
              {s.label}
            </Typography>
            <Typography
              sx={{
                fontSize: 36,
                fontWeight: 800,
                color: '#fff',
                lineHeight: 1,
              }}
            >
              {s.value}
            </Typography>
          </Box>
        ))}
      </Box>

      {/* STATUS + TOP EMPLOYEES */}
      <Box
        sx={{
          display: 'grid',
          gridTemplateColumns: 'repeat(auto-fit, minmax(400px, 1fr))',
          gap: 2,
          mb: 2,
        }}
      >
        <Paper
          elevation={0}
          sx={{
            p: 3,
            borderRadius: 4,
            height: 500,
            display: 'flex',
            flexDirection: 'column',
            border: `1px solid ${colors.border}`,
            backgroundColor: colors.bgCard,
          }}
        >
          <Typography sx={{ fontWeight: 700, fontSize: 15, color: colors.textPrimary, mb: 2 }}>
            {t('ticketsByStatus')}
          </Typography>
          <Box sx={{ flex: 1, minWidth: 0, minHeight: 0 }}>
            <StatusChart data={status} animated />
          </Box>
        </Paper>

        <Paper
          elevation={0}
          sx={{
            p: 3,
            borderRadius: 4,
            height: 500,
            display: 'flex',
            flexDirection: 'column',
            border: `1px solid ${colors.border}`,
            backgroundColor: colors.bgCard,
          }}
        >
          <Typography sx={{ fontWeight: 700, fontSize: 15, color: colors.textPrimary, mb: 2 }}>
            {t('topEmployees')}
          </Typography>
          <Box sx={{ flex: 1, minWidth: 0, minHeight: 0 }}>
            <TopEmployeesChart data={top} animated />
          </Box>
        </Paper>
      </Box>

      {/* TREND */}
      <Paper
        elevation={0}
        sx={{
          p: 3,
          borderRadius: 4,
          height: 420,
          display: 'flex',
          flexDirection: 'column',
          border: `1px solid ${colors.border}`,
          backgroundColor: colors.bgCard,
        }}
      >
        <Typography sx={{ fontWeight: 700, fontSize: 15, color: colors.textPrimary, mb: 2 }}>
          {t('ticketsOverTime')}
        </Typography>
        <Box sx={{ flex: 1, minWidth: 0, minHeight: 0 }}>
          <TrendChart data={trend} animated />
        </Box>
      </Paper>

    </Box>
  );
}