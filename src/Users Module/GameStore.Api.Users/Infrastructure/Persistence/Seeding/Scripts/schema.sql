set search_path to "user";

create table users
(
    id         text         not null
        primary key,
    name       varchar(150) not null,
    email      varchar(150) not null,
    birth_date date         not null
);

create unique index users_name_index
    on users (email);

create table user_addresses
(
    id       text not null
        primary key,
    user_id  text
        references users,
    street   varchar(255),
    city     varchar(150),
    country  varchar(150),
    zip_code varchar(50)
);
