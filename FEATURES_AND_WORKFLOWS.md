# MediAI Hospital Management System: Features & Workflows

This document outlines all the current features, modules, and role-based workflows implemented in the MediAI Hospital Management System.

## 1. Authentication & Security
The system uses ASP.NET Core Identity for robust authentication and role-based access control (RBAC).
*   **Roles:** Admin, Doctor, Patient, Receptionist.
*   **Features:**
    *   Secure Login and Registration workflows.
    *   Password management (Change Password, Reset Password).
    *   Two-Factor Authentication (2FA) support.
    *   Role-based routing via MVC Areas (`/Admin`, `/Doctor`, `/Patient`).
    *   Automatic redirection based on user role upon login.

---

## 2. Patient Portal (`/Patient`)
The Patient portal is built with a modern, glassmorphism UI pattern (via `_PatientLayout`) providing a seamless user experience.

### Features & Workflows:
*   **Dashboard (`/Patient/Dashboard`)**: Central hub showing upcoming appointments, recent prescriptions, and pending bills.
*   **Profile Management**: 
    *   View and update personal details (Name, DOB, Address, Contact).
    *   Manage Medical History (Blood Group, Allergies, Existing Conditions, Emergency Contacts).
*   **Schedules & Appointments**:
    *   Timeline view of all past and upcoming appointments.
    *   Color-coded status indicators (Pending, Confirmed, Completed, Cancelled).
*   **AI Health Insights (UI Implemented)**:
    *   Predictive modeling for cardiovascular risk.
    *   Nutritional analysis based on recent lab results.
    *   Personalized AI health recommendations (e.g., Hydration warnings).
*   **Health Analytics (UI Implemented)**:
    *   Visual charts for Heart Rate trends, Blood Pressure analysis, and BMI tracking.
*   **Emergency Services**:
    *   One-click "S.O.S" trigger to request immediate ambulance dispatch.
    *   Quick access to hospital hotlines and primary emergency contacts.
*   **Settings**: Notification preferences and security management.

---

## 3. Doctor Portal (`/Doctor`)
The Doctor portal mirrors the modern design of the Patient portal (`_DoctorLayout`) but focuses on clinical workflows and patient management.

### Features & Workflows:
*   **Dashboard (`/Doctor/Dashboard`)**: 
    *   Overview of today's appointments, total patient volume, and completion metrics.
    *   Real-time notifications for critical patient anomalies.
*   **Patient Roster**:
    *   Searchable grid of all assigned patients.
    *   Quick access to patient ID, recent visit reason, and medical history.
*   **Consultation Schedules**:
    *   Timeline of upcoming consultations.
    *   Status indicators highlighting which appointments need attention (Start Consultation, View Notes).
*   **Prescription Management**:
    *   Create detailed digital prescriptions linked to specific appointments.
    *   Standardized drug entries including: Medicine Name, Dosage, Duration (Days), and Special Instructions.
    *   *Workflow:* Once a doctor writes and submits a prescription, they are smoothly redirected back to their Dashboard.
*   **Clinical AI Insights (UI Implemented)**:
    *   High-risk alerts (e.g., Sepsis probability for specific ICU beds).
    *   Treatment optimization suggestions (e.g., drug interaction warnings, dosage adjustments).
*   **Practice Analytics (UI Implemented)**:
    *   Metrics on patient volume, average consultation time, and patient satisfaction scores.
*   **Emergency Command Center**:
    *   Monitor active hospital codes (e.g., Code Blue).
    *   Quick triggers for hospital-wide protocols (Code Red, Rapid Response, Security).

---

## 4. Admin Portal (`/Admin`)
The administrative backbone of the application, responsible for system oversight and data management.

### Features & Workflows:
*   **Dashboard**: High-level statistical overview of the hospital (Total Users, Doctors, Patients, Appointments, Revenue).
*   **User Management**:
    *   View all registered system users.
    *   Assign and revoke roles (e.g., promoting a User to a Doctor).
*   **Doctor Management**:
    *   Add, edit, and remove doctor profiles.
    *   Manage specializations, working hours, and consultation fees.
*   **Patient Management**:
    *   Full CRUD (Create, Read, Update, Delete) operations on patient records.
    *   View comprehensive patient histories.
*   **Appointment Management**:
    *   Oversee all hospital appointments.
    *   Manually schedule, reschedule, or cancel appointments across all departments.
*   **System Auditing**:
    *   Track system-wide events and user actions via User Audit Logs for security compliance.

---

## 5. Billing & Financials (Service Layer)
A dedicated module handling the financial lifecycle of a patient's visit.

### Features & Workflows:
*   **Invoice Generation**:
    *   Create detailed bills for patients including Consultation Charges, Lab Charges, and Medicine Charges.
    *   Track Payment Status (Pending, Paid, Overdue).
*   **Workflow Integration**: Bills are inherently tied to Appointments and Prescriptions, allowing for a seamless clinical-to-financial pipeline.

---

## 6. Technical Architecture & Database
*   **Framework**: ASP.NET Core 10.0 MVC.
*   **ORM**: Entity Framework Core with SQL Server (LocalDB).
*   **Frontend**: Razor Views, Tailwind CSS (via CDN), Glassmorphism UI patterns, Toastr for notifications, Material Symbols for iconography.
*   **Service Layer Pattern**: Deeply decoupled architecture using Interfaces (e.g., `IPatientService`, `IAppointmentService`) and Repository patterns for clean, testable business logic.
*   **Database Schema Highlights**:
    *   `AspNetUsers`, `AspNetRoles` (Identity)
    *   `Doctors`, `Patients` (Domain Models linked to Identity Users)
    *   `Appointments`
    *   `Prescriptions` & `PrescriptionItems`
    *   `Bills`
    *   `UserAuditLogs`
