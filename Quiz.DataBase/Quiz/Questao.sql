create table Questao (

    ID bigint NOT NULL,
	Nome varchar(255) NOT NULL,
    Enunciado varchar(255),
    Imagem varchar(max),
    Dificuladade int,
    QuestaoAtiva bit,
    IdMateria int NOT NULL,
    PRIMARY KEY (ID),
	FOREIGN KEY (IdMateria) REFERENCES Materia(ID)

)