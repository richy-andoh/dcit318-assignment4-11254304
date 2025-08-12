namespace MedicalApp.Avalonia.Models
{
    public class Doctor
    {
        public int DoctorID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public bool Availability { get; set; }
    }
} 