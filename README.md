<!-- Last updated: 2025-10-05 | Version 1.0.0 -->

**English** | [German](README.de.md)

> **Translation:** ✅ Complete | 🪧 Last updated: 2025-10-05

---


# About the Project

The **Office-Tracker** was started and implemented as a small side project alongside my current retraining as an IT specialist in application development.
The idea was for my fellow students and myself to have a small helper program to keep track of office attendance rates.
The plan with this desktop application is to continuously develop it further and offer it as an open-source project,
so that others who are interested can also contribute.

## ✨ Features Overview

### 🚀 Quick Start with Setup Wizard
On first launch, a simple wizard guides you through the few necessary steps to provide initial data for the application to work with.
You'll be able to enter a name, your default home office days (Mon. – Fri.), as well as your target quota that you must maintain at minimum/maximum
to avoid getting into trouble. At the end, you'll get a small summary and can review your entered data once more & if necessary,
return to previous steps to make changes.

![Setup-Assistent Start](Assets/Screenshots/wizard.png)

### 🗓️ Track Daily Attendance
Simply log once per day whether you're working from home or at the office/location.

![Setup-Assistent Start](Assets/Screenshots/add_current_day.png)

### 📊 Attendance Statistics
In the upper section of the application, you can always keep an eye on your current attendance statistics and see at a glance how your ratio compares to your target quota.

![Setup-Assistent Start](Assets/Screenshots/stats_overview.png)

### 🔮 Weekly Forecast (Statistics)
Based on your current attendance statistics and your planned days, the application calculates a kind of forecast for the upcoming weeks.
This allows you to see early on, without having to calculate yourself, when your target quota will be reached again. More details and information are already planned here.

![Setup-Assistent Start](Assets/Screenshots/calculated_weeks.png)

### 📝 Plan Days
You have the option, as briefly mentioned above, to plan days in advance. Enter ahead of time whether you want to change a default home office day to
an office/location day – or vice versa. This is meant to help you better manage your planning with regard to your target quota without much stress,
which you already have plenty of in everyday life.

![Setup-Assistent Start](Assets/Screenshots/plannable_days.png)

### 💾 Local Data Storage
The data is not stored on any external server but uses an SQLite database. No cloud or anything else – all information is
therefore 100% under your control. Need a new dataset? No problem – simply remove/move the database file and the program will
guide you through the wizard again.

---

## 🛠️ Installation

1. Download the latest version of Office-Tracker for your operating system from the [Releases](https://github.com/JumpSpinn/gfn-office-tracker/releases).
	* `OfficeTracker-win-x64.exe` for Windows
	* `OfficeTracker-linux-x64` / `.zip` / `.tar.gz` for Linux
2. Extract the downloaded file if necessary (if it's a .zip/.tar.gz).
3. Run the `OfficeTracker.exe` (Windows) or the `OfficeTracker` executable file (Linux).

## 📄 License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

---
