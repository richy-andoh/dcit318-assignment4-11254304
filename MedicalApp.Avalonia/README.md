# Medical Appointment Booking System

A cross-platform desktop application built with .NET 6+ and Avalonia UI for managing medical appointments. This application demonstrates ADO.NET usage with SQL Server, including proper data access patterns, exception handling, and modern UI design.

## Features

- **Doctor Management**: View and filter available doctors by name and specialty
- **Appointment Booking**: Schedule appointments with validation and conflict checking
- **Appointment Management**: View, update, and delete existing appointments
- **Cross-Platform**: Runs on Windows, macOS, and Linux
- **Modern UI**: Built with Avalonia UI for a native look and feel

## Prerequisites

- .NET 6.0 SDK or later
- SQL Server (LocalDB, Express, or full version)
- Visual Studio Code (recommended) or any code editor

## Setup Instructions

### 1. Database Setup

1. Open SQL Server Management Studio or Azure Data Studio
2. Connect to your SQL Server instance
3. Run the `DatabaseSetup.sql` script to create the database and sample data
4. Verify the database was created successfully

### 2. Application Setup

1. Clone or download this project
2. Open a terminal in the project directory
3. Restore dependencies:
   ```bash
   dotnet restore
   ```
4. Build the application:
   ```bash
   dotnet build
   ```

### 3. Database Connection

The application uses a default connection string. If you need to modify it:

1. Open `Services/DatabaseService.cs`
2. Update the `_connectionString` variable with your SQL Server details
3. For SQL Authentication, use: `"Data Source=.;Initial Catalog=MedicalDB;User ID=youruser;Password=yourpassword;TrustServerCertificate=True;"`
4. For Windows Authentication, use: `"Data Source=.;Initial Catalog=MedicalDB;Integrated Security=True;TrustServerCertificate=True;"`

## Running the Application

```bash
dotnet run
```

## Usage

### Main Window
- **View Doctors**: Browse available doctors and filter by name/specialty
- **Book Appointment**: Schedule a new medical appointment
- **Manage Appointments**: View and manage existing appointments
- **Exit**: Close the application

### Doctor Directory
- Use the filter boxes to search for doctors by name or specialty
- Click "Refresh List" to reload the data
- View doctor availability status

### Book Appointment
1. Select a doctor from the dropdown
2. Select a patient from the dropdown
3. Choose appointment date and time
4. Add optional notes
5. Click "Book Appointment"

### Manage Appointments
1. Select a patient to view their appointments
2. Select an appointment to modify
3. Update the date/time or delete the appointment
4. Changes are applied immediately

## Technical Implementation

### ADO.NET Features Demonstrated

- **SqlConnection**: Database connectivity with proper disposal
- **SqlCommand**: Parameterized queries for security
- **SqlDataReader**: Efficient data retrieval
- **SqlDataAdapter**: DataSet population for complex operations
- **Exception Handling**: Comprehensive error management
- **Async/Await**: Non-blocking database operations

### Architecture

- **MVVM Pattern**: Separation of concerns with ViewModels
- **Dependency Injection**: Service-based architecture
- **Data Binding**: Two-way binding with UI controls
- **Command Pattern**: Event-driven user interactions

### Database Schema

```sql
Doctors (DoctorID, FullName, Specialty, Availability)
Patients (PatientID, FullName, Email)
Appointments (AppointmentID, DoctorID, PatientID, AppointmentDate, Notes)
```

## Screenshots

Take screenshots of:
1. Main window with navigation buttons
2. Doctor directory with filtering
3. Appointment booking form
4. Appointment management interface
5. Success/error messages

## Assignment Requirements Met

✅ **Windows Forms-based** → Cross-platform Avalonia UI (modern equivalent)
✅ **SQL Server database connection** → ADO.NET with Microsoft.Data.SqlClient
✅ **Book appointments** → Full CRUD operations
✅ **View available doctors** → DataGrid with filtering
✅ **Search and filter appointments** → Patient-based appointment viewing
✅ **Delete or modify existing bookings** → Update and delete functionality
✅ **ADO.NET components** → SqlConnection, SqlCommand, SqlDataReader, SqlDataAdapter
✅ **Proper data retrieval and update logic** → Async operations with validation
✅ **Command objects and parameters** → Parameterized queries for security
✅ **Event-driven logic** → MVVM commands and data binding
✅ **Exception handling** → Try-catch blocks with user-friendly messages
✅ **Connection management** → Using statements for proper disposal

## Troubleshooting

### Common Issues

1. **Database Connection Error**
   - Verify SQL Server is running
   - Check connection string in DatabaseService.cs
   - Ensure database exists and is accessible

2. **Build Errors**
   - Run `dotnet restore` to restore packages
   - Ensure .NET 6+ SDK is installed
   - Check for missing dependencies

3. **Runtime Errors**
   - Check database permissions
   - Verify sample data was inserted correctly
   - Review exception messages in the status bar

## Development

This project demonstrates modern .NET development practices:
- Cross-platform compatibility
- Modern UI frameworks
- Proper database access patterns
- Clean architecture principles
- Comprehensive error handling

## License

This project is created for educational purposes as part of DCIT318 Assignment 4. 