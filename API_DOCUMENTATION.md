# The Next Event API Documentation

## Overview
This documentation covers all available API endpoints for The Next Event management system. The API is built using ASP.NET Core and provides comprehensive functionality for content management, user authentication, form handling, media management, SEO, and email services.

## Base URL
```
https://thenextevent.runasp.net/api
```

## Authentication
Most endpoints require JWT Bearer token authentication. Include the token in the Authorization header:
```
Authorization: Bearer <your-jwt-token>
```

---

## 🔐 Authentication Controller (`/api/auth`)

### 1. Login
**POST** `/api/auth/login`

Authenticate user and receive JWT token.

**Request Body:**
```json
{
  "email": "admin@example.com",
  "password": "password123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2025-09-09T16:00:00Z",
  "user": {
    "id": 1,
    "email": "admin@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "isActive": true,
    "createdAt": "2025-01-01T00:00:00Z",
    "lastLoginAt": "2025-09-09T08:00:00Z"
  }
}
```

### 2. Register New Admin
**POST** `/api/auth/register` 🔒

Create a new admin user.

**Request Body:**
```json
{
  "email": "newadmin@example.com",
  "firstName": "Jane",
  "lastName": "Smith",
  "password": "password123",
  "confirmPassword": "password123"
}
```

**Response:**
```json
{
  "id": 2,
  "email": "newadmin@example.com",
  "firstName": "Jane",
  "lastName": "Smith",
  "isActive": true,
  "createdAt": "2025-09-09T08:00:00Z",
  "lastLoginAt": null
}
```

### 3. Get Current User
**GET** `/api/auth/me` 🔒

Get information about the currently authenticated user.

**Response:**
```json
{
  "id": 1,
  "email": "admin@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "isActive": true,
  "createdAt": "2025-01-01T00:00:00Z",
  "lastLoginAt": "2025-09-09T08:00:00Z"
}
```

### 4. Get All Users
**GET** `/api/auth/users` 🔒

Get a list of all admin users.

**Response:**
```json
[
  {
    "id": 1,
    "email": "admin@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "isActive": true,
    "createdAt": "2025-01-01T00:00:00Z",
    "lastLoginAt": "2025-09-09T08:00:00Z"
  }
]
```

### 5. Update User
**PUT** `/api/auth/users/{id}` 🔒

Update user information.

**Request Body:**
```json
{
  "email": "updated@example.com",
  "firstName": "UpdatedName",
  "lastName": "UpdatedLastName"
}
```

### 6. Delete User
**DELETE** `/api/auth/users/{id}` 🔒

Soft delete a user.

### 7. Change Password
**POST** `/api/auth/change-password` 🔒

Change current user's password.

**Request Body:**
```json
{
  "currentPassword": "oldpassword",
  "newPassword": "newpassword123",
  "confirmPassword": "newpassword123"
}
```

### 8. Logout
**POST** `/api/auth/logout`

Logout user (client-side token removal).

---

## 📝 Content Controller (`/api/content`)

### 1. Get All Content
**GET** `/api/content`

Get all content items with optional filtering.

**Query Parameters:**
- `contentKey` (string, optional): Filter by content key
- `isActive` (boolean, optional): Filter by active status

**Response:**
```json
[
  {
    "id": 1,
    "contentKey": "hero_title",
    "sectionKey": "hero",
    "contentValue": "Welcome to The Next Event",
    "language": "en",
    "sortOrder": 1,
    "isActive": true,
    "createdAt": "2025-09-09T00:00:00Z",
    "updatedAt": "2025-09-09T00:00:00Z"
  }
]
```

### 2. Get Content by ID
**GET** `/api/content/{id}`

Get specific content item by ID.

### 3. Get Content by Key
**GET** `/api/content/by-key/{contentKey}` 🌐

Get content by content key (public endpoint).

### 4. Get Content by Language
**GET** `/api/content/by-language/{language}` 🌐

Get all active content for a specific language.

### 5. Get Content by Section
**GET** `/api/content/section/{sectionKey}` 🌐

