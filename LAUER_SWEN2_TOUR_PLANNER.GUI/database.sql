COMMENT ON DATABASE "Tour_Planner"
    IS 'SWEN 2 Semester Project LAUER';

CREATE TYPE e_transport_type AS ENUM ('car', 'train', 'ship', 'airplane', 'foot', 'bicycle','mixed','unknown');
CREATE TYPE rating AS ENUM ('one_star', 'two_stars', 'three_star', 'four_star', 'five_star');
CREATE TYPE difficulty AS ENUM ('easy', 'medium', 'hard', 'very_hard');

CREATE TABLE tours (
	t_id 			char(36) PRIMARY KEY,
	t_name 	char(200) NOT NULL,
	t_description 	char(200),
	t_creationTime 	timestamp NOT NULL,
    t_from          char(200) NOT NULL,
    t_to            char(200) NOT NULL,
	t_distance		float NOT NULL,
	t_estimatedTime	int NOT NULL,
	t_transport		e_transport_type NOT NULL,
	t_picture		bytea
);

CREATE TABLE tourLogs (
	tl_id 			char(36) PRIMARY KEY,
	tl_comment	 	char(1024),
	tl_creationTime timestamp NOT NULL,
	tl_time			int NOT NULL,
	tl_difficulty	difficulty NOT NULL,
	tl_rating		rating NOT NULL,
	t_id			char(36) NOT NULL,
	CONSTRAINT fk_tourLog_tour FOREIGN KEY(t_id) REFERENCES tours(t_id) ON DELETE CASCADE
);

