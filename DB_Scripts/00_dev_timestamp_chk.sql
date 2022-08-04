drop table datetimetest(
timestamp_col timestamp,
timestampz_col timestamptz
);
insert into datetimetest values(now(), now());
select * from datetimetest; 