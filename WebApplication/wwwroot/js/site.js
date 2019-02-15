// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// used material: https://codepen.io/collosic/pen/JpDfn

// Write your Javascript code.
// Variables
var playerFleet, cpuFleet;
var attemptedHits = [];
var canTouch;
var boardSize = "10";
var ships = "5,4,3,2,1";

// Object Constructors
function Fleet(name) {
    this.name = name;
    this.shipDetails = [];
    shipsList = ships.split(',').map(Number);
    for (element in shipsList) {
        if (shipsList[element] === 1) {
            this.shipDetails.push({
                name: "patrol",
                length: 1
            });
        }
        if (shipsList[element] === 2) {
            this.shipDetails.push({
                name: "cruiser",
                length: 2
            });
        }
        if (shipsList[element] === 3) {
            this.shipDetails.push({
                name: "submarine",
                length: 3
            });
        }
        if (shipsList[element] === 4) {
            this.shipDetails.push({
                name: "battleship",
                length: 4
            });
        }
        if (shipsList[element] === 5) {
            this.shipDetails.push({
                name: "carrier",
                length: 5
            });
        }
    }
    this.numOfShips = this.shipDetails.length;
    this.ships = [];
    this.currentShipSize = 0;
    this.currentShip = 0;
    this.initShips = function() {
        for(var i = 0; i < this.numOfShips; i++) {
            this.ships[i] = new Ship(this.shipDetails[i].name);
            this.ships[i].length = this.shipDetails[i].length;
        }
    };
    this.removeShip = function(pos) {
        this.numOfShips--;
        $(".text").text(output.sunk(this.name, this.ships[pos].name));
        if (this == playerFleet) bot.sizeOfShipSunk = this.ships[pos].length;
        this.ships.splice(pos, 1);
        if (this.ships.length == 0) {
            $(".text").text(output.lost(this.name));
        }
        return true;
    };
    this.shipHit = function(ship_name) {
        $(".text").text(output.hit(this.name));
        return true;
    }
    this.checkIfHit = function(point) {
        for(var i = 0; i < this.numOfShips; i++) {
            if (this.ships[i].checkLocation(point)) {
                this.ships[i].getRidOf(this.ships[i].hitPoints.indexOf(point));
                if (this.ships[i].hitPoints == 0)return this.removeShip(i);
                else return this.shipHit(this.ships[i].name);
            }
        }
        return false;
    };
}

function Ship(name){
    this.name = name;
    this.length = 0;
    this.hitPoints = [];
    this.populateHorzHits = function(start) {
        for (var i = 0; i < this.length; i++, start++) {
            this.hitPoints[i] = start;
        }
    };
    this.populateVertHits = function(start) {
        for (var i = 0; i < this.length; i++, start += parseInt(boardSize)) {
            this.hitPoints[i] = start;
        }
    };
    this.checkLocation = function(loc) {
        for (var i = 0; i < this.length; i++) {
            if (this.hitPoints[i] == loc) return true;
        }
        return false;
    };
    this.getRidOf = function(pos) {
        this.hitPoints.splice(pos, 1);
    }
}


function SetGame(canBoatsTouch, boardSizes, shipsList, player){
    canTouch = canBoatsTouch;
    boardSize = boardSizes;
    ships = shipsList;
    playerFleet = player;
}

// Console obj
var output = {
    "welcome": " > Welcome to BattleShip.  Use the menu above to get started.",
    "not": " > This option is not currently available.",
    "player1": " > Would you like to place your own ships or have the computer randomly do it for you?",
    "setting": " > Customise the game suitable for your needs.",
    "self": " > Use the mouse and the Horizontal and Vertial buttons to place your ships on the bottom grid.",
    "overlap": " > You can not overlap ships.  Please try again.",
    "canTouch": " > At the moment ships can touch, press the button again to change that.",
    "cannotTouch": " > At the moment ships can not touch, press the button again to change that.",
    "start": " > Use the mouse to fire on the top grid.  Good Luck!",
    placed: function(name) { return " > Your " + name + " been placed."; },
    hit: function(name, type) { return " > " + name + "'s ship was hit." },
    miss: function(name) { return " > " + name + " missed!" },
    sunk: function(user, type) { return " > " + user + "'s " + type + " was sunk!" },
    lost: function(name) { return " > " + name + " has lost his fleet!!  Game Over." },
};

