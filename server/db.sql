create schema if not exists main;

use main;

create or replace table users (
    id      uuid                                   not null
        primary key,
    uname   varchar(255)                           not null,
        index (uname),
    color_r tinyint unsigned                       not null,
    color_b tinyint unsigned                       not null,
    color_g tinyint unsigned                       not null,
    created timestamp(2) default current_timestamp not null,
    constraint unique (uname)
);

create or replace table boards (
    name  varchar(255)                           not null,
    id    int unsigned                           not null auto_increment,
    score int unsigned                           not null,
    user  uuid                                   not null,
    time  timestamp(2) default current_timestamp not null,
    constraint foreign key (user) references users (id),
    primary key (name, score desc, id),
    index (id, name, score desc),
    index (time desc)
);