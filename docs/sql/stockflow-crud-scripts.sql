------------------------------------------------------------
-- 1. CREATE TABLE SCRIPTS
------------------------------------------------------------

CREATE TABLE Products (
    ProductId INTEGER PRIMARY KEY,
    ProductCode TEXT NOT NULL UNIQUE,
    Name TEXT NOT NULL,
    Category TEXT NOT NULL,
    UnitPrice DECIMAL(10,2) NOT NULL,
    QuantityInStock INTEGER NOT NULL,
    ReorderLevel INTEGER NOT NULL,
    IsActive INTEGER NOT NULL
);

CREATE TABLE Orders (
    OrderId INTEGER PRIMARY KEY,
    OrderNumber TEXT NOT NULL UNIQUE,
    OrderDate DATETIME NOT NULL,
    TotalAmount DECIMAL(10,2) NOT NULL,
    OrderStatus TEXT NOT NULL,
    PaymentStatus TEXT NOT NULL
);

CREATE TABLE OrderItems (
    OrderItemId INTEGER PRIMARY KEY,
    OrderId INTEGER NOT NULL,
    ProductId INTEGER NOT NULL,
    ProductCode TEXT NOT NULL,
    ProductName TEXT NOT NULL,
    Quantity INTEGER NOT NULL,
    UnitPrice DECIMAL(10,2) NOT NULL,
    LineTotal DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

CREATE TABLE Payments (
    PaymentId INTEGER PRIMARY KEY,
    PaymentNumber TEXT NOT NULL UNIQUE,
    OrderId INTEGER NOT NULL,
    OrderNumber TEXT NOT NULL,
    PaymentDate DATETIME NOT NULL,
    PaymentMethod TEXT NOT NULL,
    AmountDue DECIMAL(10,2) NOT NULL,
    AmountPaid DECIMAL(10,2) NOT NULL,
    ChangeAmount DECIMAL(10,2) NOT NULL,
    PaymentStatus TEXT NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
);

CREATE TABLE Receipts (
    ReceiptId INTEGER PRIMARY KEY,
    ReceiptNumber TEXT NOT NULL UNIQUE,
    OrderId INTEGER NOT NULL,
    OrderNumber TEXT NOT NULL,
    PaymentId INTEGER NOT NULL,
    PaymentNumber TEXT NOT NULL,
    ReceiptDate DATETIME NOT NULL,
    TotalAmount DECIMAL(10,2) NOT NULL,
    PaymentMethod TEXT NOT NULL,
    AmountPaid DECIMAL(10,2) NOT NULL,
    ChangeAmount DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    FOREIGN KEY (PaymentId) REFERENCES Payments(PaymentId)
);

CREATE TABLE StockMovements (
    StockMovementId INTEGER PRIMARY KEY,
    ProductId INTEGER NOT NULL,
    ProductCode TEXT NOT NULL,
    ProductName TEXT NOT NULL,
    MovementType TEXT NOT NULL,
    QuantityChanged INTEGER NOT NULL,
    StockBefore INTEGER NOT NULL,
    StockAfter INTEGER NOT NULL,
    Reason TEXT NOT NULL,
    MovementDate DATETIME NOT NULL,
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

CREATE TABLE Notifications (
    NotificationId INTEGER PRIMARY KEY,
    NotificationType TEXT NOT NULL,
    Recipient TEXT NOT NULL,
    Subject TEXT NOT NULL,
    Message TEXT NOT NULL,
    CreatedAt DATETIME NOT NULL,
    Status TEXT NOT NULL
);

------------------------------------------------------------
-- 2. CREATE / INSERT SAMPLE RECORDS
------------------------------------------------------------

-- Add a product
INSERT INTO Products (
    ProductId,
    ProductCode,
    Name,
    Category,
    UnitPrice,
    QuantityInStock,
    ReorderLevel,
    IsActive
)
VALUES (
    1,
    'P001',
    'Mouse',
    'Accessories',
    250.00,
    20,
    5,
    1
);

-- Add another product
INSERT INTO Products (
    ProductId,
    ProductCode,
    Name,
    Category,
    UnitPrice,
    QuantityInStock,
    ReorderLevel,
    IsActive
)
VALUES (
    2,
    'P002',
    'Keyboard',
    'Accessories',
    750.00,
    10,
    3,
    1
);

-- Create an order
INSERT INTO Orders (
    OrderId,
    OrderNumber,
    OrderDate,
    TotalAmount,
    OrderStatus,
    PaymentStatus
)
VALUES (
    1,
    'ORD-001',
    '2026-08-30 16:40:00',
    500.00,
    'Pending Payment',
    'Unpaid'
);

-- Add item to order
INSERT INTO OrderItems (
    OrderItemId,
    OrderId,
    ProductId,
    ProductCode,
    ProductName,
    Quantity,
    UnitPrice,
    LineTotal
)
VALUES (
    1,
    1,
    1,
    'P001',
    'Mouse',
    2,
    250.00,
    500.00
);

-- Create payment
INSERT INTO Payments (
    PaymentId,
    PaymentNumber,
    OrderId,
    OrderNumber,
    PaymentDate,
    PaymentMethod,
    AmountDue,
    AmountPaid,
    ChangeAmount,
    PaymentStatus
)
VALUES (
    1,
    'PAY-001',
    1,
    'ORD-001',
    '2026-08-30 16:45:00',
    'Cash',
    500.00,
    1000.00,
    500.00,
    'Paid'
);

-- Create receipt
INSERT INTO Receipts (
    ReceiptId,
    ReceiptNumber,
    OrderId,
    OrderNumber,
    PaymentId,
    PaymentNumber,
    ReceiptDate,
    TotalAmount,
    PaymentMethod,
    AmountPaid,
    ChangeAmount
)
VALUES (
    1,
    'RCT-001',
    1,
    'ORD-001',
    1,
    'PAY-001',
    '2026-08-30 16:46:00',
    500.00,
    'Cash',
    1000.00,
    500.00
);

-- Record stock movement
INSERT INTO StockMovements (
    StockMovementId,
    ProductId,
    ProductCode,
    ProductName,
    MovementType,
    QuantityChanged,
    StockBefore,
    StockAfter,
    Reason,
    MovementDate
)
VALUES (
    1,
    1,
    'P001',
    'Mouse',
    'Stock Out',
    -2,
    20,
    18,
    'Sold through order ORD-001',
    '2026-08-30 16:41:00'
);

-- Create notification
INSERT INTO Notifications (
    NotificationId,
    NotificationType,
    Recipient,
    Subject,
    Message,
    CreatedAt,
    Status
)
VALUES (
    1,
    'Order Completed',
    'business-owner@example.com',
    'Order Completed - ORD-001',
    'Order ORD-001 has been completed.',
    '2026-08-30 16:47:00',
    'Simulated'
);

------------------------------------------------------------
-- 3. READ / SELECT SCRIPTS
------------------------------------------------------------

-- View all products
SELECT *
FROM Products;

-- View active products only
SELECT *
FROM Products
WHERE IsActive = 1;

-- Search product by product code
SELECT *
FROM Products
WHERE ProductCode = 'P001';

-- Search product by name
SELECT *
FROM Products
WHERE Name LIKE '%Mouse%';

-- View low-stock active products
SELECT *
FROM Products
WHERE IsActive = 1
AND QuantityInStock <= ReorderLevel;

-- View all orders
SELECT *
FROM Orders;

-- View completed orders
SELECT *
FROM Orders
WHERE OrderStatus = 'Completed';

-- View order items for one order
SELECT *
FROM OrderItems
WHERE OrderId = 1;

-- View payments
SELECT *
FROM Payments;

-- View receipts
SELECT *
FROM Receipts;

-- View stock movements for one product
SELECT *
FROM StockMovements
WHERE ProductId = 1;

-- View notifications
SELECT *
FROM Notifications;

------------------------------------------------------------
-- 4. UPDATE SCRIPTS
------------------------------------------------------------

-- Update product price
UPDATE Products
SET UnitPrice = 300.00
WHERE ProductCode = 'P001';

-- Update product stock quantity
UPDATE Products
SET QuantityInStock = 25
WHERE ProductCode = 'P001';

-- Deactivate product
UPDATE Products
SET IsActive = 0
WHERE ProductCode = 'P001';

-- Reactivate product
UPDATE Products
SET IsActive = 1
WHERE ProductCode = 'P001';

-- Mark order as completed and paid
UPDATE Orders
SET OrderStatus = 'Completed',
    PaymentStatus = 'Paid'
WHERE OrderNumber = 'ORD-001';

-- Update notification status
UPDATE Notifications
SET Status = 'Sent'
WHERE NotificationId = 1;

------------------------------------------------------------
-- 5. DELETE SCRIPTS
------------------------------------------------------------

-- Hard delete a notification
DELETE FROM Notifications
WHERE NotificationId = 1;

-- Hard delete a product by product code
-- Use carefully.
-- Prefer deactivation if the product has transaction history.
DELETE FROM Products
WHERE ProductCode = 'P001';

------------------------------------------------------------
-- 6. REPORTING QUERY EXAMPLES
------------------------------------------------------------

-- Total sales income
SELECT SUM(AmountDue) AS TotalSalesIncome
FROM Payments
WHERE PaymentStatus = 'Paid';

-- Total cash received
SELECT SUM(AmountPaid) AS TotalCashReceived
FROM Payments
WHERE PaymentStatus = 'Paid';

-- Total change given
SELECT SUM(ChangeAmount) AS TotalChangeGiven
FROM Payments
WHERE PaymentStatus = 'Paid';

-- Sales by payment method
SELECT PaymentMethod,
       COUNT(*) AS PaymentCount,
       SUM(AmountDue) AS TotalSales
FROM Payments
WHERE PaymentStatus = 'Paid'
GROUP BY PaymentMethod;

-- Product stock movement history
SELECT ProductCode,
       ProductName,
       MovementType,
       QuantityChanged,
       StockBefore,
       StockAfter,
       Reason,
       MovementDate
FROM StockMovements
WHERE ProductCode = 'P001';