import "./PlayerResourcesBox.css"

// eslint-disable-next-line react/prop-types
export default function PlayerResourcesBox({money, materials, products, isMainPlayer}) {
    const classes = (isMainPlayer ? "resources-box" : "resources-box another-player-resources");
    return(
        <div className={classes}>
            <ul className="resources-list">
                <li><img src="./img/moneyIcon.png" alt="money" /> - 300$</li>
                <li><img src="./img/materialUnitIcon.png" alt="material"/> - 2mu</li>
                <li><img src="./img/productUnitIcon.png" alt="product" /> - 6pu</li>
            </ul>
            
        </div>
    )
}