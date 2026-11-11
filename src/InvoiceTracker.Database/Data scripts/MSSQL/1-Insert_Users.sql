SET IDENTITY_INSERT [security].[Users] ON;

INSERT INTO [security].[User]
(
    [FirstName],
    [LastName],
    [Email],
    [IsLocked],
    [PasswordHash],
    [LastLoginUtc],
    [IsActive],
    [CreatedBy],
    [CreatedDate],
    [ModifiedBy],
    [ModifiedDate],
    [RoleId]
)
VALUES
(
    'Mpho',
    'Moaga',
    'mpho@gmail.com',
    0,
    '9ZzivzpT+8WsNOLAhepZReSDn2nrKJU30Glnay16s/kk1CcG',
    '2026-11-08T18:24:13.5379459',
    1,
    'mpho@gmail.com',
    '2026-11-08T18:00:31.9448099',
    NULL,
    '2026-11-08T18:24:14.4576970',
    1
);

(
    'Mario',
    'Andrew',
    'mario@gmail.com',
    0,
    '9ZzivzpT+8WsNOLAhepZReSDn2nrKJU30Glnay16s/kk1CcG',
    '2026-11-08T18:24:13.5379459',
    1,
    'mpho@gmail.com',
    '2026-11-08T18:00:31.9448099',
    NULL,
    '2026-11-08T18:24:14.4576970',
    1
);
