-- ⚠️ EJECUTA ESTO EN SSMS ANTES DE COMPILAR ⚠️

ALTER TABLE Inicio_Sesion ADD EsAdministrador BIT NOT NULL DEFAULT 0;
GO

-- Marca a Yoel como administrador (cámbialo si quieres otro usuario)
UPDATE Inicio_Sesion SET EsAdministrador = 1 WHERE Usuario = 'Yoel';
GO