// Objects for playing the game and bot for playing the computer
var leftBoard = {
    allHits: [],
    highlight: function(square) {
        $(square).addClass("target").off("mouseleave").on("mouseleave", function() {
            $(this).removeClass("target");
        });

        $(square).off("click").on("click", function() {
            if(!($(this).hasClass("used"))) {
                $(this).removeClass("target").addClass("used");
                var num = parseInt($(this).attr("class").slice(15));
                var bool = cpuFleet.checkIfHit(num);
                if (false == bool) {
                    $(".text").text(output.miss("You"));
                    $(this).children().addClass("miss");
                } else $(this).children().addClass("hit");
                $(".left").find(".points").off("mouseenter").off("mouseover").off("mouseleave").off("click");
                // Check if it's the end of the game
                if (cpuFleet.ships.length == 0) {
                    $(".left").find(".points").off("mouseenter").off("mouseover").off("mouseleave").off("click");

                } else setTimeout(bot.select, 800);
            } // end of if
        });
    },
}

var rightBoard = {
    currentHits: [],
    checkAttempt: function(hit) {
        if (playerFleet.checkIfHit(hit)) {
            // Insert hit into an array for book keeping
            rightBoard.currentHits.push(hit);
            if (this.currentHits.length > 1) bot.prev_hit = true;
            // display hit on the grid
            $(".right").find("." + hit).children().addClass("hit");
            if (rightBoard.hasShipBeenSunk()) {
                // clear flags
                bot.hunting = bot.prev_hit = false;
                if (bot.sizeOfShipSunk === rightBoard.currentHits.length) {
                    bot.num_misses = bot.back_count = bot.nextMove.length = rightBoard.currentHits.length = bot.sizeOfShipSunk = bot.currrent = 0;
                } else {
                    bot.special =  bot.case1 = true;
                }
                // check for special cases
                if (bot.specialHits.length > 0) bot.special = true;
                // check for end of game.	
            }
            return true;
        } else {
            $(".right").find("." + hit).children().addClass("miss");
            bot.current = rightBoard.currentHits[0];
            bot.prev_hit = false;
            if (rightBoard.currentHits.length > 1) {
                bot.back = true;
                bot.num_misses++;
            }
            if (bot.case2) {
                bot.special = true;
                bot.case2 = false;
            }
            return false;
        }
    },

    hasShipBeenSunk: function() {
        if (bot.sizeOfShipSunk > 0) return true;
        else return false;
    }
}


