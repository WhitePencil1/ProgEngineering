/* eslint-disable react/prop-types */
import "./AnotherPlayer.css"
import PlayerResourcesBox from "../PlayerResourcesBox/PlayerResourcesBox"
import PlayerIcon from "../PlayerIcon/PlayerIcon"
import FactoriesBox from "../FactoriesBox/FactoriesBox"

// eslint-disable-next-line react/prop-types
export default function AnotherPlayer({player, position}) {
    const positionColors = ["#6BBA9D", "#B1B263", "#D17E4F", "#7488BB"]

    return (
        <div className="another-player" style={{backgroundColor: positionColors[position-1]}}>
            <PlayerResourcesBox isMainPlayer={false} resources={player}/>
            <PlayerIcon isMainPlayer={false} avatar={player == null ? null : player.avatar}/>
            {player && <FactoriesBox isMainPlayer = {false} factories={player.factories}/>}
        </div>
    )
}