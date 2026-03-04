select * from users
select * from roles
select * from userroles

insert into UserRoles (userId, roleid) values (
	(select top 1 id from users where username = 'shaozelian@live.com'),
	(select top 1 id from roles where normalizedname = 'ADMINISTRATORS')
)