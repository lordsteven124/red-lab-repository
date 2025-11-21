import axios from 'axios';

const API_BASE_URL = 'http://localhost:5000/api';

const api = axios.create({
  baseURL: API_BASE_URL,
});

// Interceptor para agregar el token a las requests
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  console.log('API Request:', config.method?.toUpperCase(), config.url, config.data);
  return config;
});

// Interceptor para manejar errores de autenticación
api.interceptors.response.use(
  (response) => {
    console.log('API Response:', response.status, response.data);
    return response;
  },
  (error) => {
    console.error('API Error:', error.response?.status, error.response?.data);
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export interface LoginData {
  email: string;
  password: string;
}

export interface RegisterData {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

export interface Product {
  id: string;
  nombre: string;
  descripcion?: string;
  precio: number;
  estado: boolean;
  usuarioCreacion: string;
  fechaCreacion: string;
  usuarioModificacion?: string;
  fechaModificacion?: string;
}

export interface CreateProductData {
  nombre: string;
  descripcion?: string;
  precio: number;
  estado: boolean;
}

export interface UpdateProductData {
  nombre: string;
  descripcion?: string;
  precio: number;
  estado: boolean;
}

export interface ProductsResponse {
  products: Product[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export const authAPI = {
  login: (data: LoginData) => api.post('/auth/login', data),
  register: (data: RegisterData) => api.post('/auth/register', data),
};

export const productsAPI = {
  getProducts: (params?: {
    search?: string;
    estado?: boolean;
    page?: number;
    pageSize?: number;
  }) => api.get<ProductsResponse>('/products', { params }),
  getProduct: (id: string) => api.get<Product>(`/products/${id}`),
  createProduct: (data: CreateProductData) => api.post<Product>('/products', data),
  updateProduct: (id: string, data: UpdateProductData) => api.put(`/products/${id}`, data),
  deleteProduct: (id: string) => api.delete(`/products/${id}`),
  generateReport: () => api.get('/products/report', { responseType: 'blob' }),
};

export default api;