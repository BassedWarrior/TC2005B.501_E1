USE chronicle_doom;

INSERT INTO card (cost, name, description, category, attack, health)
VALUES
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
    (3, "Jolly Roger", "Deals 2 damage to center enemyline AND heals 2 points to center allyline", "paradox", NULL, NULL),
    (4, "Blood Chalis", "Deals 3 damage to right allyline AND grants +3 attack to left allyline", "paradox", NULL, NULL),
    (6, "Black Plague", "Grants -3 attack to all enemies", "paradox", NULL, NULL),
    (6, "Abduction", "Heals 5 health and grants -5 attack to all enemies", "paradox", NULL, NULL),
    (6, "Millenial Knowledge", "Heals 5 health in left allyline AND grants -3 attack to center allyline", "paradox", NULL, NULL),
    (7, "Dinosaur Meteor", "Deal 10 damage to all units in all timelines", "paradox", NULL, NULL),
    (8, "Tzar Bomba", "Deal 5 damage to all enemies", "paradox", NULL, NULL);

INSERT INTO enemyWave (roundID, cardID, card_times) 
VALUES
    (1, 1, 1),
    (1, 2, 1),
    (2, 5, 1),
    (2, 10, 1),
    (2, 8, 1),
    (3, 7, 1),
    (3, 5, 1),
    (3, 2, 1),
    (4, 9, 2),
    (4, 7, 2),
    (5, 11, 2),
    (5, 9, 2),
    (6, 15, 2),
    (6, 14, 2),
    (7, 6, 2),
    (7, 7, 2),
    (8, 13, 2),
    (8, 14, 2),
    (9, 15, 2),
    (9, 16, 1),
    (9, 17, 1),
    (10, 16, 2),
    (10, 17, 2),
    (10, 18, 1);