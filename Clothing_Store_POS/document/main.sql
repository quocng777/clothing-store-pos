\c pos_db;

CREATE TABLE IF NOT EXISTS products (
    id SERIAL PRIMARY KEY,       
    name VARCHAR(255) NOT NULL,  
    price DECIMAL(10, 2) NOT NULL, 
    category_id INT NOT NULL,     
    size VARCHAR(50),          
    color VARCHAR(50),          
    stock INT NOT NULL,         
    sale FLOAT,
    thumbnail VARCHAR(255)
);

CREATE TABLE IF NOT EXISTS categories (
    id SERIAL PRIMARY KEY,     
    name VARCHAR(255) NOT NULL   
);

ALTER TABLE products
ADD CONSTRAINT FK_Products_Categories
FOREIGN KEY (category_id)
REFERENCES categories(id);

CREATE TABLE IF NOT EXISTS users  (
    id SERIAL PRIMARY KEY,               
    fullname VARCHAR(255) NOT NULL,
    username VARCHAR(255) NOT NULL UNIQUE, 
    password_hash VARCHAR(255) NOT NULL, 
    email VARCHAR(255) NOT NULL UNIQUE,  
    user_role VARCHAR(255) NOT NULL,                
    is_active BOOLEAN DEFAULT TRUE,     
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP 
);

CREATE TABLE IF NOT EXISTS orders (
    id SERIAL PRIMARY KEY,
    discount_percentage DECIMAL(5, 2) DEFAULT 0,
    tax_percentage DECIMAL(5, 2) DEFAULT 0,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
);

CREATE TABLE IF NOT EXISTS order_items (
    id SERIAL PRIMARY KEY,
    order_id INT NOT NULL,
    product_id INT NOT NULL,
    quantity INT NOT NULL DEFAULT 1,
    discount_percentage DECIMAL(5, 2) DEFAULT 0,
    FOREIGN KEY (order_id) REFERENCES orders(id) ON DELETE CASCADE,
    FOREIGN KEY (product_id) REFERENCES products(id) 
);