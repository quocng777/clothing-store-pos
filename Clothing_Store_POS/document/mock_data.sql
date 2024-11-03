INSERT INTO categories (name) VALUES
('Quần áo nam'),
('Quần áo nữ'),
('Giày dép'),
('Phụ kiện'),
('Đồ thể thao');

INSERT INTO products (name, price, category_id, size, color, stock, sale) VALUES
('Áo thun nam', 250000, 1, 'M', 'Đen', 50, 0.1),
('Quần jeans nữ', 350000, 2, 'S', 'Xanh', 30, 0.2),
('Giày thể thao', 800000, 3, NULL, 'Trắng', 20, 0.15),
('Mũ thể thao', 150000, 4, NULL, 'Đỏ', 100, 0),
('Áo khoác', 600000, 1, 'L', 'Xám', 25, 0.05);

INSERT INTO users (fullname, username, password_hash, email, user_role, is_active, created_date) VALUES
    ('John Sena', 'johnsena', '$2a$11$ygrhLkennieuLLcm/RBLlefPMWEigNb1PAdo6/yTknmgsDjYAl51.', 'johnsena@gmail.com', 'admin', TRUE, CURRENT_TIMESTAMP),
    ('Tyanin Poo', 'tyanipo', '$2a$11$KXJxNbhY607P9qY/g9DafO0.HoD2DPStza4ryE3TbsQphCL0MoFE.', 'tyanipo@gmail.com', 'admin', TRUE, CURRENT_TIMESTAMP),
    ('Kim Jong Un', 'kimjongun', '$2a$11$iY3rc9rfeUQoCyMUc9cQ6O58PzAar1cuC2GrhxRmtjPZfSK.m.N9C', 'kimjongun@gmail.com', 'admin', TRUE, CURRENT_TIMESTAMP);