/*
SQLyog Community v13.1.7 (64 bit)
MySQL - 8.0.33 : Database - retail_system
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`retail_system` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;

USE `retail_system`;

/*Table structure for table `account_journal` */

DROP TABLE IF EXISTS `account_journal`;

CREATE TABLE `account_journal` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `AccountId` varchar(50) DEFAULT NULL,
  `AccountName` varchar(150) DEFAULT NULL,
  `AccountType` varchar(1) DEFAULT NULL,
  `Description` varchar(150) DEFAULT NULL,
  `Active` bit(1) DEFAULT b'1',
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `account_journal` */

insert  into `account_journal`(`Id`,`AccountId`,`AccountName`,`AccountType`,`Description`,`Active`,`CreateBy`,`CreateDate`,`UpdateBy`,`UpdateDate`) values 
(1,'10011','Pembelian','C','','','System','2023-07-16 13:21:04','System','2023-07-16 13:21:12'),
(2,'20011','Penjualan','D','','','System','2023-07-16 13:21:04','System','2023-07-16 13:21:04');

/*Table structure for table `branch` */

DROP TABLE IF EXISTS `branch`;

CREATE TABLE `branch` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CompanyId` bigint DEFAULT '0',
  `Name` varchar(50) DEFAULT NULL,
  `Address` varchar(500) DEFAULT NULL,
  `Telp` varchar(50) DEFAULT NULL,
  `Fax` varchar(15) DEFAULT NULL,
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `branch` */

insert  into `branch`(`Id`,`CompanyId`,`Name`,`Address`,`Telp`,`Fax`,`CreateBy`,`CreateDate`,`UpdateBy`,`UpdateDate`) values 
(1,1,'Cokokol','Jl. Majaphit','089652104160','123','System',NULL,'System',NULL);

/*Table structure for table `company` */

DROP TABLE IF EXISTS `company`;

CREATE TABLE `company` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) DEFAULT NULL,
  `Address` varchar(500) DEFAULT NULL,
  `Telp` varchar(50) DEFAULT NULL,
  `Fax` varchar(15) DEFAULT NULL,
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `company` */

insert  into `company`(`Id`,`Name`,`Address`,`Telp`,`Fax`,`CreateBy`,`CreateDate`,`UpdateBy`,`UpdateDate`) values 
(1,'LogicSoft','Cikokol-Tangerang','089652104160','123','System',NULL,'System',NULL);

/*Table structure for table `customer` */

DROP TABLE IF EXISTS `customer`;

CREATE TABLE `customer` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CompanyId` bigint DEFAULT '0',
  `BranchId` bigint DEFAULT '0',
  `KTPNumber` varchar(50) DEFAULT NULL,
  `Name` varchar(50) DEFAULT NULL,
  `Address` varchar(500) DEFAULT NULL,
  `Telephone` varchar(50) DEFAULT NULL,
  `WhatsApp` varchar(50) DEFAULT NULL,
  `Email` varchar(50) DEFAULT NULL,
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Name` (`Name`),
  KEY `CompanyId` (`CompanyId`),
  KEY `BranchId` (`BranchId`),
  CONSTRAINT `customer_ibfk_1` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`Id`),
  CONSTRAINT `customer_ibfk_2` FOREIGN KEY (`BranchId`) REFERENCES `branch` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `customer` */

/*Table structure for table `daily_stock` */

DROP TABLE IF EXISTS `daily_stock`;

CREATE TABLE `daily_stock` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CompanyId` bigint DEFAULT '0',
  `BranchId` bigint DEFAULT '0',
  `ProductTypeId` bigint DEFAULT '0',
  `ProductId` bigint DEFAULT '0',
  `StockFirst` bigint DEFAULT '0',
  `StockBuy` bigint DEFAULT '0',
  `StockBuyPrice` bigint DEFAULT '0',
  `StockSell` bigint DEFAULT '0',
  `StockSellPrice` bigint DEFAULT '0',
  `StockLast` bigint DEFAULT '0',
  `StockDate` date NOT NULL,
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `CompanyId` (`CompanyId`),
  KEY `BranchId` (`BranchId`),
  KEY `ProductTypeId` (`ProductTypeId`),
  KEY `ProductId` (`ProductId`),
  KEY `StockDate` (`StockDate`),
  CONSTRAINT `daily_stock_ibfk_1` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`Id`),
  CONSTRAINT `daily_stock_ibfk_2` FOREIGN KEY (`BranchId`) REFERENCES `branch` (`Id`),
  CONSTRAINT `daily_stock_ibfk_3` FOREIGN KEY (`ProductTypeId`) REFERENCES `product_type` (`Id`),
  CONSTRAINT `daily_stock_ibfk_4` FOREIGN KEY (`ProductId`) REFERENCES `product` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `daily_stock` */

