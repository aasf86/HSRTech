drop table if exists Livro;

create table Livro (
    Codigo int primary key identity,
    Titulo varchar(255) not null,
    Autor varchar(255) not null,
    Lancamento date not null
);

drop table if exists Tag;

create table Tag (
    Codigo int primary key identity,
    Descricao varchar(255) not null,
	LivroCodigo int not null references Livro(Codigo)
);

drop table if exists TipoEncadernacao;

create table TipoEncadernacao (
    Codigo int primary key identity,
    Nome varchar(255) not null,
    Descricao varchar(255) not null,
    Formato varchar(255) not null
);    