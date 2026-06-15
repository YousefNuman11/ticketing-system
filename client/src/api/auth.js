import api, { unwrap } from './client';

export const login = async (identifier, password) => {
  const res = await api.post('/auth/login', { identifier, password });
  return unwrap(res); // { token, user }
};

export const register = async (payload) => {
  const res = await api.post('/auth/register', payload);
  return unwrap(res); // { token, user }
};
