🛠️ المميزات والتقنيات المستخدمة
* **.NET 10 / ASP.NET Core Web API**
* **Asp.Versioning.Mvc** لإدارة الإصدارات عبر الـ URL[cite: 5].
* **URL Path Versioning** لتحديد الإصدار مباشرة من الـ Route[cite: 3].
* **Frontend UI (wwwroot)** واجهة محلية بسيطة لاختبار الإصدارات بضغطة زر.

---

## 📂 هيكلية المشروع (Project Structure)
```text
VersioningDemo/
│
├── Controllers/
│   ├── V1/
│   │   └── ProductsController.cs   # الإصدار الأول (يعيد بيانات المنتج الأساسية)
│   └── V2/
│       └── ProductsController.cs   # الإصدار الثاني (يعيد البيانات مع إضافة الـ categoryName)
│
├── wwwroot/
│   └── index.html                  # واجهة أمامية لاختبار الإصدارات بسهولة
│
├── Program.cs                      # إعدادات الـ API Versioning والـ Pipeline
└── VersioningDemo.csproj           # ملف المشروع والحزم المُثبتة

```

---

## 🚀 طريقة تشغيل المشروع محلياً (How to Run)

1. **استنساخ المستودع أو فتح المجلد:**
افتح الـ Terminal في مجلد المشروع الرئيسي.
2. **استعادة الحزم وتشغيل المشروع:**
نفذ الأوامر التالية:
```bash
dotnet restore
dotnet run

```


3. **فتح الواجهة أو الـ Endpoints:**
* **الواجهة الأمامية (تلقائياً):** افتح المتصفح على الرابط:
`http://localhost:5084`
* **الإصدار الأول (V1):**
`http://localhost:5084/api/v1/products`
* **الإصدار الثاني (V2):**
`http://localhost:5084/api/v2/products`



---

## 💡 شكل الـ Responses للفرق بين الإصدارين

* الاستجابة للإصدار الأول (`GET /api/v1/products`):


```json
{
  "id": 1,
  "name": "Laptop",
  "price": 25000
}

```


* الاستجابة للإصدار الثاني (`GET /api/v2/products`):


```json
{
  "id": 1,
  "name": "Laptop",
  "price": 25000,
  "categoryName": "Electronics"
}
