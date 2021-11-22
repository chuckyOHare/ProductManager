import EditForm from './EditForm';
import CreateForm from './CreateForm';
import List from './List';
import { GetProducts } from './ProductsIo';
import Loading from './Loading';
import React, { useState, useEffect } from 'react';
// const axios = require('axios');

function App() {
  const [selectedProduct, setSelectedProduct] = useState();
  const [isCreatingProduct, setIsCreateingProduct] = useState();
  const [allProducts, setAllProducts] = useState();
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    GetProducts((products) => {
      setAllProducts(products);
      setIsLoading(false);
    })
  }, []);

  const RefreshProducts = () =>{
    setIsLoading(true);
    console.log("here");
    GetProducts((products) => {
      setAllProducts(products);
      setIsLoading(false);
    })
  }

  if (isLoading) {
    return <Loading />
  }

  const onCancel = () => {
    setSelectedProduct(null);
    setIsCreateingProduct(false);
    RefreshProducts();
  }

  if (isCreatingProduct) {
    return <CreateForm onCancel={onCancel} />
  }

  if (selectedProduct) {
    return (
      <EditForm product={selectedProduct} onCancel={onCancel}/>
    )
  }

  console.log("then there");

  return (
    <div className='product-index'>
      <List products={allProducts} onRowClicked={setSelectedProduct} onDeleteProduct={RefreshProducts}/>
      <button type="button" onClick={() => setIsCreateingProduct(true)}>Create new</button>
    </div>
  )
}

export default App;
