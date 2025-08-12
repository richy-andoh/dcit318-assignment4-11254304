using Avalonia.Controls;

namespace MedicalApp.Avalonia.Views
{
    public partial class AppointmentWindow : Window
    {
        public AppointmentWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.AppointmentViewModel();
        }
    }
} 