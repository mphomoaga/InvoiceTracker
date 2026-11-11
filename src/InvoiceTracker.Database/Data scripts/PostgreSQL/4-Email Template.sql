
<html lang="en">
<head>
    <meta charset="UTF-8" />

    <style>
        @page {
            size: A4;
            margin: 15mm;
        }

        :root {
            --primary: #5b66f6;
            --primary-dark: #4550dc;
            --primary-soft: #eef0ff;
            --ink: #20233a;
            --muted: #777d91;
            --line: #e9ebf2;
            --surface: #ffffff;
            --canvas: #f6f7fb;
        }

        * {
            box-sizing: border-box;
        }

        body {
            margin: 0;
            padding: 0;
            color: var(--ink);
            background: var(--canvas);
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            line-height: 1.5;
        }

        .invoice {
            width: 100%;
            max-width: 800px;
            margin: 0 auto;
            padding: 28px;
            background: var(--surface);
            border: 1px solid var(--line);
            border-radius: 14px;
        }

        .header {
            display: table;
            width: 100%;
            padding-bottom: 20px;
            border-bottom: 2px solid var(--primary);
        }

        .company,
        .invoice-heading {
            display: table-cell;
            width: 50%;
            vertical-align: top;
        }

        .invoice-heading {
            text-align: right;
        }

        .company-brand {
            display: table;
            margin-bottom: 12px;
        }

        .company-icon,
        .company-title {
            display: table-cell;
            vertical-align: middle;
        }

        .company-icon {
            width: 48px;
            height: 48px;
            text-align: center;
            color: var(--primary);
            background: var(--primary-soft);
            border: 1px solid var(--primary);
            border-radius: 12px;
        }

        .company-icon svg {
            display: block;
            width: 25px;
            height: 25px;
            margin: 11px auto 0;
            color: var(--primary);
        }

        .company-title {
            padding-left: 12px;
        }

        .company-label {
            margin: 3px 0 0;
            color: var(--primary);
            font-size: 10px;
            font-weight: bold;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }

        .muted {
            margin: 0;
            color: var(--muted);
        }

        .invoice-title {
            margin: 0;
            color: var(--primary);
            font-size: 32px;
            letter-spacing: 1px;
        }

        .invoice-number {
            margin-top: 5px;
            color: var(--ink);
            font-size: 15px;
            font-weight: bold;
        }

        .details {
            display: table;
            width: 100%;
            margin: 24px 0;
        }

        .customer,
        .invoice-dates {
            display: table-cell;
            width: 50%;
            vertical-align: top;
        }

        .invoice-dates {
            text-align: right;
        }

        .section-title {
            margin: 0 0 8px;
            color: var(--primary);
            font-size: 11px;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }

        .date-table {
            display: inline-table;
            min-width: 240px;
            border-collapse: collapse;
        }

        .date-table td {
            padding: 3px 0 3px 15px;
        }

        .date-table td:first-child {
            color: var(--muted);
            font-weight: bold;
            text-align: left;
        }

        .date-table td:last-child {
            color: var(--ink);
            text-align: right;
        }

        .line-items {
            width: 100%;
            border-collapse: collapse;
        }

        .line-items thead {
            display: table-header-group;
        }

        .line-items tr {
            page-break-inside: avoid;
        }

        .line-items th {
            padding: 11px 10px;
            color: #ffffff;
            background: var(--primary);
            font-size: 10px;
            text-align: left;
            text-transform: uppercase;
        }

        .line-items td {
            padding: 11px 10px;
            border-bottom: 1px solid var(--line);
            vertical-align: top;
        }

        .line-items tbody tr:nth-child(even) {
            background: #fbfbfd;
        }

        .description {
            width: 60%;
        }

        .quantity {
            width: 15%;
        }

        .amount {
            width: 25%;
        }

        .text-center {
            text-align: center !important;
        }

        .text-right {
            text-align: right !important;
        }

        .summary {
            display: table;
            width: 100%;
            margin-top: 20px;
            page-break-inside: avoid;
        }

        .notes,
        .totals {
            display: table-cell;
            vertical-align: top;
        }

        .notes {
            width: 55%;
            padding-right: 30px;
        }

        .totals {
            width: 45%;
        }

        .notes-box {
            padding: 12px;
            color: var(--muted);
            background: var(--canvas);
            border: 1px solid var(--line);
            border-radius: 10px;
        }

        .notes-box p {
            margin: 0;
        }

        .totals-table {
            width: 100%;
            border-collapse: collapse;
        }

        .totals-table td {
            padding: 7px 8px;
        }

        .totals-table td:first-child {
            color: var(--muted);
            font-weight: bold;
        }

        .totals-table td:last-child {
            text-align: right;
        }

        .amount-due td {
            color: var(--primary-dark) !important;
            background: var(--primary-soft);
            font-weight: bold;
        }

        .payment-details {
            margin-top: 22px;
            padding: 14px;
            background: var(--primary-soft);
            border-left: 4px solid var(--primary);
            border-radius: 8px;
            page-break-inside: avoid;
        }

        .payment-details h3 {
            margin: 0 0 8px;
            color: var(--primary-dark);
            font-size: 12px;
        }

        .payment-grid {
            display: table;
            width: 100%;
        }

        .payment-column {
            display: table-cell;
            width: 50%;
            vertical-align: top;
        }

        .payment-column p {
            margin: 3px 0;
        }

        .label {
            display: inline-block;
            min-width: 105px;
            font-weight: bold;
        }
    </style>
