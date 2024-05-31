USE chronicle_doom;
DELIMITER //

CREATE PROCEDURE SignUpUser(
    IN p_username VARCHAR(20),
    IN p_password VARCHAR(20)
)
BEGIN
    -- Verificar si el usuario ya existe
    IF EXISTS (SELECT 1 FROM player WHERE username = p_username) THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Username already exists';
    ELSE
        -- Insertar el nuevo usuario, asumiendo que la fecha de creación es la fecha actual y highscore_deck es 0
        INSERT INTO player (username, password, creationDate, highscore_deck) 
        VALUES (p_username, p_password, CURDATE(), 0);
    END IF;
END //
DELIMITER;

USE chronicle_doom;
DELIMITER //

CREATE PROCEDURE SignInUser(
    IN p_username VARCHAR(20),
    IN p_password VARCHAR(20)
)
BEGIN
    DECLARE user_count INT;

    -- Verificar si el usuario y la contraseña coinciden
    SELECT COUNT(*) INTO user_count
    FROM player
    WHERE username = p_username AND password = p_password;

    IF user_count = 0 THEN
        -- Lanzar un error si las credenciales no coinciden
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Invalid username or password';
    END IF;
END //
DELIMITER ;

USE chronicle_doom;
DELIMITER //

CREATE PROCEDURE UpdateDeck(
    IN p_username VARCHAR(20),
    IN p_deckname VARCHAR(30),
    IN p_json_cardid TINYINT UNSIGNED,
    IN p_card_times TINYINT UNSIGNED
)
BEGIN
    DECLARE deck_id INT;

    SELECT deckID INTO deck_id
    FROM deck WHERE owner = p_username;

    UPDATE deck SET name = p_deckname WHERE deckID = deck_id;
    UPDATE deck SET creationDate = CURDATE() WHERE deckID = deck_id;
    
    INSERT INTO deckCard (deckID, cardID, card_times)
    SELECT deck_id, p_json_cardid, p_card_times;
END //
DELIMITER ;

USE chronicle_doom;
DELIMITER //

CREATE PROCEDURE DeleteDeck(
    IN p_username VARCHAR(20)
)
BEGIN
    DECLARE deck_id INT;

    SELECT deckID INTO deck_id
    FROM deck WHERE owner = p_username;

    IF deck_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'No decks found for the given user.';
    ELSEIF (SELECT COUNT(*) FROM deck WHERE owner = p_username) > 1 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Multiple decks for the same user.';
    END IF;

    DELETE FROM deckCard WHERE deckID = deck_id;
END //
DELIMITER ;