create table if not exists messages
(
    id          uuid primary key,
    content     text not null,
    dateCreated timestamp with time zone default now(),
    dateDeleted timestamp with time zone
);

insert into messages
values ('6afecf1d-3577-4daa-acb8-4ae6e5b73c28', 'This is my initial message')