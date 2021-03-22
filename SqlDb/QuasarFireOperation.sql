-- Tabla Satelites

CREATE TABLE dbo.SATELLITES (		
		id			int IDENTITY(1,1) PRIMARY KEY,
		name		varchar(50) NOT NULL,
		position_x  float NOT NULL,
		position_y  float NOT NULL
	);

INSERT INTO SATELLITES VALUES ('Kenobi',-500,-200);
INSERT INTO SATELLITES VALUES ('Skywalker',100,-100);
INSERT INTO SATELLITES VALUES ('Sato',500,100);

-- Tabla Estado Mensajes

CREATE TABLE dbo.STATUS_RESPONSE (
		id 				int NOT NULL,
		status_name		varchar(50)
	);

ALTER TABLE dbo.STATUS_RESPONSE ADD CONSTRAINT PK_STATUS_RESPONSE PRIMARY KEY CLUSTERED (id);

INSERT INTO STATUS_RESPONSE VALUES (1,'ENVIADO CORRECTAMENTE');
INSERT INTO STATUS_RESPONSE VALUES (2,'ERROR: FALTA DETERMINAR COORDENADA');
INSERT INTO STATUS_RESPONSE VALUES (3,'ERROR: FALTA DESIFRAR MENSAJE');
INSERT INTO STATUS_RESPONSE VALUES (4,'ERROR: PETICIÓN INVALIDA');

-- Tabla Mensajes Enviados por la API correctos

CREATE TABLE dbo.RESPONSE (
		  id		  int IDENTITY(1,1) PRIMARY KEY,
		  x_location  float NOT NULL,
		  y_location  float NOT NULL,
		  message	  varchar (max),
		  status_id  int
   );

ALTER TABLE dbo.RESPONSE ADD CONSTRAINT FK_RESPONSE_STATUS
FOREIGN KEY (status_id)  REFERENCES dbo.STATUS_RESPONSE  (id) ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Tabla Mensajes Recibidos

CREATE TABLE dbo.MESSAGES_SECRET (
		  id				int IDENTITY(1,1) PRIMARY KEY,
		  satelite_id		int,
		  distance			float,
		  message			varchar (max),
		  response_id 		int,
		  date_process		datetime,
		  process			int -- Si fue procesado colocamos 1 si no 0

   );

ALTER TABLE dbo.MESSAGES_SECRET ADD CONSTRAINT FK_MESSAGES_SECRET_SATELLITES
FOREIGN KEY (satelite_id)  REFERENCES dbo.SATELLITES  (id) ON DELETE NO ACTION ON UPDATE NO ACTION;