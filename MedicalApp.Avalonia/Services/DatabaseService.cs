using Microsoft.Data.SqlClient;
using MedicalApp.Avalonia.Models;
using System.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalApp.Avalonia.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            // For cross-platform development, we'll use a simple connection string
            // In production, this should come from configuration
            _connectionString = "Data Source=.;Initial Catalog=MedicalDB;Integrated Security=True;TrustServerCertificate=True;";
            
            // You can also read from appsettings.json if needed:
            // var configuration = new ConfigurationBuilder()
            //     .SetBasePath(Directory.GetCurrentDirectory())
            //     .AddJsonFile("appsettings.json")
            //     .Build();
            // _connectionString = configuration.GetConnectionString("MedicalDB");
        }

        public async Task<List<Doctor>> GetDoctorsAsync(string? nameFilter = null, string? specialtyFilter = null)
        {
            var doctors = new List<Doctor>();
            
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(@"
                    SELECT DoctorID, FullName, Specialty, Availability
                    FROM Doctors
                    WHERE (@name IS NULL OR FullName LIKE '%' + @name + '%')
                      AND (@specialty IS NULL OR Specialty LIKE '%' + @specialty + '%')
                    ORDER BY FullName", connection);

                command.Parameters.Add("@name", SqlDbType.VarChar).Value = nameFilter ?? (object)DBNull.Value;
                command.Parameters.Add("@specialty", SqlDbType.VarChar).Value = specialtyFilter ?? (object)DBNull.Value;

                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    doctors.Add(new Doctor
                    {
                        DoctorID = reader.GetInt32("DoctorID"),
                        FullName = reader.GetString("FullName"),
                        Specialty = reader.GetString("Specialty"),
                        Availability = reader.GetBoolean("Availability")
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve doctors: {ex.Message}", ex);
            }

            return doctors;
        }

        public async Task<List<Patient>> GetPatientsAsync()
        {
            var patients = new List<Patient>();
            
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("SELECT PatientID, FullName, Email FROM Patients ORDER BY FullName", connection);

                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    patients.Add(new Patient
                    {
                        PatientID = reader.GetInt32("PatientID"),
                        FullName = reader.GetString("FullName"),
                        Email = reader.GetString("Email")
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve patients: {ex.Message}", ex);
            }

            return patients;
        }

        public async Task<List<Appointment>> GetAppointmentsByPatientAsync(int patientId)
        {
            var appointments = new List<Appointment>();
            
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(@"
                    SELECT a.AppointmentID, a.DoctorID, a.PatientID, a.AppointmentDate, a.Notes,
                           d.FullName AS DoctorName, p.FullName AS PatientName
                    FROM Appointments a
                    INNER JOIN Doctors d ON d.DoctorID = a.DoctorID
                    INNER JOIN Patients p ON p.PatientID = a.PatientID
                    WHERE a.PatientID = @patientId
                    ORDER BY a.AppointmentDate", connection);

                command.Parameters.Add("@patientId", SqlDbType.Int).Value = patientId;

                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    appointments.Add(new Appointment
                    {
                        AppointmentID = reader.GetInt32("AppointmentID"),
                        DoctorID = reader.GetInt32("DoctorID"),
                        PatientID = reader.GetInt32("PatientID"),
                        AppointmentDate = reader.GetDateTime("AppointmentDate"),
                        Notes = reader.IsDBNull("Notes") ? null : reader.GetString("Notes"),
                        DoctorName = reader.GetString("DoctorName"),
                        PatientName = reader.GetString("PatientName")
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve appointments: {ex.Message}", ex);
            }

            return appointments;
        }

        public async Task<bool> BookAppointmentAsync(int doctorId, int patientId, DateTime appointmentDate, string? notes)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                // Check doctor availability
                using (var checkCommand = new SqlCommand("SELECT Availability FROM Doctors WHERE DoctorID = @doctorId", connection))
                {
                    checkCommand.Parameters.Add("@doctorId", SqlDbType.Int).Value = doctorId;
                    var availability = await checkCommand.ExecuteScalarAsync();
                    
                    if (availability == null)
                        throw new Exception("Doctor not found.");
                    
                    if (availability is bool isAvailable && !isAvailable)
                        throw new Exception("Selected doctor is not available.");
                }

                // Check for time slot conflicts
                using (var conflictCommand = new SqlCommand(@"
                    SELECT COUNT(*) FROM Appointments 
                    WHERE DoctorID = @doctorId AND AppointmentDate = @appointmentDate", connection))
                {
                    conflictCommand.Parameters.Add("@doctorId", SqlDbType.Int).Value = doctorId;
                    conflictCommand.Parameters.Add("@appointmentDate", SqlDbType.DateTime).Value = appointmentDate;
                    
                    var conflicts = (int)await conflictCommand.ExecuteScalarAsync();
                    if (conflicts > 0)
                        throw new Exception("This time slot is already booked for the selected doctor.");
                }

                // Insert appointment
                using var insertCommand = new SqlCommand(@"
                    INSERT INTO Appointments (DoctorID, PatientID, AppointmentDate, Notes)
                    VALUES (@doctorId, @patientId, @appointmentDate, @notes)", connection);

                insertCommand.Parameters.Add("@doctorId", SqlDbType.Int).Value = doctorId;
                insertCommand.Parameters.Add("@patientId", SqlDbType.Int).Value = patientId;
                insertCommand.Parameters.Add("@appointmentDate", SqlDbType.DateTime).Value = appointmentDate;
                insertCommand.Parameters.Add("@notes", SqlDbType.VarChar, 400).Value = notes ?? (object)DBNull.Value;

                var rowsAffected = await insertCommand.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to book appointment: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateAppointmentAsync(int appointmentId, DateTime newDate)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand(@"
                    UPDATE Appointments 
                    SET AppointmentDate = @newDate 
                    WHERE AppointmentID = @appointmentId", connection);

                command.Parameters.Add("@newDate", SqlDbType.DateTime).Value = newDate;
                command.Parameters.Add("@appointmentId", SqlDbType.Int).Value = appointmentId;

                await connection.OpenAsync();
                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update appointment: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAppointmentAsync(int appointmentId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("DELETE FROM Appointments WHERE AppointmentID = @appointmentId", connection);

                command.Parameters.Add("@appointmentId", SqlDbType.Int).Value = appointmentId;

                await connection.OpenAsync();
                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete appointment: {ex.Message}", ex);
            }
        }
    }
} 