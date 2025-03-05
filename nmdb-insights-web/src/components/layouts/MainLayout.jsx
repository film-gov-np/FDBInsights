import { Outlet, Link } from "react-router-dom";

const MainLayout = () => {
  return (
    <div className="bg-zinc-300 h-screen w-full">
      <header>
        <h1>Main Layout</h1>
        <nav className=" ">
          <ul className="flex">
            <li>
              <Link to="/">Home</Link>
            </li>
            <li>
              <Link to="/dashboard">Dashboard</Link>
            </li>
            <li>
              <Link to="/profile">Profile</Link>
            </li>
          </ul>
        </nav>
      </header>
      <main>
        <Outlet />
      </main>
    </div>
  );
};

export default MainLayout;
