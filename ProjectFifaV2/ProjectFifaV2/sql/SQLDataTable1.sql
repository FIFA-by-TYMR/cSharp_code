
 
DECLARE @jojo TABLE(TeamName varchar(8000), PredictedHomeScore varchar(8000), batman integer);
DECLARE @i	integer, @stop integer;
set @i = 0;
set @stop = (select count(*) from TblGames)
WHILE @i < @stop
Begin
insert into @jojo (TeamName,PredictedHomeScore) values ((SELECT  TblTeams.TeamName FROM  TblGames left join TblTeams ON TblGames.HomeTeam = TblTeams.Team_ID where Game_id = @i ),(select TblPredictions.PredictedHomeScore from TblPredictions WHERE Game_id = @i )) ;


 set @i = @i+1;
END
select * from  @jojo