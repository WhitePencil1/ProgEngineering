import "./Book.css"

// eslint-disable-next-line react/prop-types
export default function AvatarItem ({path, onClick, classes}) {

    return (
        <li className={classes} onClick={onClick}><img src={path} alt="Avatar" className="avatar-icon"/></li>
    ) 
}