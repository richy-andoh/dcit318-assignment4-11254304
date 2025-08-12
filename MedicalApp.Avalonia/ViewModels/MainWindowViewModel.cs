using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MedicalApp.Avalonia.Views;
using System;
using System.Threading.Tasks;

namespace MedicalApp.Avalonia.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _greeting = "Welcome to Medical Appointment Booking System!";

        [RelayCommand]
        private void ShowDoctors()
        {
            var window = new DoctorListWindow();
            window.Show();
        }

        [RelayCommand]
        private void BookAppointment()
        {
            var window = new AppointmentWindow();
            window.Show();
        }

        [RelayCommand]
        private void ManageAppointments()
        {
            var window = new ManageAppointmentsWindow();
            window.Show();
        }

        [RelayCommand]
        private void Exit()
        {
            Environment.Exit(0);
        }
    }
}
