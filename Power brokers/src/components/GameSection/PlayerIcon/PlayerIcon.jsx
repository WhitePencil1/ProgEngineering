import "./PlayerIcon.css"
import { avatarsWays } from "../../../data"

// eslint-disable-next-line react/prop-types
export default function PlayerIcon({isCurrentPlayer, isMainPlayer, avatar, nickname}) {
    let classes = (isCurrentPlayer ? "player-icon" : "player-icon another-player-icon");
    classes = (isMainPlayer ? classes + " main-player" : classes);
    return (
        <div className = {classes}>
            <div className={isCurrentPlayer ? "cur-player-nickname" : "another-player-nickname"}>{nickname}</div>
            <div ><img src={avatarsWays[avatar]} alt="Cat" /></div>
        </div>
        
    )
}