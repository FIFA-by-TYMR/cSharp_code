select * from TblGames
select * from TblTeams
DECLARE @i INT = 1;
declare @f INT = 6;

WHILE @i < @f
BEGIN
insert into TblGames(HomeTeam,AwayTeam) values(1,@i)
insert into TblGames(HomeTeam,AwayTeam) values(2,@i)
insert into TblGames(HomeTeam,AwayTeam) values(3,@i)
insert into TblGames(HomeTeam,AwayTeam) values(4,@i)
insert into TblGames(HomeTeam,AwayTeam) values(5,@i)
insert into TblGames(HomeTeam,AwayTeam) values(6,@i)


   SET @i = @i + 1;
END;
select * from TblGames
select * from TblTeamss