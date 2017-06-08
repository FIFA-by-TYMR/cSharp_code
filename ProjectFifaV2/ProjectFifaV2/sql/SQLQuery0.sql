DECLARE @cnt integer = 0, @all integer;
DECLARE @cnt1 integer = 0, @all1 integer;
DECLARE @finalawayscore integer, @finalhomescore integer, @prevscore integer;
DECLARE @userID TABLE( Id integer IDENTITY (1, 1)   ,IDs varchar(8000),PRIMARY KEY CLUSTERED (id ASC));

SET @all = (select COUNT(Game_id) from TblGames)
SET @all = (select COUNT(id) from TblPredictions)

WHILE @cnt < @all

BEGIN

SET @finalawayscore = (select HomeTeamScore from TblGames where Game_id = @cnt)
SET @finalhomescore = (select AwayTeamScore from TblGames where Game_id = @cnt)

WHILE @cnt1 < @all1
BEGIN

insert into @userID values ((select User_id from TblPredictions where Game_id = @cnt and id = @cnt1 and PredictedAwayScore = @finalawayscore and PredictedHomeScore = @finalhomescore ))
select IDs from @userID;
set @cnt1 = @cnt1 +1 

End

while @cnt1 < @all1
BEGIN

SET @prevscore = ((select Score from TblUsers where id = ((select IDs from @userID where id = @cnt1 ))))
SET @prevscore = @prevscore + 3; 
update TblUsers set Score = @prevscore where id = ((select IDs from @userID ));
SET @cnt1 = @cnt1 + 1

End

SET @cnt = @cnt + 1;

END;

