create table entries
(
	key1 int,
	key2 int,
	key3 int,
	segment int,
	offset int,
	length int,
	dateAdded int default CURRENT_TIMESTAMP,
	constraint table_name_pk primary key (key1, key2, key3)
);

create table segments
(
	id INTEGER constraint segments_pk primary key autoincrement,
	dateAdded int default CURRENT_TIMESTAMP,
	size int
);

create table mounts
(
	id INTEGER not null constraint mounts_pk primary key autoincrement,
	dateAdded int default CURRENT_TIMESTAMP not null,
	machineName VARCHAR(64) not null,
	username VARCHAR(64) not null,
	os VARCHAR(128),
	uuid VARCHAR(36)
);

