import "./GameSection.css"
import CurrentPlayer from "./CurrentPlayer/CurrentPlayer"
import AnotherPlayer from "./AnotherPlayer/AnotherPlayer"


// eslint-disable-next-line react/prop-types
export default function GameSection({players}) {
    return (
        <section className="players-box">
            <CurrentPlayer player={players[1]}/>
            <AnotherPlayer player={players[2]} position={2}/>
            <AnotherPlayer player={players[3]} position={3}/>
            <AnotherPlayer player={players[4]} position={4}/>
        </section>
    )
}