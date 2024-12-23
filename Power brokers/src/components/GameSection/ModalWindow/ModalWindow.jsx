/* eslint-disable react/prop-types */
import "./ModalWindow.css"
import Modal from 'react-modal';


Modal.setAppElement('#root');

export default function ModalWindow({isOpen, onClose, onSubmit, children}) {




    return (
        <Modal
            isOpen={isOpen}
            overlayClassName={"modal-overlay"}
            className={"modal-content"}>

            {onClose && <button className="modal-close-btn" onClick={() => onClose()}>X</button>}
            {children}
            
            
            <button className="modal-submit-btn" onClick={() => onSubmit()}>Подтвердить</button>
        </Modal>
    );
}