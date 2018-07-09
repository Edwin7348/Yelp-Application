-- "Team CE"
-- Connor Hamilton
-- Edwin Aguilera
-- CptS 451, Spring 2018
-- Milestone II
---------------------------------------------------------------

CREATE TYPE DayOfWeek AS ENUM ('Monday','Tuesday','Wednesday','Thursday','Friday','Saturday','Sunday');

CREATE TABLE Business (
	business_id	CHAR(22),
	name		VARCHAR(255) NOT NULL,
	address		VARCHAR(255),
	city		VARCHAR(255),
	state		CHAR(2),
	postal_code	CHAR(5),
	latitude	FLOAT,
	longitude	FLOAT,
	stars		DOUBLE PRECISION,
	review_count	INTEGER DEFAULT 0,
	is_open		INTEGER,
	--Milestone II addtions:
	numCheckins	INTEGER DEFAULT 0,
	reviewrating	DOUBLE PRECISION DEFAULT 0.0,
	PRIMARY KEY (business_id)
);

CREATE TABLE BusinessHours (
	business_id	CHAR(22),
	day		DAYOFWEEK,
	open		TIME NOT NULL,
	close		TIME NOT NULL,
	PRIMARY KEY (day, business_id),
	FOREIGN KEY (business_id) REFERENCES Business (business_id) ON DELETE CASCADE
);

CREATE TABLE BusinessCategory (
	business_id	CHAR(22),
	category	VARCHAR(255),
	PRIMARY KEY (business_id, category),
	FOREIGN KEY (business_id) REFERENCES Business (business_id) ON DELETE CASCADE
);

CREATE TABLE Users (
	user_id		CHAR(22),
	average_stars	DOUBLE PRECISION,
	cool		INTEGER DEFAULT 0,
	fans		INTEGER DEFAULT 0,
	funny		INTEGER DEFAULT 0,
	name		VARCHAR(255) NOT NULL,
	review_count	INTEGER DEFAULT 0,
	useful		INTEGER DEFAULT 0,
	yelping_since	DATE NOT NULL,
	PRIMARY KEY (user_id)
);

CREATE TABLE Friends (
	user_id		CHAR(22),
	friend_id	CHAR(22),
	PRIMARY KEY (user_id, friend_id),
	FOREIGN KEY (user_id) REFERENCES Users (user_id) ON DELETE CASCADE,
	FOREIGN KEY (friend_id) REFERENCES Users (user_id) ON DELETE CASCADE
);

CREATE TABLE Checkin (
	business_id	CHAR(22),
	day		DAYOFWEEK,
	morning		INTEGER,
	afternoon	INTEGER,
	evening		INTEGER,
	night		INTEGER,
	PRIMARY KEY (day, business_id),
	FOREIGN KEY (business_id) REFERENCES Business (business_id) ON DELETE CASCADE
);

CREATE TABLE Review (
	review_id	CHAR(22),
	user_id		CHAR(22),
	business_id	CHAR(22),
	stars		INTEGER NOT NULL CHECK (stars > 0 AND stars < 6),
	created_on	DATE NOT NULL DEFAULT CURRENT_DATE,
	review_text	TEXT,
	useful		INTEGER,
	funny		INTEGER,
	cool		INTEGER,
	PRIMARY KEY (review_id),
	FOREIGN KEY (user_id) REFERENCES Users (user_id) ON DELETE SET NULL,
	FOREIGN KEY (business_id) REFERENCES Business (business_id) ON DELETE CASCADE
);
