<div align="center">
  <img src="https://raw.githubusercontent.com/atanu16/Incident-Tracker/main/ICO.png" alt="Incident Tracker Logo" width="100"/>
  <h1>Incident Tracker 🎯</h1>
  <p>A lightning-fast, premium desktop application for tracking, managing, and resolving incidents — backed locally by Excel.</p>

  <p>
    <a href="https://dotnet.microsoft.com/"><img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet&logoColor=white" alt=".NET 8.0" /></a>
    <a href="#"><img src="https://img.shields.io/badge/Platform-Windows-0078D6?style=flat-square&logo=windows&logoColor=white" alt="Platform" /></a>
    <a href="#"><img src="https://img.shields.io/badge/Architecture-MVVM-6C63FF?style=flat-square" alt="MVVM" /></a>
    <a href="#"><img src="https://img.shields.io/badge/License-MIT-success?style=flat-square" alt="License" /></a>
  </p>
</div>

---

## ✨ Features

- **🚀 Local-First Architecture**: Completely cloud-free. Data is instantly saved and retrieved from a local Excel file (`XYZ.xlsx`) utilizing the blazing-fast `ClosedXML` library. No network latency, no subscriptions.
- **🎨 Premium UI/UX Ecosystem**: A meticulously crafted interface featuring complete glassmorphism effects, custom window controls, beautifully styled DataGrids, completely custom floating Scrollbars, and elegant popup ComboBoxes.
- **🌓 Dynamic Theme Engine**: First-class support for both **Dark** and **Light** modes. Theme swapping happens instantaneously with fully dynamic resource dictionaries without restarting the app.
- **📊 Real-Time Dashboard**: Get immediate insight into your operations with a real-time count of active vs. completed incidents, filtering options, and inline loading indicators.
- **⚡ Advanced MVVM Design**: Built with clean, modern C# and strict MVVM architecture. Zero heavy MVVM frameworks — utilizing lightweight, highly-optimized async `RelayCommand` implementations.

---

## 📸 Screenshots

*(Add screenshots of your application here — simply drag and drop them into GitHub when editing)*

| Dark Mode                                      | Light Mode                                     |
| ---------------------------------------------- | ---------------------------------------------- |
| `![Dark Mode Placeholder](link_to_dark_image)` | `![Light Mode Placeholder](link_to_light_image)` |

| Edit Mode                                      | Modals & Dropdowns                             |
| ---------------------------------------------- | ---------------------------------------------- |
| `![Edit Mode Placeholder](link_to_edit_image)` | `![Modal Placeholder](link_to_modal_image)`    |

---

## 🛠️ Technology Stack

* **Framework:** `.NET 8.0` (WPF - Windows Presentation Foundation)
* **Language:** `C# 12.0` / `XAML`
* **Data Access:** `ClosedXML` (for direct XLSX read/write)
* **Architecture:** Pattern-compliant MVVM (Model-View-ViewModel)

---

## 🚀 Getting Started

### Prerequisites
* Windows 10 or Windows 11
* [.NET 8.0 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Installation & Run

1. **Clone the repository**
   ```bash
   git clone https://github.com/atanu16/Incident-Tracker.git
   cd "Incident Tracker"
   ```

2. **Restore dependencies & Build**
   ```bash
   dotnet restore
   dotnet build -c Release
   ```

3. **Run the Application**
   ```bash
   dotnet run
   ```

4. **Publish the application in exe**
   ```bash
   dotnet publish IncidentTracker.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o ./publish
   ```
   The exe will be located at `Root\publish\IncidentTracker.exe`

   > **Note:** The application expects a valid `XYZ.xlsx` file in the root execution directory. If one isn't found, it will safely handle it depending on setup. 

---

## 📁 Project Structure

```bash
📦 IncidentTracker
 ┣ 📂 Commands            # Lightweight custom async RelayCommands
 ┣ 📂 Controls            # Reusable core UI components (Notifications, etc)
 ┣ 📂 Converters          # IValueConverters for XAML data binding (Booleans, Status Colors)
 ┣ 📂 Models              # Business logic entities and enums
 ┣ 📂 Services            # Excel Service handling raw data flow & Theme switching
 ┣ 📂 Themes              # Global dictionaries (DarkTheme.xaml, LightTheme.xaml, Styles.xaml)
 ┣ 📂 ViewModels          # ViewModel logic mapping strictly to Views
 ┣ 📂 Views               # Independent UserControls (Dashboard, Edit, AllRecords, AddRecord)
 ┣ 📜 App.xaml            # Global application entrypoint
 ┗ 📜 MainWindow.xaml     # The custom window shell hosting the navigation logic
```

---

## 🤝 Contributing

Contributions, issues, and feature requests are welcome! 
Feel free to check the [issues page](https://github.com/atanu16/Incident-Tracker/issues).

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📝 License

Distributed under the MIT License. See `LICENSE` for more information.

---

<div align="center">
  <sub>Built with ❤️ by Atanu Bera</sub>
</div>