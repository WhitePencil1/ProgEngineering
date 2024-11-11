import "./Book.css"
import {avatarsWays} from "/src/data.js"
import AvatarItem from "./AvatarItem"
import { useState } from "react"


// eslint-disable-next-line react/prop-types
export default function AvatarList ({setCurPlayerAvatar}) {
    const [curAvatarIndex, setCurAvatarIndex] = useState(-1);
    return (
        <ul className="avatar-list">
            {avatarsWays.map((avatar, index) => <AvatarItem classes={curAvatarIndex == index ? "avatar-box avatar-active" : "avatar-box"} 
            curAvatarIndex={curAvatarIndex} key={avatar} path={avatar} index={index} onClick={() => {
                    setCurPlayerAvatar(index);
                    setCurAvatarIndex(index) 
                }}/>)}
        </ul>
    )
}