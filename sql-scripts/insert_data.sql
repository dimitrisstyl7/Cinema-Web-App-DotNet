-- insert user roles into Role table
insert into Role (name)
values ('customer'),
       ('app_admin'),
       ('content_admin');

-- insert users into User table
-- username: customer1, password: customer1
-- username: appAdmin, password: appAdmin
-- username: contentAdmin1, password: contentAdmin1
insert into [User] (role_id, username, email, first_name, last_name, password)
values (1, 'customer1', 'customer1@email.com', 'Customer', 'One',
        '$2a$10$elKRD7GTtyImpjd155Q9eOXJxJha4mVxe0k4zyxmPehAU6SpZ6dBa');
insert into [User] (role_id, username, email, first_name, last_name, password)
values (2, 'appAdmin', 'appAdmin@email.com', 'App', 'Admin',
        '$2a$10$RGZsOH9pX4TcPSs0OMpMTuLMxWvKKX1HB0FEtAnR5aXjDYTOg7wSa');
insert into [User] (role_id, username, email, first_name, last_name, password)
values (3, 'contentAdmin1', 'contentAdmin1@email.com', 'ContentAdmin', 'One',
        '$2a$10$Z0MANUmLp9tMPa2cJar7eOKRwNmFQsggItkWNtZJ5EeI8cO3JhY5a');

-- insert application admin into AppAdmin table
insert into AppAdmin (user_id)
values (2);

-- insert customer into Customer table
insert into Customer (user_id)
values (1);

-- insert cinema into Cinema table
insert into Cinema (name, address, city, zip_code, email, no_of_screening_rooms)
values ('Cinema One', 'Address One', 'City One', '12345', 'cinemaOne@email.com', 3);

-- insert content admin into ContentAdmin table
insert into ContentAdmin (user_id, cinema_id)
values (3, 1);

-- insert genres into Genre table
insert into Genre (name)
values ('Action'),
       ('Comedy'),
       ('Drama'),
       ('Horror'),
       ('Thriller'),
       ('Romance'),
       ('Sci-Fi'),
       ('Fantasy'),
       ('Mystery'),
       ('Crime'),
       ('Animation'),
       ('Adventure'),
       ('Family'),
       ('Biography'),
       ('History'),
       ('War'),
       ('Music'),
       ('Sport'),
       ('Western'),
       ('Musical'),
       ('Documentary');

-- insert movie into Movie table
insert into Movie (genre_id, title, duration, content, description, release_year, director)
values (1, 'Plane', 107, 'content',
        'Brodie Torrance saves his passengers from a lightning strike by making a risky landing on a war-torn island - only to find that surviving the landing was just the beginning. When most of the passengers are taken hostage by dangerous rebels, the only person Torrance can count on for help is Louis Gaspare, an accused murderer who was being transported by the FBI.',
        '2023', N'Jean-François Richet');
		
insert into Movie (genre_id, title, duration, content, description, release_year, director)
values (4, 'Eternal Echoes', 130, 'content',
        'In a tale of love and loss spanning multiple lifetimes, Emily and Alexander find themselves drawn to each other in every incarnation. As they navigate the challenges of each era, from ancient civilizations to a distant future, they discover that their connection is truly eternal. A heart-wrenching and poignant exploration of the enduring nature of true love.',
        '2024', N'Greta Gerwig');

insert into Movie (genre_id, title, duration, content, description, release_year, director)
values (2, 'Shadows of Deceit', 115, 'content',
        'Detective Sarah Lawson is on the trail of a cunning serial killer who leaves cryptic clues at crime scenes. As she unravels the mystery, she discovers a dark web of deception and betrayal that leads her to question her own colleagues. The tension builds as Sarah races against time to catch the elusive killer before they strike again.',
        '2024', N'David Fincher');

insert into Movie (genre_id, title, duration, content, description, release_year, director)
values (5, 'Realm of Enchantment', 140, 'content',
        'In a magical world threatened by an ancient evil, a reluctant hero must embark on a quest to find a legendary artifact that can save their realm. Along the way, they encounter mythical creatures, forge alliances, and confront their own fears. A visually stunning and epic journey into a realm where the line between reality and fantasy blurs.',
        '2025', N'Peter Jackson');



-- insert screening rooms into ScreeningRoom table
insert into ScreeningRoom (cinema_id, name, total_no_of_seats, [3D])
values ('1', 'Screening Room 1', 60, 0),
       ('1', 'Screening Room 2', 70, 1),
       ('1', 'Screening Room 3', 80, 0);

-- insert screening into Screening table
insert into Screening (movie_id, screening_room_id, start_time, remaining_no_of_seats)
values (1, 1, CURRENT_TIMESTAMP, 55);

-- insert reservation into Reservation table
insert into Reservation (customer_id, screening_id, no_of_booked_seats)
values (1, 1, 5);