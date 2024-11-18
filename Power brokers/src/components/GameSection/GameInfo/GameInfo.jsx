import "./GameInfo.css"

export default function GameInfo() {

    return(
        <div className="game-info-box">
                <ul className="game-info">
                    <li>Bank buy 8 pu for 1000</li>
                    <li>Bank sell 6 mu for 500</li>
                    <li>Upgrade your factory</li>
                    <li>Time left: 30s</li>
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