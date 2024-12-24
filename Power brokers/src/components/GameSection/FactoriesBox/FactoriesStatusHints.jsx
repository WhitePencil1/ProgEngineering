/* eslint-disable react/prop-types */
import "./FactoriesBox.css"


export default function FactoriesStatusHints({factoryData}) {


    return(
        <ul className="factories-status">
            {factoryData.esm != 0 && <li><img src="/public/img/inProduction.png" alt="" />{factoryData.esm} mu</li>}
            {factoryData.turnForNext > 0 && <li><img src="/public/img/underConstruction.png" alt="" />{factoryData.turnForNext} month</li>}
            {factoryData.isCredit && <li><img src="/public/img/isCredit.png" alt="credit" />{factoryData.turnForCredit} month</li>}
        </ul>
    )
}