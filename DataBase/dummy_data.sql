-- Insert data into match table
INSERT INTO player (username, password, creationDate, highscore_deck) VALUES
('user1', 'password1', '2023-01-01', 10),
('user2', 'password2', '2023-01-02', 20),
('user3', 'password3', '2023-01-03', 30),
('user4', 'password4', '2023-01-03', 234),
('user5', 'password5', '2023-02-03', 12),
('user6', 'password6', '2023-02-04', 66),
('user7', 'password7', '2023-02-04', 122),
('user8', 'password8', '2023-03-04', 23),
('user9', 'password9', '2024-04-04', 58),
('user10', 'password10', '2024-04-07', 58),
('user11', 'password11', '2024-04-07', 34),
('user12', 'password12', '2024-05-07', 231),
('user13', 'password13', '2024-05-07', 11),
('user14', 'password14', '2022-08-08', 212),
('user15', 'password15', '2022-08-09', 34),
('user16', 'password16', '2022-08-09', 91),
('user17', 'password17', '2022-08-09', 99),
('user18', 'password18', '2021-02-07', 45),
('user19', 'password19', '2021-02-06', 63),
('user20', 'password20', '2021-02-05', 45),
('user21', 'password21', '2021-03-02', 19),
('user22', 'password22', '2021-03-01', 11),
('user23', 'password23', '2024-03-01',73),
('user24', 'password24', '2024-09-01', 35),
('user25', 'password25', '2024-10-02', 12),
('user26', 'password26', '2024-10-09', 46),
('user27', 'password27', '2024-11-09', 81),
('user28', 'password28', '2024-11-11', 12),
('user29', 'password29', '2023-12-12', 92),
('user30', 'password30', '2021-12-12', 35);

-- Insert data into card table
INSERT INTO card (cost, name, description, category, attack, health) VALUES
(1, "Caveman", "Gain +1|+0 for each enemy in the same timeline", "unit", 1, 3),
(1, "Mongols", "Gain +1|+1 for every Mongols unit in play", "unit", 1, 1),
(2, "Huns", "Gain +0|+1 every time it gets redeployed to another timeline", "unit", 2, 1),
(2, "Natives", "Gain +0|+2 for each consecuttive turn spent on the same timeline.", "unit", 1, 1),
(3, "Spartan", "When in a formation, grant allies in same timeline 0|+2", "unit", 2, 3),
(3, "Vikings", "Gain +2|+0 when deployed from the quantum Tunnel", "unit", 1, 3),
(4, "Templar", "Generate +1 Khronos and an Millenial Knowledge Card", "unit", 1, 4),
(5, "Knights", "Gain +2|+2 for each ally in the quantum tunnel", "unit", 3, 3),
(5, "Samurai", "Deal double damage to injured opponents", "unit", 3, 5),
(6, "Ninjas", "Concuss one enemy in the same timeline before attacking", "unit", 7, 2),
(7, "Pirates", "Generate a Jolly Roger Card", "unit", 6, 6),
(7, "Cowboys", "Attack before receiving damage", "unit", 8, 4),
(8, "Soldiers", "Generate an Tzar Bomba Card", "unit", 7, 5),
(9, "Alien", "Generate an Abduction Card", "unit", 7, 9),
(10, "Terminator", "Deal damage to all copies of the same enemy accross all timelines", "unit", 10, 10),
(3, "Jolly Roger", "Deal 2 damage to an enemy to heal 2 to a unit.", "paradox", NULL, NULL),
(4, "Blood Chalis", "Deal 3 damage to a unit to grant +3|+0 to a unit", "paradox", NULL, NULL),
(6, "Black Plague", "Grant -3|+0 to all enemies", "paradox", NULL, NULL),
(6, "Abduction", "Place an enemy unit in your hand. This unit can now be played as normal", "paradox", NULL, NULL),
(6, "Millenial Knowledge", "Grant +5|+5 to an allied unit", "paradox", NULL, NULL),
(7, "Dinosaur Meteor", "Deal 10 damage to all units in all timelines", "paradox", NULL, NULL),
(8, "Tzar Bomba", "Deal 5 damage to all enemies", "paradox", NULL, NULL);

