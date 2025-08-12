using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MedicalApp.Avalonia.Models;
using MedicalApp.Avalonia.Services;
using System.Collections.ObjectModel;
using System;
using System.Threading.Tasks;

namespace MedicalApp.Avalonia.ViewModels
{
    public partial class ManageAppointmentsViewModel : ViewModelBase
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private ObservableCollection<Patient> _patients = new();

        [ObservableProperty]
        private ObservableCollection<Appointment> _appointments = new();

        [ObservableProperty]
        private Patient? _selectedPatient;

        [ObservableProperty]
        private Appointment? _selectedAppointment;

        [ObservableProperty]
        private DateTime _newAppointmentDate = DateTime.Now.AddDays(1);

        [ObservableProperty]
        private bool _isLoading = false;

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        public ManageAppointmentsViewModel()
        {
            _databaseService = new DatabaseService();
            _ = LoadPatientsAsync();
        }

        [RelayCommand]
        private async Task LoadPatientsAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Loading patients...";

                var patients = await _databaseService.GetPatientsAsync();
                
                Patients.Clear();
                foreach (var patient in patients)
                {
                    Patients.Add(patient);
                }

                StatusMessage = "Patients loaded successfully.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading patients: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task LoadAppointmentsAsync()
        {
            if (SelectedPatient == null)
            {
                StatusMessage = "Please select a patient first.";
                return;
            }

            try
            {
                IsLoading = true;
                StatusMessage = "Loading appointments...";

                var appointments = await _databaseService.GetAppointmentsByPatientAsync(SelectedPatient.PatientID);
                
                Appointments.Clear();
                foreach (var appointment in appointments)
                {
                    Appointments.Add(appointment);
                }

                StatusMessage = $"Loaded {Appointments.Count} appointments for {SelectedPatient.FullName}.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error loading appointments: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task UpdateAppointmentAsync()
        {
            if (SelectedAppointment == null)
            {
                StatusMessage = "Please select an appointment to update.";
                return;
            }

            if (NewAppointmentDate <= DateTime.Now)
            {
                StatusMessage = "New appointment date must be in the future.";
                return;
            }

            try
            {
                IsLoading = true;
                StatusMessage = "Updating appointment...";

                var success = await _databaseService.UpdateAppointmentAsync(
                    SelectedAppointment.AppointmentID,
                    NewAppointmentDate);

                if (success)
                {
                    StatusMessage = "Appointment updated successfully!";
                    await LoadAppointmentsAsync(); // Refresh the list
                }
                else
                {
                    StatusMessage = "Failed to update appointment. Please try again.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error updating appointment: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task DeleteAppointmentAsync()
        {
            if (SelectedAppointment == null)
            {
                StatusMessage = "Please select an appointment to delete.";
                return;
            }

            try
            {
                IsLoading = true;
                StatusMessage = "Deleting appointment...";

                var success = await _databaseService.DeleteAppointmentAsync(SelectedAppointment.AppointmentID);

                if (success)
                {
                    StatusMessage = "Appointment deleted successfully!";
                    await LoadAppointmentsAsync(); // Refresh the list
                }
                else
                {
                    StatusMessage = "Failed to delete appointment. Please try again.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error deleting appointment: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        partial void OnSelectedPatientChanged(Patient? value)
        {
            if (value != null)
            {
                _ = LoadAppointmentsAsync();
            }
        }

        partial void OnSelectedAppointmentChanged(Appointment? value)
        {
            if (value != null)
            {
                NewAppointmentDate = value.AppointmentDate;
            }
        }
    }
} 