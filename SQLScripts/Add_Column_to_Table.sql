USE RefScores
Go

ALTER TABLE tbl_Games_History
ADD edit_timestamp datetime NOT NULL;
Go