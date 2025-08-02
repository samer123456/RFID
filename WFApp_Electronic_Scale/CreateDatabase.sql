-- =============================================
-- إنشاء قاعدة بيانات الميزان الإلكتروني
-- =============================================

-- إنشاء قاعدة البيانات
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'WeightScaleDB')
BEGIN
    CREATE DATABASE WeightScaleDB;
    PRINT 'تم إنشاء قاعدة البيانات WeightScaleDB بنجاح';
END
ELSE
BEGIN
    PRINT 'قاعدة البيانات WeightScaleDB موجودة بالفعل';
END
GO

-- استخدام قاعدة البيانات
USE WeightScaleDB;
GO

-- إنشاء جدول الأوزان
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Weights' AND xtype='U')
BEGIN
    CREATE TABLE Weights (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Weight DECIMAL(10,3) NOT NULL,
        WeightUnit NVARCHAR(10) DEFAULT 'KG',
        ReadingTime DATETIME DEFAULT GETDATE(),
        UserId NVARCHAR(50),
        UserName NVARCHAR(100),
        City NVARCHAR(100),
        Notes NVARCHAR(500)
    );
    
    PRINT 'تم إنشاء جدول Weights بنجاح';
END
ELSE
BEGIN
    PRINT 'جدول Weights موجود بالفعل';
END
GO

-- إنشاء فهرس على عمود ReadingTime لتحسين الأداء
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Weights_ReadingTime')
BEGIN
    CREATE INDEX IX_Weights_ReadingTime ON Weights(ReadingTime);
    PRINT 'تم إنشاء فهرس على عمود ReadingTime';
END
GO

-- إنشاء فهرس على عمود UserId
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Weights_UserId')
BEGIN
    CREATE INDEX IX_Weights_UserId ON Weights(UserId);
    PRINT 'تم إنشاء فهرس على عمود UserId';
END
GO

-- إنشاء مستخدم للتطبيق (اختياري)
-- يمكنك تعديل اسم المستخدم وكلمة المرور حسب احتياجاتك
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'WeightScaleUser')
BEGIN
    CREATE LOGIN WeightScaleUser WITH PASSWORD = 'YourStrongPassword123!';
    PRINT 'تم إنشاء مستخدم WeightScaleUser';
END
GO

-- إنشاء مستخدم في قاعدة البيانات
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'WeightScaleUser')
BEGIN
    CREATE USER WeightScaleUser FOR LOGIN WeightScaleUser;
    PRINT 'تم إنشاء مستخدم قاعدة البيانات WeightScaleUser';
END
GO

-- منح الصلاحيات للمستخدم
GRANT SELECT, INSERT, UPDATE, DELETE ON Weights TO WeightScaleUser;
PRINT 'تم منح الصلاحيات للمستخدم WeightScaleUser';
GO

-- إنشاء إجراء مخزن للحصول على إحصائيات الأوزان
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetWeightStatistics')
BEGIN
    EXEC('
    CREATE PROCEDURE GetWeightStatistics
        @FromDate DATETIME = NULL,
        @ToDate DATETIME = NULL,
        @UserId NVARCHAR(50) = NULL
    AS
    BEGIN
        SET NOCOUNT ON;
        
        SELECT 
            COUNT(*) AS TotalReadings,
            AVG(Weight) AS AverageWeight,
            MIN(Weight) AS MinWeight,
            MAX(Weight) AS MaxWeight,
            COUNT(DISTINCT UserId) AS UniqueUsers,
            COUNT(DISTINCT City) AS UniqueCities
        FROM Weights
        WHERE (@FromDate IS NULL OR ReadingTime >= @FromDate)
            AND (@ToDate IS NULL OR ReadingTime <= @ToDate)
            AND (@UserId IS NULL OR UserId = @UserId);
    END
    ');
    PRINT 'تم إنشاء إجراء مخزن GetWeightStatistics';
END
GO

-- إنشاء إجراء مخزن لحذف السجلات القديمة
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'CleanOldWeights')
BEGIN
    EXEC('
    CREATE PROCEDURE CleanOldWeights
        @DaysToKeep INT = 365
    AS
    BEGIN
        SET NOCOUNT ON;
        
        DECLARE @CutoffDate DATETIME = DATEADD(DAY, -@DaysToKeep, GETDATE());
        
        DELETE FROM Weights 
        WHERE ReadingTime < @CutoffDate;
        
        PRINT ''تم حذف السجلات الأقدم من '' + CAST(@DaysToKeep AS VARCHAR) + '' يوم'';
    END
    ');
    PRINT 'تم إنشاء إجراء مخزن CleanOldWeights';
END
GO

-- إدراج بيانات تجريبية (اختياري)
-- يمكنك حذف هذا الجزء إذا كنت لا تريد بيانات تجريبية
IF NOT EXISTS (SELECT * FROM Weights)
BEGIN
    INSERT INTO Weights (Weight, WeightUnit, ReadingTime, UserId, UserName, City, Notes)
    VALUES 
        (75.5, 'KG', DATEADD(DAY, -1, GETDATE()), 'admin', 'مدير النظام', 'الرياض', 'قراءة تجريبية'),
        (82.3, 'KG', DATEADD(HOUR, -2, GETDATE()), 'user', 'مستخدم عادي', 'جدة', 'قراءة تجريبية'),
        (68.7, 'KG', DATEADD(HOUR, -1, GETDATE()), 'admin', 'مدير النظام', 'الرياض', 'قراءة تجريبية');
    
    PRINT 'تم إدراج بيانات تجريبية';
END
GO

PRINT 'تم إكمال إعداد قاعدة البيانات بنجاح!';
PRINT 'يمكنك الآن تشغيل التطبيق والاتصال بقاعدة البيانات.';
GO 