import "./Book.css"
import {avatarsWays} from "/src/data.js"
import AvatarItem from "./AvatarItem"

export default function AvatarList () {
    return (
        <ul className="avatar-list">
            {avatarsWays.map((avatar) => <AvatarItem key={avatar} path={avatar}/>)}
        </ul>
    )
}