# إعداد قاعدة البيانات SQL Server

## المتطلبات الأساسية

1. **SQL Server** - يمكنك استخدام:
   - SQL Server Express (مجاني)
   - SQL Server Developer Edition
   - SQL Server Standard/Enterprise

2. **SQL Server Management Studio (SSMS)** - لإدارة قاعدة البيانات

## خطوات الإعداد

### 1. تثبيت SQL Server
- قم بتحميل SQL Server Express من موقع Microsoft الرسمي
- اتبع خطوات التثبيت
- تأكد من تفعيل خدمة SQL Server

### 2. إنشاء قاعدة البيانات
```sql
-- إنشاء قاعدة البيانات
CREATE DATABASE WeightScaleDB;
GO

-- استخدام قاعدة البيانات
USE WeightScaleDB;
GO

-- إنشاء جدول الأوزان
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
```

### 3. إعداد المستخدم والصلاحيات

#### باستخدام SQL Authentication:
```sql
-- إنشاء مستخدم جديد
CREATE LOGIN WeightScaleUser WITH PASSWORD = 'YourStrongPassword123!';
GO

-- إنشاء مستخدم في قاعدة البيانات
USE WeightScaleDB;
CREATE USER WeightScaleUser FOR LOGIN WeightScaleUser;
GO

-- منح الصلاحيات
GRANT SELECT, INSERT, UPDATE, DELETE ON Weights TO WeightScaleUser;
GO
```

#### باستخدام Windows Authentication:
- تأكد من أن حساب Windows لديه صلاحيات الوصول لقاعدة البيانات

### 4. تعديل إعدادات الاتصال في التطبيق

في ملف `DatabaseManager.cs`، قم بتعديل معلومات الاتصال:

```csharp
// لـ SQL Authentication
public DatabaseManager(string serverName = "localhost", string databaseName = "WeightScaleDB", 
    string username = "WeightScaleUser", string password = "YourStrongPassword123!")

// لـ Windows Authentication
public DatabaseManager(string serverName = "localhost", string databaseName = "WeightScaleDB")
{
    connectionString = $"Server={serverName};Database={databaseName};Integrated Security=true;";
}
```

### 5. اختبار الاتصال

1. شغل التطبيق
2. تأكد من ظهور رسالة "تم الاتصال بقاعدة البيانات بنجاح" في السجل
3. إذا ظهرت رسالة خطأ، تحقق من:
   - اسم الخادم
   - اسم قاعدة البيانات
   - اسم المستخدم وكلمة المرور
   - إعدادات الجدار الناري

## استكشاف الأخطاء

### خطأ الاتصال:
- تأكد من تشغيل خدمة SQL Server
- تحقق من إعدادات الجدار الناري
- تأكد من صحة معلومات الاتصال

### خطأ في إنشاء الجدول:
- تأكد من وجود صلاحيات CREATE TABLE
- تحقق من وجود مساحة كافية في قاعدة البيانات

### خطأ في حفظ البيانات:
- تأكد من صحة تنسيق البيانات
- تحقق من وجود صلاحيات INSERT

## الميزات المتاحة

1. **حفظ الوزن تلقائياً** - عند قراءة الوزن من الميزان
2. **عرض سجل الأوزان** - مع إمكانية التصفية حسب التاريخ
3. **حذف السجلات** - للمستخدمين المصرح لهم
4. **تصدير البيانات** - يمكن إضافة هذه الميزة لاحقاً

## ملاحظات مهمة

- احتفظ بنسخة احتياطية من قاعدة البيانات بانتظام
- استخدم كلمات مرور قوية
- راقب حجم قاعدة البيانات لتجنب امتلاء القرص
- قم بتحديث إعدادات الأمان حسب سياسات الشركة 