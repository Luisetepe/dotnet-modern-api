create schema if not exists "user";
set search_path to "user";

create table "user".users
(
    id         text         not null
        primary key,
    name       varchar(150) not null,
    email      varchar(150) not null,
    birth_date date         not null
);

create unique index users_name_index
    on "user".users (email);

create table "user".user_addresses
(
    id       text not null
        primary key,
    user_id  text
        references "user".users,
    street   varchar(255),
    city     varchar(150),
    country  varchar(150),
    zip_code varchar(50)
);

INSERT INTO "user".users (id, name, email, birth_date)
VALUES ('01J5APKNJXTPTBND8GA59GBWCH', 'John Doe', 'john.doe@example.com', '1992-03-16');

INSERT INTO "user".user_addresses (id, user_id, street, city, country, zip_code)
VALUES ('01J5APKNJXHV6RQTHV10ZEBA6A', '01J5APKNJXTPTBND8GA59GBWCH', '123 Main St', 'Anytown', 'USA', '12345');

