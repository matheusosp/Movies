
import './App.css';
import Routes from "./routes";
import React from "react";
import {TokenServiceProvider} from "./core/token/token.service";

function App() {
  return (
    <div className="App">
      <Routes />
    </div>
  );
}

export default App;
