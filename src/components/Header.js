import React from 'react';
import { Link } from 'react-router-dom';

function Header() {
  return (
    <header>
      <nav>
        <ul>
          <li>
            <img src='/logo.jpg' alt='Logo' width={90}/>
          </li>
          <li>
            <Link to="/">Iniciar Sesión</Link>
          </li>
          <li>
            <Link to="/Private">Productos</Link>
          </li>
        </ul>
      </nav>
    </header>
  );
}

export default Header;
