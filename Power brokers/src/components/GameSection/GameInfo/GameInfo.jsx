/* eslint-disable react/prop-types */
import { useState } from "react"
import { stages } from "../../../data"
import "./GameInfo.css"
import Timer from "./Timer"
import { instance } from "../../../utils/axios"

export default function GameInfo({bankData = {}, stageTime, curStage, setCurStage}) {
    
    const [count, setCount] = useState(0);
    const [price, setPrice] = useState(0);
    instance.defaults.timeout = 0;

    async function submitHandle() {
        console.log("Запрос " + stages[curStage].api + "Count=0&Price=0" + " отправлен");
        await instance.post(stages[curStage].api, {Count: count, Price: price}).then(response => console.log(response));
        console.log("Запрос выполнен");        
    }
 
    return(
        <div className="game-info-box">
                <ul className="game-info">
                    <li>Bank buy {bankData.egpCount} pu for {bankData.egpPrice}</li>
                    <li>Bank sell {bankData.esmCount} mu for {bankData.esmPrice}</li>
                    <li>Upgrade your factory</li>
                    <Timer initialTime={stageTime} curStage={curStage} setCurStage={setCurStage}></Timer>
                </ul>
                
                <div className="player-requests-input-box">
                        <div className="player-request-input-container">
                            <div className="">
                                <label htmlFor="resources-number">You sell:</label>
                                <input type="number" required id="resources-number" name="resources-number" onChange={(evt) => setCount(evt.target.value)} value={count}/>
                                <span>mu</span>
                            </div>
                            <button className="request-stage-info-btn" onClick={() => console.log("Click!")}/>
                        </div>
                        <div className="player-request-input-container">
                            <div className="">
                                <label htmlFor="price">For price:</label>
                                <input type="number" required id="price" name="price" onChange={(evt) => setPrice(evt.target.value)} value={price}/>
                                <span>$</span>
                            </div>
                            <input type="submit" id="player-request-submit" value={""} onClick={() => submitHandle()}/>
                        </div>
                </div>
        </div>
    )
}