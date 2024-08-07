drop table if exists Livro;
go

create table Livro (
    Codigo int primary key identity,
    Titulo varchar(255) not null,
    Autor varchar(255) not null,
    Lancamento date not null
);
go

drop table if exists Tag;
go

create table Tag (
    Codigo int primary key identity,
    Descricao varchar(255) not null,
	LivroCodigo int not null references Livro(Codigo)
);
go

drop table if exists TipoEncadernacao;
go

create table TipoEncadernacao (
    Codigo int primary key identity,
    Nome varchar(255) not null,
    Descricao varchar(255) not null,
    Formato varchar(255) not null
);    
go

drop table if exists LivroDigital;
go

create table LivroDigital (
    Codigo int primary key references Livro(Codigo) on delete cascade,
    Formato varchar(255) not null,    
);
go

drop table if exists LivroImpresso;
go

create table LivroImpresso (
    Id int primary key identity,
    Codigo int not null references Livro(Codigo)  on delete cascade,
    Peso decimal(5, 2) not null,
    TipoEncadernacaoCodigo int not null references TipoEncadernacao(Codigo)    
);
go

drop view if exists VW_LivroDetails;
go

create or alter view VW_LivroDetails 
as
select 
    l.Codigo,
    l.Lancamento,
    format(l.Lancamento, 'yyyy') Ano,
    format(l.Lancamento, 'MM') Mes,
    l.Titulo,
    l.Autor,
    l.Lancamento DataLancamento,
    'Digital' Tipo,
    ld.Formato,
    null Peso,
    '' Descricao,
    '' TipoEncadernacao
FROM 
    Livro l,
    LivroDigital ld
where
    l.Codigo = ld.Codigo    
UNION ALL
select 
    l.Codigo,
    l.Lancamento,
    cast(format(l.Lancamento, 'yyyy') as int) Ano,
    cast(format(l.Lancamento, 'MM') as int) Mes,    
    l.Titulo,
    l.Autor,
    l.Lancamento DataLancamento,
    'Digital' Tipo,
    te.Formato,
    li.Peso,
    te.Descricao,
    te.Nome TipoEncadernacao
FROM 
    Livro l,
    LivroImpresso li,
    TipoEncadernacao te
where     
    l.Codigo = li.Codigo
and li.TipoEncadernacaoCodigo = te.Codigo;
go

create or alter procedure PR_LivrosDetails 
@mes int, 
@ano int
as
begin
    select *
    from VW_LivroDetails vw
    where ((@ano > 0 and vw.Ano = @ano) or (@ano <= 0))
    and  ((@mes > 0 and vw.Mes = @mes) or (@mes <= 0))
    order by vw.Titulo
end;
go