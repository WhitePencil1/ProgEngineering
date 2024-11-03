import "./Book.css"

export default function PlayersListEmpty () {

    return(
        <ul className="players-list">
            <li className="player"><div className="player-avatar-container main-player"><img/></div><p className="player-nickname unselectable"></p></li>
            <li className="player"><div className="player-avatar-container"><img/></div><p className="player-nickname unselectable"></p></li>
            <li className="player"><div className="player-avatar-container"><img/></div><p className="player-nickname unselectable"></p></li>
            <li className="player"><div className="player-avatar-container"><img /></div><p className="player-nickname unselectable"></p></li>
        </ul>
    )
}