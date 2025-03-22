import { createBrowserRouter, Navigate, Outlet } from "react-router-dom";
import App from "@/App";
import MainLayout from "@/components/layouts/MainLayout";
import Dashboard from "@/components/Dashboard";
import NotFound from "@/components/NotFound";
import React from "react";
import { Paths } from "@/constants/routePaths";
import TheaterAnalyticsDashboard from "./components/theater-analytics-dashboard";

// Mock authentication function (replace with real auth logic)
const isAuthenticated = () => {
  return localStorage.getItem("token") !== null; // Example: checking a token in localStorage
};

// Protected Route Component
const ProtectedRoute = () => {
  return isAuthenticated() ? <Outlet /> : <Navigate to={Paths.Route_Home} replace />;
};

const router = createBrowserRouter([
  {
    path: Paths.Route_Home,
    element: <MainLayout />,
    children: [
      { path: "", element: <App /> },
      {
        path: Paths.Route_Dashboard,
        element: <ProtectedRoute />,
        children: [
          { path: "", element: <Dashboard /> },
          {
            path: Paths.Route_TheaterAnalytics,
            element: <TheaterAnalyticsDashboard />,
          }],
      }, ,

      { path: "*", element: <NotFound /> },
    ],
  },
]);

export default router;
