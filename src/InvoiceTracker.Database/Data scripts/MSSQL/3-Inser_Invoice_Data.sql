INSERT INTO [billing].[Invoice]
(
    [PersonId],
    [Reference],
    [InvoiceDate],
    [InvoiceDueDate],
    [InvoiceStatusId],
    [InvoiceAmount],
    [IsActive],
    [CreatedBy],
    [CreatedDate],
    [ModifiedBy],
    [ModifiedDate],
    [InvoiceStatus]
)
VALUES
(1, 'INV-202611-0001', DATEADD(DAY,-30,GETDATE()), DATEADD(DAY,-15,GETDATE()), 3, 1250.00, 1, 'System', GETDATE(), 'System', GETDATE()),

(2, 'INV-202611-0002', DATEADD(DAY,-25,GETDATE()), DATEADD(DAY,-10,GETDATE()), 4, 2750.50, 1, 'System', GETDATE(), 'System', GETDATE()),

(3, 'INV-202611-0003', DATEADD(DAY,-20,GETDATE()), DATEADD(DAY,-5,GETDATE()), 3, 980.75, 1, 'System', GETDATE(), 'System', GETDATE()),

(4, 'INV-202611-0004', DATEADD(DAY,-15,GETDATE()), DATEADD(DAY,15,GETDATE()), 2, 3450.00, 1, 'System', GETDATE(), 'System', GETDATE()),

(5, 'INV-202611-0005', DATEADD(DAY,-10,GETDATE()), DATEADD(DAY,20,GETDATE()), 2, 5125.25, 1, 'System', GETDATE(), 'System', GETDATE()),

(6, 'INV-202611-0006', DATEADD(DAY,-8,GETDATE()), DATEADD(DAY,22,GETDATE()), 1, 850.00, 1, 'System', GETDATE(), 'System', GETDATE()),

(7, 'INV-202611-0007', DATEADD(DAY,-5,GETDATE()), DATEADD(DAY,25,GETDATE()), 1, 1750.00, 1, 'System', GETDATE(), 'System', GETDATE()),

(8, 'INV-202611-0008', DATEADD(DAY,-40,GETDATE()), DATEADD(DAY,-20,GETDATE()), 4, 6250.99, 1, 'System', GETDATE(), 'System', GETDATE()),

(9, 'INV-202611-0009', DATEADD(DAY,-12,GETDATE()), DATEADD(DAY,18,GETDATE()), 2, 4300.00, 1, 'System', GETDATE(), 'System', GETDATE()),

(10,'INV-202611-0010', DATEADD(DAY,-35,GETDATE()), DATEADD(DAY,-5,GETDATE()), 3, 2999.99, 1, 'System', GETDATE(), 'System', GETDATE());
