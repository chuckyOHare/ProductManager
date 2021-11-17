const axios = require('axios');

const PRODUCTAPI_BASEURL = 'http://localhost:7071/api/';

export const GetProducts = (callback) => {
    axios.get(PRODUCTAPI_BASEURL + 'products')
    .then(function (response) {
      return callback(response.data);
    })
    .catch(function (error) {
    })
    .then(function () {
    }); 
}

export const CreateProduct = (product, callback) => {
  axios.post(PRODUCTAPI_BASEURL + 'product', product)
  .then(function () {
    return callback();
  })
  .catch(function (error) {
  })
  .then(function () {
  });
}

export const EditProduct = (product, callback) => {
  axios.put(PRODUCTAPI_BASEURL + 'product', product)
  .then(function () {
    return callback();
  })
  .catch(function (error) {
  })
  .then(function () {
  });
}

export const DeleteProduct = (partitionKey, rowKey, callback) => {
  axios.delete(PRODUCTAPI_BASEURL + 'product/' + partitionKey + '/' + rowKey)
  .then(function () {
    console.log('here?');
    return callback();
  })
  .catch(function (error) {
  })
  .then(function () {
  });
}