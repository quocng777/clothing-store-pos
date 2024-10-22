INSERT INTO categories (name) VALUES
('Quần áo nam'),
('Quần áo nữ'),
('Giày dép'),
('Phụ kiện'),
('Đồ thể thao');

INSERT INTO products (name, price, category_id, size, color, stock, sale) VALUES
('Áo thun nam', 250000, 1, 'M', 'Đen', 50, 0.1), -- Tham chiếu đến category_id 1
('Quần jeans nữ', 350000, 2, 'S', 'Xanh', 30, 0.2), -- Tham chiếu đến category_id 2
('Giày thể thao', 800000, 3, NULL, 'Trắng', 20, 0.15), -- Tham chiếu đến category_id 3
('Mũ thể thao', 150000, 4, NULL, 'Đỏ', 100, 0), -- Tham chiếu đến category_id 4
('Áo khoác', 600000, 1, 'L', 'Xám', 25, 0.05); -- Tham chiếu đến category_id 1