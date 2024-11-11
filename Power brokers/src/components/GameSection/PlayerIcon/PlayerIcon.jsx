import "./PlayerIcon.css"

// eslint-disable-next-line react/prop-types
export default function PlayerIcon({isMainPlayer}) {
    const classes = (isMainPlayer ? "player-icon" : "player-icon another-player-icon")
    return (
        <div className = {classes}><img src="./img/Avatars/avatarFriren.jpg" alt="Cat" /></div>
    )
}