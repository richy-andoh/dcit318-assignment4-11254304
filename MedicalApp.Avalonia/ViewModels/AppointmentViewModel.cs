using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MedicalApp.Avalonia.Models;
using MedicalApp.Avalonia.Services;
using System.Collections.ObjectModel;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace MedicalApp.Avalonia.ViewModels
{
    public partial class AppointmentViewModel : ViewModelBase
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private ObservableCollection<Doctor> _doctors = new();

        [ObservableProperty]
        private ObservableCollection<Patient> _patients = new();

        [ObservableProperty]
        private Doctor? _selectedDoctor;

        [ObservableProperty]
        private Patient? _selectedPatient;

        [ObservableProperty]
        private DateTime _appointmentDate = DateTime.Now.AddDays(1);

        [ObservableProperty]
        private string _notes = string.Empty;

        [ObservableProperty]
        private bool _isLoading = false;

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        public AppointmentViewModel()
        {
            _databaseService = new DatabaseService();
            _ = LoadDataAsync();
        }

        [RelayCommand]
        private async Task LoadDataAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Loading data...";

                var doctorsTask = _databaseService.GetDoctorsAsync();
                var patientsTask = _databaseService.GetPatientsAsync();

                await Task.WhenAll(doctorsTask, patientsTask);

                var doctors = await doctorsTask;
                var patients = await patientsTask;

                Doctors.Clear();
                foreach (var doctor in doctors.Where(d => d.Availability))
                {
                    Doctors.Add(doctor);
                }

                Patients.Clear();
                foreach (var patient in patients)
                {
                    Patients.Add(patient);
                }

                StatusMessage = "Data loaded successfully.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading data: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task BookAppointmentAsync()
        {
            if (SelectedDoctor == null || SelectedPatient == null)
            {
                StatusMessage = "Please select both a doctor and a patient.";
                return;
            }

            if (AppointmentDate <= DateTime.Now)
            {
                StatusMessage = "Appointment date must be in the future.";
                return;
            }

            try
            {
                IsLoading = true;
                StatusMessage = "Booking appointment...";

                var success = await _databaseService.BookAppointmentAsync(
                    SelectedDoctor.DoctorID,
                    SelectedPatient.PatientID,
                    AppointmentDate,
                    string.IsNullOrWhiteSpace(Notes) ? null : Notes);

                if (success)
                {
                    StatusMessage = "Appointment booked successfully!";
                    // Reset form
                    SelectedDoctor = null;
                    SelectedPatient = null;
                    AppointmentDate = DateTime.Now.AddDays(1);
                    Notes = string.Empty;
                }
                else
                {
                    StatusMessage = "Failed to book appointment. Please try again.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error booking appointment: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
} 