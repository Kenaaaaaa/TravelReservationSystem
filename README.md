# ✈️ Brikena Travel Reservation System

A modern and user-friendly ASP.NET Core MVC application that allows users to browse and book travel trips, and provides an admin panel to manage trips and reservations.

## 🌐 Features

- 🔐 User Registration & Login (with role-based access: Admin/User)
- 📅 Trip Listing & Reservation
- ✅ Admin Dashboard:
  - Add/Edit/Delete Trips
  - View & Approve/Refuse Pending Reservations
- 🎨 Modern UI with animations, waves, plane effects, and carousel
- 🔒 Session-based authentication
- 📊 Real-time reservation notification badge for admin

## 📁 Project Structure

```
TravelReservationSystem/
│
├── Controllers/          # MVC controllers (Trip, User, Reservation)
├── Models/               # Entity models
├── Views/                # Razor views per controller
│   ├── Trip/
│   ├── User/
│   ├── Reservation/
│   └── Shared/
├── wwwroot/              # Static files (CSS, JS, images)
├── Data/                 # AppDbContext and seed data
├── Helpers/              # Utility functions (e.g., AdminHelper)
└── README.md             # This file
```

## 🛠️ Technologies

- ASP.NET Core MVC
- Entity Framework Core (with SQLite/MySQL support)
- Bootstrap 5
- Session-based authentication
- HTML/CSS/JS (custom animations and effects)

## 🚀 Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/yourusername/TravelReservationSystem.git
cd TravelReservationSystem
```

### 2. Set up the database

Update the `appsettings.json` connection string, then run:

```bash
dotnet ef database update
```

### 3. Run the app

```bash
dotnet run
```

Open [http://localhost:5233](http://localhost:5233) in your browser.

## 👩‍💻 Default Roles

- If the email contains **"admin"**, the user is registered as an **Admin**.
- Otherwise, the user is registered as a **User**.

## 📧 Contact

If you have any questions or suggestions, please contact us at:  
📩 [info@brikenatravel.com](mailto:info@brikenatravel.com)

---

Enjoy your trip management journey! 🌍💼✈️
