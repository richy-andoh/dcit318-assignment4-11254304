using Avalonia.Controls;

namespace MedicalApp.Avalonia.Views
{
    public partial class ManageAppointmentsWindow : Window
    {
        public ManageAppointmentsWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.ManageAppointmentsViewModel();
        }
    }
} 