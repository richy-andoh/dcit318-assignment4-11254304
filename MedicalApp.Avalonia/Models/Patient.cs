namespace MedicalApp.Avalonia.Models
{
    public class Patient
    {
        public int PatientID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
} 