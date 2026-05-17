# PROJECT TITLE
**AI-Powered Real-Time Hospital Management System**

# TECH STACK
**Frontend:**
* ASP.NET Core MVC (.NET 8)
* Razor Views
* Bootstrap 5
* JavaScript
* Chart.js
* FontAwesome

**Backend:**
* C#
* ASP.NET Core MVC
* Entity Framework Core
* ASP.NET Identity

**Database:**
* Microsoft SQL Server LocalDB
* Entity Framework Migrations

**Real-Time Features:**
* SignalR
* Toastr Notifications

---

## 1. PROJECT OVERVIEW
### Purpose of the Project
The AI-Powered Real-Time Hospital Management System is an enterprise-grade web application designed to digitize, streamline, and automate the daily operations of a modern hospital facility. 

### Business Goals
* **Operational Efficiency:** Reduce administrative overhead by digitizing patient records, appointments, and billing.
* **Improved Patient Care:** Provide doctors with immediate access to medical histories and prescription tools.
* **Real-Time Communication:** Minimize wait times and bottlenecks via instant notifications using SignalR.
* **Scalability:** Build a robust, N-tier architecture that can grow from a single clinic to a multi-branch hospital network.

### Target Users
* **Administrators:** Oversee hospital operations, manage staff, and analyze financial/operational metrics.
* **Doctors:** Manage patient appointments, view medical records, and issue prescriptions.
* **Receptionists/Front Desk:** Handle patient registration, appointment scheduling, and initial billing.
* **Patients:** (Future scope) Access personal health records, view prescriptions, and book appointments online.

### Real-World Problem Being Solved
Traditional hospitals suffer from siloed data, paper-based records, and high latency in communication between departments. This system solves these issues by creating a centralized, real-time source of truth. When a doctor issues a prescription, the billing department is instantly updated, and the patient's medical record is automatically appended.

### Overall Workflow
1. **Registration:** Patient is registered in the system.
2. **Scheduling:** An appointment is booked with a specific doctor/department.
3. **Consultation:** Doctor accesses the patient's record, conducts the consultation, and creates a prescription.
4. **Billing:** The system automatically generates an invoice based on the consultation and prescribed items.
5. **Analytics:** Dashboards update in real-time to reflect the day's revenue, patient flow, and departmental load.

---

## 2. SYSTEM ARCHITECTURE
This application follows a highly decoupled **Clean N-Tier Architecture** utilizing the MVC pattern.

### Architectural Patterns Used
* **MVC (Model-View-Controller):** Separates the user interface (Views) from the routing/HTTP handling (Controllers) and the domain logic (Models).
* **Repository Pattern:** Abstracts the Data Access Layer (DAL). It acts as an in-memory collection of domain entities, completely hiding Entity Framework Core logic from the business layer.
* **Service Pattern:** Contains all the Business Logic. Services orchestrate operations, enforce business rules, and interact with Repositories.
* **Dependency Injection (DI):** ASP.NET Core's built-in IoC container is used to inject Services into Controllers, and Repositories into Services. This ensures loose coupling and high testability.

### Workflow & Data Flow
**Controller → Service → Repository → DbContext → SQL Server**

1. **Request:** The user interacts with the UI, sending an HTTP request to a **Controller**.
2. **Validation & Routing:** The Controller validates the incoming `ViewModel`/`DTO`. If valid, it passes the data to the appropriate **Service**.
3. **Business Logic:** The Service applies business rules (e.g., "Is the doctor available at this time?").
4. **Data Access:** The Service calls the **Repository** to fetch or persist data.
5. **Database Execution:** The Repository translates the request into LINQ queries via EF Core's **DbContext**, executing SQL against the **SQL Server**.
6. **Response:** The data flows back up the chain, is mapped to a `ViewModel`, and returned as a View or JSON response by the Controller.

### Identity & Real-Time Workflows
* **Identity Workflow:** Uses ASP.NET Core Identity for robust authentication and role-based authorization (RBAC). Cookie-based sessions secure all endpoints.
* **SignalR Workflow:** A persistent WebSocket connection is maintained between the client and server. When a database event occurs (e.g., a new appointment), the server pushes a payload to the specific user's browser, triggering a Toastr notification.

