// src/components/MainLayout.js
import { Outlet, Link } from "react-router-dom";
import mainLogo from "/emblem_nepal.png";
import "./MainLayout.css";

const MainLayout = () => {
  return (
    <div className="bg-zinc-300 h-screen w-full">
      <header className="w-full bg-zinc-800 text-white">
        <nav className="navbar">
          <ul className="flex justify-center items-center w-full py-4 m-0 p-0">
            <li className="flex-1 text-center">
              <Link to="/">
                <img
                  src={mainLogo}
                  className="logo mx-auto h-12"
                  alt="NMDB Insights"
                />
              </Link>
            </li>
            <li className="flex-1 text-center">
              <Link className="font-bold hover:text-blue-500" to="/">
                Home
              </Link>
            </li>
            <li className="flex-1 text-center">
              <Link className="font-bold hover:text-blue-500" to="/dashboard">
                Dashboard
              </Link>
            </li>
            <li className="flex-1 text-center">
              <Link className="font-bold hover:text-blue-500" to="/profile">
                Profile
              </Link>
            </li>
          </ul>
        </nav>
      </header>
      <main className="flex-1 w-full p-4">
        <Outlet />
      </main>
    </div>
  );
};

export default MainLayout;
