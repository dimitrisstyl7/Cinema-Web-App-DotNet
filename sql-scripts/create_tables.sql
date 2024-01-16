-- drop tables
drop table if exists Reservation;
drop table if exists Screening;
drop table if exists ScreeningRoom;
drop table if exists Movie;
drop table if exists Genre;
drop table if exists ContentAdmin;
drop table if exists Cinema;
drop table if exists Customer;
drop table if exists AppAdmin;
drop table if exists [User];
drop table if exists Role;

-- create table Role
create table Role
(
    id   int primary key identity (1,1),
    name varchar(20) not null unique
);

-- create table User
create table [User]
(
    id         int primary key identity (1,1),
    role_id    int         not null,
    username   varchar(30) not null unique,
    email      varchar(50) not null unique,
    first_name varchar(30) not null,
    last_name  varchar(30) not null,
    password   varchar(60) not null,
    created_at datetime    not null default CURRENT_TIMESTAMP,
    foreign key (role_id) references Role (id)
);

-- create table AppAdmin
create table AppAdmin
(
    id      int primary key identity (1,1),
    user_id int not null unique,
    foreign key (user_id) references [User] (id)
);

-- create table Customer
create table Customer
(
    id      int primary key identity (1,1),
    user_id int not null unique,
    foreign key (user_id) references [User] (id)
);

-- create table Cinema
create table Cinema
(
    id                    int primary key identity (1,1),
    name                  varchar(50) not null,
    address               varchar(50) not null,
    city                  varchar(50) not null,
    zip_code              varchar(10) not null,
    email                 varchar(50) not null unique,
    -- each cinema has unique email
    no_of_screening_rooms int         not null,
    unique (name, address, city, zip_code)
    -- avoid duplicate cinemas
);

-- create table ContentAdmin
create table ContentAdmin
(
    id        int primary key identity (1,1),
    user_id   int not null,
    cinema_id int not null,
    unique (user_id, cinema_id),
    -- one user can be content admin for more than one cinema
    foreign key (user_id) references [User] (id),
    foreign key (cinema_id) references Cinema (id)
);

-- create table Genre
create table Genre
(
    id   int primary key identity (1,1),
    name varchar(20) not null unique
);

-- create table Movie
create table Movie
(
    id               int primary key identity (1,1),
    content_admin_id int          not null,
    genre_id         int          not null,
    title            varchar(50)  not null,
    duration         int          not null, -- in minutes
    content          varchar(max) not null,
    description      varchar(max) not null,
    release_date     varchar(4)   not null,
    director         varchar(50)  not null,
    foreign key (genre_id) references Genre (id),
    foreign key (content_admin_id) references ContentAdmin (id)
);

-- create table ScreeningRoom
create table ScreeningRoom
(
    id                int primary key identity (1,1),
    cinema_id         int         not null,
    name              varchar(20) not null,
    total_no_of_seats int         not null,
    [3D]              bit         not null,
    unique (cinema_id, name),
    -- one cinema can not have two screening rooms with the same name
    foreign key (cinema_id) references Cinema (id)
);

-- create table Screening
create table Screening
(
    id                    int primary key identity (1,1),
    movie_id              int      not null,
    screening_room_id     int      not null,
    start_time            datetime not null,
    remaining_no_of_seats int      not null,
    unique (movie_id, screening_room_id, start_time),
    -- one movie can not be screened in the same screening room at the same time
    foreign key (movie_id) references Movie (id),
    foreign key (screening_room_id) references ScreeningRoom (id)
);

-- create table Reservation
create table Reservation
(
    id                 int primary key identity (1,1),
    customer_id        int not null,
    screening_id       int not null,
    no_of_booked_seats int not null,
    unique (customer_id, screening_id),
    -- one customer can not have two reservations for the same screening
    foreign key (customer_id) references Customer (id),
    foreign key (screening_id) references Screening (id)
);