-- Insert data into deck table
INSERT INTO deck (name, creationDate, owner) VALUES
('Deck1', '2023-01-01', 'user1'),
('Deck2', '2023-02-02', 'user1'),
('Deck3', '2023-03-03', 'user1'),
('Deck1', '2023-04-01', 'user2'),
('Deck2', '2023-05-02', 'user2'),
('Deck1', '2023-01-01', 'user3'),
('Deck2', '2023-07-02', 'user3'),
('Deck3', '2024-02-01', 'user3'),
('Deck1', '2024-01-02', 'user4'),
('Deck1', '2024-09-01', 'user5'),
('Deck1', '2024-01-02', 'user6'),
('Deck1', '2024-01-01', 'user7'),
('Deck1', '2024-03-02', 'user8'),
('Deck1', '2024-04-01', 'user9'),
('Deck1', '2024-04-02', 'user10'),
('Deck1', '2024-04-01', 'user23'),
('Deck1', '2024-04-02', 'user22'),
('Deck1', '2024-04-01', 'user20'),
('Deck2', '2022-04-02', 'user20'),
('Deck3', '2022-04-01', 'user20'),
('Deck1', '2022-04-02', 'user25'),
('Deck2', '2022-04-01', 'user25'),
('Deck3', '2022-07-02', 'user25'),
('Deck1', '2022-02-01', 'user14'),
('Deck2', '2022-02-02', 'user14'),
('Deck3', '2021-02-01', 'user14'),
('Deck1', '2021-02-02', 'user19'),
('Deck2', '2021-01-01', 'user19'),
('Deck1', '2021-09-02', 'user29'),
('Deck2', '2021-09-01', 'user29'),
('Deck3', '2021-09-02', 'user29'),
('Deck1', '2021-09-01', 'user27'),
('Deck2', '2021-09-02', 'user27'),
('Deck1', '2021-10-01', 'user30'),
('Deck2', '2021-10-02', 'user30'),
('Deck1', '2021-10-01', 'user24'),
('Deck2', '2021-10-02', 'user24');


-- Insert data into match table
INSERT INTO `match` (username, deck, datePlayed, timePlayed, score, turnsPlayed, khronosSpent, damageDealt, damageTaken, playedRowOne, playedRowTwo, playedRowThree, averageTurnTime, numCardsPlayed, cardsReturnedToQuantum, cardsBuffed) VALUES
('user1', 1, '2023-01-01', '12:00:00', 100, 10, 5, 100, 50, 3, 2, 1, 30.5, 15, 2, 3),
('user2', 1, '2023-01-02', '13:00:00', 200, 12, 6, 200, 60, 4, 3, 2, 28.7, 16, 1, 4),
('user3', 1, '2023-01-03', '14:00:00', 300, 15, 7, 300, 70, 5, 4, 3, 27.2, 18, 3, 5),
('user4', 1, '2023-01-01', '12:00:00', 420, 20, 8, 400, 80, 6, 5, 4, 29.0, 20, 4, 6),
('user6', 1, '2023-01-02', '13:00:00', 230, 11, 5, 250, 55, 3, 2, 1, 30.1, 17, 2, 4),
('user6', 2, '2023-01-03', '14:00:00', 850, 22, 9, 500, 90, 7, 6, 5, 28.5, 21, 1, 5),
('user6', 3, '2024-01-01', '12:00:00', 130, 13, 4, 120, 40, 2, 1, 1, 27.9, 12, 3, 2),
('user7', 1, '2024-01-02', '13:00:00', 550, 18, 6, 350, 75, 4, 3, 2, 29.7, 19, 2, 3),
('user7', 2, '2024-01-03', '14:00:00', 300, 14, 7, 270, 65, 5, 4, 3, 30.2, 15, 1, 4),
('user14', 1, '2024-01-01', '12:00:00', 100, 10, 5, 100, 50, 3, 2, 1, 30.5, 15, 2, 3),
('user14', 2, '2024-01-02', '13:00:00', 200, 12, 6, 200, 60, 4, 3, 2, 28.7, 16, 1, 4),
('user14', 3, '2024-01-03', '14:00:00', 300, 15, 7, 300, 70, 5, 4, 3, 27.2, 18, 3, 5),
('user19', 1, '2024-01-01', '12:00:00', 500, 19, 8, 400, 85, 6, 5, 4, 29.0, 20, 4, 6),
('user19', 2, '2023-01-02', '13:00:00', 250, 11, 5, 250, 55, 3, 2, 1, 30.1, 17, 2, 4),
('user19', 3, '2021-01-03', '14:00:00', 330, 15, 7, 300, 70, 5, 4, 3, 27.2, 18, 3, 5),
('user20', 1, '2021-01-01', '12:00:00', 220, 14, 6, 200, 60, 4, 3, 2, 28.7, 16, 1, 4),
('user20', 2, '2021-01-02', '13:00:00', 700, 20, 9, 500, 90, 7, 6, 5, 28.5, 21, 1, 5),
('user22', 1, '2021-01-03', '13:00:00', 750, 22, 10, 600, 100, 8, 7, 6, 27.5, 22, 4, 6),
('user22', 2, '2021-01-01', '13:00:00', 320, 13, 7, 300, 65, 5, 4, 3, 30.2, 15, 1, 4),
('user22', 3, '2021-01-02', '13:00:00', 420, 17, 8, 400, 75, 6, 5, 4, 27.9, 18, 2, 5),
('user24', 1, '2021-01-03', '01:00:00', 420, 16, 7, 350, 70, 5, 4, 3, 29.1, 19, 3, 4),
('user24', 2, '2021-01-01', '01:00:00', 330, 14, 6, 300, 65, 4, 3, 2, 30.3, 15, 2, 3),
('user23', 1, '2021-01-02', '13:00:00', 720, 21, 9, 500, 85, 6, 5, 4, 28.6, 20, 1, 5),
('user23', 2, '2021-01-03', '04:00:00', 720, 20, 9, 500, 85, 6, 5, 4, 28.6, 20, 1, 5),
('user23', 3, '2021-01-01', '12:00:00', 910, 24, 11, 600, 100, 8, 7, 6, 27.4, 22, 3, 6),
('user25', 1, '2021-01-02', '03:00:00', 730, 22, 10, 550, 90, 7, 6, 5, 28.4, 21, 2, 5),
('user26', 1, '2021-01-03', '02:00:00', 520, 17, 7, 350, 75, 5, 4, 3, 29.5, 19, 3, 4),
('user26', 2, '2021-01-01', '07:00:00', 810, 23, 10, 600, 95, 7, 6, 5, 27.8, 21, 2, 5),
('user26', 3, '2022-01-02', '06:00:00', 230, 12, 5, 200, 55, 3, 2, 1, 30.0, 15, 1, 3),
('user27', 1, '2022-01-03', '04:00:00', 330, 14, 6, 300, 65, 4, 3, 2, 30.3, 15, 2, 3),
('user27', 1, '2022-01-05', '02:00:00', 250, 13, 5, 220, 60, 3, 2, 1, 30.7, 14, 1, 2);


