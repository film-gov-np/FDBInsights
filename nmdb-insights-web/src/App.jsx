import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/emblem_nepal.png'
import './App.css'

function App() {
  const [count, setCount] = useState(0)

  return (
    <>
      <div>
        <img src={viteLogo} className="logo" alt="Vite logo" />
      </div>
      <h1>NMDB Insights</h1>
    </>
  )
}

export default App
