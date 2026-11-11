INSERT INTO communication."EmailAudit"
(
    "ItemType",
    "ItemTypeRef",
    "From",
    "To",
    "CC",
    "Subject",
    "Body",
    "HasAttachments",
    "IsSent",
    "SentDate",
    "IsActive",
    "CreatedBy",
    "CreatedDate",
    "ModifiedBy",
    "ModifiedDate"
)
SELECT
    'Invoice' AS "ItemType",

    number AS "ItemTypeRef",

    'accounts@invoicetracker.com' AS "From",

    'customer' || number || '@email.com' AS "To",

    NULL AS "CC",

    'Invoice Statement INV-202611'
        || LPAD(number::text, 2, '0') AS "Subject",

    '<html>'
        || '<body>'
        || '<h2>Invoice Statement</h2>'
        || '<p>Dear Customer,</p>'
        || '<p>Please find your invoice statement INV-202611'
        || LPAD(number::text, 2, '0')
        || ' attached.</p>'
        || '<p>Kind Regards,<br/>Accounts Team</p>'
        || '</body>'
        || '</html>' AS "Body",

    CASE
        WHEN number IN (4, 7, 11, 15, 19)
            THEN false
        ELSE true
    END AS "HasAttachments",

    true AS "IsSent",

    CURRENT_TIMESTAMP
        - ((number - 1) * INTERVAL '1 hour') AS "SentDate",

    true AS "IsActive",

    'SYSTEM' AS "CreatedBy",

    CURRENT_TIMESTAMP AS "CreatedDate",

    NULL AS "ModifiedBy",

    NULL AS "ModifiedDate"

FROM generate_series(1, 21) AS invoice(number)

RETURNING
    "EmailAuditId",
    "ItemTypeRef",
    "To",
    "Subject",
    "HasAttachments",
    "SentDate";