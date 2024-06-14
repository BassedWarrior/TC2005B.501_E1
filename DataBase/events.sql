SET GLOBAL event_scheduler = ON;

-- Creamos el evento para actualizar estadísticas de jugador semanalmente.
DELIMITER //

CREATE EVENT weekly_player_stats_update
ON SCHEDULE EVERY 1 WEEK
STARTS CURRENT_TIMESTAMP
DO
BEGIN
    UPDATE player p
    SET p.highscore_deck = (
        SELECT MAX(m.score)
        FROM `match` m
        WHERE m.username = p.username
    );
END //

DELIMITER ;