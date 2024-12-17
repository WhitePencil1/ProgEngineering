import "./PlayerIcon.css"
import { avatarsWays } from "../../../data"

// eslint-disable-next-line react/prop-types
export default function PlayerIcon({isMainPlayer, avatar}) {
    const classes = (isMainPlayer ? "player-icon" : "player-icon another-player-icon")
    return (
        <div className = {classes}><img src={avatarsWays[avatar]} alt="Cat" /></div>
    )
}