CREATE DATABASE IF NOT EXISTS cadastro
    CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

USE cadastro;

CREATE TABLE IF NOT EXISTS clientes (
    Id       INT AUTO_INCREMENT PRIMARY KEY,
    Nome     VARCHAR(100) NOT NULL,
    Email    VARCHAR(100),
    Telefone VARCHAR(20),
    Cidade   VARCHAR(60)
);