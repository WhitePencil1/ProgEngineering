/* eslint-disable react/prop-types */
import "./PlayerResourcesBox.css"


export default function PlayerResourcesBox({isMainPlayer, resources}) {
    const classes = (isMainPlayer ? "resources-box" : "resources-box another-player-resources");
    return(
        <div className={classes}>
            
            {resources &&
            <ul className="resources-list">
                <li><img src="./img/moneyIcon.png" alt="money" /> - {resources.money}$</li>
                <li><img src="./img/materialUnitIcon.png" alt="material"/> - {resources.esm}mu</li>
                <li><img src="./img/productUnitIcon.png" alt="product" /> - {resources.egp}pu</li>
            </ul>}
            
        </div>
    )
}