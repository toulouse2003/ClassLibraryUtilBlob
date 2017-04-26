use employee;
create table employee (
	lastname varchar(32) not null,
	firstname varchar(32) not null,
	title varchar(32) not null,
	hiredate datetime not null,
	reportsto int not null,
	id int identity(1, 1) not null,
	photo varbinary(MAX) not null,
	primary key(id),
	foreign key(reportsto) references employee(id)
);
INSERT into employee 
(
    lastname, 
    firstname, 
    title,
	hiredate,
	reportsto, 
    photo
)
SELECT 'Larsen', 'Niels Müller', 'Lærer',
       '2017-04-01', 1, photo.*
FROM OPENROWSET 
    (BULK 'c:\users\niml\documents\nmls\niels_pasfotofeb17.jpg', SINGLE_BLOB) photo;

select * from employee;
