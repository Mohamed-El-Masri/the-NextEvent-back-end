# 🚀 The Next Event API - .NET 8 Deployment Ready

## ✅ تم إصلاح خطأ 500 - Internal Server Error

### 🔧 المشاكل المحلولة:

1. **✅ تحديث إلى .NET 8**
   - تغيير من .NET 9 إلى .NET 8 للتوافق مع Monster ASP
   - تحديث جميع packages للإصدار 8.0

2. **✅ معالجة أخطاء قاعدة البيانات**
   - إضافة try/catch للـ database migrations
   - التحقق من الاتصال قبل تطبيق migrations
   - API تعمل حتى لو كانت قاعدة البيانات معطلة

3. **✅ Global Exception Handler**
   - معالج عام للأخطاء مع رسائل واضحة
   - حماية من crashes غير متوقعة

4. **✅ Enhanced Health Checks**
   - فحص حالة قاعدة البيانات في real-time
   - معلومات تفصيلية عن حالة النظام

### 📁 ملفات النشر الجاهزة

استخدم الأمر `deploy-ready.bat` أو ارفع محتويات مجلد `publish/` مباشرة.

### 🎯 خطوات النشر السريعة

1. **ارفع الملفات**: جميع محتويات `publish/` إلى `wwwroot` في Monster ASP
2. **تأكد من web.config**: موجود في المجلد الجذر
3. **اختبر**: `https://thenextevent.runasp.net/`

### ✅ Endpoints للاختبار

بعد النشر:

1. **🏠 Root**: `https://thenextevent.runasp.net/`
   ```json
   {
     "message": "The Next Event API is running successfully!",
     "databaseStatus": "Connected" // أو "Disconnected"
   }
   ```

2. **❤️ Health**: `https://thenextevent.runasp.net/health`
3. **📚 Swagger**: `https://thenextevent.runasp.net/swagger`
4. **📝 Content API**: `https://thenextevent.runasp.net/api/content`

### 🔍 استكشاف الأخطاء

- **خطأ 500**: تحقق من logs في Monster ASP Control Panel
- **قاعدة البيانات**: API تعمل حتى لو كانت قاعدة البيانات معطلة
- **Swagger**: تأكد من رفع جميع Swashbuckle DLLs

### 📚 الوثائق الإضافية

- `DEPLOYMENT_FIXED.md` - تعليمات النشر المفصلة
- `API_DOCUMENTATION.md` - وثائق الـ APIs
- `api-tester.html` - أداة اختبار الـ endpoints

---

## 🎉 النتيجة

الـ API جاهز الآن للعمل على Monster ASP مع:
- ✅ .NET 8 compatibility
- ✅ مقاوم للأخطاء (Error resilient)
- ✅ Swagger UI يعمل
- ✅ CORS مفتوح للجميع
- ✅ Database fault tolerance

---

**ملاحظة**: استخدم `deploy-ready.bat` لإنشاء package جاهز للنشر.

---

---

# The Next Event API - Original Documentation

ASP.NET Core backend for The Next Event landing page system with admin dashboard.

## Overview

This is a monolithic ASP.NET Core Web API that provides backend services for:
- Bilingual (English/Arabic) landing page content management
- Form submissions handling (Plan Event, Partner, Service Provider)
- Admin dashboard with JWT authentication
- Cloudinary integration for media management
- Email notifications via Gmail API
- SEO metadata management
- CSV export functionality

## Technology Stack

- **Framework**: ASP.NET Core 9.0
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: JWT Bearer tokens
- **Cloud Storage**: Cloudinary for images
- **Email**: Gmail API integration
- **Documentation**: Swagger/OpenAPI

## Project Structure

```
TheNextEventAPI/
├── Controllers/         # API controllers
├── Data/               # Database context
├── DTOs/               # Data transfer objects
├── Models/             # Entity models
├── Services/           # Business logic services
├── Migrations/         # EF Core migrations
└── Program.cs          # Application entry point
```

## Setup Instructions

### Prerequisites

- .NET 9.0 SDK
- SQL Server or SQL Server LocalDB
- Cloudinary account (for image management)
- Gmail API credentials (for email notifications)

### Configuration

1. Update `appsettings.json` with your configurations:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your_sql_server_connection_string"
  },
  "Jwt": {
    "Key": "your_jwt_secret_key_at_least_32_characters",
    "Issuer": "TheNextEventAPI",
    "Audience": "TheNextEventClient"
  },
  "Cloudinary": {
    "CloudName": "your_cloudinary_cloud_name",
    "ApiKey": "your_cloudinary_api_key",
    "ApiSecret": "your_cloudinary_api_secret"
  },
  "Gmail": {
    "ClientId": "your_gmail_client_id",
    "ClientSecret": "your_gmail_client_secret"
  }
}
```

### Database Setup

1. Run migrations to create the database:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

2. Create an admin user manually in the database or via seeding.

### Running the Application

```bash
dotnet run
```

The API will be available at:
- HTTPS: `https://localhost:7074`
- HTTP: `http://localhost:5074`
- Swagger UI: `https://localhost:7074/swagger`

## API Endpoints

### Authentication
- `POST /api/auth/login` - Admin login
- `POST /api/auth/logout` - Admin logout
- `POST /api/auth/reset-password` - Password reset
- `POST /api/auth/change-password` - Change password

### Content Management
- `GET /api/content/{section}` - Get content by section
- `GET /api/content/all` - Get all content
- `PUT /api/content/{section}/{contentKey}` - Update content (requires auth)

### Media Management
- `GET /api/media/signature` - Get Cloudinary signature (requires auth)
- `GET /api/media/all` - Get all media (requires auth)
- `DELETE /api/media/{publicId}` - Delete media (requires auth)

### Form Submissions
- `POST /api/forms/{formType}/submit` - Submit form
- `GET /api/forms/{formType}/config` - Get form configuration
- `GET /api/admin/forms/{formType}/submissions` - Get submissions (requires auth)
- `GET /api/admin/forms/{formType}/export` - Export to CSV (requires auth)

### SEO Management
- `GET /api/seo/metadata` - Get SEO metadata
- `PUT /api/seo/metadata` - Update SEO metadata (requires auth)

## Database Schema

The database includes tables for:
- `AdminUsers` - Admin authentication
- `WebsiteContent` - Bilingual content management
- `FormSubmissions` - Form data storage
- `EmailConfiguration` - Email settings
- `SeoMetadata` - SEO metadata

## Security Features

- JWT authentication for admin endpoints
- Password hashing with BCrypt
- CORS configuration for frontend integration
- Input validation and sanitization
- Secure Cloudinary uploads with signed parameters

## Development

### Adding New Endpoints

1. Create DTOs in the `DTOs` folder
2. Add controller methods in the `Controllers` folder
3. Implement business logic in `Services` if needed
4. Update documentation

### Database Changes

1. Modify models in the `Models` folder
2. Add new migration: `dotnet ef migrations add MigrationName`
3. Update database: `dotnet ef database update`

## Deployment

For production deployment:

1. Update connection strings and configurations
2. Set up SSL certificates
3. Configure production logging
4. Set up database backup strategy
5. Configure environment-specific settings

## Contributing

1. Follow ASP.NET Core coding conventions
2. Add XML documentation for public APIs
3. Include unit tests for new features
4. Update API documentation
