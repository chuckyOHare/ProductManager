const Input = (props) => {
    return (
        <div className="input-group">
            <label htmlFor={props.label}>{props.label}</label>
            <input type="text" defaultValue={props.product.defaultValue}></input>
        </div>
    )
}

export default Input