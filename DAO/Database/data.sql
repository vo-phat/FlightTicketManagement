/* =========================================================
   1. ROLES
   ========================================================= */
INSERT INTO roles (role_code, role_name) VALUES
    ('USER',  'Người dùng'),
    ('STAFF', 'Nhân viên'),
    ('ADMIN', 'Quản trị viên')
ON DUPLICATE KEY UPDATE
    role_name = VALUES(role_name);


/* =========================================================
   2. PERMISSIONS (map 1-1 với Perm trong project)
   ========================================================= */
INSERT INTO permissions (permission_code, permission_name) VALUES
    ('home.view',             'Xem trang chủ'),

    ('flights.read',          'Xem / tra cứu chuyến bay'),
    ('fare_rules.manage',     'Quản lý quy tắc giá vé'),

    ('tickets.create_search', 'Tạo / tìm đặt chỗ'),
    ('tickets.mine',          'Xem đặt chỗ của tôi'),
    ('tickets.operate',       'Vận hành vé'),
    ('tickets.history',       'Xem lịch sử vé'),

    ('baggage.checkin',       'Check-in hành lý'),
    ('baggage.track',         'Theo dõi hành lý'),
    ('baggage.report',        'Báo cáo hành lý thất lạc'),

    ('catalogs.airlines',     'Quản lý hãng hàng không'),
    ('catalogs.aircrafts',    'Quản lý máy bay'),
    ('catalogs.airports',     'Quản lý sân bay'),
    ('catalogs.routes',       'Quản lý tuyến bay'),
    ('catalogs.cabin_classes','Quản lý hạng vé'),
    ('catalogs.seats',        'Quản lý ghế máy bay'),

    ('payments.pos',          'Thanh toán POS'),

    ('customers.profiles',    'Hồ sơ khách hàng'),
    ('accounts.manage',       'Quản lý tài khoản & phân quyền'),

    ('notifications.read',    'Xem thông báo'),

    ('reports.view',          'Xem báo cáo'),

    ('system.roles',          'Cấu hình vai trò & phân quyền')
ON DUPLICATE KEY UPDATE
    permission_name = VALUES(permission_name);


/* =========================================================
   3. TÀI KHOẢN
   ========================================================= */
/* SHA-256:
   admin     → 8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918
   staff123  → ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f
   user123   → 397a6f558fbecb63c19f78146d3665bd41fda95e9800cf4fce4a28d8acc57aef
*/

INSERT INTO accounts (email, password, failed_attempts, is_active, created_at) VALUES
    ('admin',
     '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918',
     5, 1, NOW()
    ),
    ('staff@gmail.com',
     '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918',
     5, 1, NOW()
    ),
    ('user@gmail.com',
     '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918',
     5, 1, NOW()
    )
ON DUPLICATE KEY UPDATE
    password = VALUES(password);


/* =========================================================
   4. ACCOUNT_ROLE (gán vai trò cho tài khoản)
   ========================================================= */
-- ADMIN → ADMIN
INSERT IGNORE INTO account_role (account_id, role_id)
SELECT a.account_id, r.role_id
FROM accounts a
JOIN roles r ON r.role_code = 'ADMIN'
WHERE a.email = 'admin';

-- STAFF → STAFF
INSERT IGNORE INTO account_role (account_id, role_id)
SELECT a.account_id, r.role_id
FROM accounts a
JOIN roles r ON r.role_code = 'STAFF'
WHERE a.email = 'staff@gmail.com';

-- USER → USER
INSERT IGNORE INTO account_role (account_id, role_id)
SELECT a.account_id, r.role_id
FROM accounts a
JOIN roles r ON r.role_code = 'USER'
WHERE a.email = 'user@gmail.com';


/* =========================================================
   5. ROLE_PERMISSIONS
   ========================================================= */

-- USER
INSERT IGNORE INTO role_permissions (role_id, permission_id)
SELECT r.role_id, p.permission_id
FROM roles r
JOIN permissions p ON p.permission_code IN (
    'home.view',
    'tickets.create_search',
    'tickets.mine',
    'customers.profiles',
    'notifications.read'
)
WHERE r.role_code = 'USER';

-- STAFF
INSERT IGNORE INTO role_permissions (role_id, permission_id)
SELECT r.role_id, p.permission_id
FROM roles r
JOIN permissions p ON p.permission_code IN (
    'home.view',
    'flights.read',
    'tickets.create_search',
    'tickets.operate',
    'tickets.history',
    'baggage.checkin',
    'baggage.track',
    'payments.pos',
    'customers.profiles',
    'notifications.read',
    'reports.view'
)
WHERE r.role_code = 'STAFF';

-- ADMIN (tất cả quyền)
INSERT IGNORE INTO role_permissions (role_id, permission_id)
SELECT r.role_id, p.permission_id
FROM roles r
JOIN permissions p
WHERE r.role_code = 'ADMIN';
