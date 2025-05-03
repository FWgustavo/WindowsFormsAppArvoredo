create database if not exists Arvoredo;

use Arvoredo;

CREATE TABLE if not exists Usuarios (
	ID INT NOT NULL AUTO_INCREMENT,
    Login VARCHAR(50) NOT NULL UNIQUE,
    Senha VARCHAR(100) NOT NULL,
    Nome VARCHAR(100) NOT NULL,
    Email VARCHAR(100),
    NivelAcesso INT DEFAULT 1,
    Ativo BIT DEFAULT 1,
    DataCriacao DATETIME,
    PRIMARY KEY (ID)
);
INSERT INTO Usuarios (Login, Senha, Nome, Email, NivelAcesso)
VALUES ('Guadm', 'a123', 'Administrador', 'admin@arvoredo.com', 3);

INSERT INTO Usuarios (Login, Senha, Nome, Email, NivelAcesso)
VALUES ('Gu2', '123', 'Usuário Padrão', 'usuario@arvoredo.com', 1);