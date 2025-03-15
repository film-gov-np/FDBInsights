import { createBrowserRouter } from "react-router-dom";
import App from "@/App";
import MainLayout from "@/components/layouts/MainLayout";
import Dashboard from "@/components/Dashboard";
import NotFound from "@/components/NotFound";
import React from "react";
import { Paths } from "@/constants/routePaths";

const router = createBrowserRouter([
  {
    path: Paths.Route_Home,
    element: <MainLayout />,
    children: [
      { path: "", element: <App /> },
      { path: Paths.Route_Dashboard, element: <Dashboard /> },

      { path: "*", element: <NotFound /> },
    ],
  },
]);

export default router;
