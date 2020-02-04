using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace timesheet.wpf.Views
{
    /// <summary>
    /// Interaction logic for EmployeeList.xaml
    /// </summary>
    public partial class EmployeeList : UserControl
    {
        public EmployeeList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Navigation to details page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void goToDetails(object sender, RoutedEventArgs e)
        {
            Hyperlink obj = (Hyperlink)sender;
            EmployeeDetails employeeDetails = new EmployeeDetails(obj.CommandParameter.ToString());
            Application.Current.Windows[0].Close();
            employeeDetails.Show();
          
        }
    }
}
