USE chronicle_doom;

INSERT INTO card (cost, name, description, category, attack, health)
VALUES
    (1, "Caveman", "Its robust skin and rudimentary techniques allow a high resistance to withstand direct attacks", "unit", 1, 3),
    (1, "Mongols", "The Pax Mongolica was left behind, masters of fast-moving warfare are ready to strike without looking back", "unit", 1, 2),
    (2, "Huns", "Known for their ferocity and excellent horseback riding skills, the Huns under Attila became a formidable force", "unit", 2, 3),
    (2, "Natives", "Indigenous tribes developed rich cultures and societies, with deep knowledge innovating agricultural techniques", "unit", 1, 3),
    (3, "Spartan", "Trained in a rigorous military program. Their discipline and combat skills were unmatched in ancient Greece.", "unit", 3, 3),
    (3, "Vikings", "Norse explorers and warriors, known for their seafaring skills, raiding and trading across Europe", "unit", 2, 3),
    (4, "Templar", "Medieval Christian military order renowned for their role in the Crusades and their vast financial network", "unit", 3, 4),
    (5, "Knights", "Heavily armored cavalrymen known for their chivalry and role in feudal society, following an honor code", "unit", 2, 6),
    (5, "Samurai", "Warrior class of feudal Japan, adhering to the Bushido code, which emphasized loyalty and martial arts proficiency", "unit", 7, 3),
    (6, "Ninjas", "Shinobi, were covert agents in feudal Japan specializing in espionage, sabotage, and warfare", "unit", 5, 4),
    (7, "Pirates", "Seafaring outlaws who engaged in robbery and plundering ships on the high seas", "unit", 6, 6),
    (7, "Cowboys", "Cattle herders in the American West known for their skills in riding, roping, and herding", "unit", 8, 4),
    (8, "Soldiers", "Highly trained professionals equipped with advanced weaponry and technology, dedicated to defending their nations", "unit", 7, 7),
    (9, "Alien", "Aliens, from other planets with advanced technology and potentially superior intelligence", "unit", 8, 10),
    (10, "Terminator", "Fictional cyborgs from the future, designed as nearly indestructible killing machines with advanced AI", "unit", 12, 10),
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
    (2, 3, 2),
    (2, 2, 1),
    (3, 4, 1),
    (3, 2, 2),
    (4, 5, 2),
    (4, 2, 2),
    (5, 5, 1),
    (5, 6, 2),
    (6, 7, 2),
    (6, 6, 1),
    (7, 8, 2),
    (8, 9, 2),
    (8, 10, 1),
    (9, 11, 2),
    (9, 5, 1),
    (9, 4, 1),
    (10, 12, 2),
    (10, 13, 1),
    (10, 14, 2);


/* ORIGINAL CARD ABILITES
    (1, "Caveman", "Gain +1|+0 for each enemy in the same timeline", "unit", 1, 3),
    (1, "Mongols", "Gain +1|+1 for every Mongols unit in play", "unit", 1, 2),
    (2, "Huns", "Gain +0|+1 every time it gets redeployed to another timeline", "unit", 2, 3),
    (2, "Natives", "Gain +0|+2 for each consecuttive turn spent on the same timeline.", "unit", 1, 3),
    (3, "Spartan", "When in a formation, grant allies in same timeline 0|+2", "unit", 3, 3),
    (3, "Vikings", "Gain +2|+0 when deployed from the quantum Tunnel", "unit", 2, 3),
    (4, "Templar", "Generate +1 Khronos and an Millenial Knowledge Card", "unit", 3, 4),
    (5, "Knights", "Gain +2|+2 for each ally in the quantum tunnel", "unit", 2, 6),
    (5, "Samurai", "Deal double damage to injured opponents", "unit", 7, 3),
    (6, "Ninjas", "Concuss one enemy in the same timeline before attacking", "unit", 5, 4),
    (7, "Pirates", "Generate a Jolly Roger Card", "unit", 6, 6),
    (7, "Cowboys", "Attack before receiving damage", "unit", 8, 4),
    (8, "Soldiers", "Generate an Tzar Bomba Card", "unit", 7, 7),
    (9, "Alien", "Generate an Abduction Card", "unit", 8, 10),
    (10, "Terminator", "Deal damage to all copies of the same enemy accross all timelines", "unit", 12, 10),
*/