</head>

<body>
    <main class="invoice">

        <section class="header">
            <div class="company">
                <div class="company-brand">
                    <div class="company-icon">
                        <svg
                            viewBox="0 0 24 24"
                            fill="none"
                            xmlns="http://www.w3.org/2000/svg"
                            aria-hidden="true">

                            <path
                                d="M7 3H14L18 7V21H7V3Z"
                                stroke="currentColor"
                                stroke-width="2"
                                stroke-linejoin="round" />

                            <path
                                d="M14 3V8H18"
                                stroke="currentColor"
                                stroke-width="2"
                                stroke-linejoin="round" />

                            <path
                                d="M10 12H15"
                                stroke="currentColor"
                                stroke-width="2"
                                stroke-linecap="round" />

                            <path
                                d="M10 16H15"
                                stroke="currentColor"
                                stroke-width="2"
                                stroke-linecap="round" />
                        </svg>
                    </div>

                    <div class="company-title">
                        <h1 class="company-label">
                            Invoice Tracker
                        </h1>
                    </div>
                </div>
            </div>

            <div class="invoice-heading">
                <h2 class="invoice-title">INVOICE</h2>

                <div class="invoice-number">
                    [InvoiceNumber]
                </div>
            </div>
        </section>

        <section class="details">
            <div class="customer">
                <h3 class="section-title">Bill To</h3>

                <p class="muted">
                    Customer Name: [CustomerName]<br />
                    Customer ID Number: [CustomerIdNumber]
                </p>
            </div>

            <div class="invoice-dates">
                <table class="date-table">
                    <tr>
                        <td>Invoice date</td>
                        <td>[InvoiceDate]</td>
                    </tr>

                    <tr>
                        <td>Due date</td>
                        <td>[DueDate]</td>
                    </tr>
                </table>
            </div>
        </section>

        <table class="line-items">
            <thead>
                <tr>
                    <th class="description">Description</th>
                    <th class="quantity text-center">Qty</th>
                    <th class="amount text-right">Amount</th>
                </tr>
            </thead>

            <tbody>
                <tr>
                    <td>[Description]</td>
                    <td class="quantity text-center">1</td>
                    <td class="amount text-right">[Amount]</td>
                </tr>
            </tbody>
        </table>

        <section class="summary">
            <div class="notes">
                <h3 class="section-title">Notes</h3>

                <div class="notes-box">
                    <p>[InvoiceNotes]</p>
                </div>
            </div>

            <div class="totals">
                <table class="totals-table">
                    <tr>
                        <td>Discount</td>
                        <td>0%</td>
                    </tr>

                    <tr class="amount-due">
                        <td>Amount due</td>
                        <td>[AmountDue]</td>
                    </tr>
                </table>
            </div>
        </section>

        <section class="payment-details">
            <h3>Payment Details</h3>

            <div class="payment-grid">
                <div class="payment-column">
                    <p>
                        <span class="label">Bank:</span>
                        Demo Bank
                    </p>

                    <p>
                        <span class="label">Account holder:</span>
                        InvoiceTracker Demo (Pty) Ltd
                    </p>

                    <p>
                        <span class="label">Account number:</span>
                        1234567890
                    </p>
                </div>

                <div class="payment-column">
                    <p>
                        <span class="label">Account type:</span>
                        Business Cheque
                    </p>

                    <p>
                        <span class="label">Branch code:</span>
                        470010
                    </p>

                    <p>
                        <span class="label">Reference:</span>
                        [InvoiceNumber]
                    </p>
                </div>
            </div>
        </section>

    </main>
</body>
</html>