var bot = {
    back: false,
    hunting: false,
    prev_hit: false,
    first_hit: false,
    special: false,
    case1: false,
    case2: false,
    num_misses: 0,
    back_count: 0,
    randPool: [],
    nextMove: [],
    attempted: [],
    specialHits: [],
    direction: "",
    current: 0,
    numAttemptsAfterHit: 0,
    sizeOfShipSunk: 0,
    randomGen: function(size) {
        return Math.floor(Math.random() * size);
    },
    select: function() {
        if (bot.hunting) {
            bot.battleLogic();
        } else if (bot.special) {
            bot.specialCase();
        } else {
            // grab a random number from the pool and increase attempts
            bot.current = bot.randPool[bot.randomGen(bot.randPool.length)];
            bot.attempted.push(bot.current);
            bot.first_hit = true;
            // remove current guess from the random pool and check if hit
            bot.removeGuess(bot.randPool.indexOf(bot.current));
            bot.hunting = rightBoard.checkAttempt(bot.current);
        }
        setTimeout(highlightBoard(), 50);
    },

    removeGuess: function(index) {
        bot.randPool.splice(index, 1);
    },

    battleLogic: function() {
        if (bot.first_hit) {
            bot.createMoves();
            bot.first_hit = false;
        }

        if (bot.num_misses > 1) {
            bot.specialCase();
        } else if (bot.back) {
            bot.back = false;
            bot.backy();
            bot.deployHit(bot.current);
        } else if (bot.prev_hit) {
            bot.continueHits();
            bot.deployHit(bot.current);
            console.log(bot.prev_hit);
        } else {
            bot.direction = bot.nextMove.pop();
            console.log(bot.direction + " " + bot.current);
            bot.getNumericalDirection(bot.direction);
            bot.prev_hit = bot.deployHit(bot.current);
            console.log(bot.prev_hit);
        }
    },

    deployHit: function(hit) {
        if (bot.special) {
            bot.specialCase();
        } else {
            bot.attempted.push(hit);
            bot.removeGuess(bot.randPool.indexOf(hit));
            return rightBoard.checkAttempt(hit);
        }
    },

    createMoves: function() {
        if(bot.current == 1) {
            bot.getRandomMoves(["right", "down"]);
        }
        else if(bot.current == parseInt(boardSize)) {
            bot.getRandomMoves(["left", "down"]);
        }
        else if(bot.current == parseInt(boardSize)*parseInt(boardSize)-parseInt(boardSize)-1) {
            bot.getRandomMoves(["up", "right"]);
        }
        else if(bot.current == parseInt(boardSize)*parseInt(boardSize)) {
            bot.getRandomMoves(["left", "up"]);
        }
        else if(!(bot.current % parseInt(boardSize))){
            bot.getRandomMoves(["up", "down", "left"]);
        }
        else if(bot.current < parseInt(boardSize)) {
            bot.getRandomMoves(["right", "down", "left"]);
        }
        else if(bot.current % parseInt(boardSize) == 1) {
            bot.getRandomMoves(["up", "right", "down"]);
        }
        else if(bot.current > parseInt(boardSize)*parseInt(boardSize)-parseInt(boardSize)-1) {
            bot.getRandomMoves(["up", "right", "left"]);
        }
        else {
            bot.getRandomMoves(["up", "right", "down", "left"]);
        }
    },

    getRandomMoves: function(possibleMoves) {
        while (possibleMoves.length != 0) {
            // pick a random direction
            var dir = bot.randomGen(possibleMoves.length);
            // Go Up
            if (possibleMoves[dir] == "up") {
                if (bot.randPool.some(function(x) { return x == bot.current - parseInt(boardSize); })) {
                    bot.nextMove.push("up");
                }
            }
            // Go right
            if (possibleMoves[dir] == "right") {
                if (bot.randPool.some(function(x) { return x == bot.current + 1; })) {
                    bot.nextMove.push("right");
                }
            }
            // Go down
            if (possibleMoves[dir] == "down") {
                if (bot.randPool.some(function(x) { return x == bot.current + parseInt(boardSize); })) {
                    bot.nextMove.push("down");
                }
            }
            // Go left
            if (possibleMoves[dir] == "left") {
                if (bot.randPool.some(function(x) { return x == bot.current - 1; })) {
                    bot.nextMove.push("left");
                }
            }
            possibleMoves.splice(dir, 1);
        }
    },

    getNumericalDirection: function(dir) {
        if (dir == "up") bot.current -= parseInt(boardSize);
        if (dir == "right") bot.current += 1;
        if (dir == "down") bot.current += parseInt(boardSize);
        if (dir == "left") bot.current -= 1;
        console.log(bot.current + " attempted " + bot.attempted);
        // check if already used
        if (bot.attempted.some(function(x) { return x == bot.current; }) && bot.specialHits.length == 0) {
            bot.current = rightBoard.currentHits[0];
            if (bot.back_count > 1) bot.special = true;
            else bot.backy();
        }
        return false;
    },

    continueHits: function() {
        console.log("cont " + bot.direction);
        if (bot.direction == "up") {
            if (bot.checkLocation("up")) {
                bot.direction = "down";
                return bot.getNumericalDirection(bot.direction);
            } else return bot.getNumericalDirection(bot.direction);
        }
        if (bot.direction == "right") {
            if (bot.checkLocation("right")) {
                bot.direction = "left";
                return bot.getNumericalDirection(bot.direction);
            } else return bot.getNumericalDirection(bot.direction);
        }
        if (bot.direction == "down") {
            if (bot.checkLocation("down")) {
                bot.direction = "up";
                return bot.getNumericalDirection(bot.direction);
            } else return bot.getNumericalDirection(bot.direction);
        }
        if (bot.direction == "left") {
            if (bot.checkLocation("left")) {
                bot.direction = "right";
                return bot.getNumericalDirection(bot.direction);
            } else return bot.getNumericalDirection(bot.direction);
        }
    },

    backy: function() {
        bot.back_count++;
        if (bot.direction == "up") {
            bot.direction = "down";
            return bot.continueHits();
        }
        if (bot.direction == "right") {
            bot.direction = "left";
            return bot.continueHits();
        }
        if (bot.direction == "down") {
            bot.direction = "up";
            return bot.continueHits();
        }
        if (bot.direction == "left") {
            bot.direction = "right";
            return bot.continueHits();
        }
    },

    checkLocation: function(dir) {
        if (dir == "up") {
            if (bot.current < parseInt(boardSize)+1) return true
        }
        if (dir == "right") {
            if (bot.current % parseInt(boardSize) == 0) return true
        }
        if (dir == "down") {
            if (bot.current > parseInt(boardSize)*parseInt(boardSize)-parseInt(boardSize)) return true
        }
        if (dir == "left") {
            if (bot.current % parseInt(boardSize) == 1) return true
        }
        return false;
    },

    specialCase: function() {
        bot.num_misses = bot.back_count = bot.nextMove.length = 0;
        if (bot.case1) {
            bot.prev_hit = true;
            if (bot.getNewCurrent(bot.direction)) {
                rightBoard.currentHits.length = 0;
                rightBoard.currentHits.push(bot.current);
                bot.first_hit = true;
                bot.prev_hit = false;
            }
            bot.special = bot.case1 = bot.back = false;
            bot.hunting = true;
            bot.sizeOfShipSunk = 0;
            bot.battleLogic();
        } else {
            if (bot.specialHits.length == 0) {
                for(var i = 0; i < rightBoard.currentHits.length; i++) {
                    bot.specialHits.push(rightBoard.currentHits[i]);
                }
                rightBoard.currentHits.length = 0;
            }
            bot.current = bot.specialHits.pop();
            rightBoard.currentHits.push(bot.current);
            bot.special = bot.back = bot.prev_hit = false;
            bot.first_hit = bot.hunting = true;
            bot.battleLogic();
        }
    },

    getNewCurrent: function(direction) {
        var difference = rightBoard.currentHits.length - bot.sizeOfShipSunk;
        if (bot.direction == "up") {
            bot.direction = "down";
            if (difference > 1) {
                bot.current += parseInt(boardSize) * (rightBoard.currentHits.length - 1);
                var temp = bot.current + (parseInt(boardSize) * (difference - 1));
                rightBoard.currentHits.length = 0;
                for (var i = 0; i < difference; i++) {
                    rightBoard.currentHits.push(temp);
                    temp += parseInt(boardSize);
                }
                bot.case2 = true;
                return false;
            }
            bot.current += parseInt(boardSize) * bot.sizeOfShipSunk;
            return true;
        }
        if (bot.direction == "right") {
            bot.direction = "left";
            if (difference > 1) {
                bot.current -= rightBoard.currentHits.length - 1;
                var temp = bot.current + (difference - 1);
                rightBoard.currentHits.length = 0;
                for (var i = 0; i < difference; i++) {
                    rightBoard.currentHits.push(temp);
                    temp -= 1;
                }
                bot.case2 = true;
                return false;
            }
            bot.current -= bot.sizeOfShipSunk;
            return true;
        }
        if (bot.direction == "down") {
            bot.direction = "up";
            if (difference > 1) {
                bot.current -= parseInt(boardSize) * (rightBoard.currentHits.length - 1);
                var temp = bot.current - (parseInt(boardSize) * (difference - 1));
                rightBoard.currentHits.length = 0;
                for (var i = 0; i < difference; i++) {
                    rightBoard.currentHits.push(temp);
                    temp -= parseInt(boardSize);
                }
                bot.case2 = true;
                return false;
            }
            bot.current -= parseInt(boardSize) * bot.sizeOfShipSunk;
            return true;
        }
        if (bot.direction == "left") {
            bot.direction = "right";
            if (difference > 1) {
                bot.current += rightBoard.currentHits.length - 1;
                var temp = bot.current - (difference - 1);
                rightBoard.currentHits.length = 0;
                for (var i = 0; i < difference; i++) {
                    rightBoard.currentHits.push(temp);
                    temp += 1;
                }
                bot.case2 = true;
                return false;
            }
            bot.current += bot.sizeOfShipSunk;
            return true;
        }
    }
}

