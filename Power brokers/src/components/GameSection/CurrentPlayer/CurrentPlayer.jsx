/* eslint-disable react/prop-types */
import "./CurrentPlayer.css"
import PlayerResourcesBox from "../PlayerResourcesBox/PlayerResourcesBox"
import PlayerIcon from "../PlayerIcon/PlayerIcon"
import FactoriesBox from "../FactoriesBox/FactoriesBox"
import GameInfo from "../GameInfo/GameInfo"
import { useEffect, useState } from "react"


// eslint-disable-next-line react/prop-types
export default function CurrentPlayer({player, gameData, stageTime, curStage, setCurStage, setIsOpenModal}) {
    const [factoryRequest, setFactoryRequest] = useState([]);

    useEffect(() => {
        console.log(factoryRequest);
    }, [factoryRequest])

    return (
        <div className="current-player">
            <PlayerResourcesBox isMainPlayer={true} resources={player} setIsOpenModal={setIsOpenModal}/>
            <div className="game-turn">{gameData.turn} month</div>
            <GameInfo bankData={gameData.bank} stageTime={stageTime} curStage={curStage} setCurStage={setCurStage}/>
            <PlayerIcon isMainPlayer={true} avatar={player == null ? null : player.avatar}/>
            {player && <FactoriesBox isMainPlayer={true} factories={player.factories} curStage={curStage} factoryRequest={factoryRequest} setFactoryRequest={setFactoryRequest}/>}
        </div>
    )
}