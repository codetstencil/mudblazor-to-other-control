# ChinookManager

This is a simple application that allows users to manage **Artists**, **Albums**, and **Album Views** with full CRUD (Create, Read, Update, Delete) functionality.

## The only change required is in the Presentation layer

---

## 🚀 Features
- **Login Page**: Secure authentication before accessing the app.
- **Artist Management**: Add, edit, delete, and view artists.
- **Album Management**: Create, update, delete, and view albums.
- **Album View**: Browse and manage album details.

---

## 🔑 Login Credentials
Click [login credentials](https://github.com/codetstencil/mudblazor-to-other-control/blob/main/AdminCredentials.txt) to get the login credentials.

---

## 🗄️ Database Setup
This project uses **SQL Server** as the database. A `.bak` file is included in the repository to restore the database.

### ⚠️ Version Compatibility
If you encounter errors related to SQL Server version differences (e.g., version 16 vs. 15), see the image below;
<img width="555" height="180" alt="image" src="https://github.com/user-attachments/assets/5546b27e-226e-4fdf-952e-fc6db92006d7" />

1. Update your SQLServer to the required version (v16). This should resolve the issue, and the restore would work. If this doesn't, then check the second step below.
2. Use **Docker** to spin up a SQL Server instance of the required version.
3. Update the **connection string** in [appsettings.json](https://github.com/codetstencil/mudblazor-to-other-control/blob/main/src/Api/appsettings.json) to point to your SQL Server instance.

Example connection string in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=YOUR LOCAL DB INSTANCE;Initial Catalog=Chinook;Integrated Security=SSPI;TrustServerCertificate=True;MultipleActiveResultSets=True;Connect Timeout=120;;"
}
