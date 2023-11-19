import axios from 'axios';
import {useEffect} from "react";


const api = axios.create({
    baseURL: process.env.REACT_APP_MOVIES_API_URL,
});

api.interceptors.request.use(
    (config) => {

        const user = JSON.parse(atob(localStorage.getItem('user')));
        if (user) {
            config.headers.Authorization = `Bearer ${user.accessToken}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

api.interceptors.response.use(
    (response) => {
        // Aqui você pode adicionar lógica após receber uma resposta.
        return response;
    },
    (error) => {
        // Trate erros na resposta.
        return Promise.reject(error);
    }
);

export default api;