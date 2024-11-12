/* eslint-disable react/prop-types */
import { useState } from "react"
import BookHeader from "./BookHeader"
import PlayersList from "./PlayersList"
import "./GameLoader.css"


export default function ConnectStage({setStage, players, setPlayers}) {
    const [connectStage, setConnectStage] = useState("KeyEntering");


    //TEST FUNCTION
    function StartLoading() {
        setConnectStage("RoomSearch");
        setTimeout(() => {
            setPlayers([players[0], {nickname: "CoolBoy", avatar: 3, isMainPlayer: true}, {nickname: "CoolMan", avatar: 7, isMainPlayer: false}]);
            setConnectStage("GameWaiting")}, 4000);
    }
    return(
        <>
            <div className="book-background">
                <div className="book-page">
                    <BookHeader>Enter room key</BookHeader>
                    <input type="text" id="room-key-input" className="player-nickname" maxLength={5}/>
                    <button className="book-btn book-back-btn" onClick={() => {setStage("registration")}}></button>
                </div>
                <div className="book-page">
                    <div>
                        <BookHeader>Players list</BookHeader>
                        <p className="book-content">Competitors {players.length} of 4</p>
                    </div>
                    <PlayersList playersData={players}/>
                    <button className= {connectStage == "RoomSearch" ? "loader book-btn book-next-btn" : "book-btn book-next-btn"} onClick={StartLoading}></button>
                </div>
            </div>
        </>
    )
}