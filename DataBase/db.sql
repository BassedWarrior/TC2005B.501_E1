CREATE DATABASE chronical-doom;
USE chronical-doom;

CREATE TABLE player (
    username: VARCHAR(20) NOT NULL,
    password: VARCHAR(20) NOT NULL,
    creationDate: DATE NOT NULL,
    PRIMARY KEY (username),
);

CREATE TABLE card (
    cardID: TINYINT UNSIGNED NOT NULL,
    cost: TINYINT UNSIGNED NOT NULL,
    name: VARCHAR(30) NOT NULL,
    description: TINYTEXT NOT NULL,
    category: ENUM("unit", "paradox") NOT NULL,
    health: TINYINT UNSIGNED,
    attack: TINYINT UNSIGNED,
    image: BINARY NOT NULL,
    PRIMARY KEY (cardID) AUTOINCREMENT,
);

CREATE TABLE deck (
    deckID: INT UNSIGNED NOT NULL,
    name: VARCHAR(30) NOT NULL,
    highscore: SMALLINT UNSIGNED NOT NULL,
    creationDate: DATE NOT NULL,
    owner: VARCHAR(30) NOT NULL,
    PRIMARY KEY (deckID) AUTOINCREMENT,
    FOREIGN KEY (owner) REFERENCES player.username,
);

CREATE TABLE gameRound (
    roundID: TINYINT NOT NULL,
    PRIMARY KEY (roundID),
);

CREATE TABLE match (
    matchID: INT UNSIGNED NOT NULL,
    username: VARCHAR(30) NOT NULL,
    deck: INT UNSIGNED NOT NULL,
    datePlayed: DATE NOT NULL,
    timePlayed: TIME NOT NULL,
    score: SMALLINT UNSIGNED NOT NULL,
    PRIMARY KEY (matchID),
    FOREIGN KEY (username) REFERENCES player.username,
    FOREIGN KEY (deck) REFERENCES deck.deckID,
);

CREATE TABLE game (
    gameID: INT UNSIGNED NOT NULL,
    username: VARCHAR(30) NOT NULL,
    score: SMALLINT UNSIGNED NOT NULL,
    gameRound: INT UNSIGNED NOT NULL,
    kronos: TINYINT UNSIGNED NOT NULL,
    deckCards: TINYINT UNSIGNED NOT NULL,
    PRIMARY KEY (gameID) AUTOINCREMENT,
    FOREIGN KEY (username) REFERENCES player.username,
    FOREIGN KEY (gameRound) REFERENCES gameRound.roundID
);
