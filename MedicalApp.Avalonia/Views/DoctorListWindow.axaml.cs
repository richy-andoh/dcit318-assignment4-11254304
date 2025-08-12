using Avalonia.Controls;

namespace MedicalApp.Avalonia.Views
{
    public partial class DoctorListWindow : Window
    {
        public DoctorListWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.DoctorListViewModel();
        }
    }
} 