//  Create the games grids and layout
function createBoard(t) {
    var gridSize = parseInt(boardSize)*30;
    $(".grid").css("width", gridSize+"px");
    $(".grid").css("height", gridSize+"px");
    var size = parseInt(boardSize)+1;
    var sum = parseInt(boardSize)*parseInt(boardSize);
    var letters = sum-parseInt(boardSize);
    for (var i = 1; i <= sum; i++) {
        // The number and letter designators
        if (i < parseInt(size)) {
            $(".left").prepend("<span class='aTops'>" + Math.abs(i - parseInt(size)) + "</span>");
            $(".right").prepend("<span class='aTops'>" + Math.abs(i - parseInt(size)) + "</span>");
            $(".grid").append("<li class='points offset1 " + i + "'><span class='hole'></span></li>");
        } else {
            $(".grid").append("<li class='points offset2 " + i + "'><span class='hole'></span></li>");
        }
        if (i === parseInt(size)) {
            $(".left").prepend("<span class='aTops hidezero'>" + Math.abs(i - parseInt(size)) + "</span>");
            $(".right").prepend("<span class='aTops hidezero'>" + Math.abs(i - parseInt(size)) + "</span>");
        }
        if (i > parseInt(letters)) {
            $(".left").append("<span class='aLeft'>" +
                String.fromCharCode(97 + (i - parseInt(letters)-1)).toUpperCase() + "</span>");
            $(".right").append("<span class='aLeft'>" +
                String.fromCharCode(97 + (i - parseInt(letters)-1)).toUpperCase() + "</span>");
        }
    }
    $(".text").text(output.welcome);
}

