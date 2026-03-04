-- User role related
select * from users
select * from roles
select * from userroles

insert into UserRoles (userId, roleid) values (
	(select top 1 id from users where username = 'shaozelian@live.com'),
	(select top 1 id from roles where normalizedname = 'ADMINISTRATORS')
)

update users set firstname = N'泽连', lastname = N'邵' where username = 'shaozelian@live.com'
update users set firstname = N'家誉', lastname = N'邵' where username = 'jiayu.sh.shao@outlook.com'
update users set firstname = N'家然', lastname = N'邵' where username = 'jiaran.shao@outlook.com'


-- Assessment related
select ew.*
From EnglishWords ew
inner join [WordsAssessmentDetails] ad on ew.id = ad.WordId
inner join [WordsAssessments] wa on ad.WordsAssessementId = wa.Id
inner join [AssessmentInfos] wi on wa.AssessmentInfoId = wi.id
where wi.Id = '7DDAE132-96B7-4FF2-A270-22CFA9EC64B1' -- AssessmentInfoId
order by ew.Sort

--delete from [AssessmentResults]
--delete from [WordsAssessmentDetails]
--delete from [WordsAssessments]
--delete from [AssessmentInfos]