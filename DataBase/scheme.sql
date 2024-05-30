CREATE DATABASE IF NOT EXISTS chronicle_doom;
USE chronicle_doom;

CREATE TABLE IF NOT EXISTS player (
    username VARCHAR(20) NOT NULL,
    password VARCHAR(20) NOT NULL,
    creationDate DATE NOT NULL,
    highscore_deck INT UNSIGNED NOT NULL,
    PRIMARY KEY (username)
) ENGINE=InnoDB, CHARSET=ascii;

CREATE TABLE IF NOT EXISTS card (
    cardID TINYINT UNSIGNED NOT NULL AUTO_INCREMENT,
    cost TINYINT UNSIGNED NOT NULL,
    name VARCHAR(30) NOT NULL,
    description TINYTEXT NOT NULL,
    category ENUM('unit', 'paradox') NOT NULL,
    health TINYINT UNSIGNED,
    attack TINYINT UNSIGNED,
    PRIMARY KEY (cardID)
) ENGINE=InnoDB, CHARSET=ascii;

CREATE TABLE IF NOT EXISTS deck (
    deckID INT UNSIGNED NOT NULL AUTO_INCREMENT,
    name VARCHAR(30) NOT NULL,
    creationDate DATE NOT NULL,
    owner VARCHAR(20) NOT NULL,
    PRIMARY KEY (deckID),
    FOREIGN KEY (owner) REFERENCES player(username)
) ENGINE=InnoDB, CHARSET=ascii;

-- This stores the outcome of a full match after the player is done playing.
CREATE TABLE IF NOT EXISTS `match` (
    matchID INT UNSIGNED NOT NULL AUTO_INCREMENT,
    username VARCHAR(20) NOT NULL,
    deck INT UNSIGNED NOT NULL,
    datePlayed DATE NOT NULL,
    timePlayed TIME NOT NULL,
    score SMALLINT UNSIGNED NOT NULL,
    turnsPlayed TINYINT UNSIGNED NOT NULL,
    khronosSpent TINYINT UNSIGNED NOT NULL,
    damageDealt SMALLINT UNSIGNED NOT NULL,
    damageTaken SMALLINT UNSIGNED NOT NULL,
    playedRowOne TINYINT UNSIGNED NOT NULL,
    playedRowTwo TINYINT UNSIGNED NOT NULL,
    playedRowThree TINYINT UNSIGNED NOT NULL,
    averageTurnTime FLOAT NOT NULL,
    numCardsPlayed TINYINT UNSIGNED NOT NULL,
    cardsReturnedToQuantum TINYINT UNSIGNED NOT NULL,
    cardsBuffed TINYINT UNSIGNED NOT NULL,
    PRIMARY KEY (matchID),
    FOREIGN KEY (username) REFERENCES player(username),
    FOREIGN KEY (deck) REFERENCES deck(deckID),
    KEY (score)
) ENGINE=InnoDB, CHARSET=ascii;

CREATE TABLE IF NOT EXISTS gameRound (
    roundID TINYINT UNSIGNED NOT NULL,
    cardID TINYINT UNSIGNED NOT NULL,
    PRIMARY KEY (roundID, cardID),
    FOREIGN KEY (cardID) REFERENCES card(cardID)
) ENGINE=InnoDB, CHARSET=ascii;

-- This stores the state of the game for each player.
CREATE TABLE IF NOT EXISTS game (
    gameID INT UNSIGNED NOT NULL AUTO_INCREMENT,
    username VARCHAR(20) NOT NULL,
    score SMALLINT UNSIGNED NOT NULL,
    gameRound TINYINT UNSIGNED NOT NULL,
    kronos TINYINT UNSIGNED NOT NULL,
    deckCards TINYINT UNSIGNED NOT NULL,
    PRIMARY KEY (gameID),
    FOREIGN KEY (username) REFERENCES player(username),
    FOREIGN KEY (gameRound) REFERENCES gameRound(roundID),
    KEY (score)
) ENGINE=InnoDB, CHARSET=ascii;

CREATE TABLE IF NOT EXISTS deckCard (
    deckID INT UNSIGNED NOT NULL,
    cardID TINYINT UNSIGNED NOT NULL,
    PRIMARY KEY (deckID, cardID),
    FOREIGN KEY (deckID) REFERENCES deck(deckID),
    FOREIGN KEY (cardID) REFERENCES card(cardID)
) ENGINE=InnoDB, CHARSET=ascii;


CREATE TABLE IF NOT EXISTS EnemyWave (
    roundID TINYINT UNSIGNED NOT NULL,
    cardID TINYINT UNSIGNED NOT NULL,
    times TINYINT UNSIGNED NOT NULL,
    times TINYINT UNSIGNED NOT NULL,
    PRIMARY KEY (roundID, cardID),
    FOREIGN KEY (cardID) REFERENCES card(cardID)
) ENGINE=InnoDB, CHARSET=ascii;