---

## 3. COMPLETE PROJECT STRUCTURE

```text
AIHospitalManagementSys/
│
├── Areas/             # Multi-tenant UI partitions (Admin, Doctor, Patient). Contains isolated Controllers/Views.
├── Constants/         # System-wide constants, Enums (e.g., Roles, Status Codes), and static configuration strings.
├── Controllers/       # Global routing controllers (e.g., Authentication, Home) that do not belong to a specific Area.
├── Data/              # ApplicationDbContext, EF Core Migrations, and SeedData classes.
├── DTOs/              # Data Transfer Objects. Used to pass strict data shapes between APIs/Services without exposing Domain models.
├── Helpers/           # Utility classes, extension methods, and shared logic (e.g., File Upload helpers).
├── Hubs/              # SignalR Hubs (e.g., NotificationHub) managing real-time WebSocket connections.
├── Mappings/          # AutoMapper profiles defining translation logic between Domain Entities ↔ DTOs ↔ ViewModels.
├── Middleware/        # Custom HTTP Pipeline interceptors (e.g., Global Exception Handling, Request Logging).
├── Models/            # Core Domain Entities mapping directly to SQL Tables (e.g., ApplicationUser, Patient, Doctor).
├── Repositories/      # Data Access Layer. Contains generic and entity-specific repository interfaces and implementations.
├── Services/          # Business Logic Layer. Where the actual application rules reside.
├── Validators/        # FluentValidation rules for strict incoming request validation.
├── ViewModels/        # Classes specifically tailored to carry data required by Razor Views.
├── Views/             # Global Razor Views, Shared Layouts, and partials.
└── wwwroot/           # Static assets: CSS, JS (Chart.js, custom scripts), Images, and external libraries.
```

---

## 4. DATABASE DESIGN
The database is heavily normalized, utilizing Entity Framework Core Code-First approach.

### Core Architecture Concepts
* **BaseEntity:** An abstract base class containing common audit properties (`Id`, `CreatedAt`, `UpdatedAt`, `IsDeleted`). All domain models inherit from this to ensure consistent auditing and support soft-deletes.
* **Central Entity (Appointment):** The `Appointment` table is the nexus of the database. It ties together the `Patient`, the `Doctor`, the `Prescription`, and the `Bill`. Nearly all major queries pivot around appointments.
* **Prescription & PrescriptionItems Separation:** A `Prescription` represents the overall medical directive from a consultation. `PrescriptionItems` are split into a separate table (1-to-Many) to allow itemized tracking of medicines, dosages, and durations, which is critical for accurate pharmacy billing and analytics.

### Key Relationships
* **ApplicationUser (Identity):** 1-to-1 with `Doctor`, `Patient`, and `Admin` profiles.
* **Department to Doctor:** 1-to-Many.
* **Doctor to Appointment:** 1-to-Many.
* **Patient to Appointment:** 1-to-Many.
* **Appointment to Prescription:** 1-to-1.
* **Appointment to Bill:** 1-to-1.
* **Prescription to PrescriptionItems:** 1-to-Many.

---

## 5. IMPLEMENTED MODULES

### Authentication & Identity
* **Purpose:** Secure the application and manage user access.
* **Features:** Login, Registration, Password Reset, Role-based Access Control (RBAC).
* **Architecture:** Utilizes ASP.NET Core Identity.

### Admin Dashboard
* **Purpose:** Central command center for hospital administrators.
* **Features:** Real-time metrics, revenue charts, department load visualizations.
* **Architecture:** Chart.js integration fetching aggregated data via DashboardService.

### Department Management
* **Purpose:** Organize hospital specialties.
* **Features:** CRUD operations for departments (e.g., Cardiology, Neurology).

### Doctor Management
* **Purpose:** Manage medical staff.
* **Features:** Doctor profiles, department assignments, scheduling, and availability tracking.

