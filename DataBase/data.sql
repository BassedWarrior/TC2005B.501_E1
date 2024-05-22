USE chronical_doom;

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
    (3, "Jolly Roger", "Deal 2 damage to an enemy to heal 2 to a unit.", "paradox", NULL, NULL),
    (4, "Blood Chalis", "Deal 3 damage to a unit to grant +3|+0 to a unit", "paradox", NULL, NULL),
    (6, "Black Plague", "Grant -3|+0 to all enemies", "paradox", NULL, NULL),
    (6, "Abduction", "Place an enemy unit in your hand. This unit can now be played as normal", "paradox", NULL, NULL),
    (6, "Millenial Knowledge", "Grant +5|+5 to an allied unit", "paradox", NULL, NULL),
    (7, "Dinosaur Meteor", "Deal 10 damage to all units in all timelines", "paradox", NULL, NULL),
    (8, "Tzar Bomba", "Deal 5 damage to all enemies", "paradox", NULL, NULL);
