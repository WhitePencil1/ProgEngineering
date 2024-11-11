import "./CurrentPlayer.css"
import PlayerResourcesBox from "../PlayerResourcesBox/PlayerResourcesBox"
import PlayerIcon from "../PlayerIcon/PlayerIcon"
import FactoriesBox from "../FactoriesBox/FactoriesBox"


// eslint-disable-next-line react/prop-types
export default function CurrentPlayer({player}) {
    return (
        <div className="current-player">
            <PlayerResourcesBox isMainPlayer={true}/>
            <div className="game-turn">5 month</div>
            <ul className="game-info">
                <li>Bank buy 8 pu for 1000</li>
                <li>Bank sell 6 mu for 500</li>
                <li>Upgrade your factory</li>
                <li>Time left: 30s</li>
            </ul>
            <PlayerIcon isMainPlayer={true}/>
            <FactoriesBox isMainPlayer={true}/>
        </div>
    )
}