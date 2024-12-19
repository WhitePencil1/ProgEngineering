import { useState } from "react";
import { useEffect } from "react";
import "./WelcomeSection.css"


// eslint-disable-next-line react/prop-types
export default function WelcomeSection({setNextStage, setRole, setPlayers, setRoomCode}) {
    const [playAnimation, setPlayAnimation] = useState(false)

    async function handleNextClick (role) {
        setPlayAnimation(true)
        setRole(role)
        setTimeout(() => setNextStage("registration"), 2000)
    }




    return (
        
        <section className = {playAnimation === false ? "welcome-section" : "welcome-section sailAway"} >
            <h1>Start game</h1>
            <button className="welcome-button" onClick={() => handleNextClick("create")}>Create room</button>
            <button className="welcome-button" onClick={() => handleNextClick("connect")}>Connect</button>
        </section>
    )
}