Get all content items for a specific section.

### 6. Create Content
**POST** `/api/content` 🔒

Create new content item.

**Request Body:**
```json
{
  "contentKey": "about_title",
  "sectionKey": "about",
  "contentValue": "About Our Events",
  "language": "en",
  "sortOrder": 1,
  "isActive": true
}
```

### 7. Update Content
**PUT** `/api/content/{id}` 🔒

Update existing content item.

**Request Body:**
```json
{
  "contentKey": "about_title",
  "sectionKey": "about", 
  "contentValue": "Updated About Our Events",
  "language": "en",
  "sortOrder": 1,
  "isActive": true
}
```

### 8. Delete Content
**DELETE** `/api/content/{id}` 🔒

Delete content item.

### 9. Update Sort Order
**PUT** `/api/content/{id}/sort-order` 🔒

Update content sort order.

**Request Body:**
```json
{
  "sortOrder": 5
}
```

### 10. Toggle Active Status
**PUT** `/api/content/{id}/toggle-active` 🔒

Toggle content active/inactive status.

### 11. Bulk Update Content
**PUT** `/api/content/bulk-update` 🔒

Update multiple content items at once.

**Request Body:**
```json
[
  {
    "contentKey": "item1",
    "sectionKey": "section1",
    "contentValue": "Updated Value 1",
    "language": "en",
    "sortOrder": 1,
    "isActive": true
  },
  {
    "contentKey": "item2", 
    "sectionKey": "section1",
    "contentValue": "Updated Value 2",
    "language": "en",
    "sortOrder": 2,
    "isActive": true
  }
]
```

---

## 📧 Email Controller (`/api/email`)

### 1. Get Email Configuration
**GET** `/api/email/configuration` 🔒

Get current email configuration settings.

**Response:**
```json
{
  "id": 1,
  "smtpServer": "smtp.gmail.com",
  "smtpPort": 587,
  "smtpUsername": "your-email@gmail.com",
  "smtpPassword": "encrypted-password",
  "fromEmail": "noreply@yourdomain.com",
  "fromName": "The Next Event",
  "enableSsl": true,
  "isActive": true
}
```

### 2. Update Email Configuration
**PUT** `/api/email/configuration` 🔒

Update email configuration settings.

**Request Body:**
```json
{
  "smtpServer": "smtp.gmail.com",
  "smtpPort": 587,
  "smtpUsername": "your-email@gmail.com", 
  "smtpPassword": "your-app-password",
  "fromEmail": "noreply@yourdomain.com",
  "fromName": "The Next Event",
  "enableSsl": true,
  "isActive": true
}
```

### 3. Test Email Configuration
**POST** `/api/email/test` 🔒

Test email configuration by sending a test email.

**Request Body:**
```json
{
  "testEmail": "test@example.com"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Test email sent successfully"
}
```

### 4. Send Custom Email
**POST** `/api/email/send` 🔒

Send a custom email.

**Request Body:**
```json
{
  "to": "recipient@example.com",
  "subject": "Test Subject",
  "body": "Email body content",
  "isHtml": true
}
```

### 5. Send Form Submission Notification
**POST** `/api/email/notify-form-submission` 🔒

Send notification for new form submission.

**Request Body:**
```json
{
  "formSubmissionId": 123
}
```

### 6. Get Email Statistics
**GET** `/api/email/statistics` 🔒

Get email sending statistics.

**Response:**
```json
{
  "totalSent": 1250,
  "totalFailed": 23,
  "successRate": 98.16,
  "todaySent": 45,
  "weekSent": 312,
  "monthSent": 1250
}
```

### 7. Get Email History
**GET** `/api/email/history` 🔒

Get email sending history with pagination.

**Query Parameters:**
- `page` (int, default: 1): Page number
- `pageSize` (int, default: 20): Items per page

---

## 📋 Forms Controller (`/api/forms`)

### 1. Get Form Submissions
**GET** `/api/forms` 🔒

Get all form submissions with filtering and pagination.

