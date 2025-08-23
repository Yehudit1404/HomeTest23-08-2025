# Home Test – Contact Requests API & Angular UI

## 📌 Overview
This project contains two parts:

1. **Contacts.Api** (ASP.NET Core 8 Web API)  
   - Provides CRUD operations for contact requests  
   - Includes API key protection (`X-Api-Key`) for write operations  
   - Implements a simulated stored procedure endpoint for a monthly report  
   - In-memory EF database (so the project runs without a real DB)  
   - Basic error handling, validation and Swagger UI enabled

2. **contacts-ui** (Angular 18 + Angular Material)  
   - Responsive web application with two main pages:
     - **Contact Form** – allows users to submit a request (name, phone, email, department(s), description)  
     - **Monthly Report** – displays the report returned by the API (grouped per month)
   - Mobile-friendly and RTL (Right-to-Left) support  
   - Uses Angular Material components for modern, clean UI

---

## ⚙️ Technologies Used
- **Backend:** .NET 8, Entity Framework Core (InMemory), xUnit for tests  
- **Frontend:** Angular 18 (standalone components), Angular Material  
- **Other:** Swagger/OpenAPI, CORS enabled for local dev

---

## 🚀 How to Run Locally

### 1. Clone the repository
```bash
git clone https://github.com/Yehudit1404/HomeTest23-08-2025.git
cd HomeTest23-08-2025
