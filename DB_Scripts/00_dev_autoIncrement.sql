drop table color;
CREATE TABLE color (
    color_id BIGINT GENERATED ALWAYS AS IDENTITY (INCREMENT 1 START 1000) PRIMARY KEY,
    color_name VARCHAR(50) NOT NULL
);
insert into color(color_name) values('red');
insert into color(color_name) values('orange');
select * from color; 
-----------------------------------------------------

drop table Shape;
drop sequence Shape_Id_seq;
CREATE SEQUENCE Shape_Id_seq start 1000 increment 1;
CREATE TABLE Shape
(
    Id bigint NOT NULL DEFAULT nextval('Shape_Id_seq'),
    ShapeName character varying(50) NOT NULL
);
ALTER SEQUENCE Shape_Id_seq OWNED BY Shape.Id;
insert into Shape(Id,ShapeName) values(nextval('Shape_Id_seq'), 'square');
insert into Shape(Id,ShapeName) values(nextval('Shape_Id_seq'), 'circle');
insert into Shape(Id,ShapeName) values(default, 'rectangle');
select * from Shape; 