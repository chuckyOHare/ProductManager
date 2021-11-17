import './App.scss';
import React, { useState } from 'react';
import { DeleteProduct } from './ProductsIo';


function List(props) {
    console.log('tick');

    // const [isLoading, setIsLoading] = useState(false);

    const Delete = (partitionKey, rowKey) => {
        DeleteProduct(partitionKey, rowKey, () => {
            console.log("start");
            props.onDeleteProduct();
        });
    }

    return (
        <div className="product-list">
            <table>
                <thead>
                    <tr>
                        <th>
                            Name
                        </th>
                        <th>
                            Description
                        </th>
                        <th>
                            Price
                        </th>
                        <th>
                            Category
                        </th>
                        <th>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    {props.products.map(product => {
                        return (
                            <tr>
                                <td onClick={() => props.onRowClicked(product)}>
                                    {product.name}
                                </td>
                                <td onClick={() => props.onRowClicked(product)}>
                                    {product.description}
                                </td>
                                <td onClick={() => props.onRowClicked(product)}>
                                    {product.price}
                                </td>
                                <td onClick={() => props.onRowClicked(product)}>
                                    {product.category}
                                </td>
                                <td>
                                    <button type="button" onClick={() => {
                                        // console.log(product);
                                        Delete(product.partitionKey, product.rowKey)
                                        }}>Delete</button>
                                </td>
                            </tr>
                        )
                    })}
                </tbody>
            </table>
        </div>
    );
}

export default List;
