import { useEffect, useState } from "react";
import "./WelcomeSection.css"
import { instance } from "../../utils/axios";


// eslint-disable-next-line react/prop-types
export default function WelcomeSection({setNextStage, setRole}) {
    const [playAnimation, setPlayAnimation] = useState(false)

    async function handleNextClick (role) {
        setPlayAnimation(true)
        setRole(role)
        setTimeout(() => setNextStage("registration"), 2000)
    }

    useEffect(() => {
        async function restoreData() {
            try{
                await instance.delete("player");
            } catch(err) {
                console.log(err);
            }
            finally {
                console.log("Restore сработал");
            }
        }
        restoreData();
    }, [])


    return (
        
        <section className = {playAnimation === false ? "welcome-section" : "welcome-section sailAway"} >
            <h1>Power Brokers</h1>
            <button className="welcome-button" onClick={() => handleNextClick("create")}>Create room</button>
            <button className="welcome-button" onClick={() => handleNextClick("connect")}>Connect</button>
            <button className="welcome-button" onClick={() => window.open("/src/help.html", "_blank", "noopener,noreferrer")}>Help</button>
        </section>
    )
}