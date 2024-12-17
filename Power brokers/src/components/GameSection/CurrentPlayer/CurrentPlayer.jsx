import "./CurrentPlayer.css"
import PlayerResourcesBox from "../PlayerResourcesBox/PlayerResourcesBox"
import PlayerIcon from "../PlayerIcon/PlayerIcon"
import FactoriesBox from "../FactoriesBox/FactoriesBox"
import GameInfo from "../GameInfo/GameInfo"


// eslint-disable-next-line react/prop-types
export default function CurrentPlayer({player, gameData}) {
    


    return (
        <div className="current-player">
            {console.log(gameData)}
            
            <PlayerResourcesBox isMainPlayer={true}/>
            <div className="game-turn">5 month</div>
            <GameInfo />
            <PlayerIcon isMainPlayer={true}/>
            <FactoriesBox isMainPlayer={true}/>
        </div>
    )
}