// Start the game setup
$(document).ready(function() {
    $(".one").on("click", function() {
        $(".text").text(output.player1);
        createBoard(this);
        gameSetup(this);
    });
    $(".load").on("click", function(e) {
        createBoard(this);

    });
    $(".settings").on("click", function(e) {
        $(".text").text(output.setting);
        gameSettings(this);
    });
});

function gameSetup(t) {
    $(t).off() && $(".two").off();
    $(".one").addClass("self").removeClass("one").text("Place My Own");
    $(".load").addClass("random").removeClass("load").text("Random");
    $(".settings").addClass("hidden").removeClass("settings");


    $(".self").off("click").on("click", function() {
        $(".text").text(output.self);
        selfSetup(playerFleet);
    });
    $(".random").off("click").on("click", function() {
        playerFleet = new Fleet("Player 1");
        playerFleet.initShips();
        randomSetup(playerFleet);
    });
}


function gameSettings(t) {
    $(t).off() && $(".two").off();
    $(".one").addClass("ships").removeClass("one").text("Change Ships");
    $(".load").addClass("boardSize").removeClass("load").text("Change Board");
    $(".settings").addClass("canTouch").removeClass("settings").text("Can Ships Touch");

    $(".ships").off("click").on("click", function() {
        $(".text").text(output.canTouch);
        getShips(this);
    });
    $(".boardSize").off("click").on("click", function() {
        $(".text").text(output.canTouch);
        getBoardSize(this);
    });
    $(".canTouch").off("click").on("click", function() {
        $(".text").text(output.canTouch);
        getCanShipsTouch(this);
    });
}


