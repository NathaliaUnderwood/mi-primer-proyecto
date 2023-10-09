import React, { useState } from 'react';
import { BrowserRouter as Router,Routes, Route, Navigate } from 'react-router-dom';
import './style/styles.css'; // Importa el archivo CSS
import '../node_modules/bootstrap/dist/css/bootstrap.min.css'
import Login from './components/Login';
import Header from './components/Header';
import HomePage from './components/HomePage'; // Componente para la página principal después del inicio de sesión

function App() {

  function Private () {
 
  const [setLoggedIn] = useState(false); // Variable de estado para controlar la autenticación

  // Función para actualizar el estado de autenticación
  const loggedIn = (value) => {
    setLoggedIn(value);
  };

  return loggedIn ?  <Navigate to="/" />  :  <Navigate to="/Tienda" />
  
}

  return (
    <Router>
    <div className='body'>
      <Header />
        <Routes>
        <Route exact path="/" element={<Login />} />
        <Route path="/Private" element={<Private />} />
        <Route path="/Tienda" element={<HomePage/>} />
        </Routes>
    </div>
          </Router>
  );
}



export default App;