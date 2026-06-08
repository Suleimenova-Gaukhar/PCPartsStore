# PC Parts Store - Suleimenova Gaukhar

A desktop application built with Avalonia UI and C# for the "Medii și Tehnologii de Programare" subject. The app simulates a PC components store where customers can browse and buy parts, and an admin can manage the store.

---

## How to run

1. Make sure you have .NET 9 SDK installed
2. Clone or download the project
3. Open a terminal in the `PCPartsStore` folder
4. Run:
```
dotnet run
```
The database is created automatically on first launch with sample products already loaded.

---

## How the app works

When you open the app you get to choose who you are — Customer or Admin. Each role has its own set of pages and functionality.

### Customer

- **Catalog** — browse all PC components, search by name or manufacturer, filter by category (CPU, GPU, RAM, etc.). Each product shows its status (Active, Low stock, Sold out) and price in RON. You can add items to the cart directly from the list.
- **Cart** — review what you added, change quantities, remove items. The app won't let you add more than the available stock. Total price updates automatically.
- **Checkout** — fill in your name, email and phone number to place the order. All fields are required. After a successful order you get redirected to My Orders with a confirmation message.
- **My Orders** — see all your past orders, click on one to see its full details. You can cancel an order as long as it hasn't been processed yet.

### Admin

- **Products** — same view as the catalog but with edit buttons instead of add to cart. Search and filter work the same way. You can add new products with the button in the top right.
- **Add product** — fill in name, manufacturer, category, price and stock. Status (Active/Low stock/Sold out) is calculated automatically based on the stock number.
- **Edit product** — edit any field of an existing product and save. You can also delete a product — a confirmation step prevents accidental deletions.
- **All orders** — see every order placed by all customers with their details and customer info. You can change the status of an order (Confirmed → On the way → Delivered). Once an order is Delivered or Cancelled the status is locked and cannot be changed.
- **Statistics** — see total revenue broken down by component category, plus total sales for today, this week, this month and this year.

---

## Architecture

The app uses the **MVVM pattern**.

```
Models/       — plain data classes (Component, Order, CartItem, Client)
Services/     — business logic and database access (ProductService, OrderService, CartService)
ViewModels/   — one ViewModel per page, handles all logic and data binding
Views/        — XAML files that define the UI, no logic here
Data/         — AppDbContext for SQLite database access
```

- **Models** hold the data
- **Services** talk to the database and handle operations like placing orders or updating stock
- **ViewModels** connect the UI to the services using data binding and commands
- **Views** just show what the ViewModel provides — they don't know anything about the database

Navigation works through `MainWindowViewModel` which holds a `CurrentView` property. When you click a sidebar button it swaps `CurrentView` to a different ViewModel and `ViewLocator` automatically finds and renders the matching View.

---

## Tech stack

- **Avalonia UI 12** — cross-platform desktop UI framework
- **C# / .NET 9** — programming language and runtime
- **SQLite** — local database stored as a single `.db` file
- **Entity Framework Core 9** — ORM for database access
- **CommunityToolkit.Mvvm** — provides `[ObservableProperty]` and `[RelayCommand]` for clean MVVM code

---

## Notes

- The database is created automatically on first run — no setup needed
- All prices are in RON
- Stock levels update automatically after each purchase
- The status of a component (Active / Low stock / Sold out) is calculated automatically: 0 = Sold out, 1-4 = Low stock, 5+ = Active