### Patient Management
* **Purpose:** Master patient index.
* **Features:** Patient registration, demographic tracking, medical history aggregation.

### Appointment Management
* **Purpose:** The core scheduling engine.
* **Features:** Booking, status tracking (Pending, Confirmed, Completed, Cancelled), doctor-patient assignment.

### Billing System
* **Purpose:** Financial tracking.
* **Features:** Automatic invoice generation post-appointment, payment status tracking.

### Prescription System & Medical Records
* **Purpose:** Digital medical directives.
* **Features:** Creation of prescriptions, itemized medicines, dosage instructions.

### Notifications & SignalR Real-Time Features
* **Purpose:** Instant communication.
* **Features:** Toastr alerts pushed to specific users when appointments are booked or statuses change.

---

## 6. CURRENT IMPLEMENTATION STATUS

### COMPLETED
* Authentication & Identity Engine
* Base Architecture (N-Tier, Repositories, Services)
* Admin Dashboard UI & Logic
* Department, Doctor, and Patient CRUD Modules
* Appointment Management Core
* Prescription System
* SignalR Real-Time Infrastructure

### IN PROGRESS
* Billing & Invoicing Automation (Connecting Prescriptions to Bills)
* Advanced Medical Records Analytics
* AI integration for scheduling optimizations

### PENDING
* Online Payment Gateway Integration
* Patient-facing portal (Self-service booking)
* Automated SMS/Email Appointment Reminders
* Pharmacy Module integration

---

## 7. ROLE-BASED SYSTEM DESIGN

### Admin Workflows
* **Permissions:** Superuser. Full access to all data.
* **Responsibilities:** Manage departments, doctors, and system settings. View financial analytics.

### Doctor Workflows
* **Permissions:** Scoped to assigned patients and appointments.
* **Responsibilities:** View daily schedules, review patient medical histories, write prescriptions, and complete appointments.

### Receptionist Workflows (Future/Implicit Admin)
* **Permissions:** Scoped to scheduling and registration.
* **Responsibilities:** Register walk-in patients, manage appointment queues, collect payments.

### Patient Workflows (Pending)
* **Permissions:** Strictly scoped to own data.
* **Responsibilities:** Book appointments online, view digital prescriptions, pay bills.

---

## 8. DASHBOARD SYSTEM
Dashboards are context-aware and role-specific.

* **Admin Dashboard:** Focuses on macro metrics. Includes KPI Cards (Total Revenue, Active Doctors, Total Patients) and Chart.js visualizations showing week-over-week patient flow and departmental revenue distributions.
* **Doctor Dashboard:** Focuses on micro, operational metrics. Displays today's appointment queue, pending consultations, and quick links to patient medical histories.
* **Patient Dashboard (Pending):** Will display upcoming appointments, recent prescriptions, and outstanding bills.

---

## 9. REAL-TIME FEATURES
The application relies heavily on **SignalR** to eliminate page reloads for critical updates.

* **NotificationHub:** The central WebSocket hub routing messages to connected clients.
* **Workflow:** When an entity is modified (e.g., `AppointmentService.ConfirmAppointment()`), the Service injects `IHubContext<NotificationHub>`. It sends an async payload to the specific Doctor's User ID. The client-side JavaScript intercepts this payload and triggers a Bootstrap/Toastr notification instantly.

---

## 10. BILLING & PRESCRIPTION FLOW
The most critical workflow in the system ensuring clinical actions translate to financial records.

1. **Appointment Completion:** Doctor marks the appointment as 'Completed'.
2. **Prescription Generation:** Doctor creates a `Prescription` linked to the `AppointmentId`.
3. **Itemization:** Doctor adds `PrescriptionItems` (Medicines, tests).
4. **Bill Generation (Event):** The system calculates the consultation fee + total cost of `PrescriptionItems`.
5. **Invoice:** A `Bill` entity is created with status 'Unpaid'. It is immediately visible on the Admin dashboard and Patient billing queue.

---

## 11. CODING STANDARDS & RULES
To maintain enterprise quality, all developers **MUST** adhere to the following rules:

