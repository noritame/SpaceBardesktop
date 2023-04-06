CREATE DATABASE SpaceBar
GO
Use SpaceBar		
GO
Create table tblUsuario(
codigo_usuario int primary key IDENTITY(1,1),
Nome_usuario varChar(30) not null,
tag_usuario varchar(20) not null, 
login_usuario varchar(30) not null,
senha_usuario varChar(30) not null,
email_usuario varchar(30),
icon_usuario VARBINARY(MAX)
)
GO
Create table tblCriador(
cod_Criador int primary key IDENTITY(1,1),
Nome varchar(30) not null,
Senha varChar(30) not null,
Data_Nasc DATE not null,
endereco varchar(30),
genero varchar(30),
CEP numeric(30) not null,
icon VARBINARY(MAX)
)
GO
create table tblPublicacao (
cod_Criador int foreign key references tblCriador,
cod_User int foreign key references tblUsuario,
descricao_pub VARCHAR(max) not null,
curtidas_pub int,
imagem_pub IMAGE,
titulo_pub VARCHAR (80),
data_pub DATE NOT NULL,
cod_comentario int foreign key references tblComentarios
)
GO
create table tblComentarios (
    cod_comentario int PRIMARY KEY NOT NULL,
    conteudo_comentario VARCHAR(200) NOT NULL,
    cod_User int foreign key references tblUsuario,
    data_comentario DATE NOT NULL
)
GO
create table tblAdm(
cod_adm int primary key IDENTITY(1,1),
Nome Varchar(50) not null,
Senha NUMERIC(30) not null,
icon VARBINARY(MAX)
)
GO
create table tblValidacao(
cod_Valida int primary key,
cod_Adm int foreign key references tblAdm,
cod_User int foreign key references tblUsuario,
)
GO
Insert into tblAdm (Nome, Senha) values ('Igor', '1234'), ('Clara','4321'),('Pedro','3241')
GO
SELECT * from tblAdm

DECLARE @cod_Valida INT = 1;
if @cod_Valida =1

PRINT 'Publicação aprovada';

ELSE
PRINT 'Publicação Negada';