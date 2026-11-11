INSERT INTO [dbo].[Person]
(
    [Title],
    [Intials],
    [FullNames],
    [Surname],
    [IdNumber],
    [PassportNumber],
    [IsActive],
    [CreatedBy],
    [CreatedDate],
    [ModifiedBy],
    [ModifiedDate]
)
VALUES
('Mr',  'TJ', 'Thomas John',       'Smith',      '8001015009087', NULL,          1, 'System', GETDATE(), 'System', GETDATE()),
('Ms',  'LN', 'Lerato Naledi',     'Mokoena',    '8502026009088', NULL,          1, 'System', GETDATE(), 'System', GETDATE()),
('Mrs', 'PM', 'Puleng Maria',      'Aphane',     '9107010541088', NULL,          1, 'System', GETDATE(), 'System', GETDATE()),
('Mr',  'KR', 'Kabelo Raymond',    'Molefe',     '8703155809089', NULL,          1, 'System', GETDATE(), 'System', GETDATE()),
('Dr',  'SN', 'Sipho Nicholas',    'Nkosi',      '7908115709090', 'P12345678',   1, 'System', GETDATE(), 'System', GETDATE()),
('Mrs', 'AM', 'Amanda Michelle',   'Johnson',    '8306045209091', 'P23456789',   1, 'System', GETDATE(), 'System', GETDATE()),
('Mr',  'DB', 'David Brian',       'Wilson',     '7509255309092', NULL,          1, 'System', GETDATE(), 'System', GETDATE()),
('Ms',  'NT', 'Nomsa Thandeka',    'Zulu',       '9204175509093', NULL,          1, 'System', GETDATE(), 'System', GETDATE()),
('Mr',  'MH', 'Michael Henry',     'Brown',      '8812035109094', 'P34567890',   1, 'System', GETDATE(), 'System', GETDATE()),
('Prof','RG', 'Robert George',     'Taylor',     '7005105409095', 'P45678901',   1, 'System', GETDATE(), 'System', GETDATE());