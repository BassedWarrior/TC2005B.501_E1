USE chronicle_doom;

DELIMITER //

CREATE TRIGGER after_player_create
AFTER INSERT ON player
FOR EACH ROW
BEGIN
    DECLARE deck_name VARCHAR(30);
    DECLARE i INT DEFAULT 1;

    WHILE i <= 3 DO
        SET deck_name = CONCAT('DECK ', i, ": ", NEW.username);
        INSERT INTO deck (name, creationDate, owner) VALUES (deck_name, CURDATE(), NEW.username);
        SET i = i + 1;
    END WHILE;
END //

DELIMITER ;
