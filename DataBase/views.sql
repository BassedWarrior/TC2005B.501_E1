USE chronicle_doom;

CREATE VIEW top_5_scores AS
SELECT username, score
FROM `match`
ORDER BY score DESC
LIMIT 5;


CREATE VIEW lowest_damage_players AS
SELECT m.username, m.damageTaken
FROM `match` m
JOIN player p ON m.username = p.username
ORDER BY m.damageTaken ASC
LIMIT 5;


CREATE VIEW time_played AS
SELECT 
    `match`.username,
    SEC_TO_TIME(AVG(TIME_TO_SEC(`match`.timePlayed))) AS average_time_played
FROM 
    `match`
GROUP BY 
    `match`.username
LIMIT 5;


CREATE VIEW card_count_by_category AS
SELECT category, COUNT(*) AS count
FROM card
GROUP BY category;


CREATE VIEW average_damage_ratio AS
SELECT 
    AVG(damageDealt) AS averageDamageDealt,
    AVG(damageTaken) AS averageDamageTaken
FROM `match`;


CREATE VIEW player_registration_year AS
SELECT 
    YEAR(creationDate) AS registration_year,
    COUNT(*) AS player_count
FROM 
    player
GROUP BY 
    registration_year;


CREATE VIEW deck_composition AS
SELECT 
    d.deckID,
    d.name AS deck_name,
    COUNT(dc.cardID) AS total_cards
FROM 
    deck d
LEFT JOIN 
    deckCard dc ON d.deckID = dc.deckID
GROUP BY 
    d.deckID, d.name;


CREATE VIEW highest_scoring_decks AS
SELECT 
    d.name AS deck_name,
    MAX(m.score) AS highest_score
FROM 
    `match` m
JOIN 
    deck d ON m.deck = d.deckID
GROUP BY 
    d.name;


CREATE VIEW average_kronos_spent AS
SELECT 
    username,
    AVG(khronosSpent) AS avg_kronos_spent
FROM 
    `match`
GROUP BY 
    username;


CREATE VIEW average_turns_played AS
SELECT 
    username,
    AVG(turnsPlayed) AS avg_turns_played
FROM
    `match`
GROUP BY
    username;