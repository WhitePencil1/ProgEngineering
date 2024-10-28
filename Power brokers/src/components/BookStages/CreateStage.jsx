import BookHeader from "./BookHeader"
import PlayersList from "./PlayersList"


// eslint-disable-next-line react/prop-types
export default function CreateStage({setStage}) {

    return(
        <>
            <div className="book-background">
                <div className="book-page">
                    <BookHeader>Your room key</BookHeader>
                    <h2 className="room-key centered">Y2PHJK3</h2>
                    <button className="book-btn book-back-btn" onClick={() => {setStage("registration")}}></button>
                </div>
                <div className="book-page">
                    <div>
                        <BookHeader>Players list</BookHeader>
                        <p className="book-content">Competitors ? of 4</p>
                    </div>
                    <PlayersList />
                    <button className="book-btn book-start-btn"></button>
                </div>
            </div>
        </>
    )
}