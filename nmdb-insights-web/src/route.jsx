import { createBrowserRouter } from "react-router-dom";
import App from "@/App";
import MainLayout from "@/components/layouts/MainLayout";
import Dashboard from "@/components/Dashboard";
import NotFound from "@/components/NotFound";
import React from "react";
import { Paths } from "@/constants/routePaths";
import TheaterAnalyticsDashboard from "./components/theater-analytics-dashboard";

const router = createBrowserRouter([
  {
    path: Paths.Route_Home,
    element: <MainLayout />,
    children: [
      { path: "", element: <App /> },
      { path: Paths.Route_Dashboard, element: <Dashboard /> },
      {
        path: Paths.Route_TheaterAnalytics,
        element: <TheaterAnalyticsDashboard />,
      },

      { path: "*", element: <NotFound /> },
    ],
  },
]);

export default router;
