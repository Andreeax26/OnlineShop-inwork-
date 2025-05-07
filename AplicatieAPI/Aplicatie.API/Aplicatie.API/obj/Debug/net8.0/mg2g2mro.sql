CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Discounts` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Desc` longtext CHARACTER SET utf8mb4 NOT NULL,
    `DiscountPercent` decimal(65,30) NOT NULL,
    `Active` tinyint(1) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `ModifiedAt` datetime(6) NOT NULL,
    `DeletedAt` datetime(6) NULL,
    CONSTRAINT `PK_Discounts` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ProductCategories` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Desc` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `ModifiedAt` datetime(6) NOT NULL,
    `DeletedAt` datetime(6) NULL,
    CONSTRAINT `PK_ProductCategories` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ProductInventories` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Quantity` int NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `ModifiedAt` datetime(6) NOT NULL,
    `DeletedAt` datetime(6) NULL,
    CONSTRAINT `PK_ProductInventories` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Users` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Username` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Password` longtext CHARACTER SET utf8mb4 NOT NULL,
    `FirstName` longtext CHARACTER SET utf8mb4 NOT NULL,
    `LastName` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Telephone` int NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `ModifiedAt` datetime(6) NOT NULL,
    CONSTRAINT `PK_Users` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Products` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Desc` longtext CHARACTER SET utf8mb4 NOT NULL,
    `SKU` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CategoryId` int NOT NULL,
    `InventoryId` int NOT NULL,
    `DiscountId` int NULL,
    `Price` decimal(65,30) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `ModifiedAt` datetime(6) NOT NULL,
    `DeletedAt` datetime(6) NULL,
    CONSTRAINT `PK_Products` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Products_Discounts_DiscountId` FOREIGN KEY (`DiscountId`) REFERENCES `Discounts` (`Id`),
    CONSTRAINT `FK_Products_ProductCategories_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `ProductCategories` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Products_ProductInventories_InventoryId` FOREIGN KEY (`InventoryId`) REFERENCES `ProductInventories` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `OrdersDetails` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` int NOT NULL,
    `PaymentId` int NOT NULL,
    `Total` decimal(65,30) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `ModifiedAt` datetime(6) NOT NULL,
    CONSTRAINT `PK_OrdersDetails` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_OrdersDetails_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `ShoppingSessions` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` int NOT NULL,
    `Total` decimal(65,30) NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `ModifiedAt` datetime(6) NOT NULL,
    CONSTRAINT `PK_ShoppingSessions` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_ShoppingSessions_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `UserAddresses` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` int NOT NULL,
    `AddressLine1` longtext CHARACTER SET utf8mb4 NOT NULL,
    `AddressLine2` longtext CHARACTER SET utf8mb4 NOT NULL,
    `City` longtext CHARACTER SET utf8mb4 NOT NULL,
    `PostalCode` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Country` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Telephone` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Mobile` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_UserAddresses` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_UserAddresses_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `UserPayments` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `UserId` int NOT NULL,
    `PaymentType` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Provider` longtext CHARACTER SET utf8mb4 NOT NULL,
    `AccountNo` int NOT NULL,
    `Expiry` datetime(6) NOT NULL,
    CONSTRAINT `PK_UserPayments` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_UserPayments_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `OrderItems` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `OrderId` int NOT NULL,
    `ProductId` int NOT NULL,
    `Quantity` int NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `ModifiedAt` datetime(6) NOT NULL,
    CONSTRAINT `PK_OrderItems` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_OrderItems_OrdersDetails_OrderId` FOREIGN KEY (`OrderId`) REFERENCES `OrdersDetails` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_OrderItems_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `Products` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `PaymentDetails` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `OrderId` int NOT NULL,
    `Amount` decimal(65,30) NOT NULL,
    `Provider` longtext CHARACTER SET utf8mb4 NOT NULL,
    `Status` longtext CHARACTER SET utf8mb4 NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `ModifiedAt` datetime(6) NOT NULL,
    CONSTRAINT `PK_PaymentDetails` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_PaymentDetails_OrdersDetails_OrderId` FOREIGN KEY (`OrderId`) REFERENCES `OrdersDetails` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `CartItems` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `SessionId` int NOT NULL,
    `ProductId` int NOT NULL,
    `Quantity` int NOT NULL,
    `CreatedAt` datetime(6) NOT NULL,
    `ModifiedAt` datetime(6) NOT NULL,
    CONSTRAINT `PK_CartItems` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_CartItems_Products_ProductId` FOREIGN KEY (`ProductId`) REFERENCES `Products` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_CartItems_ShoppingSessions_SessionId` FOREIGN KEY (`SessionId`) REFERENCES `ShoppingSessions` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_CartItems_ProductId` ON `CartItems` (`ProductId`);

CREATE INDEX `IX_CartItems_SessionId` ON `CartItems` (`SessionId`);

CREATE INDEX `IX_OrderItems_OrderId` ON `OrderItems` (`OrderId`);

CREATE INDEX `IX_OrderItems_ProductId` ON `OrderItems` (`ProductId`);

CREATE INDEX `IX_OrdersDetails_UserId` ON `OrdersDetails` (`UserId`);

CREATE UNIQUE INDEX `IX_PaymentDetails_OrderId` ON `PaymentDetails` (`OrderId`);

CREATE INDEX `IX_Products_CategoryId` ON `Products` (`CategoryId`);

CREATE INDEX `IX_Products_DiscountId` ON `Products` (`DiscountId`);

CREATE INDEX `IX_Products_InventoryId` ON `Products` (`InventoryId`);

CREATE INDEX `IX_ShoppingSessions_UserId` ON `ShoppingSessions` (`UserId`);

CREATE INDEX `IX_UserAddresses_UserId` ON `UserAddresses` (`UserId`);

CREATE INDEX `IX_UserPayments_UserId` ON `UserPayments` (`UserId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250315160705_InitialCreate', '8.0.13');

COMMIT;

START TRANSACTION;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250315185059_UpdateRelationships', '8.0.13');

COMMIT;

