DELETE FROM Malzemeler;
DBCC CHECKIDENT ('[Malzemeler]', RESEED, 0);
GO
DELETE FROM Resimler;
DBCC CHECKIDENT ('[Resimler]', RESEED, 0);
GO
DELETE FROM Tarifler;
DBCC CHECKIDENT ('[Tarifler]', RESEED, 0);
GO
DELETE FROM YapimAsamalari;
DBCC CHECKIDENT ('[YapimAsamalari]', RESEED, 0);
GO
DELETE FROM Yorumlar;
DBCC CHECKIDENT ('[Yorumlar]', RESEED, 0);
GO