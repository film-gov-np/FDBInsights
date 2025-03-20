import TableData from "./TableData";
import { Link } from "react-router-dom";
export default function Dashboard() {
  return (
    <div>
      <h1>Dashboard</h1>
      <Link to="/theater"> Theater Analytics </Link>

      <TableData />
    </div>
  );
}
