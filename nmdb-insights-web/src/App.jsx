import { useState } from "react";
import mainLogo from "/emblem_nepal.png";
import "./App.css";
import Login from "./components/Login";

function App() {
  const [count, setCount] = useState(0);

  return (
    <div>
      <div>
        <img src={mainLogo} className="logo" alt="NMDB Insights" />
      </div>

      <h1>NMDB Insights</h1>
      <Login />
    </div>
  );
}

export default App;
