# Device Manufacturing Inventory Manager
This is a Windows Forms application built with C# and PostgreSQL that helps manage a small inventory system for a device manufacturing operation.

## Features
##### Devices & Parts Management:
Add, view, and manage devices and parts in the inventory.

##### Bill of Materials (BOM):
Define which parts and how many of each are required to build one device.

##### Build Device Function:
Automatically deducts parts based on BOM when building devices, and logs the action.

##### User Action Logging:
Tracks which user performed inventory actions (building devices, reporting parts, etc.).

##### Database Logging:
All part and device operations are logged for traceability.

## Tech Stack:
Frontend: Windows Forms (C#)
Database: PostgreSQL

## Getting Started:
##### Prerequisites:
.NET Framework (WinForms)
PostgreSQL instance with access

##### Setup:
1. Download release
2. Run scheme.sql in the PostgreSQL database
3. Create logins for other users (check scheme.sql for help)
4. Run the app and connect to PostgreSQL instance
5. Done

For troubleshooting run the app with argument `-d` to open debug console.

## License:
MIT License — Feel free to use and modify.