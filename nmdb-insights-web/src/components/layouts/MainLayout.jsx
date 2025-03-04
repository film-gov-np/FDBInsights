import { Outlet, Link } from 'react-router-dom';

const MainLayout = () => {
    return (
        <div>
            <header>
                <h1>Main Layout</h1>
                <nav>
                    <ul>
                        <li><Link to="/">Home</Link></li>
                        <li><Link to="/dashboard">Dashboard</Link></li>
                        <li><Link to="/profile">Profile</Link></li>
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
