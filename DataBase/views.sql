USE chronicle_doom;

CREATE VIEW top_5_scores AS
SELECT username, score
FROM `match`
ORDER BY score DESC
LIMIT 5;


CREATE VIEW LowestDamagePlayers AS
SELECT m.username, m.damageTaken
FROM `match` m
JOIN player p ON m.username = p.username
ORDER BY m.damageTaken ASC
LIMIT 5;


CREATE VIEW TimePlayed AS
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