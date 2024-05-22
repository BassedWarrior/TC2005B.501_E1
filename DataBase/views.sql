USE chronical_doom;

CREATE VIEW unitCards 
AS SELECT * FROM card
WHERE card.category LIKE "unit";

CREATE VIEW paradoxCards 
AS SELECT cardID, cost, name, description, category FROM card
WHERE card.category LIKE "paradox";
