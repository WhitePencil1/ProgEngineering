/* eslint-disable react/prop-types */
import "./Tooltip.css";


export default function Tooltip({ children, text, isVisible }) {
  //const [visible, setVisible] = useState(false);

  return (
    <div
      className={isVisible == true ? "tooltip-wrapper shake" : "tooltip-wrapper"}
    >
      {children} {/* Элемент, к которому привязывается подсказка */}
      {isVisible && <div className="tooltip">{text}</div>} {/* Всплывающая подсказка */}
    </div>
  );
}

