# دليل البدء السريع

## الخطوات الأساسية لتشغيل التطبيق

### 1. إعداد قاعدة البيانات
```sql
-- شغل ملف CreateDatabase.sql في SQL Server Management Studio
-- أو نفذ الأوامر التالية يدوياً:

CREATE DATABASE WeightScaleDB;
GO
USE WeightScaleDB;
GO
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

### 2. تعديل إعدادات الاتصال
في ملف `DatabaseSettings.cs`، عدّل:
```csharp
public static string ServerName { get; set; } = "localhost"; // اسم خادم SQL
public static string DatabaseName { get; set; } = "WeightScaleDB";
public static string Username { get; set; } = "sa"; // أو اسم المستخدم الخاص بك
public static string Password { get; set; } = "your_password"; // كلمة المرور
```

### 3. تشغيل التطبيق
1. افتح المشروع في Visual Studio
2. اضغط F5 أو زر Start
3. سجل دخولك باستخدام:
   - **المدير**: admin / admin123
   - **المستخدم**: user / user123

### 4. توصيل الميزان
1. تأكد من توصيل الميزان بالكمبيوتر
2. حدد منفذ COM الصحيح في التطبيق
3. اضغط "Open Port"

### 5. قراءة وحفظ الوزن
- سيتم قراءة الوزن تلقائياً
- سيتم حفظه في قاعدة البيانات
- اضغط "سجل الأوزان" لعرض السجل

## استكشاف الأخطاء الشائعة

### خطأ في الاتصال بقاعدة البيانات
- تأكد من تشغيل SQL Server
- تحقق من إعدادات الجدار الناري
- تأكد من صحة اسم المستخدم وكلمة المرور

### خطأ في الاتصال بالميزان
- تأكد من توصيل الميزان
- تحقق من منفذ COM
- تأكد من تشغيل برنامج تشغيل الميزان

### الوزن لا يتم حفظه
- تحقق من إعداد "AutoSaveWeight" في DatabaseSettings
- تأكد من وجود صلاحيات INSERT في قاعدة البيانات

## بيانات تسجيل الدخول الافتراضية
- **المدير**: admin / admin123
- **المستخدم**: user / user123

## الملفات المهمة
- `Database_Setup.md` - دليل مفصل لإعداد قاعدة البيانات
- `CreateDatabase.sql` - سكريبت إنشاء قاعدة البيانات
- `README.md` - دليل شامل للتطبيق 