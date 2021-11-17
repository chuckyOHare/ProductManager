import { CreateProduct } from './ProductsIo';
import React, { useState } from 'react';
import Loading from './Loading';

function CreateForm(props) {
    const [isLoading, setIsLoading] = useState(false);
    const [name, setName] = useState();
    const [category, setCategory] = useState();
    const [price, setPrice] = useState();
    const [description, setDescription] = useState();

    const onSubmitCreate = () => {
        setIsLoading(true);
        CreateProduct({
            name: name,
            category: category,
            price: price,
            description: description
        }, () => {
            setIsLoading(false);
            props.onCancel();
        });
    }

    if (isLoading){
        return <Loading/>
    }

    return (
        <div className="product-form create">
            <h3>Create</h3>
            <form>
                <div>
                    <label htmlFor='Name'>Name</label>
                    <input type="text" onChange={event => setName(event.target.value)}></input>
                </div>
                <div>
                    <label htmlFor='Price'>Price</label>
                    <input type="number" onChange={event => setPrice(event.target.value)}></input>
                </div>
                <div>
                    <label htmlFor='Description'>Description</label>
                    <input type="text" onChange={event => setDescription(event.target.value)}></input>
                </div>
                <div>
                    <label htmlFor='Category'>Category</label>
                    <input type="text" onChange={event => setCategory(event.target.value)}></input>
                </div>
            </form>
            <div>
                <button type='button' onClick={onSubmitCreate}>Create</button>
                <button type='button' onClick={props.onCancel}>Cancel</button>
            </div>
        </div>
    );
}

export default CreateForm;