* **Strict Layer Separation:** **NO** business logic inside Controllers. Controllers handle HTTP, routing, and validation. Everything else belongs in the `Services` layer.
* **No Direct DbContext in Views or Controllers:** The UI and Controllers must NEVER interact with Entity Framework. Use Dependency Injection to call a Service, which calls a Repository.
* **Asynchronous Programming:** All database and I/O operations must use `async/await`. Use `Task<T>` for all service and repository method signatures.
* **View Models & DTOs:** Never pass Domain Entities directly to Views. Always map Domain Models to `ViewModels` before passing them to the UI to prevent over-posting attacks and decouple the database schema from the UI.
* **Naming Conventions:** 
  * Interfaces must start with `I` (e.g., `IPatientService`).
  * Async methods must end with `Async` (e.g., `GetPatientByIdAsync`).
  * Private readonly fields must use `_camelCase`.
* **Validation:** Use FluentValidation for backend validation, coupled with DataAnnotations for client-side unobtrusive validation.

---

## 12. DATABASE CONNECTION & MIGRATIONS
The project utilizes Microsoft SQL Server LocalDB for development.

* **Connection String:** Located in `appsettings.json`.
  * `"Server=(localdb)\\MSSQLLocalDB;Database=AIHospitalDb;Trusted_Connection=True;MultipleActiveResultSets=true"`
* **EF Core Migrations Workflow:**
  * When domain models change, open Package Manager Console.
  * Run: `Add-Migration <DescriptiveName>`
  * Run: `Update-Database`
* Do not manually edit the SQL schema. Always use EF Core Migrations to ensure state tracking.

---

## 13. HOW TO RUN THE PROJECT
### Setup Guide for New Developers
1. **Prerequisites:** Install Visual Studio 2022 (with ASP.NET & web development workload) and .NET 8 SDK.
2. **Clone:** Pull the latest code from the `main` branch.
3. **Restore Packages:** Open the solution and build it to restore all NuGet packages.
4. **Database Setup:** 
   * Open Package Manager Console.
   * Run `Update-Database` to create the LocalDB instance and apply all schemas.
5. **Run the Project:** Press `F5` or click "Run".
6. **Default Credentials:**
   * The `SeedData.cs` automatically creates a default Admin.
   * Email: `admin@hospital.com`
   * Password: `Admin123!` (Check SeedData for exact details).

---

## 14. HOW TEAM MEMBERS SHOULD CONTRIBUTE
* **Branching Strategy:** We use Feature Branching. Create a branch from `main` named `feature/your-module-name`.
* **Adding a New Module:**
  1. Create Domain Entity in `Models/`.
  2. Generate EF Migration.
  3. Create `IRepository` and `Repository` implementation.
  4. Create `IService` and `Service` implementation. Register them in `Program.cs` for DI.
  5. Create `ViewModel`.
  6. Create `Controller` and `Views`.
* **Pull Requests:** Never push directly to `main`. Open a Pull Request. Ensure the project builds, and check that no business logic has leaked into the controllers before requesting a review.

---

## 15. FUTURE ENHANCEMENTS
* **AI & Machine Learning:** Predictive analytics for patient influx and intelligent appointment scheduling.
* **Advanced Analytics:** Custom report builders for hospital administrators.
* **Online Payments:** Stripe/PayPal integration for telemedicine and prescription billing.
* **Cloud Deployment:** Migration to Microsoft Azure (App Services + Azure SQL).
* **Mobile App:** A companion mobile app using an exposed REST/GraphQL API.

---

## 16. FINAL PROJECT STATUS SUMMARY
The **AI-Powered Real-Time Hospital Management System** is currently in a highly stable, mature development state. The core architectural foundations—Clean N-Tier separation, generic repository patterns, and identity management—are fully established and proven. 

The application is highly scalable; new modules can be plugged in via the Service/Repository layers without destabilizing existing features. With real-time SignalR integrations already active, the system provides a modern, responsive user experience. It is nearing production readiness, pending the finalization of the billing loop and comprehensive unit testing.
