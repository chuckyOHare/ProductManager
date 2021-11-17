import { EditProduct } from './ProductsIo';
import React, { useState } from 'react';
import Loading from './Loading';

function EditForm(props) {
    const [isLoading, setIsLoading] = useState(false);
    const [name, setName] = useState(props.name);
    const [category, setCategory] = useState(props.category);
    const [price, setPrice] = useState(props.price);
    const [description, setDescription] = useState(props.description);

    const onSubmitEdit = () => {
        setIsLoading(true);
        EditProduct({
            name: name,
            category: category,
            price: price,
            description: description,
            rowKey: props.rowKey,
            partitionKey: props.partitionKey
        }, () => {
            setIsLoading(false);
            props.onCancel();
        });
    }

    if (isLoading){
        return <Loading/>
        // todo test these. Not using useeffect for these so may not work as expected
    }

    return (
        <div className="product-form edit">
            <h3>Edit</h3>
            <form>
                <div>
                    <label htmlFor={props.Name}>Name</label>
                    <input type="text" defaultValue={props.product.name} onChange={event => setName(event.target.value)}></input>
                </div>
                <div>
                    <label>Price</label>
                    <input type="number" defaultValue={props.product.price} onChange={event => setPrice(event.target.value)}></input>
                </div>
                <div>
                    <label>Description</label>
                    <input type="text" defaultValue={props.product.description} onChange={event => setDescription(event.target.value)}></input>
                </div>
                <div>
                    <label>Category</label>
                    <input type="text" defaultValue={props.product.category} onChange={event => setCategory(event.target.value)}></input>
                </div>
            </form>
            <div>
                <button type='button' onClick={onSubmitEdit}>Save changes</button>
                <button type='button' onClick={props.onCancel}>Cancel</button>
            </div>
        </div>
    );
}

export default EditForm;