/*Table structure for table `log_error` */

DROP TABLE IF EXISTS `log_error`;

CREATE TABLE `log_error` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ServiceName` varchar(50) DEFAULT NULL,
  `ErrorDeskripsi` varchar(500) DEFAULT NULL,
  `ErrorDate` datetime DEFAULT NULL,
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `log_error` */

/*Table structure for table `log_job` */

DROP TABLE IF EXISTS `log_job`;

CREATE TABLE `log_job` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `JobName` varchar(50) DEFAULT NULL,
  `JobStart` datetime DEFAULT NULL,
  `JobFinish` datetime DEFAULT NULL,
  `Success` bit(1) DEFAULT b'0',
  `Error` bit(1) DEFAULT b'0',
  `Active` bit(1) DEFAULT b'1',
  `Description` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `log_job` */

insert  into `log_job`(`Id`,`JobName`,`JobStart`,`JobFinish`,`Success`,`Error`,`Active`,`Description`) values 
(1,'JobSendWhatsApp',NULL,NULL,'\0','\0','',NULL),
(2,'JobSendSMS',NULL,NULL,'\0','\0','',NULL),
(3,'JobSendEmail',NULL,NULL,'\0','\0','',NULL);

/*Table structure for table `menu` */

DROP TABLE IF EXISTS `menu`;

CREATE TABLE `menu` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) DEFAULT NULL,
  `ControllerName` varchar(50) DEFAULT NULL,
  `ActionName` varchar(50) DEFAULT NULL,
  `Description` varchar(500) DEFAULT NULL,
  `IsHeader` bit(1) DEFAULT b'0',
  `Icon` varchar(50) DEFAULT NULL,
  `Active` bit(1) DEFAULT b'1',
  `Sort` int DEFAULT '1',
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Name` (`Name`)
) ENGINE=InnoDB AUTO_INCREMENT=27 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `menu` */

