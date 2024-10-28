import {avatarsWays} from '/src/data.js'
import "./Book.css"

export default function PlayersList () {

    return(
        <ul className="players-list">
            <li className="player"><div className="player-avatar-container main-player"><img src={avatarsWays[0]} alt="avatar" /></div><p className="player-nickname unselectable">White Pencil</p></li>
            <li className="player"><div className="player-avatar-container"><img src={avatarsWays[1]} alt="avatar" /></div><p className="player-nickname unselectable">JoskyChel</p></li>
            <li className="player"><div className="player-avatar-container"><img src={avatarsWays[2]} alt="avatar" /></div><p className="player-nickname unselectable">IntCrab</p></li>
            <li className="player"><div className="player-avatar-container"><img src={avatarsWays[3]} alt="avatar" /></div><p className="player-nickname unselectable">MusleyChotTam</p></li>
        </ul>
    )
}