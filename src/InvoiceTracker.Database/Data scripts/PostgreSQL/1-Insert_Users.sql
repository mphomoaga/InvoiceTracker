INSERT INTO security."Users"
(
    "FirstName",
    "LastName",
    "Email",
    "IsLocked",
    "PasswordHash",
    "LastLoginUtc",
    "IsActive",
    "CreatedBy",
    "CreatedDate",
    "ModifiedBy",
    "ModifiedDate",
    "RoleId"
)
VALUES
(
    'Mpho',
    'Moaga',
    'mpho@gmail.com',
    false,
    '9ZzivzpT+8WsNOLAhepZReSDn2nrKJU30Glnay16s/kk1CcG',
    TIMESTAMP '2026-11-08 18:24:13.537945',
    true,
    'mpho@gmail.com',
    TIMESTAMP '2026-11-08 18:00:31.944809',
    NULL,
    TIMESTAMP '2026-11-08 18:24:14.457697',
    1
),
(
    'Mario',
    'Andrew',
    'mario@gmail.com',
    false,
    '9ZzivzpT+8WsNOLAhepZReSDn2nrKJU30Glnay16s/kk1CcG',
    TIMESTAMP '2026-11-08 18:24:13.537945',
    true,
    'mpho@gmail.com',
    TIMESTAMP '2026-11-08 18:00:31.944809',
    NULL,
    TIMESTAMP '2026-11-08 18:24:14.457697',
    1
)
RETURNING
    "UserId",
    "FirstName",
    "LastName",
    "Email";