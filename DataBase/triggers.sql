USE chronicle_doom;

DELIMITER //

CREATE TRIGGER after_player_create
AFTER INSERT ON player
FOR EACH ROW
BEGIN
    DECLARE deck_name VARCHAR(30);

    SET deck_name = CONCAT('DECK: ', NEW.username);
    INSERT INTO deck (name, creationDate, owner) VALUES (deck_name, CURDATE(), NEW.username);
END //

DELIMITER ;

DELIMITER //

CREATE TRIGGER prevent_card_deletion_in_use
BEFORE DELETE ON card
FOR EACH ROW
BEGIN
    DECLARE card_in_use INT;
    SELECT COUNT(*) INTO card_in_use FROM match WHERE playedRowOne = OLD.cardID OR playedRowTwo = OLD.cardID OR playedRowThree = OLD.cardID;
    
    IF card_in_use > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'No se puede eliminar la carta porque está siendo utilizada en partidas activas.';
    END IF;
END //

DELIMITER ;

DELIMITER //

CREATE TRIGGER prevent_duplicate_deck_name
BEFORE INSERT ON deck
FOR EACH ROW
BEGIN
    DECLARE deck_count INT;
    SELECT COUNT(*) INTO deck_count FROM deck WHERE name = NEW.name AND owner = NEW.owner;
    
    IF deck_count > 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Ya existe un mazo con el mismo nombre para este usuario.';
    END IF;
END //

DELIMITER ;