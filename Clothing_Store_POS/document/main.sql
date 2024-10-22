CREATE TABLE products (
    id SERIAL PRIMARY KEY,       -- Tạo cột Id với kiểu dữ liệu SERIAL (tự động tăng)
    name VARCHAR(255) NOT NULL,  -- Tạo cột Name với kiểu dữ liệu VARCHAR
    price DECIMAL(10, 2) NOT NULL, -- Tạo cột Price với kiểu dữ liệu DECIMAL
    category_id INT NOT NULL,     -- Tạo cột CategoryId với kiểu dữ liệu INT
    Size VARCHAR(50),            -- Tạo cột Size với kiểu dữ liệu VARCHAR
    Color VARCHAR(50),           -- Tạo cột Color với kiểu dữ liệu VARCHAR
    Stock INT NOT NULL,          -- Tạo cột Stock với kiểu dữ liệu INT
    Sale FLOAT                   -- Tạo cột Sale với kiểu dữ liệu FLOAT
);

CREATE TABLE categories (
    id SERIAL PRIMARY KEY,        -- Tạo cột Id với kiểu dữ liệu SERIAL (tự động tăng)
    name VARCHAR(255) NOT NULL    -- Tạo cột Name với kiểu dữ liệu VARCHAR, không cho phép giá trị NULL
);

ALTER TABLE products
ADD CONSTRAINT FK_Products_Categories
FOREIGN KEY (category_id)
REFERENCES categories(id);