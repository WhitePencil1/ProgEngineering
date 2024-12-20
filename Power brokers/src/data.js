export const avatarsWays = [
    "/img/Avatars/avatarSteve.jpg",
    "/img/Avatars/avatarFriren.jpg",
    "/img/Avatars/avatarChel.jpg",
    "/img/Avatars/avatarCat.jpg",
    "/img/Avatars/avatarGoblin.jpg",
    "/img/Avatars/avatarWomen.jpg",
    "/img/Avatars/avatarWolf.jpg",
    "/img/Avatars/avatarNaruto.jpg",
    "/img/Avatars/avatarPirat.jpeg",
]


// export const stages = {
//     Stage1: "player/Stage1",
//     Stage2: "player/Stage2",
//     Stage3: "player/Stage3",
//     Stage4: "player/Stage4",
//     Stage5: "player/Stage5",
//     Stage6: "player/Stage6",
//     Stage7: "player/Stage7",
//     Stage8: "player/Stage8",
//     Stage90: "player/Stage90",
//     Stage91: "player/Stage91",
// }


const playersAction = ["none", "upgrade", "build", "getLoan", "buyMu", "sellPu", "muDistribution", "paymentOfInterest"];


// const stageObject = {
//     stageIndex: 0,
//     curStage: stages[this.stageIndex],

//     setNextStage: async function() {
//         try {
//             this.stageIndex += 1;
//             console.log("Запрос " + this.curStage.api + " отправлен");
//             await instance.post(this.curStage.api)
//                 .then(response => console.log(response));
//             //await instance.get("players");
//             console.log("Запрос выполнен");
//         } catch(error) {
//             console.error(error);
//         }finally {
//             setTime(this.curStage.stageTime); // Сбрасываем таймер
//         }
//     }
// }

// const stageObject = {
//         stageIndex: 0,
//         curStage: stages[this.stageIndex],
    
//         setNextStage: async function() {
//             try {
//                 this.stageIndex += 1;
//                 console.log("Запрос " + this.curStage.api + " отправлен");
//                 await instance.get(this.curStage.api)
//                     .then(response => console.log(response));
//                 //await instance.get("players");
//                 console.log("Запрос выполнен");
//             } catch(error) {
//                 console.error(error);
//             }finally {
//                 setTime(this.curStage.stageTime); // Сбрасываем таймер
//             }
//         }
//     }

export const stages = [
    {
        api: "player/Stage1",
        stageTime: 1,
        playersAction: "Pay the costs"
    },
    {
        api: "player/Stage2",
        stageTime: 1,
        playersAction: "Study the market situation"
    },
    {
        api: "player/Stage3",
        stageTime: 1,
        playersAction: "Buy mu"
    },
    {
        api: "player/Stage4",
        stageTime: 60,
        playersAction: "Distribute the mu"
    },
    {
        api: "player/Stage5",
        stageTime: 60,
        playersAction: "Sell pu"
    },
    {
        api: "player/Stage6",
        stageTime: 10,
        playersAction: "Describe the loan percentage"
    },
    {
        api: "player/Stage7",
        stageTime: 90000,
        playersAction: "Pay off the loan"
    },
    {
        api: "player/Stage8",
        stageTime: 60,
        playersAction: "TakeLoan"
    },
    {
        api: "player/Stage90",
        stageTime: 90,
        playersAction: ""
    },
    {
        api: "player/Stage91",
        stageTime: 90,
        playersAction: ""
    },
    
]




// export const stages = {
//     Stage1: {
//         api: "player/Stage1",
//         stageTime: 10,
//         playersAction: playersAction[0]
//     },
//     Stage2: {
//         api: "player/Stage2",
//         stageTime: 10,
//         playersAction: playersAction[0]
//     },
//     Stage3: {
//         api: "player/Stage3",
//         stageTime: 10,
//         playersAction: playersAction[6]
//     },
//     Stage4: {
//         api: "player/Stage4",
//         stageTime: 10,
//         playersAction: playersAction[1]
//     },
//     Stage5: "player/Stage5",
//     Stage6: "player/Stage6",
//     Stage7: "player/Stage7",
//     Stage8: "player/Stage8",
//     Stage90: "player/Stage90",
//     Stage91: "player/Stage91",
// }