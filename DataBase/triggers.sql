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
