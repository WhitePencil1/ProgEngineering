/* eslint-disable react/prop-types */
export default function BlankPicture({pictureNum}) {

    let pictures = {
        1: "HappyNewYear1.jpg",
        2: "HappyNewYear2.jpg",
        3: "HappyNewYear3.jpg",
        4: "Snegovik.webp"
    }

    return(
        <img src={"/public/img/InGamePictures/" + pictures[pictureNum]} style={{width: "100%", height: "100%", objectFit: "cover"}} alt="" />
    )
}