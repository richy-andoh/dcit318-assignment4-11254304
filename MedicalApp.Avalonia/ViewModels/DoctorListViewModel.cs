using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MedicalApp.Avalonia.Models;
using MedicalApp.Avalonia.Services;
using System.Collections.ObjectModel;
using System;
using System.Threading.Tasks;

namespace MedicalApp.Avalonia.ViewModels
{
    public partial class DoctorListViewModel : ViewModelBase
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private ObservableCollection<Doctor> _doctors = new();

        [ObservableProperty]
        private string _nameFilter = string.Empty;

        [ObservableProperty]
        private string _specialtyFilter = string.Empty;

        [ObservableProperty]
        private bool _isLoading = false;

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        public DoctorListViewModel()
        {
            _databaseService = new DatabaseService();
        }

        [RelayCommand]
        private async Task LoadDoctorsAsync()
        {
            try
            {
                IsLoading = true;
                var doctors = await _databaseService.GetDoctorsAsync(
                    string.IsNullOrWhiteSpace(NameFilter) ? null : NameFilter,
                    string.IsNullOrWhiteSpace(SpecialtyFilter) ? null : SpecialtyFilter);
                
                Doctors.Clear();
                foreach (var doctor in doctors)
                {
                    Doctors.Add(doctor);
                }
            }
            catch (Exception ex)
            {
                // In a real app, you'd want to show this in the UI
                System.Diagnostics.Debug.WriteLine($"Error loading doctors: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        partial void OnNameFilterChanged(string value)
        {
            _ = LoadDoctorsAsync();
        }

        partial void OnSpecialtyFilterChanged(string value)
        {
            _ = LoadDoctorsAsync();
        }
    }
} 