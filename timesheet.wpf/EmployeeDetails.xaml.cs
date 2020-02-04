using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using timesheet.data.Models;
using timesheet.wpf.ViewModel;

namespace timesheet.wpf
{
    /// <summary>
    /// Interaction logic for EmployeeDetails.xaml
    /// </summary>
    public partial class EmployeeDetails : Window
    {
        /// <summary>
        /// EmployeeDetails
        /// </summary>
        /// <param name="employeeId"></param>
        public EmployeeDetails(string employeeId)
        {
            InitializeComponent();
            //Populating employee dropdown
            ddlEmployee.ItemsSource = EmployeeViewModel.employeesList;
            ddlEmployee.DisplayMemberPath = "Name";
            ddlEmployee.SelectedValuePath = "Id";
            ddlEmployee.SelectedValue = employeeId;

            //Populating task dropdown
            ddlTask.ItemsSource = EmployeeViewModel.taskList;
            ddlTask.DisplayMemberPath = "Name";
            ddlTask.SelectedValuePath = "Id";
            ddlTask.SelectedIndex = 0;
            txtSelected.Text = DateTime.Now.ToShortDateString();

            populateTimesheetDetails(DateTime.Now.Subtract(new TimeSpan((int)DateTime.Now.DayOfWeek, 0, 0, 0)), DateTime.Now.AddDays(7), employeeId);

        }

        /// <summary>
        /// Function to populate time sheet details of selected employee
        /// </summary>
        private void populateTimesheetDetails(DateTime startDate, DateTime endDate, string employeeId)
        {
            EmployeeViewModel objEmployeeService = new EmployeeViewModel();
            Task.Run(() => objEmployeeService.LoadDetails(startDate,endDate, employeeId));
            Thread.Sleep(1000);
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Task");
            //Populating column name with week days dynamically
            for (int j = 1; j <= 7; j++)
            {
                dataTable.Columns.Add(startDate.Date.AddDays(j).DayOfWeek.ToString(), typeof(int));
            }
            List<Tasks> taskLst = EmployeeViewModel.taskList;
            //Populating total in task list as last column
            taskLst.Insert(EmployeeViewModel.taskList.Count, new Tasks { Id = 0, Description = "Total", Name = "Total" });
            int sum = 0;
            foreach (var obj in taskLst)
            {
                DataRow dr = dataTable.NewRow();
                for (int l = 1; l <= 7; l++)
                {
                    if (obj.Name == "Total")
                    {
                        //Adding all efforts on a single day
                        dr[l]= dataTable.AsEnumerable().Sum(r => r.Field<int>(startDate.Date.AddDays(l).DayOfWeek.ToString()));
                        sum = sum +  Convert.ToInt32(dr[l]);
                    }
                    else
                    {
                        if (objEmployeeService.timesheetList != null)
                        {
                            //getting no of hours from db if it is already inserted
                            TimesheetData isExist = objEmployeeService.timesheetList.Find(x => x.workingDay.ToString("dd/MM/yyyy") == startDate.Date.AddDays(l).ToString("dd/MM/yyyy") && x.TaskId.ToString() == obj.Id.ToString());
                            if (isExist == null)
                                dr[l] = 0;
                            else
                                dr[l] = isExist.NoofHrs;
                        }
                        else
                            dr[l] =0;
                    }
                }
                dr[0] = obj.Name;
                dataTable.Rows.Add(dr);
            }
            //getting average effort in a week
            lblAverage.Content = sum / 1560;
            lnkBackward.CommandParameter = startDate.ToShortDateString();
            lnkForward.CommandParameter = startDate.ToShortDateString();
            gdDetails.ItemsSource = dataTable.DefaultView;
        }

        /// <summary>
        /// Selecting employee on dropdown for search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ddlEmployee_Changed(object sender, RoutedEventArgs e)
        {
            if (ddlEmployee.SelectedValue != null)
                populateTimesheetDetails(DateTime.Now.Subtract(new TimeSpan((int)DateTime.Now.DayOfWeek, 0, 0, 0)), DateTime.Now.AddDays(7), ddlEmployee.SelectedValue.ToString());
        }

        /// <summary>
        /// Button event for moving one week backward
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void backwardClick(object sender, RoutedEventArgs e)
        {
            Button obj = (Button)sender;
            if (obj != null)
                populateTimesheetDetails(Convert.ToDateTime(obj.CommandParameter).AddDays(-7), Convert.ToDateTime(obj.CommandParameter).AddDays(7), ddlEmployee.SelectedValue.ToString());
        }

        /// <summary>
        /// Button event for moving one week forward
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void forwardClick(object sender, RoutedEventArgs e)
        {
            Button obj = (Button)sender;
            if (obj != null)
                populateTimesheetDetails(Convert.ToDateTime(obj.CommandParameter).AddDays(7), Convert.ToDateTime(obj.CommandParameter).AddDays(7), ddlEmployee.SelectedValue.ToString());
        }


        /// <summary>
        /// Button event for navigating to list page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void listClick(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            Application.Current.Windows[0].Close();
            window.Show();
        }


        /// <summary>
        /// Button event for adding details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void lnkAddClick(object sender, RoutedEventArgs e)
        {
            popupAdd.IsOpen = true;
            txtHrs.Text = string.Empty;
            ddlTask.SelectedIndex = 0;
            txtSelected.Text = DateTime.Now.ToShortDateString();
            calWorkingDay.SelectedDate = DateTime.Now;
        }

        /// <summary>
        /// Button event for c;osing popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void closeClick(object sender, RoutedEventArgs e)
        {
            popupAdd.IsOpen = false;
        }

        /// <summary>
        /// Button event for saving details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void saveClick(object sender, RoutedEventArgs e)
        {
            int userVal;
            lblerr.Content = string.Empty;
            if (txtHrs.Text != string.Empty && int.TryParse(txtHrs.Text, out userVal))
            {

                TimesheetData objTimesheetData = new TimesheetData();
                objTimesheetData.EmployeeId = Convert.ToInt16(ddlEmployee.SelectedValue);
                objTimesheetData.TaskId = Convert.ToInt16(ddlTask.SelectedValue);
                objTimesheetData.NoofHrs = Convert.ToInt16(userVal);
                objTimesheetData.workingDay =Convert.ToDateTime(txtSelected.Text);
                EmployeeViewModel objEmployeeService = new EmployeeViewModel();
                Task.Run(() => objEmployeeService.LoadInsert(objTimesheetData));
                Thread.Sleep(1000);
                popupAdd.IsOpen = false;
                populateTimesheetDetails(DateTime.Now.Subtract(new TimeSpan((int)DateTime.Now.DayOfWeek, 0, 0, 0)), DateTime.Now.AddDays(7), ddlEmployee.SelectedValue.ToString());
            }
            else
            {
                lblerr.Content = "Input is not correct (Hrs)";
            }
        }

        /// <summary>
        /// Calendar change event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void monthlyCalendar_selectedDatesChanged(object sender,SelectionChangedEventArgs e)
        {
            txtSelected.Text = calWorkingDay.SelectedDate.Value.ToShortDateString();
        }
        
    }
}
