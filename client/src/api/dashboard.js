import api, { unwrap } from './client';

export const getTicketStatus = () =>
  api.get('/dashboard/status').then(unwrap);
export const getTopEmployees = () =>
  api.get('/dashboard/top-employees').then(unwrap);
export const getTicketTrend = () =>
  api.get('/dashboard/trend').then(unwrap);
