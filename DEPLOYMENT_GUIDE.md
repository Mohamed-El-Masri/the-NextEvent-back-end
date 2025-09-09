# Deployment Instructions for Monster ASP

## التغييرات المطلوبة للنشر على Monster ASP

### 1. إعدادات CORS
تم تحديث إعدادات CORS لتكون مفتوحة للجميع في البيئة الإنتاجية:
- سياسة "AllowAll" تسمح بجميع المصادر والطرق والرؤوس
- إضافة نطاق Monster ASP إلى السياسات المحددة

### 2. إعدادات Swagger
تم تمكين Swagger في جميع البيئات بدلاً من Development فقط:
- Swagger متاح على: `/swagger`
- API Documentation متاح على: `/swagger/index.html`

### 3. Health Check Endpoints
تم إضافة endpoints للتحقق من حالة API:
- Root endpoint: `/` - يظهر معلومات API الأساسية
- Health check: `/health` - للتحقق من حالة النظام

### 4. ملف web.config
تم إنشاء ملف web.config للتوافق مع Monster ASP يتضمن:
- إعدادات AspNetCore Module
- رؤوس CORS إضافية
- إعادة توجيه HTTP إلى HTTPS
- رؤوس الأمان
- إعدادات التخزين المؤقت

### 5. Database Migrations
تم إضافة تطبيق تلقائي لـ database migrations عند بدء التطبيق

## خطوات النشر

### 1. Build & Publish
```bash
dotnet restore
dotnet build --configuration Release
dotnet publish --configuration Release --output ./publish
```

### 2. Upload Files
قم برفع جميع الملفات من مجلد `publish` إلى مجلد `wwwroot` في Monster ASP

### 3. إعدادات قاعدة البيانات
تأكد من أن connection string صحيح في `appsettings.Production.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=db26958.public.databaseasp.net; Database=db26958; User Id=db26958; Password=yM!26cA_%K3s; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;"
  }
}
```

### 4. Test Endpoints
بعد النشر، تحقق من الـ endpoints التالية:
- `https://thenextevent.runasp.net/` - الصفحة الرئيسية للـ API
- `https://thenextevent.runasp.net/health` - فحص الحالة
- `https://thenextevent.runasp.net/swagger` - Swagger UI
- `https://thenextevent.runasp.net/api/content` - أول API endpoint

## استكشاف الأخطاء

### إذا لم يعمل Swagger:
1. تأكد من رفع جميع ملفات `wwwroot` إذا كانت موجودة
2. تحقق من أن ملف `web.config` موجود في المجلد الجذر
3. تأكد من أن Monster ASP يدعم .NET 9

### إذا كانت قاعدة البيانات لا تعمل:
1. تحقق من صحة connection string
2. تأكد من أن قاعدة البيانات متاحة ومفعلة في لوحة تحكم Monster ASP
3. راجع الـ logs في لوحة تحكم Monster ASP

### إذا كان CORS لا يعمل:
1. تأكد من أن ملف `web.config` تم رفعه بصورة صحيحة
2. تحقق من أن رؤوس CORS موجودة في الاستجابات
3. راجع browser console للأخطاء المتعلقة بـ CORS

## ملاحظات مهمة
- تم تعطيل HTTPS redirection في البيئة الإنتاجية (Monster ASP يتولى ذلك)
- تم إضافة auto-migration للقاعدة
- CORS مفتوح بالكامل للسماح بالوصول من أي مصدر
- Swagger مفعل في جميع البيئات للتسهيل على الفرونت إند
