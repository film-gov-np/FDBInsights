import { createBrowserRouter } from 'react-router-dom';
import App from './App';
import MainLayout from './components/layouts/MainLayout';
import Dashboard from './components/Dashboard';
import NotFound from './components/NotFound';
import React from 'react';

const router = createBrowserRouter([
    {
        path: '/',
        element: <MainLayout />,
        children: [
            { path: '', element: <App /> },
            { path: 'dashboard', element: <Dashboard /> },
        ],
    },
    { path: '*', element: React.createElement(NotFound) },
]);

export default router;