**Query Parameters:**
- `page` (int): Page number
- `pageSize` (int): Items per page
- `status` (string): Filter by status
- `dateFrom` (date): Filter from date
- `dateTo` (date): Filter to date
- `search` (string): Search in form data

**Response:**
```json
{
  "items": [
    {
      "id": 1,
      "formType": "contact",
      "submitterName": "John Doe",
      "submitterEmail": "john@example.com",
      "submitterPhone": "+1234567890",
      "message": "Hello, I'm interested in your services",
      "status": "new",
      "isRead": false,
      "adminNotes": "",
      "submittedAt": "2025-09-09T10:30:00Z",
      "updatedAt": "2025-09-09T10:30:00Z"
    }
  ],
  "totalCount": 25,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 3
}
```

### 2. Get Form Submission by ID
**GET** `/api/forms/{id}` 🔒

Get specific form submission details.

### 3. Submit Form
**POST** `/api/forms/submit` 🌐

Submit a new form (public endpoint).

**Request Body:**
```json
{
  "formType": "contact",
  "submitterName": "John Doe",
  "submitterEmail": "john@example.com",
  "submitterPhone": "+1234567890",
  "message": "I'm interested in your event planning services",
  "additionalData": {
    "eventType": "wedding",
    "guestCount": "150",
    "eventDate": "2025-12-01"
  }
}
```

### 4. Update Form Status
**PATCH** `/api/forms/{id}/status` 🔒

Update form submission status.

**Request Body:**
```json
{
  "status": "in_progress",
  "adminNotes": "Following up with client tomorrow"
}
```

### 5. Mark as Read/Unread
**PATCH** `/api/forms/{id}/read-status` 🔒

Mark form submission as read or unread.

**Request Body:**
```json
true
```

### 6. Delete Form Submission
**DELETE** `/api/forms/{id}` 🔒

Delete form submission.

### 7. Export to CSV
**GET** `/api/forms/export/csv` 🔒

Export form submissions to CSV file.

**Query Parameters:**
- Same filtering parameters as Get Form Submissions

**Response:** CSV file download

### 8. Get Form Statistics
**GET** `/api/forms/statistics` 🔒

Get form submission statistics.

**Response:**
```json
{
  "totalSubmissions": 1250,
  "newSubmissions": 45,
  "inProgressSubmissions": 23,
  "completedSubmissions": 1150,
  "todaySubmissions": 12,
  "weekSubmissions": 87,
  "monthSubmissions": 245
}
```

### 9. Get Daily Form Counts
**GET** `/api/forms/daily-counts` 🔒

Get daily form submission counts for charts.

**Query Parameters:**
- `days` (int, default: 30): Number of days to retrieve

**Response:**
```json
[
  {
    "date": "2025-09-09",
    "count": 12
  },
  {
    "date": "2025-09-08", 
    "count": 8
  }
]
```

### 10. Bulk Update Status
**PATCH** `/api/forms/bulk-update` 🔒

Update multiple form submissions status.

**Request Body:**
```json
{
  "formIds": [1, 2, 3, 4],
  "status": "completed",
  "adminNotes": "Bulk processing completed"
}
```

### 11. Bulk Delete
**DELETE** `/api/forms/bulk-delete` 🔒

Delete multiple form submissions.

**Request Body:**
```json
{
  "formIds": [1, 2, 3, 4]
}
```

---

## 🖼️ Media Controller (`/api/media`)

### 1. Get Upload Signature
**GET** `/api/media/signature` 🔒

Get Cloudinary upload signature for direct uploads.

**Query Parameters:**
- `publicId` (string, required): Public ID for the media

**Response:**
```json
{
  "signature": "a1b2c3d4e5f6...",
  "timestamp": 1694250000,
  "api_key": "your_api_key",
  "cloud_name": "your_cloud_name"
}
```

### 2. Get All Media
**GET** `/api/media/all` 🔒

Get all uploaded media items.

