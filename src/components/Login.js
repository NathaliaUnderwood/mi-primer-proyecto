import React, { useState } from 'react';
import { Navigate } from 'react-router-dom';

function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loggedIn, setLoggedIn] = useState(false); // Variable de estado para controlar la autenticación

  const handleSubmit = (e) => {
    e.preventDefault();
    // Realiza la validación de inicio de sesión aquí
    if (email === 'nathalia@gmail.com' && password === 'nathalia2002') {
      setLoggedIn(true); // Establece la variable de estado en true si la autenticación es exitosa
    } else {
      alert('Correo o contraseña incorrectos');
    }
  };

  if (loggedIn) {
    return <Navigate to="/Tienda" /> // Muestra el mensaje de bienvenida con el nombre del usuario
  }

  return (
      <form onSubmit={handleSubmit}>
      <h2>Iniciar Sesión</h2>
        <div className="form-group">
          <label htmlFor="email">Correo Electrónico:</label>
          <input
            type="email"
            id="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="password">Contraseña:</label>
          <input
            type="password"
            id="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <button type="submit">Iniciar Sesión</button>
      </form>
  );
}

export default Login;