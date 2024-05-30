DELIMITER //

CREATE PROCEDURE SignUpUser(
    IN p_username VARCHAR(255),
    IN p_password VARCHAR(255)
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

DELIMITER //

CREATE PROCEDURE SignInUser(
    IN p_username VARCHAR(255),
    IN p_password VARCHAR(255)
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
