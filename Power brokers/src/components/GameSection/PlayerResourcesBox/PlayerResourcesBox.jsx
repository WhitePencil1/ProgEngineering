/* eslint-disable react/prop-types */
import "./PlayerResourcesBox.css"
import { instance } from "../../../utils/axios";
import { useState } from "react";
import ModalWindow from "../ModalWindow/ModalWindow";
import JsxParser from 'react-jsx-parser';

export default function PlayerResourcesBox({isMainPlayer, resources, setIsOpenModal}) {
    const classes = (isMainPlayer ? "resources-box" : "resources-box another-player-resources");

    
    const [isLogModalOpen, setIsLogModalOpen] = useState(false);
    const [logData, setLogData] = useState("");

    async function getLog() {
        const request = await instance.get("room/log");
        const data = "<li>" + request.data.slice(-30).reverse().join('</li><li>') + "</li>";
        setLogData(<JsxParser jsx={data} />)
        setIsLogModalOpen(true);
    }


    return(
        <div className={classes}>

            <ModalWindow isOpen={isLogModalOpen} onClose={() => setIsLogModalOpen(false)} onSubmit={() => setIsLogModalOpen(false)}>
                <h2>Записи игры</h2>
                <ul>
                    <li>
                        {logData}
                    </li>
                </ul>
            </ModalWindow>


            {resources &&
            <ul className="resources-list">
                <li><img src="./img/moneyIcon.png" alt="money" /> - {resources.money}$</li>
                <li><img src="./img/materialUnitIcon.png" alt="material"/> - {resources.esm}mu</li>
                <li><img src="./img/productUnitIcon.png" alt="product" /> - {resources.egp}pu</li>
            </ul>}
            
            {isMainPlayer && 
            <ul className="buttons-list">
                <li><img src="/public/img/exitBtn.png" alt="exit" onClick={() => setIsOpenModal(true)}/></li>
                <li><img src="/public/img/logBtn.png" alt="log" onClick={() => getLog()}/></li>
            </ul>}
            
        </div>
    )
}