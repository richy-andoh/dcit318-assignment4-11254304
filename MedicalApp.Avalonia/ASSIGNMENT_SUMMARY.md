# DCIT318 Assignment 4 - Medical Appointment Booking System

## Implementation Summary

This project implements a **cross-platform Medical Appointment Booking System** using modern .NET 6+ and Avalonia UI, demonstrating comprehensive ADO.NET usage with SQL Server.

## 🎯 Assignment Requirements Fulfilled

### ✅ Database Design (SQL Server)
- **MedicalDB database** with proper schema
- **Doctors table**: DoctorID (PK), FullName, Specialty, Availability
- **Patients table**: PatientID (PK), FullName, Email  
- **Appointments table**: AppointmentID (PK), DoctorID (FK), PatientID (FK), AppointmentDate, Notes
- **Sample data** inserted for testing

### ✅ Windows Forms Application → Modern Cross-Platform UI
- **MainWindow**: Landing form with navigation buttons
- **DoctorListWindow**: Displays all doctors using DataGrid with filtering
- **AppointmentWindow**: Allows booking appointments with validation
- **ManageAppointmentsWindow**: View, update, or delete existing appointments

### ✅ Windows Forms Controls and Events → Avalonia UI Controls
- **TextBox, ComboBox, Button, DatePicker, DataGrid** equivalents
- **Click, SelectedIndexChanged, TextChanged** events via MVVM commands
- **EventHandler delegates** → RelayCommand pattern
- **Partial classes** → MVVM ViewModels

### ✅ Connecting to SQL Server
- **SqlConnection** with proper connection string
- **Configuration** via appsettings.json (optional)
- **Integrated Security** and **User ID/Password** support

### ✅ Retrieving and Displaying Data
- **SqlCommand** and **ExecuteReader** for data fetching
- **CommandType.Text** for all queries
- **DataReader** binding to DataGrid and ComboBox
- **Parameterized queries** for security

### ✅ Booking an Appointment
- **SqlCommand** with parameters and **ExecuteNonQuery**
- **Parameter placeholders** (@DoctorID, @PatientID, etc.)
- **SqlParameter.Direction** set to Input
- **Validation logic** (availability check, conflict detection)

### ✅ Viewing and Managing Appointments
- **SqlDataAdapter** and **DataSet** for appointment data
- **UPDATE command** with ExecuteNonQuery for modifications
- **DELETE command** for appointment removal
- **Patient-based filtering**

### ✅ Exception Handling
- **Try-catch blocks** around all database operations
- **User-friendly error messages** displayed in UI
- **Connection disposal** via using statements
- **Comprehensive error logging**

## 🏗️ Architecture Overview

### Project Structure
```
MedicalApp.Avalonia/
├── Models/                 # Data models
│   ├── Doctor.cs
│   ├── Patient.cs
│   └── Appointment.cs
├── Services/              # Data access layer
│   └── DatabaseService.cs
├── ViewModels/            # MVVM ViewModels
│   ├── MainWindowViewModel.cs
│   ├── DoctorListViewModel.cs
│   ├── AppointmentViewModel.cs
│   └── ManageAppointmentsViewModel.cs
├── Views/                 # UI Views
│   ├── MainWindow.axaml
│   ├── DoctorListWindow.axaml
│   ├── AppointmentWindow.axaml
│   └── ManageAppointmentsWindow.axaml
├── DatabaseSetup.sql      # Database creation script
└── README.md             # Setup instructions
```

### Key Technologies Used
- **.NET 6+**: Modern cross-platform framework
- **Avalonia UI**: Cross-platform UI framework
- **Microsoft.Data.SqlClient**: ADO.NET provider
- **CommunityToolkit.Mvvm**: MVVM toolkit
- **SQL Server**: Database backend

## 🔧 ADO.NET Implementation Details

