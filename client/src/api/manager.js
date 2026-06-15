import api, { unwrap, unwrapList } from './client';

export const getEmployees = (params) =>
  api.get('/manager/employees', { params }).then(unwrapList);
export const getClients = (params) =>
  api.get('/manager/clients', { params }).then(unwrapList);
export const getClientsWithTickets = (params) =>
  api.get('/manager/clients-with-tickets', { params }).then(unwrapList);
export const getUser = (id) =>
  api.get(`/manager/users/${id}`).then(unwrap);
export const createEmployee = (payload) =>
  api.post('/manager/employee', payload).then(unwrap);
export const updateUser = (id, payload) =>
  api.put(`/manager/users/${id}`, payload).then(unwrap);
export const toggleUserStatus = (id) =>
  api.put(`/manager/users/${id}/toggle-status`).then(unwrap);
export const getAllTickets = (params) =>
  api.get('/manager/all', { params }).then(unwrapList);
export const getTicketDetails = (id) =>
  api.get(`/manager/info/${id}`).then(unwrap);
export const assignTicket = (ticketId, employeeId) =>
  api.put(`/manager/${ticketId}/assign/${employeeId}`).then(unwrap);