**Response:**
```json
[
  {
    "publicId": "samples/landscapes/beach-boat",
    "url": "https://res.cloudinary.com/demo/image/upload/v1312461204/samples/landscapes/beach-boat.jpg",
    "secureUrl": "https://res.cloudinary.com/demo/image/upload/v1312461204/samples/landscapes/beach-boat.jpg",
    "format": "jpg",
    "width": 1200,
    "height": 800,
    "bytes": 120000,
    "createdAt": "2025-09-09T00:00:00Z"
  }
]
```

### 3. Delete Media
**DELETE** `/api/media/{publicId}` 🔒

Delete media item from Cloudinary.

**Response:**
```json
{
  "message": "Image deleted successfully"
}
```

---

## 🔍 SEO Controller (`/api/seo`)

### 1. Get SEO Metadata
**GET** `/api/seo` 🔒

Get all SEO metadata with pagination.

**Query Parameters:**
- `page` (int, default: 1): Page number  
- `pageSize` (int, default: 20): Items per page

**Response:**
```json
{
  "items": [
    {
      "id": 1,
      "pageUrl": "/",
      "metaTitle": "The Next Event - Premier Event Planning Services",
      "metaDescription": "Transform your special moments into unforgettable experiences with our professional event planning services.",
      "metaKeywords": "event planning, wedding planning, corporate events",
      "ogTitle": "The Next Event - Premier Event Planning",
      "ogDescription": "Professional event planning services for all occasions",
      "ogImage": "https://example.com/og-image.jpg",
      "ogType": "website",
      "twitterCard": "summary_large_image",
      "twitterTitle": "The Next Event",
      "twitterDescription": "Professional event planning services",
      "twitterImage": "https://example.com/twitter-image.jpg",
      "canonicalUrl": "https://thenextevent.com/",
      "robots": "index,follow",
      "isActive": true,
      "createdAt": "2025-09-09T00:00:00Z",
      "updatedAt": "2025-09-09T00:00:00Z"
    }
  ],
  "totalCount": 15,
  "pageNumber": 1,
  "pageSize": 20,
  "totalPages": 1
}
```

### 2. Get SEO Metadata by ID
**GET** `/api/seo/{id}` 🔒

Get specific SEO metadata by ID.

### 3. Get SEO Metadata by URL
**GET** `/api/seo/by-url` 🌐

Get SEO metadata for a specific page URL.

**Query Parameters:**
- `url` (string, required): Page URL

### 4. Create SEO Metadata
**POST** `/api/seo` 🔒

Create new SEO metadata.

**Request Body:**
```json
{
  "pageUrl": "/about",
  "metaTitle": "About Us - The Next Event",
  "metaDescription": "Learn about our passion for creating exceptional events and memorable experiences.",
  "metaKeywords": "about us, event planning team, experience",
  "ogTitle": "About The Next Event",
  "ogDescription": "Learn about our event planning expertise",
  "ogImage": "https://example.com/about-og.jpg",
  "ogType": "website",
  "twitterCard": "summary_large_image",
  "twitterTitle": "About The Next Event",
  "twitterDescription": "Our event planning expertise",
  "twitterImage": "https://example.com/about-twitter.jpg",
  "canonicalUrl": "https://thenextevent.com/about",
  "robots": "index,follow",
  "isActive": true
}
```

### 5. Update SEO Metadata
**PUT** `/api/seo/{id}` 🔒

Update existing SEO metadata.

### 6. Delete SEO Metadata
**DELETE** `/api/seo/{id}` 🔒

Delete SEO metadata.

### 7. Generate Sitemap
**GET** `/api/seo/sitemap` 🌐

Generate and return XML sitemap.

**Response:** XML sitemap content

### 8. Generate Robots.txt
**GET** `/api/seo/robots` 🌐

Generate and return robots.txt file.

**Response:** robots.txt content

### 9. Validate SEO Metadata
**POST** `/api/seo/validate` 🔒

Validate SEO metadata for completeness and best practices.

