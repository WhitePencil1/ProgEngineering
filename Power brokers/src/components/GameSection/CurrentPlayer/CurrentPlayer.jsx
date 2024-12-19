/* eslint-disable react/prop-types */
import "./CurrentPlayer.css"
import PlayerResourcesBox from "../PlayerResourcesBox/PlayerResourcesBox"
import PlayerIcon from "../PlayerIcon/PlayerIcon"
import FactoriesBox from "../FactoriesBox/FactoriesBox"
import GameInfo from "../GameInfo/GameInfo"


// eslint-disable-next-line react/prop-types
export default function CurrentPlayer({player, gameData, stageTime, nextStage}) {
    return (
        <div className="current-player">
            <PlayerResourcesBox isMainPlayer={true} resources={player}/>
            <div className="game-turn">{gameData.turn} month</div>

            <GameInfo bankData={gameData.bank} stageTime={stageTime} nextStage={nextStage}/>

            <PlayerIcon isMainPlayer={true} avatar={player == null ? null : player.avatar}/>
            {player && <FactoriesBox isMainPlayer={true} factories={player.factories}/>}
        </div>
    )
}