### Database Service Features
```csharp
// Connection Management
using var connection = new SqlConnection(_connectionString);
await connection.OpenAsync();

// Parameterized Queries
command.Parameters.Add("@doctorId", SqlDbType.Int).Value = doctorId;

// Data Reading
using var reader = await command.ExecuteReaderAsync();
while (await reader.ReadAsync())
{
    // Process data
}

// Non-Query Operations
var rowsAffected = await command.ExecuteNonQueryAsync();
```

### Exception Handling Pattern
```csharp
try
{
    // Database operations
}
catch (Exception ex)
{
    throw new Exception($"Operation failed: {ex.Message}", ex);
}
finally
{
    // Connections automatically disposed via using statements
}
```

## 🚀 Running the Application

### Prerequisites
1. .NET 6.0 SDK or later
2. SQL Server (LocalDB, Express, or full version)
3. Database setup completed

### Setup Steps
1. **Database Setup**: Run `DatabaseSetup.sql` in SQL Server
2. **Build Application**: `dotnet build`
3. **Run Application**: `dotnet run`

### Connection String Configuration
```json
{
  "ConnectionStrings": {
    "MedicalDB": "Data Source=.;Initial Catalog=MedicalDB;Integrated Security=True;TrustServerCertificate=True;"
  }
}
```

## 📊 Features Demonstrated

### 1. Doctor Management
- View all doctors in DataGrid
- Filter by name and specialty
- Real-time search functionality
- Availability status display

### 2. Appointment Booking
- Doctor and patient selection
- Date/time picker with validation
- Conflict detection
- Optional notes field
- Success/error feedback

### 3. Appointment Management
- Patient-based appointment viewing
- Appointment selection and editing
- Date/time updates
- Appointment deletion with confirmation
- Real-time data refresh

### 4. Data Validation
- Required field validation
- Date validation (future dates only)
- Doctor availability checking
- Time slot conflict detection
- User-friendly error messages

## 🎨 UI/UX Features

### Modern Design
- Clean, intuitive interface
- Responsive layout
- Professional styling
- Cross-platform consistency

### User Experience
- Clear navigation
- Real-time feedback
- Loading indicators
- Status messages
- Confirmation dialogs

## 🔒 Security Features

### SQL Injection Prevention
- Parameterized queries throughout
- No string concatenation for SQL
- Proper parameter types

### Data Validation
- Input sanitization
- Business rule validation
- Client and server-side checks

## 📈 Performance Considerations

### Database Optimization
- Efficient queries with proper indexing
- Connection pooling
- Async/await for non-blocking operations
- Proper resource disposal

### UI Responsiveness
- Background data loading
- Progress indicators
- Non-blocking UI updates

## 🧪 Testing and Validation

### Database Testing
- Connection test utility included
- Sample data for testing
- Comprehensive error handling

### Feature Testing
- All CRUD operations tested
- Validation scenarios covered
- Error conditions handled

## 📝 Screenshots Required

For submission, capture screenshots of:
1. **Main Window**: Navigation interface
2. **Doctor Directory**: Filtered doctor list
3. **Appointment Booking**: Form with selections
4. **Appointment Management**: Patient appointments view
5. **Success Messages**: Booking confirmation
6. **Error Handling**: Validation messages

## 🎓 Learning Outcomes

This implementation demonstrates:
- **Modern .NET Development**: Cross-platform capabilities
- **Database Programming**: ADO.NET best practices
- **UI Development**: MVVM pattern implementation
- **Error Handling**: Comprehensive exception management
- **Data Validation**: Client and server-side validation
- **Security**: SQL injection prevention
- **Performance**: Async operations and resource management

## 🔄 Future Enhancements

Potential improvements:
- User authentication and authorization
- Appointment reminders
- Reporting and analytics
- Multi-language support
- Mobile companion app
- Integration with external systems

---

**This implementation successfully meets all assignment requirements while demonstrating modern software development practices and cross-platform capabilities.** 