/* eslint-disable react/prop-types */
import BookHeader from "./BookHeader"
import PlayersList from "./PlayersList"


export default function CreateStage({setStage, players, setPlayers, roomCode}) {
    return(
        <>
            <div className="book-background">
                <div className="book-page">
                    <BookHeader>Your room key</BookHeader>
                    <h2 className="room-key centered">{roomCode}</h2>
                    <button className="book-btn book-back-btn" onClick={() => {setStage("registration")}}></button>
                </div>
                <div className="book-page">
                    <div>
                        <BookHeader isCreateRole = {true}>Players list</BookHeader>
                        <p className="book-content">Competitors {players.length} of 4</p>
                    </div>
                    <PlayersList playersData={players}/>
                    <button className="book-btn book-start-btn"></button>
                </div>
            </div>
        </>
    )
}