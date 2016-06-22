CREATE TRIGGER [dbo].[trg_Games_History] ON [dbo].[tbl_Games] FOR INSERT, UPDATE, DELETE
AS
DECLARE @updateType varchar(1)

IF @@rowcount = 0 
	RETURN

SET NOCOUNT ON;

INSERT INTO dbo.tbl_Games_History(id_League, id_team_home, id_team_away, game_venue, game_city, game_state, game_datetime, edit_by, edit_timestamp, id_game, updateType)
SELECT Coalesce(i.id_League, d.id_League) As id_League,
	Coalesce(i.id_team_home, d.id_team_home) As id_team_home,
	Coalesce(i.id_team_away, d.id_team_away) As id_team_away,
	Coalesce(i.game_venue, d.game_venue) As game_venue,
	Coalesce(i.game_city, d.game_city) As game_city,
	Coalesce(i.game_state, d.game_state) As game_state,
	Coalesce(i.game_datetime, d.game_datetime) As game_datetime,
	CURRENT_USER,
	CURRENT_TIMESTAMP, 
	Coalesce(i.id_game, d.id_game) As id_game,
	CASE WHEN i.id_game IS NULL THEN 'D'
            WHEN d.id_game IS NULL THEN 'I'
            ELSE 'U'
       END AS UpdateType
FROM   inserted As i
 FULL
  JOIN deleted As d
    ON d.id_game = i.id_game;