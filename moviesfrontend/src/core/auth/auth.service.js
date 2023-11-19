import React, { useState } from 'react';
import axios from 'axios';

const KEY = 'user';

function AuthService() {
    const [authenticatedUser, setAuthenticatedUser] = useState(null);

    const authenticate = async (data) => {
        try {
            const response = await axios.post(`${process.env.REACT_APP_BASE_URL}/accounts/login`, data);
            setAuthenticatedUser(response.data);
            return response.data;
        } catch (error) {
            console.error('Erro na autenticação:', error);
            throw error;
        }
    };

    const setToken = (user) => {
        const data = JSON.stringify(user);
        localStorage.setItem(KEY, btoa(data));
    };

    return { authenticate, setToken };
}

export default AuthService;