# Device Manufacturing Inventory Manager

A Windows Forms application built with C# and PostgreSQL, designed to manage inventory, tracking, device assembly, and user accountability in a small-to-medium device manufacturing operation.

---

## Table of Contents
- [Overview](#overview)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Database Setup](#database-setup)
  - [Running the Application](#running-the-application)
- [Usage](#usage)
- [Troubleshooting](#troubleshooting)
- [License](#license)

---

## Overview

**Device Manufacturing Inventory Manager** simplifies the process of managing components, devices, and their associated bill of materials (BOM). It's tailored for use in small-scale manufacturing setups where it's essential to track parts usage, device assembly, and user activity for accountability and traceability.

Whether you're assembling complex electronics or building mechanical devices, this system ensures that every part movement is logged, and every device build is accounted for in your PostgreSQL database.

---

## Features

### Devices & Parts Management
- Add, edit, and delete parts or devices.
- Manage quantities, part numbers, descriptions.

### Bill of Materials (BOM)
- Create BOMs for each device, specifying required parts and their quantities.
- Update and revise BOMs as product designs evolve.

###  Build Device Functionality
- Instantly deducts necessary parts from inventory when building a device.
- Generates logs recording each device build, who performed it, and when.

### User Action Logging
- Tracks and stores every key action performed by a user, including:
  - Device builds
  - Part additions or removals
  - Inventory adjustments
- Supports multi-user environments with distinct logins for accountability.

###  Database Logging
- All operations (inserts, updates, deletions) are logged in the PostgreSQL backend.
- Enables full traceability of inventory and build actions for auditing or reviews.

---

## Tech Stack

- **Frontend:** Windows Forms using C#
- **Backend Database:** PostgreSQL
- **Database Access:** Npgsql (PostgreSQL .NET Data Provider)

## Getting Started

### Prerequisites

Ensure you have the following installed on your development machine:

- [.NET 8.0 Framework](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) 
- [PostgreSQL](https://www.postgresql.org/download/)

### Installation

Download the latest release from the [Releases](#) section.

### Database Setup

1. Create a PostgreSQL database.
2. Run the `schema.sql` script provided, to create the necessary tables and initial structure.
3. Configure user roles and permissions according to the example in `schema.sql`.

### Running the Application

1. Launch the application by running the compiled `.exe`.
2. On startup, enter your PostgreSQL connection credentials.
3. You’re ready to go!

---

## Usage

- **Inventory Management:** Add, update, or remove parts as needed using the dedicated interface.
- **Build a Device:** Select a device and initiate a build. The system will adjust inventory levels accordingly.
- **User Accounts:** Log in with your credentials to track your activities in the database logs.
- **Report Broken Parts:** Adjust the inventory levels when parts are broken.
- **Calculate BOM** calculate how many parts you need to buy to complete given amount of devices.
- 
> Tip: Use the `-d` argument when launching the application to enable a debug console, helpful for diagnosing database or connection issues.

---

## Troubleshooting

- **Database Connection Errors:** Ensure your connection string is correct and that the PostgreSQL server is reachable.
- **Permissions Issues:** Make sure the PostgreSQL user has the correct read/write access to the necessary tables.
- **Missing Tables or Columns:** Confirm that you've run the latest version of `schema.sql`.

---

## License

This project is licensed under the MIT License.  
You are free to use, modify, and distribute this software as needed.  
See [`LICENSE`](./LICENSE) for full license text.
