import { createBrowserRouter, Navigate, Outlet } from "react-router-dom";
import App from "@/App";
import MainLayout from "@/components/layouts/MainLayout";
import Dashboard from "@/components/Dashboard";
import NotFound from "@/components/NotFound";
import React from "react";
import { Paths } from "@/constants/routePaths";
import TheaterAnalyticsDashboard from "./components/theater-analytics-dashboard";
import SideMenuLayout from "./components/layouts/SideMenuLayout";
import LoginForm from "./components/ui/login";

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
      { path: "", element: <LoginForm /> },


      { path: "*", element: <NotFound /> },
    ],
  },
  {
    path: Paths.Route_Home,
    element: <SideMenuLayout />,
    children: [
      { path: Paths.Route_Dashboard, element: <Dashboard /> },
      {
        path: Paths.Route_TheaterAnalytics,
        element: <TheaterAnalyticsDashboard />,
      },
    ]
  }
]);

export default router;
