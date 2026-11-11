INSERT INTO [communication].[EmailAudit]
(
    [ItemType],
    [ItemTypeRef],
    [From],
    [To],
    [CC],
    [Subject],
    [Body],
    [HasAttachments],
    [IsSent],
    [SentDate],
    [IsActive],
    [CreatedBy],
    [CreatedDate],
    [ModifiedBy],
    [ModifiedDate]
)
VALUES
('Invoice',1,'accounts@invoicetracker.com','customer1@email.com',NULL,'Invoice Statement INV-20261101',
'<html><body><h2>Invoice Statement</h2><p>Dear Customer,</p><p>Please find your invoice statement INV-20261101 attached.</p><p>Kind Regards,<br/>Accounts Team</p></body></html>',1,1,GETDATE(),1,'SYSTEM',GETDATE(),NULL,NULL),

('Invoice',2,'accounts@invoicetracker.com','customer2@email.com',NULL,'Invoice Statement INV-20261102',
'<html><body><h2>Invoice Statement</h2><p>Dear Customer,</p><p>Please find your invoice statement INV-20261102 attached.</p><p>Kind Regards,<br/>Accounts Team</p></body></html>',1,1,DATEADD(HOUR,-1,GETDATE()),1,'SYSTEM',GETDATE(),NULL,NULL),

('Invoice',3,'accounts@invoicetracker.com','customer3@email.com',NULL,'Invoice Statement INV-20261103',
'<html><body><h2>Invoice Statement</h2><p>Dear Customer,</p><p>Please find your invoice statement INV-20261103 attached.</p><p>Kind Regards,<br/>Accounts Team</p></body></html>',1,1,DATEADD(HOUR,-2,GETDATE()),1,'SYSTEM',GETDATE(),NULL,NULL),

('Invoice',4,'accounts@invoicetracker.com','customer4@email.com',NULL,'Invoice Statement INV-20261104',
'<html><body><h2>Invoice Statement</h2><p>Dear Customer,</p><p>Please find your invoice statement INV-20261104 attached.</p><p>Kind Regards,<br/>Accounts Team</p></body></html>',0,1,DATEADD(HOUR,-3,GETDATE()),1,'SYSTEM',GETDATE(),NULL,NULL),

('Invoice',5,'accounts@invoicetracker.com','customer5@email.com',NULL,'Invoice Statement INV-20261105',
'<html><body><h2>Invoice Statement</h2><p>Dear Customer,</p><p>Please find your invoice statement INV-20261105 attached.</p><p>Kind Regards,<br/>Accounts Team</p></body></html>',1,1,DATEADD(HOUR,-4,GETDATE()),1,'SYSTEM',GETDATE(),NULL,NULL),

('Invoice',6,'accounts@invoicetracker.com','customer6@email.com',NULL,'Invoice Statement INV-20261106',
'<html><body><h2>Invoice Statement</h2><p>Dear Customer,</p><p>Please find your invoice statement INV-20261106 attached.</p><p>Kind Regards,<br/>Accounts Team</p></body></html>',1,1,DATEADD(HOUR,-5,GETDATE()),1,'SYSTEM',GETDATE(),NULL,NULL),

('Invoice',7,'accounts@invoicetracker.com','customer7@email.com',NULL,'Invoice Statement INV-20261107',
'<html><body><h2>Invoice Statement</h2><p>Dear Customer,</p><p>Please find your invoice statement INV-20261107 attached.</p><p>Kind Regards,<br/>Accounts Team</p></body></html>',0,1,DATEADD(HOUR,-6,GETDATE()),1,'SYSTEM',GETDATE(),NULL,NULL),

('Invoice',8,'accounts@invoicetracker.com','customer8@email.com',NULL,'Invoice Statement INV-20261108',
'<html><body><h2>Invoice Statement</h2><p>Dear Customer,</p><p>Please find your invoice statement INV-20261108 attached.</p><p>Kind Regards,<br/>Accounts Team</p></body></html>',1,1,DATEADD(HOUR,-7,GETDATE()),1,'SYSTEM',GETDATE(),NULL,NULL),

('Invoice',9,'accounts@invoicetracker.com','customer9@email.com',NULL,'Invoice Statement INV-20261109',
'<html><body><h2>Invoice Statement</h2><p>Dear Customer,</p><p>Please find your invoice statement INV-20261109 attached.</p><p>Kind Regards,<br/>Accounts Team</p></body></html>',1,1,DATEADD(HOUR,-8,GETDATE()),1,'SYSTEM',GETDATE(),NULL,NULL),

('Invoice',10,'accounts@invoicetracker.com','customer10@email.com',NULL,'Invoice Statement INV-20261110',
'<html><body><h2>Invoice Statement</h2><p>Dear Customer,</p><p>Please find your invoice statement INV-20261110 attached.</p><p>Kind Regards,<br/>Accounts Team</p></body></html>',1,1,DATEADD(HOUR,-9,GETDATE()),1,'SYSTEM',GETDATE(),NULL,NULL),

