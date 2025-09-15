# 🍳 Cooking Recipes Application

[![.NET](https://img.shields.io/badge/.NET-6-blue)](https://dotnet.microsoft.com/) [![License](https://img.shields.io/badge/License-MIT-green)](LICENSE)

A **WPF desktop application** for managing users and cooking recipes. Built with **MVVM**, `INotifyPropertyChanged`, and `ObservableCollection`. Users can register, log in, update profiles, manage recipes, and export data.  

---

## 🚀 Features

### User Management
- ✅ Register new users with username & password.
- ✅ Login with secure password hashing & salt.
- ✅ Update password with old password validation.
- ✅ Store user credentials in `users.txt`.

### Profile Information
- Add/edit **Surname, Lastname, Email, Phone**.
- Delete profile information.
- Data stored in `User_Data/{username}.txt`.

### Recipe Management
- View all recipes in the repository.
- Add, edit, or delete recipes with confirmation.
- Export recipes to **CSV format**.
- Save recipes to a text file per user: `User_Recipes/{username}.txt`.

### Security & Data
- Passwords are **hashed and salted**.
- Usernames are **Base64 encoded**.
- File operations wrapped in **try-catch** blocks.

---

## 🏗 Architecture

The project follows the **MVVM pattern**:

### Models
- **User:** Stores credentials and profile info.
- **Recipe:** Stores recipe details like Food, Category, Ingredients, Instructions, etc.

### ViewModels
- **RegisterModel:** Handles registration & validation.
- **UpdatePassViewModel:** Handles password updates.
- **UserInfoWindowViewModel:** Manages user profile info.
- **ViewRecipesModel:** Handles viewing, editing, deleting, exporting, and saving recipes.

### Repositories
- **UserRepository:** Holds `ObservableCollection` of users’ info.
- **RecipeRepository:** Holds `ObservableCollection` of recipes.

### Commands
- **RelayCommand:** Implements `ICommand` for UI actions.

---

## 🛠 Installation

1. Clone the repository:

```bash
git clone https://github.com/yourusername/CookingRecipesApp.git
```

2. Open the solution in **Visual Studio**.
3. Build & run the project.

---

## 📂 Directory Structure

```
CookingRecipes/
│
├─ User_Data/           # User profile info
├─ User_Recipes/        # User saved recipes
├─ users.txt            # Login credentials
├─ Models/
│   ├─ User.cs
│   └─ Recipe.cs
├─ ViewModels/
│   ├─ RegisterModel.cs
│   ├─ UpdatePassViewModel.cs
│   ├─ UserInfoWindowViewModel.cs
│   └─ ViewRecipesModel.cs
├─ Views/               # XAML pages
├─ MainViewModel.cs
└─ RelayCommand.cs
```

---

## 💡 Usage

1. **Register a new user** via the registration window.
2. **Login** using credentials.
3. **Update password** from the profile page.
4. **Add or edit user information**.
5. **View recipes**, edit, delete, export to CSV, or save to a text file.

---

## 📈 Future Improvements

- Add **search & filter** functionality for recipes.
- Enhance **UI with modern WPF styles**.
- Encrypt user files for **extra security**.
- Support multiple **export formats** (JSON, XML).

---

## ⚖ License

This project is licensed under the **MIT License**.