function getShips(t) {
    ships = prompt("Insert comma separated values for ship sizes", ships);
    if (ships === "") {
        document.getElementById("alert").innerHTML =
            "You did not enter any value!";
    }
    $(t).off() && $(".two").off();
    $(".ships").addClass("one").removeClass("ship").text("One-Player");
    $(".boardSize").addClass("load").removeClass("boardSize").text("Two-Player");
    $(".canTouch").addClass("settings").removeClass("canTouch").text("Settings");

    $(".one").off("click").on("click", function() {
        $(".text").text(output.player1);
        createBoard(this);
        gameSetup(this);
    });
    $(".load").off("click").on("click", function() {
        e.preventDefault();
        if (!$("div").hasClass("error")) {
            $(".text").text(output.not);
            $(this).addClass("error");
        }
    });
    $(".settings").off("click").on("click", function() {
        $(".text").text(output.setting);
        gameSettings(this);
    });
}

function getBoardSize(t) {
    boardSize = prompt("Enter your board size", boardSize);
    if (boardSize === "") {
        document.getElementById("alert").innerHTML =
            "You did not enter any value!";
    }
    $(t).off() && $(".two").off();
    $(".ships").addClass("one").removeClass("ship").text("One-Player");
    $(".boardSize").addClass("load").removeClass("boardSize").text("Two-Player");
    $(".canTouch").addClass("settings").removeClass("canTouch").text("Settings");

    $(".one").off("click").on("click", function() {
        $(".text").text(output.player1);
        createBoard(this);
        gameSetup(this);
    });
    $(".load").off("click").on("click", function() {
        e.preventDefault();
        if (!$("div").hasClass("error")) {
            $(".text").text(output.not);
            $(this).addClass("error");
        }
    });
    $(".settings").off("click").on("click", function() {
        $(".text").text(output.setting);
        gameSettings(this);
    });
}

function getCanShipsTouch(t) {
    canTouch = prompt("Please enter Y or N for whether ships can touch", "Y");
    if (canTouch === "") {
        document.getElementById("alert").innerHTML =
            "You did not enter any value!";
    }
    $(t).off() && $(".two").off();
    $(".ships").addClass("one").removeClass("ship").text("One-Player");
    $(".boardSize").addClass("load").removeClass("boardSize").text("Two-Player");
    $(".canTouch").addClass("settings").removeClass("canTouch").text("Settings");

    $(".one").off("click").on("click", function() {
        $(".text").text(output.player1);
        createBoard(this);
        gameSetup(this);
    });
    $(".load").off("click").on("click", function() {
        e.preventDefault();
        if (!$("div").hasClass("error")) {
            $(".text").text(output.not);
            $(this).addClass("error");
        }
    });
    $(".settings").off("click").on("click", function() {
        $(".text").text(output.setting);
        gameSettings(this);
    });
}

function selfSetup() {
    $(".self").addClass("horz").removeClass("self").text("Horizontal");
    $(".random").addClass("vert").removeClass("random").text("Vertical");

    // initialize the fleet
    playerFleet = new Fleet("Player 1");
    playerFleet.initShips();
    // light up the players ship board for placement
    placeShip(playerFleet.ships[playerFleet.currentShip], playerFleet);
}