insert  into `menu`(`Id`,`Name`,`ControllerName`,`ActionName`,`Description`,`IsHeader`,`Icon`,`Active`,`Sort`,`CreateBy`,`CreateDate`,`UpdateBy`,`UpdateDate`) values 
(1,'Toko','Company','Index','','\0',NULL,'',1,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09'),
(2,'Cabang','Branch','Index','','\0',NULL,'',2,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09'),
(3,'Master','','','','',NULL,'',3,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09'),
(4,'Menu','Menu','Index','','\0',NULL,'',1,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09'),
(5,'Profil','Profil','Index','','\0',NULL,'',2,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09'),
(6,'Profil Menu','ProfilMenu','Index','','\0',NULL,'',3,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09'),
(7,'User','User','Index','','\0',NULL,'',4,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09'),
(8,'Profil User','ProfilUser','Index','','\0',NULL,'',5,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09'),
(9,'Management','','','','',NULL,'',4,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09'),
(10,'Tipe Produk','ProductType','Index','','\0',NULL,'',6,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09'),
(11,'Produk','Product','Index','','\0',NULL,'',7,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09'),
(12,'Pelanggan','Customer','Index','','\0',NULL,'',8,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09'),
(13,'Supplier','Supplier','Index','','\0',NULL,'',9,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09'),
(14,'Stok Opname','StockOpname','Index','','\0',NULL,'',10,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09'),
(15,'Rekap Stok','','','','',NULL,'',5,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09'),
(16,'Stok Harian','DailyStock','Index','','\0',NULL,'',16,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09'),
(17,'Stok Bulanan','MonthlyStock','Index','','\0',NULL,'',17,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09'),
(18,'Stok Rangking','RankingProduct','IndexRankingStock','','\0',NULL,'',18,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09'),
(19,'Transaksi','','','','',NULL,'',6,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09'),
(20,'Pembelian','PurchaseOrder','Index','','\0',NULL,'',19,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09'),
(21,'Penjualan','SalesOrder','Index','','\0',NULL,'',20,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09'),
(22,'Log Error','LogError','Index','','\0',NULL,'',15,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09'),
(23,'Pesan','Message','Index','','\0',NULL,'',11,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09'),
(25,'Jadwal','Schedule','Index','','\0',NULL,'',12,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09'),
(26,'Kirim WhatsApp','SendWhatsApp','Index','','\0',NULL,'',13,'System','2023-06-25 14:57:09','System','2023-06-25 14:57:09');

/*Table structure for table `message` */

DROP TABLE IF EXISTS `message`;

CREATE TABLE `message` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CompanyId` bigint DEFAULT '0',
  `MessageData` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Active` bit(1) DEFAULT b'1',
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `message` */

/*Table structure for table `monthly_stock` */

DROP TABLE IF EXISTS `monthly_stock`;

CREATE TABLE `monthly_stock` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CompanyId` bigint DEFAULT '0',
  `BranchId` bigint DEFAULT '0',
  `ProductTypeId` bigint DEFAULT '0',
  `ProductId` bigint DEFAULT '0',
  `StockFirst` bigint DEFAULT '0',
  `StockBuy` bigint DEFAULT '0',
  `StockBuyPrice` bigint DEFAULT '0',
  `StockSell` bigint DEFAULT '0',
  `StockSellPrice` bigint DEFAULT '0',
  `StockLast` bigint DEFAULT '0',
  `Year` int DEFAULT '0',
  `Month` int DEFAULT '0',
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `CompanyId` (`CompanyId`),
  KEY `BranchId` (`BranchId`),
  KEY `ProductTypeId` (`ProductTypeId`),
  KEY `ProductId` (`ProductId`),
  KEY `Year` (`Year`),
  KEY `Month` (`Month`),
  CONSTRAINT `monthly_stock_ibfk_1` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`Id`),
  CONSTRAINT `monthly_stock_ibfk_2` FOREIGN KEY (`BranchId`) REFERENCES `branch` (`Id`),
  CONSTRAINT `monthly_stock_ibfk_3` FOREIGN KEY (`ProductTypeId`) REFERENCES `product_type` (`Id`),
  CONSTRAINT `monthly_stock_ibfk_4` FOREIGN KEY (`ProductId`) REFERENCES `product` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `monthly_stock` */

/*Table structure for table `product` */

DROP TABLE IF EXISTS `product`;

CREATE TABLE `product` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ProductNo` varchar(500) DEFAULT NULL,
  `CompanyId` bigint DEFAULT '0',
  `BranchId` bigint DEFAULT '0',
  `ProductTypeId` bigint DEFAULT '0',
  `ProductName` varchar(50) DEFAULT NULL,
  `BuyPrice` bigint DEFAULT '0',
  `SellPrice` bigint DEFAULT '0',
  `PathFile` varchar(500) DEFAULT NULL,
  `Description` varchar(500) DEFAULT NULL,
  `Active` bit(1) DEFAULT b'1',
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `ProductName` (`ProductName`),
  KEY `CompanyId` (`CompanyId`),
  KEY `BranchId` (`BranchId`),
  KEY `ProductTypeId` (`ProductTypeId`),
  CONSTRAINT `product_ibfk_1` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`Id`),
  CONSTRAINT `product_ibfk_2` FOREIGN KEY (`BranchId`) REFERENCES `branch` (`Id`),
  CONSTRAINT `product_ibfk_3` FOREIGN KEY (`ProductTypeId`) REFERENCES `product_type` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `product` */

/*Table structure for table `product_price_rate` */

DROP TABLE IF EXISTS `product_price_rate`;

CREATE TABLE `product_price_rate` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CompanyId` bigint NOT NULL DEFAULT '0',
  `ProductTypeId` bigint NOT NULL DEFAULT '0',
  `ProductId` bigint NOT NULL DEFAULT '0',
  `PriceFrom` bigint DEFAULT '0',
  `PriceTo` bigint DEFAULT '0',
  `PercentPrice` int DEFAULT '0',
  `TotalPrice` bigint DEFAULT '0',
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `CompanyId` (`CompanyId`),
  KEY `ProductTypeId` (`ProductTypeId`),
  KEY `ProductId` (`ProductId`),
  CONSTRAINT `product_price_rate_ibfk_1` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `product_price_rate` */

/*Table structure for table `product_type` */

DROP TABLE IF EXISTS `product_type`;

CREATE TABLE `product_type` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CompanyId` bigint DEFAULT '0',
  `BranchId` bigint DEFAULT '0',
  `ProductTypeName` varchar(50) DEFAULT NULL,
  `Description` varchar(500) DEFAULT NULL,
  `Active` bit(1) DEFAULT NULL,
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `ProductTypeName` (`ProductTypeName`),
  KEY `CompanyId` (`CompanyId`),
  KEY `BranchId` (`BranchId`),
  CONSTRAINT `product_type_ibfk_1` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`Id`),
  CONSTRAINT `product_type_ibfk_2` FOREIGN KEY (`BranchId`) REFERENCES `branch` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `product_type` */

/*Table structure for table `profil` */

DROP TABLE IF EXISTS `profil`;

CREATE TABLE `profil` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) DEFAULT NULL,
  `Description` varchar(500) DEFAULT NULL,
  `Active` bit(1) DEFAULT b'1',
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Name` (`Name`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `profil` */

insert  into `profil`(`Id`,`Name`,`Description`,`Active`,`CreateBy`,`CreateDate`,`UpdateBy`,`UpdateDate`) values 
(1,'Administrator','Admin','','System',NULL,'System',NULL),
(2,'Kasir','Kasir','','System',NULL,'System',NULL),
(3,'Reporting','Report','','System',NULL,'System',NULL),
(4,'Admin','Admin','','System',NULL,'System',NULL);

/*Table structure for table `profil_menu` */

DROP TABLE IF EXISTS `profil_menu`;

CREATE TABLE `profil_menu` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ProfilId` bigint DEFAULT '0',
  `ParentMenuId` bigint DEFAULT '0',
  `MenuId` bigint DEFAULT '0',
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `ParentMenuId` (`ParentMenuId`),
  KEY `MenuId` (`MenuId`),
  KEY `ProfilId` (`ProfilId`),
  CONSTRAINT `profil_menu_ibfk_1` FOREIGN KEY (`ProfilId`) REFERENCES `profil` (`Id`),
  CONSTRAINT `profil_menu_ibfk_2` FOREIGN KEY (`ParentMenuId`) REFERENCES `menu` (`Id`),
  CONSTRAINT `profil_menu_ibfk_3` FOREIGN KEY (`MenuId`) REFERENCES `menu` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=55 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `profil_menu` */

insert  into `profil_menu`(`Id`,`ProfilId`,`ParentMenuId`,`MenuId`,`CreateBy`,`CreateDate`,`UpdateBy`,`UpdateDate`) values 
(2,1,1,1,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(3,1,2,2,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(4,1,3,3,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(5,1,3,4,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(6,1,3,5,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(7,1,3,6,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(8,1,3,7,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(9,1,3,8,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(10,1,9,9,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(11,1,9,10,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(12,1,9,11,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(13,1,9,12,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(14,1,9,13,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(15,1,9,14,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(16,1,15,15,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(17,1,15,16,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(18,1,15,17,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(19,1,15,18,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(20,1,19,19,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(21,1,19,20,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(22,1,19,21,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(23,4,1,1,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(24,4,2,2,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(25,4,3,3,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(26,4,3,7,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(27,4,3,8,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(28,4,9,9,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(29,4,9,10,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(30,4,9,11,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(31,4,9,12,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(32,4,9,13,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(33,4,9,14,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(34,4,15,15,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(35,4,15,16,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(36,4,15,17,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(37,4,15,18,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(38,4,19,19,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(39,4,19,20,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(40,4,19,21,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(41,2,15,15,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(42,2,15,16,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(43,2,15,17,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(44,2,15,18,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(45,2,19,19,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(46,2,19,20,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(47,2,19,21,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(48,1,9,22,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(49,1,9,23,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(50,1,9,25,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(51,1,9,26,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(52,4,9,23,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(53,4,9,25,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46'),
(54,4,9,26,'System','2023-06-25 14:56:46','System','2023-06-25 14:56:46');

/*Table structure for table `profil_user` */

DROP TABLE IF EXISTS `profil_user`;

CREATE TABLE `profil_user` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ProfilId` bigint DEFAULT '0',
  `UserId` bigint DEFAULT '0',
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `ProfilId` (`ProfilId`),
  KEY `UserId` (`UserId`),
  CONSTRAINT `profil_user_ibfk_1` FOREIGN KEY (`ProfilId`) REFERENCES `profil` (`Id`),
  CONSTRAINT `profil_user_ibfk_2` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `profil_user` */

insert  into `profil_user`(`Id`,`ProfilId`,`UserId`,`CreateBy`,`CreateDate`,`UpdateBy`,`UpdateDate`) values 
(1,1,1,'System',NULL,'System',NULL);

/*Table structure for table `purchase_order_detail` */

DROP TABLE IF EXISTS `purchase_order_detail`;

CREATE TABLE `purchase_order_detail` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `PurchaseHeaderId` bigint DEFAULT '0',
  `ProductTypeId` bigint DEFAULT '0',
  `ProductId` bigint DEFAULT '0',
  `Quantity` int DEFAULT NULL,
  `Price` int DEFAULT NULL,
  `Subtotal` bigint DEFAULT '0',
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `PurchaseHeaderId` (`PurchaseHeaderId`),
  KEY `ProductTypeId` (`ProductTypeId`),
  KEY `ProductId` (`ProductId`),
  CONSTRAINT `purchase_order_detail_ibfk_1` FOREIGN KEY (`PurchaseHeaderId`) REFERENCES `purchase_order_header` (`Id`),
  CONSTRAINT `purchase_order_detail_ibfk_2` FOREIGN KEY (`ProductTypeId`) REFERENCES `product_type` (`Id`),
  CONSTRAINT `purchase_order_detail_ibfk_3` FOREIGN KEY (`ProductId`) REFERENCES `product` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `purchase_order_detail` */

/*Table structure for table `purchase_order_header` */

DROP TABLE IF EXISTS `purchase_order_header`;

CREATE TABLE `purchase_order_header` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CompanyId` bigint NOT NULL DEFAULT '0',
  `BranchId` bigint NOT NULL DEFAULT '0',
  `PurchaseNo` varchar(50) DEFAULT NULL,
  `PurchaseDate` datetime NOT NULL,
  `SupplierId` bigint NOT NULL DEFAULT '0',
  `Quantity` bigint DEFAULT '0',
  `Total` bigint DEFAULT '0',
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `CompanyId` (`CompanyId`),
  KEY `BranchId` (`BranchId`),
  KEY `PurchaseNo` (`PurchaseNo`),
  KEY `PurchaseDate` (`PurchaseDate`),
  KEY `SupplierId` (`SupplierId`),
  CONSTRAINT `purchase_order_header_ibfk_1` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`Id`),
  CONSTRAINT `purchase_order_header_ibfk_2` FOREIGN KEY (`BranchId`) REFERENCES `branch` (`Id`),
  CONSTRAINT `purchase_order_header_ibfk_3` FOREIGN KEY (`SupplierId`) REFERENCES `supplier` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `purchase_order_header` */

/*Table structure for table `sales_order_detail` */

DROP TABLE IF EXISTS `sales_order_detail`;

CREATE TABLE `sales_order_detail` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `SalesHeaderId` bigint DEFAULT '0',
  `ProductTypeId` bigint DEFAULT '0',
  `ProductId` bigint DEFAULT '0',
  `Quantity` int DEFAULT '0',
  `Price` int DEFAULT '0',
  `Subtotal` bigint DEFAULT '0',
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `SalesHeaderId` (`SalesHeaderId`),
  KEY `ProductTypeId` (`ProductTypeId`),
  KEY `ProductId` (`ProductId`),
  CONSTRAINT `sales_order_detail_ibfk_1` FOREIGN KEY (`SalesHeaderId`) REFERENCES `sales_order_header` (`Id`),
  CONSTRAINT `sales_order_detail_ibfk_2` FOREIGN KEY (`ProductTypeId`) REFERENCES `product_type` (`Id`),
  CONSTRAINT `sales_order_detail_ibfk_3` FOREIGN KEY (`ProductId`) REFERENCES `product` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `sales_order_detail` */

/*Table structure for table `sales_order_header` */

DROP TABLE IF EXISTS `sales_order_header`;

CREATE TABLE `sales_order_header` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CompanyId` bigint NOT NULL DEFAULT '0',
  `BranchId` bigint NOT NULL DEFAULT '0',
  `InvoiceNo` varchar(50) DEFAULT NULL,
  `SalesOrderDate` datetime NOT NULL,
  `SalesId` bigint DEFAULT '0',
  `CustomerId` bigint NOT NULL DEFAULT '0',
  `Description` varchar(500) DEFAULT NULL,
  `Quantity` int DEFAULT '0',
  `Total` bigint DEFAULT '0',
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `CompanyId` (`CompanyId`),
  KEY `BranchId` (`BranchId`),
  KEY `InvoiceNo` (`InvoiceNo`),
  KEY `SalesOrderDate` (`SalesOrderDate`),
  KEY `CustomerId` (`CustomerId`),
  CONSTRAINT `sales_order_header_ibfk_1` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`Id`),
  CONSTRAINT `sales_order_header_ibfk_2` FOREIGN KEY (`BranchId`) REFERENCES `branch` (`Id`),
  CONSTRAINT `sales_order_header_ibfk_3` FOREIGN KEY (`CustomerId`) REFERENCES `customer` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `sales_order_header` */

/*Table structure for table `schedule` */

DROP TABLE IF EXISTS `schedule`;

CREATE TABLE `schedule` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CompanyId` bigint DEFAULT '0',
  `ScheduleDate` datetime DEFAULT NULL,
  `Active` bit(1) DEFAULT b'1',
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `schedule` */

/*Table structure for table `send_email` */

DROP TABLE IF EXISTS `send_email`;

CREATE TABLE `send_email` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CompanyId` bigint DEFAULT '0',
  `BranchId` bigint DEFAULT '0',
  `CustomerId` bigint DEFAULT '0',
  `ScheduleId` bigint DEFAULT '0',
  `MessageId` bigint DEFAULT '0',
  `Active` bit(1) DEFAULT b'1',
  `Send` bit(1) DEFAULT b'0',
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `CompanyId` (`CompanyId`),
  KEY `BranchId` (`BranchId`),
  KEY `CustomerId` (`CustomerId`),
  KEY `ScheduleId` (`ScheduleId`),
  KEY `ScheduleId_2` (`ScheduleId`,`MessageId`),
  KEY `MessageId` (`MessageId`),
  CONSTRAINT `send_email_ibfk_1` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`Id`),
  CONSTRAINT `send_email_ibfk_2` FOREIGN KEY (`BranchId`) REFERENCES `branch` (`Id`),
  CONSTRAINT `send_email_ibfk_3` FOREIGN KEY (`CustomerId`) REFERENCES `customer` (`Id`),
  CONSTRAINT `send_email_ibfk_4` FOREIGN KEY (`ScheduleId`) REFERENCES `schedule` (`Id`),
  CONSTRAINT `send_email_ibfk_5` FOREIGN KEY (`MessageId`) REFERENCES `message` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `send_email` */

/*Table structure for table `send_email_fail` */

DROP TABLE IF EXISTS `send_email_fail`;

CREATE TABLE `send_email_fail` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Email` varchar(50) DEFAULT NULL,
  `Message` varchar(500) DEFAULT NULL,
  `ErrorDescription` varchar(500) DEFAULT NULL,
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `send_email_fail` */

/*Table structure for table `send_sms` */

DROP TABLE IF EXISTS `send_sms`;

CREATE TABLE `send_sms` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CompanyId` bigint DEFAULT '0',
  `BranchId` bigint DEFAULT '0',
  `CustomerId` bigint DEFAULT '0',
  `ScheduleId` bigint DEFAULT '0',
  `MessageId` bigint DEFAULT '0',
  `Active` bit(1) DEFAULT b'1',
  `Send` bit(1) DEFAULT b'0',
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `CompanyId` (`CompanyId`),
  KEY `BranchId` (`BranchId`),
  KEY `CustomerId` (`CustomerId`),
  KEY `ScheduleId` (`ScheduleId`),
  KEY `MessageId` (`MessageId`),
  CONSTRAINT `send_sms_ibfk_1` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`Id`),
  CONSTRAINT `send_sms_ibfk_2` FOREIGN KEY (`BranchId`) REFERENCES `branch` (`Id`),
  CONSTRAINT `send_sms_ibfk_3` FOREIGN KEY (`CustomerId`) REFERENCES `customer` (`Id`),
  CONSTRAINT `send_sms_ibfk_4` FOREIGN KEY (`ScheduleId`) REFERENCES `schedule` (`Id`),
  CONSTRAINT `send_sms_ibfk_5` FOREIGN KEY (`MessageId`) REFERENCES `message` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `send_sms` */

/*Table structure for table `send_sms_fail` */

DROP TABLE IF EXISTS `send_sms_fail`;

CREATE TABLE `send_sms_fail` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `NoSMS` varchar(50) DEFAULT NULL,
  `Message` varchar(500) DEFAULT NULL,
  `ErrorDescription` varchar(500) DEFAULT NULL,
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `send_sms_fail` */

/*Table structure for table `send_whatsapp` */

DROP TABLE IF EXISTS `send_whatsapp`;

CREATE TABLE `send_whatsapp` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CompanyId` bigint DEFAULT '0',
  `BranchId` bigint DEFAULT '0',
  `CustomerId` bigint DEFAULT '0',
  `ScheduleId` bigint DEFAULT '0',
  `MessageId` bigint DEFAULT '0',
  `Active` bit(1) DEFAULT b'1',
  `Send` bit(1) DEFAULT b'0',
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `CustomerId` (`CustomerId`),
  KEY `ScheduleId` (`ScheduleId`),
  KEY `MessageId` (`MessageId`),
  KEY `CompanyId` (`CompanyId`),
  KEY `BranchId` (`BranchId`),
  CONSTRAINT `send_whatsapp_ibfk_1` FOREIGN KEY (`CustomerId`) REFERENCES `customer` (`Id`),
  CONSTRAINT `send_whatsapp_ibfk_2` FOREIGN KEY (`ScheduleId`) REFERENCES `schedule` (`Id`),
  CONSTRAINT `send_whatsapp_ibfk_3` FOREIGN KEY (`MessageId`) REFERENCES `message` (`Id`),
  CONSTRAINT `send_whatsapp_ibfk_4` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`Id`),
  CONSTRAINT `send_whatsapp_ibfk_5` FOREIGN KEY (`BranchId`) REFERENCES `branch` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `send_whatsapp` */

/*Table structure for table `send_whatsapp_fail` */

DROP TABLE IF EXISTS `send_whatsapp_fail`;

CREATE TABLE `send_whatsapp_fail` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `NoWhatsApp` varchar(50) DEFAULT NULL,
  `Message` varchar(500) DEFAULT NULL,
  `ErrorDescription` varchar(500) DEFAULT NULL,
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `send_whatsapp_fail` */

/*Table structure for table `stock_opname` */

DROP TABLE IF EXISTS `stock_opname`;

CREATE TABLE `stock_opname` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CompanyId` bigint DEFAULT '0',
  `BranchId` bigint DEFAULT '0',
  `ProductTypeId` bigint DEFAULT '0',
  `ProductId` bigint DEFAULT '0',
  `Description` varchar(500) DEFAULT NULL,
  `StockFirst` bigint DEFAULT '0',
  `StockOpnameDefault` bigint DEFAULT '0',
  `StockOpnameDate` date DEFAULT NULL,
  `Year` int DEFAULT '0',
  `Month` int DEFAULT '0',
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `CompanyId` (`CompanyId`),
  KEY `BranchId` (`BranchId`),
  KEY `ProductTypeId` (`ProductTypeId`),
  KEY `ProductId` (`ProductId`),
  CONSTRAINT `stock_opname_ibfk_1` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`Id`),
  CONSTRAINT `stock_opname_ibfk_2` FOREIGN KEY (`BranchId`) REFERENCES `branch` (`Id`),
  CONSTRAINT `stock_opname_ibfk_3` FOREIGN KEY (`ProductTypeId`) REFERENCES `product_type` (`Id`),
  CONSTRAINT `stock_opname_ibfk_4` FOREIGN KEY (`ProductId`) REFERENCES `product` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `stock_opname` */

/*Table structure for table `supplier` */

DROP TABLE IF EXISTS `supplier`;

CREATE TABLE `supplier` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CompanyId` bigint DEFAULT '0',
  `BranchId` bigint DEFAULT '0',
  `Name` varchar(50) DEFAULT NULL,
  `Address` varchar(500) DEFAULT NULL,
  `Telp` varchar(15) DEFAULT NULL,
  `Fax` varchar(15) DEFAULT NULL,
  `Active` bit(1) DEFAULT b'1',
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Name` (`Name`),
  KEY `CompanyId` (`CompanyId`),
  KEY `BranchId` (`BranchId`),
  CONSTRAINT `supplier_ibfk_1` FOREIGN KEY (`CompanyId`) REFERENCES `company` (`Id`),
  CONSTRAINT `supplier_ibfk_2` FOREIGN KEY (`BranchId`) REFERENCES `branch` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `supplier` */

/*Table structure for table `user_token` */

DROP TABLE IF EXISTS `user_token`;

CREATE TABLE `user_token` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `UserId` bigint DEFAULT '0',
  `Token` varchar(500) DEFAULT NULL,
  `TokenExpired` datetime DEFAULT NULL,
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `UserId` (`UserId`),
  CONSTRAINT `user_token_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `user_token` */

insert  into `user_token`(`Id`,`UserId`,`Token`,`TokenExpired`,`CreateBy`,`CreateDate`,`UpdateBy`,`UpdateDate`) values 
(1,1,'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VyTmFtZSI6ImFndXN3aWRpZSIsIk5hbWUiOiJBZ3VzIFdpZGl5YW50byIsIkVtYWlsIjoiYWd1c3dpZGllQGdtYWlsLmNvbSIsIm5iZiI6MTY5MDIzOTczNSwiZXhwIjoxNjkwMjY4NTM1LCJpYXQiOjE2OTAyMzk3MzUsImlzcyI6ImxQOWZIME45Q1lnakt5M0FleHFPTHc9PSIsImF1ZCI6IklyZ3NxaExBRmYzZlNOVnJhODRGdUE9PSJ9.yMuLFqd30Y15bS4Q6V3Q7RwjvSdIEj8ziuaTPwprrpo','2023-07-25 07:02:15','aguswidie','2023-06-25 13:41:25','aguswidie','2023-07-25 06:02:15');

/*Table structure for table `users` */

DROP TABLE IF EXISTS `users`;

CREATE TABLE `users` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `CompanyId` bigint DEFAULT '0',
  `BranchId` bigint DEFAULT '0',
  `UserName` varchar(50) DEFAULT NULL,
  `Name` varchar(50) DEFAULT NULL,
  `Email` varchar(50) DEFAULT NULL,
  `Password` varchar(500) DEFAULT NULL,
  `PasswordExpired` datetime DEFAULT NULL,
  `Description` varchar(500) DEFAULT NULL,
  `Active` bit(1) DEFAULT b'0',
  `LastLogin` datetime DEFAULT NULL,
  `CreateBy` varchar(50) DEFAULT NULL,
  `CreateDate` datetime DEFAULT NULL,
  `UpdateBy` varchar(50) DEFAULT NULL,
  `UpdateDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Data for the table `users` */

insert  into `users`(`Id`,`CompanyId`,`BranchId`,`UserName`,`Name`,`Email`,`Password`,`PasswordExpired`,`Description`,`Active`,`LastLogin`,`CreateBy`,`CreateDate`,`UpdateBy`,`UpdateDate`) values 
(1,1,1,'aguswidie','Agus Widiyanto','aguswidie@gmail.com','M5y+2DtY0Rw9dcNC3JuGRA==','2023-08-31 13:32:07','TEST','',NULL,'System',NULL,'System',NULL);

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
