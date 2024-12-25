/* eslint-disable react/prop-types */
import { useState } from "react"
import { stages } from "../../../data"
import "./GameInfo.css"
import Timer from "./Timer"
import { instance } from "../../../utils/axios"

export default function GameInfo({bankData = {}, stageTime, curStage, setCurStage}) {
    
    const [count, setCount] = useState(0);
    const [price, setPrice] = useState(0);
    const [isSubmit, setIsSubmit] = useState(false);
    const [isVisible, setIsVisible] = useState(true);

    instance.defaults.timeout = 0;

    async function submitHandle() {
        console.log("Запрос " + stages[curStage].api + "Count=count&Price=price" + " отправлен");
        setIsVisible(false)
        await instance.post(stages[curStage].api, {Count: count, Price: price}).then(response => console.log(response));
        setIsSubmit(true);
        console.log("Запрос выполнен");        
    }
 
    return(
        <div className="game-info-box">
                <ul className="game-info">
                    <li>Bank buy {bankData.egpCount} pu for {bankData.egpPrice}</li>
                    <li>Bank sell {bankData.esmCount} mu for {bankData.esmPrice}</li>
                    <li>{stages[curStage].playersAction}</li>
                    <Timer initialTime={stageTime} curStage={curStage} setCurStage={setCurStage} isSubmit={isSubmit} setIsSubmit={setIsSubmit}></Timer>
                </ul>
                
                {(curStage === 2 || curStage === 4) && isVisible ? 
                <div className="player-requests-input-box">
                        <div className="player-request-input-container">
                            <div className="">
                                <label htmlFor="resources-number">You {curStage === 2 ? "buy" : "sell"}:</label>
                                <input type="number" min={0} max={10} required id="resources-number" name="resources-number" onChange={(evt) => setCount(evt.target.value)} value={count}/>
                                <span>{curStage === 2 ? "mu" : "pu"}</span>
                            </div>
                            <button className="request-stage-info-btn" onClick={() => console.log("Click!")}/>
                        </div>
                        <div className="player-request-input-container">
                            <div className="">
                                <label htmlFor="price">For price:</label>
                                <input type="number" min={0} max={10} required id="price" name="price" onChange={(evt) => setPrice(evt.target.value)} value={price}/>
                                <span>$</span>
                            </div>
                            <input type="submit" id="player-request-submit" value={""} onClick={() => submitHandle()}/>
                        </div>
                </div> : <></>
                }
                
        </div>
    )
}