/* eslint-disable react/prop-types */
import "./GameInfo.css"
import Timer from "./Timer"

export default function GameInfo({bankData = {}}) {
    function testTimer() {
        console.log("Никита ЛОХ!");
    }
    
    return(
        <div className="game-info-box">
                <ul className="game-info">
                    <li>Bank buy {bankData.egpCount} pu for {bankData.egpPrice}</li>
                    <li>Bank sell {bankData.esmCount} mu for {bankData.esmPrice}</li>
                    <li>Upgrade your factory</li>
                    <Timer initialTime={5} onTimeEnd={() => testTimer()}></Timer>
                </ul>
                
                <form action="" className="player-requests-input-box">
                        <div className="player-request-input-container">
                            <div className="">
                                <label htmlFor="resources-number">You sell:</label>
                                <input type="number" required id="resources-number" name="resources-number" />
                                <span>mu</span>
                            </div>
                            <button className="request-stage-info-btn" onClick={() => console.log("Click!")}/>
                        </div>
                        <div className="player-request-input-container">
                            <div className="">
                                <label htmlFor="price">For price:</label>
                                <input type="number" required id="price" name="price" />
                                <span>$</span>
                            </div>
                            <input type="submit" id="player-request-submit" value={""}/>
                        </div>
                </form>
        </div>
    )
}