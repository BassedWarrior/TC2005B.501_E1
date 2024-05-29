USE chronicle_doom;

CREATE VIEW unitCards 
AS SELECT * FROM card
WHERE card.category LIKE "unit";

CREATE VIEW paradoxCards 
AS SELECT cardID, cost, name, description, category FROM card
WHERE card.category LIKE "paradox";

CREATE VIEW playerGames
AS SELECT game.username, COUNT(*) AS games FROM game
GROUP BY game.username;

CREATE VIEW playerHighscore
AS SELECT game.username, MAX(game.score) AS highscore FROM game
GROUP BY game.username;

CREATE VIEW completePlayerDeck
AS SELECT deck.owner AS player, deck.deckID, deck.name FROM deck
INNER JOIN (
    SELECT deckID, COUNT(*) FROM deckCard
    GROUP BY deckID
    HAVING COUNT(*) = 18
) AS fullDeck USING (deckID);

CREATE VIEW deckCardFrequency
AS SELECT cardID, COUNT(*) AS frequency FROM deckCard
GROUP BY cardID;

CREATE VIEW gameAverageRoundKronos
AS SELECT game.gameRound, AVG(game.kronos) FROM game
GROUP BY game.gameRound;

CREATE VIEW matchAveragePlayerTimePlayed
AS SELECT `match`.username, AVG(`match`.timePlayed) FROM `match`
GROUP BY `match`.username;

CREATE VIEW matchGamesPerDay
AS SELECT `match`.datePlayed, COUNT(*) FROM `match`
GROUP BY `match`.datePlayed;

CREATE VIEW deckCardAverageCost
AS SELECT deckCard.deckID, AVG(card.cost) FROM deckCard
INNER JOIN card ON deckCard.cardID = card.cardID
GROUP BY deckCard.deckID;
