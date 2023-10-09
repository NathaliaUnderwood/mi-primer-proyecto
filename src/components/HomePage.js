import React from 'react';
import ProductList from './ProductList'; // Importa el componente ProductList

function HomePage({ userName }) {
  // Supongamos que tienes una lista de productos en este componente
  const products = [
    { id: 1, name: 'Labiales', price: 10, image: 'labiales.jpg' },
    { id: 2, name: 'Bases', price: 15, image: 'bases.jpg' },
    { id: 3, name: 'Paleta de sombras', price: 30, image: 'paletas.jpg' },
    { id: 4, name: 'Rubores', price: 20, image: 'blush.jpg' }
    // Añade más productos aquí
  ];

  return (
    <div>
      <h1>Cosméticos para tí:</h1>

      {/* Renderiza el componente ProductList aquí */}
      <ProductList products={products} />
    </div>
  );
}

export default HomePage;