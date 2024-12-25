/* eslint-disable react/prop-types */
import {avatarsWays} from '/src/data.js'
import "./Book.css"



export default function PlayersList ({playersData, isMainPlayer}) {
    return(
        <ul className="players-list">
            {playersData.map((player) => 

            <li className="player" key={player.id}>
                {/* <div className= {player.isMainPlayer ? "player-avatar-container main-player" : "player-avatar-container"}>
                    <img src={avatarsWays[player.avatar]} alt="avatar" />
                </div> */}
                <div className= "player-avatar-container" style={isMainPlayer ? {backgroundImage: "url('/img/mainPlayerAvatarContainer.png')"} : {}}>
                    <img src={avatarsWays[player.avatar]} alt="avatar" />
                </div>
                <p className="player-nickname unselectable">{player.name}</p>
            </li>)}
        </ul>
    )
}