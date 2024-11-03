import { useState } from "react"
import BookHeader from "./BookHeader"
import PlayersList from "./PlayersList"
import PlayersListEmpty from "./PlayersListEmpty"
import "./GameLoader.css"


// eslint-disable-next-line react/prop-types
export default function ConnectStage({setStage}) {
    const [connectStage, setConnectStage] = useState("KeyEntering");

    function StartLoading() {
        setConnectStage("RoomSearch");
        setTimeout(() => setConnectStage("GameWaiting"), 4000)
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
                        <p className="book-content">Competitors 0 of 4</p>
                    </div>
                    {connectStage == "GameWaiting" ? <PlayersList/> : <PlayersListEmpty />}
                    <button className= {connectStage == "RoomSearch" ? "loader book-btn book-next-btn" : "book-btn book-next-btn"} onClick={StartLoading}></button>

                </div>
            </div>
        </>
    )
}