using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;

namespace MedicalApp.Avalonia
{
    public static class DatabaseTest
    {
        public static async Task TestConnectionAsync()
        {
            var connectionString = "Data Source=.;Initial Catalog=MedicalDB;Integrated Security=True;TrustServerCertificate=True;";
            
            try
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                Console.WriteLine("✅ Database connection successful!");
                
                // Test a simple query
                using var command = new SqlCommand("SELECT COUNT(*) FROM Doctors", connection);
                var doctorCount = await command.ExecuteScalarAsync();
                Console.WriteLine($"📊 Found {doctorCount} doctors in the database.");
                
                using var command2 = new SqlCommand("SELECT COUNT(*) FROM Patients", connection);
                var patientCount = await command2.ExecuteScalarAsync();
                Console.WriteLine($"👥 Found {patientCount} patients in the database.");
                
                using var command3 = new SqlCommand("SELECT COUNT(*) FROM Appointments", connection);
                var appointmentCount = await command3.ExecuteScalarAsync();
                Console.WriteLine($"📅 Found {appointmentCount} appointments in the database.");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Database connection failed: {ex.Message}");
                Console.WriteLine("Please ensure:");
                Console.WriteLine("1. SQL Server is running");
                Console.WriteLine("2. Database 'MedicalDB' exists");
                Console.WriteLine("3. You have proper permissions");
                Console.WriteLine("4. Run the DatabaseSetup.sql script first");
            }
        }
    }
} 