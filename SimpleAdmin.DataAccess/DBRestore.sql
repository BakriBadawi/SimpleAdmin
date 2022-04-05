RESTORE DATABASE [SimpleAdmin] FROM  
DISK = N'/tmp/SimpleAdmin.bak' WITH  FILE = 1,
MOVE N'SimpleAdmin' to '/var/opt/mssql/data/SimpleAdmin.mdf',
MOVE N'SimpleAdmin_log' to '/var/opt/mssql/data/SimpleAdmin_log.ldf',
NOUNLOAD, REPLACE, STATS=5 
GO

