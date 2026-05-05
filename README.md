# Project Status: Backend API - Notes App

Technical summary of the current infrastructure and system configuration for the Notes application backend.

## 🏗️ System Architecture
The system utilizes a decoupled cloud-based architecture:
- **Framework:** .NET 10 Web API.
- **Database:** Azure SQL Database (Relational).
- **Hosting:** Azure App Service (F1 - Free Plan).
- **Deployment:** Integrated CI/CD via GitHub Actions.

## ⚙️ Implemented Configurations

### 1. Data Layer (Azure SQL)
- **Instance:** Logical SQL Server provisioned in a region with available quota.
- **Network Security:** - Firewall enabled for internal Azure service communication.
    - Local IP whitelisting configured for development environment access.
- **Access:** Configured via SQL Server Authentication.

### 2. Application Layer (App Service)
- **Platform:** App Service running on a .NET-optimized environment.
- **Secret Management:** Connection string injection implemented via Azure Environment Variables (`ConnectionStrings__DefaultConnection`).
- **Code Security:** The `appsettings.json` file in the repository contains no sensitive credentials, delegating real authentication to the Azure Portal configuration.

### 3. Deployment Workflow (CI/CD)
- **Repository:** GitHub (`MyNotesApp`).
- **Automation:** A production-ready workflow is established where every push to the `main` branch triggers a GitHub Action to build, test, and deploy the code directly to the Azure production environment.

## 📍 Current Status
- **Infrastructure:** Fully deployed and operational.
- **Connectivity:** Link between App Service and SQL Database verified through environment variables.
- **Codebase:** Repository synchronized and hardened against credential leaks.