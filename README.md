# 📞 Contact Requests – Full Stack Application

## 📌 Overview
This repository contains a **full-stack Contact Requests system** built as part of a technical assignment.  
It consists of two main parts:

1. **Backend – Contacts.Api (.NET 8 Web API)**  
   - Provides CRUD operations for contact requests  
   - Protected with API key (`X-Api-Key`) for all write operations  
   - Includes a simulated stored procedure endpoint for monthly reports  
   - Uses EF Core InMemory database (no external DB required)  
   - Swagger/OpenAPI documentation available  
   - Unit tests with xUnit

2. **Frontend – contacts-ui (Angular 18 + Angular Material)**  
   - Responsive web application with **RTL support** (Hebrew/Arabic)  
   - Contact Form page – submit name, phone, email, departments, description  
   - Monthly Report page – display data from the backend report endpoint  
   - Built with Angular standalone components and Angular Material  
   - Mobile-friendly and optimized for different screen sizes  

---

## 📂 Project Structure
