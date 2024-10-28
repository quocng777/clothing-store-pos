CREATE TABLE products (
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

CREATE TABLE categories (
    id SERIAL PRIMARY KEY,     
    name VARCHAR(255) NOT NULL   
);

ALTER TABLE products
ADD CONSTRAINT FK_Products_Categories
FOREIGN KEY (category_id)
REFERENCES categories(id);

CREATE TABLE users (
    id SERIAL PRIMARY KEY,               
    username VARCHAR(255) NOT NULL UNIQUE, 
    password_hash VARCHAR(255) NOT NULL, 
    password_salt VARCHAR(10) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,  
    user_role VARCHAR(255) NOT NULL,                
    is_active BOOLEAN DEFAULT TRUE,     
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP 
);