('Invoice',11,'accounts@invoicetracker.com','customer11@email.com',NULL,'Invoice Statement INV-20261111',
'<html><body><h2>Invoice Statement</h2><p>Dear Customer,</p><p>Please find your invoice statement INV-20261111 attached.</p><p>Kind Regards,<br/>Accounts Team</p></body></html>',0,1,DATEADD(HOUR,-10,GETDATE()),1,'SYSTEM',GETDATE(),NULL,NULL),

('Invoice',12,'accounts@invoicetracker.com','customer12@email.com',NULL,'Invoice Statement INV-20261112',
'<html><body><h2>Invoice Statement</h2><p>Dear Customer,</p><p>Please find your invoice statement INV-20261112 attached.</p><p>Kind Regards,<br/>Accounts Team</p></body></html>',1,1,DATEADD(HOUR,-11,GETDATE()),1,'SYSTEM',GETDATE(),NULL,NULL),

('Invoice',13,'accounts@invoicetracker.com','customer13@email.com',NULL,'Invoice Statement INV-20261113',
'<html><body><h2>Invoice Statement</h2><p>Dear Customer,</p><p>Please find your invoice statement INV-20261113 attached.</p><p>Kind Regards,<br/>Accounts Team</p></body></html>',1,1,DATEADD(HOUR,-12,GETDATE()),1,'SYSTEM',GETDATE(),NULL,NULL),

('Invoice',14,'accounts@invoicetracker.com','customer14@email.com',NULL,'Invoice Statement INV-20261114',
'<html><body><h2>Invoice Statement</h2><p>Dear Customer,</p><p>Please find your invoice statement INV-20261114 attached.</p><p>Kind Regards,<br/>Accounts Team</p></body></html>',1,1,DATEADD(HOUR,-13,GETDATE()),1,'SYSTEM',GETDATE(),NULL,NULL),

('Invoice',15,'accounts@invoicetracker.com','customer15@email.com',NULL,'Invoice Statement INV-20261115',
'<html><body><h2>Invoice Statement</h2><p>Dear Customer,</p><p>Please find your invoice statement INV-20261115 attached.</p><p>Kind Regards,<br/>Accounts Team</p></body></html>',0,1,DATEADD(HOUR,-14,GETDATE()),1,'SYSTEM',GETDATE(),NULL,NULL),

('Invoice',16,'accounts@invoicetracker.com','customer16@email.com',NULL,'Invoice Statement INV-20261116',
'<html><body><h2>Invoice Statement</h2><p>Dear Customer,</p><p>Please find your invoice statement INV-20261116 attached.</p><p>Kind Regards,<br/>Accounts Team</p></body></html>',1,1,DATEADD(HOUR,-15,GETDATE()),1,'SYSTEM',GETDATE(),NULL,NULL),

('Invoice',17,'accounts@invoicetracker.com','customer17@email.com',NULL,'Invoice Statement INV-20261117',
'<html><body><h2>Invoice Statement</h2><p>Dear Customer,</p><p>Please find your invoice statement INV-20261117 attached.</p><p>Kind Regards,<br/>Accounts Team</p></body></html>',1,1,DATEADD(HOUR,-16,GETDATE()),1,'SYSTEM',GETDATE(),NULL,NULL),

('Invoice',18,'accounts@invoicetracker.com','customer18@email.com',NULL,'Invoice Statement INV-20261118',
'<html><body><h2>Invoice Statement</h2><p>Dear Customer,</p><p>Please find your invoice statement INV-20261118 attached.</p><p>Kind Regards,<br/>Accounts Team</p></body></html>',1,1,DATEADD(HOUR,-17,GETDATE()),1,'SYSTEM',GETDATE(),NULL,NULL),

('Invoice',19,'accounts@invoicetracker.com','customer19@email.com',NULL,'Invoice Statement INV-20261119',
'<html><body><h2>Invoice Statement</h2><p>Dear Customer,</p><p>Please find your invoice statement INV-20261119 attached.</p><p>Kind Regards,<br/>Accounts Team</p></body></html>',0,1,DATEADD(HOUR,-18,GETDATE()),1,'SYSTEM',GETDATE(),NULL,NULL),

('Invoice',20,'accounts@invoicetracker.com','customer20@email.com',NULL,'Invoice Statement INV-20261120',
'<html><body><h2>Invoice Statement</h2><p>Dear Customer,</p><p>Please find your invoice statement INV-20261120 attached.</p><p>Kind Regards,<br/>Accounts Team</p></body></html>',1,1,DATEADD(HOUR,-19,GETDATE()),1,'SYSTEM',GETDATE(),NULL,NULL),

('Invoice',21,'accounts@invoicetracker.com','customer21@email.com',NULL,'Invoice Statement INV-20261121',
'<html><body><h2>Invoice Statement</h2><p>Dear Customer,</p><p>Please find your invoice statement INV-20261121 attached.</p><p>Kind Regards,<br/>Accounts Team</p></body></html>',1,1,DATEADD(HOUR,-20,GETDATE()),1,'SYSTEM',GETDATE(),NULL,NULL);