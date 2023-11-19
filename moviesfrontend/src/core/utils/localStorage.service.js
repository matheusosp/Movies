import React, { createContext, useContext, useState } from 'react';
import { Observable } from 'rxjs';

const LocalStorageServiceContext = createContext();

export const LocalStorageServiceProvider = ({ children }) => {
    const [balanceSubject] = useState(() => new Observable(subscriber => {
        const amount = parseInt(window.localStorage.getItem("balance"), 10) || 0;
        subscriber.next(amount);
    }));

    const hasToken = key => !!getToken(key);

    const setToken = (key, token) => {
        window.localStorage.setItem(key, token);
    };

    const getToken = key => window.localStorage.getItem(key);

    const removeToken = key => {
        window.localStorage.removeItem(key);
    };

    const getBalance = () => {
        return window.localStorage.getItem("balance");
    };

    const setBalance = amount => {
        window.localStorage.setItem("balance", amount.toString());
        balanceSubject.next(amount);
    };

    const observeBalance = () => {
        return balanceSubject;
    };

    const contextValue = {
        hasToken,
        setToken,
        getToken,
        removeToken,
        getBalance,
        setBalance,
        observeBalance,
    };

    return (
        <LocalStorageServiceContext.Provider value={contextValue}>
            {children}
        </LocalStorageServiceContext.Provider>
    );
};

export const useLocalStorageService = () => {
    const context = useContext(LocalStorageServiceContext);
    if (!context) {
        throw new Error('useLocalStorageService deve ser usado dentro de um LocalStorageServiceProvider');
    }
    return context;
};
