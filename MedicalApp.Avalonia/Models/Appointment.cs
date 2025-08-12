using System;

namespace MedicalApp.Avalonia.Models
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public int DoctorID { get; set; }
        public int PatientID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? Notes { get; set; }
        
        // Navigation properties for display
        public string DoctorName { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
    }
} 