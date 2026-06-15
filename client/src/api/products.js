import api, { unwrap, unwrapList } from './client';

export const getProducts = (params) =>
  api.get('/products', { params }).then(unwrapList);
export const createProduct = (payload) =>
  api.post('/products', payload).then(unwrap);
