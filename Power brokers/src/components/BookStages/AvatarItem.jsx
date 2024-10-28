import "./Book.css"

// eslint-disable-next-line react/prop-types
export default function AvatarItem ({path}) {

    return (
        <li className="avatar-box"><img src={path} alt="Avatar" className="avatar-icon"/></li>
    ) 
}