function randomSetup(fleet) {
    // Decide if the ship will be placed vertically or horizontally 
    // if 0 then ship will be places horizontally if 1 vertically
    // setShip(location, ship, "vert", fleet, "self");
    if (fleet.currentShip >= fleet.numOfShips) return; // regard against undefined length

    var orien = Math.floor((Math.random() * parseInt(boardSize)) + 1);
    var length = fleet.ships[fleet.currentShip].length;

    if (orien < 6) {
        // create a random number betwee 1 and 6
        var shipOffset = parseInt(boardSize)+1 - fleet.ships[fleet.currentShip].length;
        var horiz = Math.floor((Math.random() * shipOffset) + 1);
        var vert = Math.floor(Math.random() * parseInt(boardSize)-1);
        var randNum = parseInt(String(vert) + String(horiz));
        if (fleet == cpuFleet) checkOverlap(randNum, length, "horz", fleet);
        else setShip(randNum, fleet.ships[fleet.currentShip], "horz", fleet, "random");
    } else {
        var shipOffset = parseInt(boardSize)*parseInt(boardSize)+parseInt(boardSize) - (fleet.ships[fleet.currentShip].length * parseInt(boardSize));
        var randNum = Math.floor((Math.random() * shipOffset) + 1);

        if (fleet == cpuFleet) checkOverlap(randNum, length, "vert", fleet);
        else setShip(randNum, fleet.ships[fleet.currentShip], "vert", fleet, "random");
    }
}

function createCpuFleet() {
    // create a random ship placement for the cpu's fleet
    cpuFleet = new Fleet("Player 2");
    cpuFleet.initShips();
    randomSetup(cpuFleet);
}


function placeShip(ship, fleet) {
    // check orientation of ship and highlight accordingly
    var orientation = "horz";
    $(".vert").off("click").on("click", function() {
        orientation = "vert";
    });
    $(".horz").off("click").on("click", function() {
        orientation = "horz";
    });
    // when the user enters the grid have the ships lenght highlighted with the
    // ships length.
    $(".right").find(".points").off("mouseenter").on("mouseenter", function() {
        var num = $(this).attr('class').slice(15);
        //
        if (orientation == "horz") displayShipHorz(parseInt(num), ship, this, fleet);
        else displayShipVert(parseInt(num), ship, this, fleet);
    });
}


function displayShipHorz(location, ship, point, fleet) {
    var endPoint = location + ship.length - 2;
    if (!(endPoint % parseInt(boardSize) >= 0 && endPoint % parseInt(boardSize) < ship.length - 1)) {
        for (var i = location; i < (location + ship.length); i++) {
            $(".right ." + i).addClass("highlight");
        }
        $(point).off("click").on("click", function() {
            setShip(location, ship, "horz", fleet, "self");
        });
    }
    $(point).off("mouseleave").on("mouseleave", function() {
        removeShipHorz(location, ship.length);
    });
}

function displayShipVert(location, ship, point, fleet) {
    var endPoint = (ship.length * parseInt(boardSize)) - parseInt(boardSize);
    var inc = 0;
    if (location + endPoint <= parseInt(boardSize)*parseInt(boardSize)) {
        for (var i = location; i < (location + ship.length); i++) {
            $(".right ." + (location + inc)).addClass("highlight");
            inc = inc + parseInt(boardSize);
        }
        $(point).off("click").on("click", function() {
            setShip(location, ship, "vert", fleet, "self");
        });
    }
    $(point).off("mouseleave").on("mouseleave", function() {
        removeShipVert(location, ship.length);
    });
}

function removeShipHorz(location, length) {
    for (var i = location; i < location + length; i++) {
        $(".right ." + i).removeClass("highlight");
    }
}

function removeShipVert(location, length) {
    var inc = 0;
    for (var i = location; i < location + length; i++) {
        $(".right ." + (location + inc)).removeClass("highlight");
        inc = inc + parseInt(boardSize);
    }
}

