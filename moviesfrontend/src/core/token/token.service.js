import React, { createContext, useContext, useEffect, useState } from 'react';
const { default: jwt_decode } = require("jwt-decode");

const KEY = 'user';

const encodeBase64 = (data) => btoa(JSON.stringify(data));
const decodeBase64 = (data) => JSON.parse(atob(data));

const hasTokenValid = (token) => {
    if (!token) {
        return false;
    }
    try {
        const decodedToken = jwt_decode(token);
        if (!decodedToken) {
            return false;
        }

        const currentTime = Date.now() / 1000;
        if (decodedToken.exp < currentTime) {
            return false;
        }
    } catch (error) {
        return false;
    }

    return true;
};

const TokenServiceContext = createContext();

export const TokenServiceProvider = ({ children }) => {
    const [user, setUser] = useState(null);

    useEffect(() => {
        const storedToken = localStorage.getItem(KEY);
        if (hasTokenValid(storedToken)) {
            setUser(decodeBase64(storedToken));
        }
    }, []);

    const setToken = (userData) => {
        const encodedData = encodeBase64(userData);
        localStorage.setItem(KEY, encodedData);
        setUser(userData);
    };

    const getUser = () => {
        const storedToken = localStorage.getItem(KEY);
        return storedToken ? decodeBase64(storedToken) : null;
    };

    const hasTokenValidReact = (token) => {
        return hasTokenValid(token);
    };

    const clearToken = () => {
        localStorage.removeItem(KEY);
        setUser(null);
    };

    const contextValue = {
        setToken,
        getUser,
        hasTokenValid: hasTokenValidReact,
        clearToken,
    };

    return (
        <TokenServiceContext.Provider value={contextValue}>
            {children}
        </TokenServiceContext.Provider>
    );
};

export const useTokenService = () => {
    const context = useContext(TokenServiceContext);
    if (!context) {
        throw new Error('useTokenService deve ser usado dentro de um TokenServiceProvider');
    }
    return context;
};