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