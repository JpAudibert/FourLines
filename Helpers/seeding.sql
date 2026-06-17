-- ==========================================
-- ROLES
-- ==========================================

INSERT INTO roles (
    id,
    name,
    created_at,
    updated_at
)
VALUES
('11111111-1111-1111-1111-111111111111', 'Admin', NOW(), NOW()),
('22222222-2222-2222-2222-222222222222', 'Facility Owner', NOW(), NOW()),
('33333333-3333-3333-3333-333333333333', 'Player', NOW(), NOW()),
('44444444-4444-4444-4444-444444444444', 'Coach', NOW(), NOW()),
('55555555-5555-5555-5555-555555555555', 'Manager', NOW(), NOW());

-- ==========================================
-- SPORTS
-- ==========================================

INSERT INTO sports (
    id,
    name,
    indoor,
    starting_players_count,
    max_players_count,
    created_at,
    updated_at
)
VALUES
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'Football', FALSE, 22, 22, NOW(), NOW()),
('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'Futsal', TRUE, 10, 14, NOW(), NOW()),
('cccccccc-cccc-cccc-cccc-cccccccccccc', 'Basketball', TRUE, 10, 12, NOW(), NOW()),
('dddddddd-dddd-dddd-dddd-dddddddddddd', 'Volleyball', TRUE, 12, 14, NOW(), NOW()),
('eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee', 'Tennis', FALSE, 2, 4, NOW(), NOW());

-- ==========================================
-- USERS
-- ==========================================

INSERT INTO users (
    id,
    role_id,
    name,
    email,
    password_hash,
    birthday,
    phone,
    registration_number,
    is_active,
    created_at,
    updated_at
)
VALUES
(
    '10000000-0000-0000-0000-000000000001',
    '11111111-1111-1111-1111-111111111111',
    'John Admin',
    'john.admin@example.com',
    '$2a$11$dummyHash1',
    '1985-05-10',
    '51999990001',
    'USR001',
    TRUE,
    NOW(),
    NOW()
),
(
    '10000000-0000-0000-0000-000000000002',
    '22222222-2222-2222-2222-222222222222',
    'Sarah Owner',
    'sarah.owner@example.com',
    '$2a$11$dummyHash2',
    '1988-07-15',
    '51999990002',
    'USR002',
    TRUE,
    NOW(),
    NOW()
),
(
    '10000000-0000-0000-0000-000000000003',
    '33333333-3333-3333-3333-333333333333',
    'Mike Player',
    'mike.player@example.com',
    '$2a$11$dummyHash3',
    '1995-02-20',
    '51999990003',
    'USR003',
    TRUE,
    NOW(),
    NOW()
),
(
    '10000000-0000-0000-0000-000000000004',
    '44444444-4444-4444-4444-444444444444',
    'Emma Coach',
    'emma.coach@example.com',
    '$2a$11$dummyHash4',
    '1990-09-05',
    '51999990004',
    'USR004',
    TRUE,
    NOW(),
    NOW()
),
(
    '10000000-0000-0000-0000-000000000005',
    '55555555-5555-5555-5555-555555555555',
    'David Manager',
    'david.manager@example.com',
    '$2a$11$dummyHash5',
    '1987-11-25',
    '51999990005',
    'USR005',
    TRUE,
    NOW(),
    NOW()
);

-- ==========================================
-- FACILITIES
-- ==========================================

INSERT INTO facilities (
    id,
    owner_id,
    name,
    address,
    city,
    state,
    zip_code,
    registration_number,
    created_at,
    updated_at
)
VALUES
(
    'f0000000-0000-0000-0000-000000000001',
    '10000000-0000-0000-0000-000000000002',
    'Arena Central',
    '123 Main Street',
    'Porto Alegre',
    'RS',
    '90000-001',
    'FAC001',
    NOW(),
    NOW()
),
(
    'f0000000-0000-0000-0000-000000000002',
    '10000000-0000-0000-0000-000000000002',
    'South Sports Club',
    '456 Oak Avenue',
    'Porto Alegre',
    'RS',
    '90000-002',
    'FAC002',
    NOW(),
    NOW()
),
(
    'f0000000-0000-0000-0000-000000000003',
    '10000000-0000-0000-0000-000000000002',
    'Elite Courts',
    '789 Pine Road',
    'Canoas',
    'RS',
    '92000-001',
    'FAC003',
    NOW(),
    NOW()
),
(
    'f0000000-0000-0000-0000-000000000004',
    '10000000-0000-0000-0000-000000000002',
    'Prime Arena',
    '101 Stadium Blvd',
    'Novo Hamburgo',
    'RS',
    '93000-001',
    'FAC004',
    NOW(),
    NOW()
),
(
    'f0000000-0000-0000-0000-000000000005',
    '10000000-0000-0000-0000-000000000002',
    'Champions Center',
    '202 Victory Street',
    'São Leopoldo',
    'RS',
    '93100-001',
    'FAC005',
    NOW(),
    NOW()
);

-- ==========================================
-- COURTS
-- ==========================================

INSERT INTO courts (
    id,
    facility_id,
    sport_id,
    name,
    is_active,
    created_at,
    updated_at
)
VALUES
(
    'c0000000-0000-0000-0000-000000000001',
    'f0000000-0000-0000-0000-000000000001',
    'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa',
    'Football Field A',
    TRUE,
    NOW(),
    NOW()
),
(
    'c0000000-0000-0000-0000-000000000002',
    'f0000000-0000-0000-0000-000000000002',
    'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb',
    'Futsal Court 1',
    TRUE,
    NOW(),
    NOW()
),
(
    'c0000000-0000-0000-0000-000000000003',
    'f0000000-0000-0000-0000-000000000003',
    'cccccccc-cccc-cccc-cccc-cccccccccccc',
    'Basketball Court',
    TRUE,
    NOW(),
    NOW()
),
(
    'c0000000-0000-0000-0000-000000000004',
    'f0000000-0000-0000-0000-000000000004',
    'dddddddd-dddd-dddd-dddd-dddddddddddd',
    'Volleyball Court',
    TRUE,
    NOW(),
    NOW()
),
(
    'c0000000-0000-0000-0000-000000000005',
    'f0000000-0000-0000-0000-000000000005',
    'eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee',
    'Tennis Court A',
    TRUE,
    NOW(),
    NOW()
);