-- insert values into the gameRound table
INSERT INTO gameRound (roundID, cardID) VALUES
(1,1),
(1,2),
(1,5),
(1,6),
(1,8),
(1,10),
(2,2),
(2,11),
(2,5),
(2,8),
(2,6),
(3,1),
(3,11),
(3,16),
(3,17),
(3,18),
(4,13),
(4,10),
(4,9),
(4,8),
(5,13),
(5,1),
(5,12),
(6,9),
(6,1),
(7,6),
(7,7),
(7,2),
(8,14), 
(9,18);


-- Insert data into game table
INSERT INTO game (username, score, gameRound, kronos, deckCards) VALUES
('user1', 100, 5, 2, 5),
('user2', 200, 8, 4, 6),
('user3', 300, 5, 4, 4),
('user4', 420, 6, 6, 4),
('user6', 230, 7, 3, 6),
('user6', 850, 3, 3, 7),
('user6', 130, 5, 3, 3),
('user7', 550, 4, 2, 5),
('user7', 300, 4, 1, 3),
('user14', 100, 2, 0, 2),
('user14', 200, 1, 4, 5),
('user14', 300, 7, 4, 7),
('user19', 500, 7, 5, 5),
('user19', 250, 4, 5, 5),
('user19', 330, 5, 6, 9),
('user20', 220, 7, 8, 9),
('user20', 700, 8, 8, 3),
('user22', 750, 3, 3, 4),
('user22', 320, 3, 3, 7),
('user22', 420, 3, 4, 3),
('user22', 420, 2, 4, 9),
('user22', 330, 9, 6, 6),
('user23', 720, 8, 2, 4),
('user23', 720, 6, 6, 3),
('user23', 910, 5, 2, 2),
('user24', 730, 5, 5, 2),
('user26', 520, 4, 8, 6),
('user26', 810, 5, 9, 6),
('user26', 230, 3, 2, 7),
('user27', 330, 4, 2, 5),
('user27', 250, 5, 1, 3);

-- Insert data intodeckCard table
INSERT INTO deckCard (deckID, cardID) VALUES
(1, 1),
(1, 2),
(1, 3),
(1, 11),
(1, 16),
(1, 17),
(1, 12),
(2, 15),
(2, 18),
(2, 19),
(2, 12),
(2, 1),
(2, 11),
(2, 17),
(3, 3),
(3, 5),
(3, 9),
(3, 10),
(4, 1),
(4, 12),
(4, 13),
(4, 10),
(5, 12),
(5, 13),
(5, 10),
(8, 12),
(8, 13),
(11, 10),
(11, 12),
(12, 13);