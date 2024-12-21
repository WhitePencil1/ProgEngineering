/* eslint-disable react/prop-types */
import "./PlayerResourcesBox.css"


export default function PlayerResourcesBox({isMainPlayer, resources, setIsOpenModal}) {
    const classes = (isMainPlayer ? "resources-box" : "resources-box another-player-resources");

    

    return(
        <div className={classes}>
            
            {resources &&
            <ul className="resources-list">
                <li><img src="./img/moneyIcon.png" alt="money" /> - {resources.money}$</li>
                <li><img src="./img/materialUnitIcon.png" alt="material"/> - {resources.esm}mu</li>
                <li><img src="./img/productUnitIcon.png" alt="product" /> - {resources.egp}pu</li>
            </ul>}
            
            {isMainPlayer && 
            <ul className="buttons-list">
                <li><img src="/public/img/exitBtn.png" alt="exit" onClick={() => setIsOpenModal(true)}/></li>
                <li><img src="/public/img/logBtn.png" alt="log" /></li>
            </ul>}
            
        </div>
    )
}