import axios from 'axios';
import { TokenKeyName } from '../constants/constants.ts';

const isDev = import.meta.env.DEV;

// DEV módban a baseURL legyen relatív `/api`, hogy Vite proxy működjön.
// PROD módban jöhet az env-ből.
const baseURL = isDev
    ? '/api'
    : import.meta.env.VITE_REST_API_URL || '/api';

const axiosInstance = axios.create({ baseURL });

axiosInstance.interceptors.request.use(
    config => {
        const token = localStorage.getItem(TokenKeyName);
        if (token) {
            config.headers['Authorization'] = `Bearer ${token}`;
        }
        return config;
    },
    error => Promise.reject(error)
);

export default axiosInstance;