**Request Body:**
```json
{
  "pageUrl": "/services",
  "metaTitle": "Our Services",
  "metaDescription": "Explore our comprehensive event planning services.",
  "metaKeywords": "services, event planning",
  "content": "Page content for analysis..."
}
```

**Response:**
```json
{
  "isValid": true,
  "score": 85,
  "recommendations": [
    {
      "type": "warning",
      "message": "Meta title could be more descriptive",
      "field": "metaTitle"
    }
  ],
  "checkedItems": [
    {
      "name": "Meta Title Length",
      "passed": true,
      "message": "Meta title is within recommended length"
    }
  ]
}
```

### 10. Get SEO Analytics
**GET** `/api/seo/analytics` 🔒

Get SEO performance analytics.

**Query Parameters:**
- `days` (int, default: 30): Number of days for analytics

**Response:**
```json
{
  "totalPages": 25,
  "indexedPages": 23,
  "avgTitleLength": 52,
  "avgDescriptionLength": 145,
  "pagesWithoutTitle": 0,
  "pagesWithoutDescription": 2,
  "duplicatesTitles": 1,
  "duplicatesDescriptions": 0
}
```

### 11. Bulk Update SEO Metadata
**PATCH** `/api/seo/bulk-update` 🔒

Update multiple SEO metadata entries.

**Request Body:**
```json
{
  "seoIds": [1, 2, 3],
  "updates": {
    "robots": "index,follow",
    "isActive": true
  }
}
```

### 12. Get SEO Recommendations
**GET** `/api/seo/recommendations` 🔒

Get SEO improvement recommendations.

**Response:**
```json
[
  {
    "priority": "high",
    "category": "meta_tags",
    "message": "2 pages are missing meta descriptions",
    "actionRequired": "Add meta descriptions to improve search visibility",
    "affectedPages": ["/contact", "/gallery"]
  }
]
```

---

## 📊 Status Codes

### Success Codes
- `200 OK` - Request successful
- `201 Created` - Resource created successfully
- `204 No Content` - Request successful, no content returned

### Error Codes
- `400 Bad Request` - Invalid request data
- `401 Unauthorized` - Authentication required or invalid
- `403 Forbidden` - Access denied
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server error

---

## 🔧 Common Request/Response Patterns

### Error Response Format
```json
{
  "message": "Error description",
  "errors": {
    "field1": ["Validation error message"],
    "field2": ["Another validation error"]
  }
}
```

### Pagination Response Format
```json
{
  "items": [...],
  "totalCount": 100,
  "pageNumber": 1,
  "pageSize": 20,
  "totalPages": 5
}
```

### Success Response Format
```json
{
  "success": true,
  "message": "Operation completed successfully",
  "data": {...}
}
```

---

## 🚀 Getting Started

1. **Authentication**: Start by calling `/api/auth/login` to get your JWT token
2. **Include Token**: Add the token to the Authorization header for protected endpoints
3. **Content Management**: Use `/api/content` endpoints to manage website content
4. **Form Handling**: Use `/api/forms/submit` for public form submissions
5. **Media Management**: Use `/api/media` for file uploads and management
6. **SEO Optimization**: Use `/api/seo` endpoints for search engine optimization

---

## 📝 Notes

- 🔒 = Requires Authentication
- 🌐 = Public Endpoint (No Authentication Required)
- All timestamps are in UTC format
- File uploads are handled through Cloudinary integration
- Email functionality requires proper SMTP configuration
- SEO features include automatic sitemap and robots.txt generation

## 💡 Tips for Frontend Integration

1. **Token Storage**: Store JWT tokens securely (localStorage/sessionStorage)
2. **Token Refresh**: Implement token refresh logic before expiration
3. **Error Handling**: Implement global error handling for API responses
4. **Loading States**: Show loading indicators during API calls
5. **Pagination**: Implement pagination for list endpoints
6. **File Uploads**: Use Cloudinary direct upload for better performance
7. **SEO Integration**: Use SEO endpoints to populate meta tags dynamically
8. **Form Validation**: Validate forms client-side before submission
