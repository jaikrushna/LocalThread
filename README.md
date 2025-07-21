
# 🧵 LocalThreads: E-Commerce Platform for Local Artisans

**LocalThreads** is a full-stack e-commerce platform aimed at supporting local artisans and small shopkeepers by enabling them to showcase, manage, and sell their products online. The platform provides customer-friendly interfaces, powerful shopkeeper tools, and robust backend APIs, all built using modern technologies like **ASP.NET Core**, **MongoDB Atlas**, **Firebase Authentication**, and **React** (frontend in progress).

---

## 📸 Preview

![Landing Page](./assets/Landing.gif)
![Shopkeeper Registration Page](./assets/Registration.gif)
![Product Details Page](./assets/Details.gif)

---

## 📦 Features

### 🛒 Customer Side
- Secure Firebase Authentication
- Profile, Wishlist, and Cart Management
- Product Listing and Category Filtering
- Order Placement and History

### 🧑‍🏭 Shopkeeper Side
- Product Management (Add, Edit, Delete)
- Real-time Inventory Updates
- Category Assignment
- Order Processing

### 🔧 Admin Panel (Planned)
- Manage Users, Shopkeepers, and Products
- Block/Unblock Shopkeeper Accounts

---

## 🛠️ Technologies Used

| Tech Stack     | Description                                  |
|----------------|----------------------------------------------|
| `C#` + `ASP.NET Core` | Backend API development                   |
| `MongoDB Atlas` | Cloud-based NoSQL database                  |
| `Firebase Auth` | Secure authentication via UID tokens        |
| `React`         | Frontend framework (currently in progress)  |
| `Google Maps API` | Address & location functionalities         |
| `AWS S3` (Planned) | Image storage & CDN for product images |
| `FluentValidation` | Input validation                          |

---

## 🧩 Backend Architecture

### 📂 Layered Structure
- **Controllers** – Handle incoming HTTP requests.
- **Services** – Business logic layer.
- **Repositories** – Data access layer.
- **DTOs** – Data Transfer Objects for clean communication.

### 📌 Key Modules

#### 🧑‍💼 Customer
- `Customer.cs`
- Embedded fields: account details, cart, liked products
- Orders stored in a separate collection with references to both `CustomerId` and `ShopId`.

#### 🏪 Shopkeeper
- Product CRUD
- Categories management
- Order receive & management

---

## 🗃️ MongoDB Schema Overview

### `Customers` Collection
```json
{
  "_id": "ObjectId",
  "firebaseUid": "string",
  "name": "string",
  "email": "string",
  "phone": "string",
  "address": {
    "street": "string",
    "city": "string",
    "pin": "string"
  },
  "likedProducts": [ "productId1", "productId2" ],
  "cart": [
    {
      "productId": "string",
      "quantity": "number"
    }
  ]
}
````

### `Orders` Collection

```json
{
  "_id": "ObjectId",
  "customerId": "ObjectId",
  "shopkeeperId": "ObjectId",
  "products": [
    {
      "productId": "string",
      "quantity": "number",
      "price": "number"
    }
  ],
  "status": "pending",
  "createdAt": "ISODate"
}
```

### `Products` Collection

```json
{
  "_id": "ObjectId",
  "name": "string",
  "description": "string",
  "price": "number",
  "categoryId": "string",
  "shopkeeperId": "ObjectId",
  "isDeleted": "false",
  "images": [ "url1", "url2" ]
}
```

---

## 🔐 Authentication

* **Firebase UID** is extracted from the Authorization Bearer token.
* On every request requiring authentication, UID is verified and mapped to internal DB user.

---

## 🛣️ API Endpoints (Sample)

### Customer

* `GET /api/customer/profile`
* `POST /api/customer/register`
* `GET /api/customer/wishlist`
* `POST /api/customer/cart`

### Shopkeeper

* `POST /api/shopkeeper/products`
* `PUT /api/shopkeeper/product/:id`
* `GET /api/shopkeeper/orders`

### Orders

* `POST /api/orders/place`
* `GET /api/orders/history`

---

## 📂 Project Structure (Backend)

```
LocalThreads/
├── Controllers/
│   ├── CustomerController.cs
│   ├── ShopkeeperController.cs
│   └── SharedController.cs
├── Models/
│   ├── Customer/
│   ├── Shopkeeper/
│   └── Shared/
├── Services/
│   ├── Interfaces/
│   └── Implementations/
├── DTOs/
├── Repositories/
├── Firebase/
└── Program.cs
```

---

## 🚀 How to Run (Backend)

1. **Clone Repo**
   `git clone https://github.com/yourusername/LocalThreads.git`

2. **Set up MongoDB Atlas URI & Firebase JSON in `appsettings.Development.json`**

3. **Run API**

   ```bash
   dotnet build
   dotnet run
   ```

4. **Test API with Postman or Swagger**

---

## 🌐 Frontend (React – Coming Soon)

> Will be hosted separately in `/frontend` folder of the repo.
> Expected stack: **React + Firebase**

---

## 🙌 Credits

* Built by Jaikrishna Binnar(https://github.com/jaikrushna)
* Designed with care for Indian artisans and small businesses

---
