import React from 'react';

function ProductList(props) {
  return (
    <div className="product-list">
      {props.products.map((product) => (
        <div key={product.id} className="product-card">
          <img src={`/images/${product.image}`} alt={product.name} className="product-image" />
          <h2>{product.name}</h2>
          <p>Precio: ${product.price}</p>
          <button >Agregar al carrito</button>
        </div>
      ))}
    </div>
  );
}

export default ProductList;