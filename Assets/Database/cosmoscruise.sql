DROP SCHEMA IF EXISTS cosmoscruise;

CREATE SCHEMA cosmoscruise;

USE cosmoscruise;

DROP TABLE IF EXISTS Player;
CREATE TABLE IF NOT EXISTS Player (
    id INT(11) NOT NULL AUTO_INCREMENT,
    name VARCHAR(45) NOT NULL,
    level INT(11) NOT NULL,
    time DOUBLE NOT NULL,
    totaltime DOUBLE NOT NULL,
    PRIMARY KEY (id)
);

INSERT Player (name, level, time, totaltime) VALUES ('Jan', 1, 2.5, 2.5);
INSERT Player VALUES (NULL, "Niels", 4, 5,15.2);
INSERT Player VALUES (NULL, "Sissy", 46, 12,15.2);

SELECT 
    *
FROM
    player;