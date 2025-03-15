import { Outlet, Link } from "react-router-dom";
import mainLogo from "/emblem_nepal.png";
import "./MainLayout.css";

const MainLayout = () => {
  return (
    <div className="bg-zinc-300 h-screen w-full flex flex-col items-center">
      <header className="w-full">
        <nav className="navbar">
          <ul className="flex justify-center space-x-4">
            <li className="flex-1 text-center">
              <img src={mainLogo} className="logo mx-auto" alt="NMDB Insights" />
            </li>
            <li className="flex-1 text-center">
              <Link to="/">Home</Link>
            </li>
            <li className="flex-1 text-center">
              <Link to="/dashboard">Dashboard</Link>
            </li>
            <li className="flex-1 text-center">
              <Link to="/profile">Profile</Link>
            </li>
          </ul>
        </nav>
      </header>
      <main className="flex-1 w-full">
        <Outlet />
      </main>
    </div>
  );
};

export default MainLayout;