function setShip(location, ship, orientation, genericFleet, type) {
    if (!(checkOverlap(location, ship.length, orientation, genericFleet))) {
        if (orientation == "horz") {
            genericFleet.ships[genericFleet.currentShip].populateHorzHits(location);
            $(".text").text(output.placed(genericFleet.ships[genericFleet.currentShip].name + " has"));
            for (var i = location; i < (location + ship.length); i++) {
                $(".right ." + i).addClass(genericFleet.ships[genericFleet.currentShip].name);
                $(".right ." + i).children().removeClass("hole");
            }
            if (++genericFleet.currentShip == genericFleet.numOfShips) {
                $(".text").text(output.placed("ships have"));
                $(".right").find(".points").off("mouseenter");
                // clear the call stack
                setTimeout(createCpuFleet, parseInt(boardSize)*parseInt(boardSize));
            } else {
                if (type == "random") randomSetup(genericFleet);
                else placeShip(genericFleet.ships[genericFleet.currentShip], genericFleet);
            }

        } else {
            var inc = 0;
            genericFleet.ships[genericFleet.currentShip].populateVertHits(location);
            $(".text").text(output.placed(genericFleet.ships[genericFleet.currentShip].name + " has"));
            for (var i = location; i < (location + ship.length); i++) {
                $(".right ." + (location + inc)).addClass(genericFleet.ships[genericFleet.currentShip].name);
                $(".right ." + (location + inc)).children().removeClass("hole");
                inc = inc + parseInt(boardSize);
            }
            if (++genericFleet.currentShip == genericFleet.numOfShips) {
                $(".text").text(output.placed("ships have"));
                $(".right").find(".points").off("mouseenter");
                // clear the call stack
                setTimeout(createCpuFleet, parseInt(boardSize)*parseInt(boardSize));
            } else {
                if (type == "random") randomSetup(genericFleet);
                else placeShip(genericFleet.ships[genericFleet.currentShip], genericFleet);
            }
        }
    } else {
        if (type == "random") randomSetup(genericFleet);
        else $(".text").text(output.overlap);
    }
} // end of setShip()

function checkOverlap(location, length, orientation, genFleet) {
    var loc = location;
    if (orientation == "horz") {
        var end = location + length;
        for (; location < end; location++) {
            for (var i = 0; i < genFleet.currentShip; i++) {
                if (genFleet.ships[i].checkLocation(location)) {
                    if (genFleet == cpuFleet) randomSetup(genFleet);
                    else return true;
                }
            } // end of for loop
        } // end of for loop
    } else {
        var end = location + (parseInt(boardSize) * length);
        for (; location < end; location += parseInt(boardSize)) {
            for (var i = 0; i < genFleet.currentShip; i++) {
                if (genFleet.ships[i].checkLocation(location)) {
                    if (genFleet == cpuFleet) randomSetup(genFleet);
                    else return true;
                }
            }
        }
    } // end of if/else 
    if (genFleet == cpuFleet && genFleet.currentShip < genFleet.numOfShips) {
        if (orientation == "horz") genFleet.ships[genFleet.currentShip++].populateHorzHits(loc);
        else genFleet.ships[genFleet.currentShip++].populateVertHits(loc);
        if (genFleet.currentShip == genFleet.numOfShips) {
            // clear the call stack
            setTimeout(startGame, 500);
        } else randomSetup(genFleet);
    }
    return false;
} // end of checkOverlap()


function startGame() {
    $(".text").text(output.start);
    // Generate all possible hits for Player 1
    for (var i = 0; i < parseInt(boardSize)*parseInt(boardSize); i++) bot.randPool[i] = i + 1;
    highlightBoard();
}

function highlightBoard() {
    if (playerFleet.ships.length == 0) {
        $(".left").find(".points").off("mouseenter").off("mouseleave").off("click");
    } else {
        $(".left").find(".points").off("mouseenter mouseover").on("mouseenter mouseover", function() {
            // only allow target highlight on none attempts
            if(!($(this).hasClass("used"))) leftBoard.highlight(this);
        });
    }
}
