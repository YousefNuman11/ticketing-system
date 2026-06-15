import axios from 'axios';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL || 'http://localhost:5233/api',
});

// Attach JWT to every request.
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

// The API always responds with the ApiResponse envelope:
// { success, statusCode, message, data, pagination, errors }
api.interceptors.response.use(
  (response) => response,
  (error) => {
    const status = error.response?.status;
    if (status === 401) {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      if (!window.location.pathname.startsWith('/login')) {
        window.location.href = '/login';
      }
    }
    const data = error.response?.data;
    const message =
      data?.message ||
      (data?.errors && data.errors.join(', ')) ||
      error.message ||
      'Something went wrong';
    return Promise.reject(new Error(message));
  }
);

// Helpers to read the envelope.
export const unwrap = (res) => res.data?.data;
export const unwrapList = (res) => ({
  items: res.data?.data ?? [],
  pagination: res.data?.pagination ?? null,
});

export default api;
