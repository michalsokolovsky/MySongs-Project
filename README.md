
# 🎵 MySongs

![React](https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB)
![TypeScript](https://img.shields.io/badge/TypeScript-007ACC?style=for-the-badge&logo=typescript&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![ASP.NET](https://img.shields.io/badge/ASP.NET-5C2D91?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Redux](https://img.shields.io/badge/Redux-593D88?style=for-the-badge&logo=redux&logoColor=white)

## 💡 הרעיון מאחורי הפרויקט

מוזיקה היא שפה אוניברסלית — אבל לנהל את השירים שאוהבים, לגלות חדשים ולעקוב אחרי היסטוריית ההאזנה? זה כבר דורש כלי טוב.

MySongs נולדה מתוך הרצון ליצור חוויה מוזיקלית אישית ומאורגנת — מקום אחד שבו אפשר לנהל שירים, תגיות, המלצות חכמות ועוד. עם תמיכה ב-AI, המערכת לומדת את הטעם המוזיקלי שלך ומציעה שירים שתאהב.

## 🎯 מה המערכת מציעה?

🎧 **משתמשים** — כניסה אישית, ניהול שירים אהובים ומעקב אחר היסטוריית האזנה  
🏷️ **תגיות** — ארגון שירים לפי ז'אנר, מצב רוח או כל קטגוריה שתבחרו  
🤖 **המלצות AI** — קבלת המלצות מותאמות אישית על בסיס ההעדפות שלך  

## ✨ תכונות עיקריות

| תכונה | תיאור |
|-------|-------|
| 🔐 אבטחת כניסה | אימות מאובטח עם JWT |
| 🎵 ניהול שירים | הוספה, עריכה ומחיקה של שירים |
| 🏷️ מערכת תגיות | ארגון שירים לפי תגיות אישיות |
| 📜 היסטוריית האזנה | מעקב אחרי כל השירים שהאזנת להם |
| 🤖 המלצות חכמות | שירים מומלצים בעזרת OpenAI |
| 👤 ניהול משתמשים | פרופיל אישי ועדכון פרטים |

## 🛠️ טכנולוגיות

### Frontend
| טכנולוגיה | שימוש |
|-----------|-------|
| React + TypeScript | ממשק משתמש |
| Redux Toolkit | ניהול state |
| Axios | קריאות API |
| React Router | ניווט |

### Backend
| טכנולוגיה | שימוש |
|-----------|-------|
| ASP.NET Core 8 | שרת |
| C# | שפת תכנות |
| JWT Authentication | אבטחה |
| Entity Framework Core | ORM |
| Swagger | תיעוד API |

### Database
| טכנולוגיה | שימוש |
|-----------|-------|
| SQL Server / LocalDB | בסיס נתונים |

## 🚀 הרצה מקומית

### דרישות מוקדמות
- Node.js ו-npm
- .NET 8 SDK
- SQL Server או LocalDB
- (אופציונלי) מפתח OpenAI להפעלת שירות ה-AI

### 1️⃣ Backend
```bash
cd backend
dotnet restore
dotnet run
```

### 2️⃣ Frontend
```bash
cd frontend
npm install
npm start
```

### 3️⃣ פתח בדפדפן
```
http://localhost:3000
```

## ⚙️ קונפיגורציה

הגדרות חשובות ב-`backend/MySongs.Api/appsettings.json`:

| מפתח | תיאור |
|------|-------|
| `ConnectionStrings:DefaultConnection` | חיבור למסד הנתונים |
| `Jwt:Key / Issuer / Audience` | הגדרות אימות JWT |
| `OpenAI:ApiKey` | מפתח לשירות ה-AI (אופציונלי) |

> 💡 ניתן להוסיף שינויי פיתוח ב-`appsettings.Development.json`

## 📁 מבנה הפרויקט

```
MySongs-Project/
├── backend/
│   ├── MySongs.Api/          ← ASP.NET Core API
│   │   └── Controllers/
│   │       ├── AuthController
│   │       ├── SongsController
│   │       ├── TagsController
│   │       ├── UsersController
│   │       ├── ListeningHistoryController
│   │       └── RecommendationsController
│   ├── MySongs.Common/       ← DTO משותפים
│   ├── MySongs.Repository/   ← Data Access Layer
│   ├── MySongs.Services/     ← Business Logic Layer
│   └── MySongs.Mock/         ← נתוני דמו
└── frontend/
    └── src/
        ├── components/       ← רכיבים משותפים
        ├── pages/            ← דפי האפליקציה
        ├── store/            ← Redux slices
        └── api/              ← קריאות API
```

## 💡 טיפים

- ודא ש-SQL Server או LocalDB מופעלים לפני הפעלת ה-API
- בעיה בחיבור למסד הנתונים? עדכן את `DefaultConnection` ב-`appsettings.json`
- הלקוח לא מתחבר לשרת? בדוק את כתובת ה-API ב-`src/api/songsApi.tsx`
- Swagger זמין בסביבת הפיתוח בכתובת `https://localhost:5001/swagger`

---

פרויקט זה פותח מתוך אהבה למוזיקה ולקוד — כי כל שיר טוב מגיע עם חוויה טובה. 🎵

⭐ אם הפרויקט עזר לך, שקול לתת לו כוכב!
>>>>>>> e33bdcb304dcced99d7c9469166de8b3bf378001
