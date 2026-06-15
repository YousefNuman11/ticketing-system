import api, { unwrap, unwrapList } from './client';

// Client
export const getMyTickets = (params) =>
  api.get('/ticket/myTickets', { params }).then(unwrapList);
export const getTicketById = (id) =>
  api.get(`/ticket/${id}`).then(unwrap);
export const createTicket = (payload) =>
  api.post('/ticket', payload).then(unwrap);
export const updateTicket = (id, payload) =>
  api.put(`/ticket/${id}`, payload).then(unwrap);
export const closeTicket = (id) =>
  api.put(`/ticket/${id}/close`).then(unwrap);

// Employee
export const getAssignedTickets = (params) =>
  api.get('/ticket/assigned', { params }).then(unwrapList);
export const resolveTicket = (id) =>
  api.put(`/ticket/${id}/resolve`).then(unwrap);

// Shared
export const getComments = (ticketId, params) =>
  api.get(`/ticket/${ticketId}/comments`, { params }).then(unwrapList);
export const addComment = (ticketId, text) =>
  api.post(`/ticket/${ticketId}/comments`, { text }).then(unwrap);
export const getAttachments = (ticketId, params) =>
  api.get(`/ticket/${ticketId}/attachments`, { params }).then(unwrapList);
export const uploadAttachment = (ticketId, file) => {
  const form = new FormData();
  form.append('file', file);
  // Let the browser/axios set the multipart boundary automatically.
  return api.post(`/ticket/${ticketId}/attachments`, form).then(unwrap);
};
