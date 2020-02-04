using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using timesheet.core.Base;
using timesheet.core.Singleton;
using timesheet.data.Models;
using timesheet.data.Services;

namespace timesheet.wpf.ViewModel
{
    public class EmployeeViewModel : BaseViewModel
    {
        private EmployeeService _employeeService;
        public  static List<Employee> employeesList;
        public static List<Tasks> taskList;

        /// <summary>
        /// employees
        /// </summary>
        public List<Employee> employees
        {
            get;set;
        }
        /// <summary>
        /// timesheetList
        /// </summary>
        public List<TimesheetData> timesheetList
        {
            get; set;
        }

        /// <summary>
        /// EmployeeViewModel
        /// </summary>
        public EmployeeViewModel()
        {
            _employeeService = (EmployeeService)SingletonInstances.GetEmployeeService(typeof(EmployeeService));
            Task.Run(new Action(OnLoaded));
            
        }
        /// <summary>
        /// OnLoaded
        /// </summary>
        private async void OnLoaded()
        {
            await Load();
        }
        /// <summary>
        /// Load
        /// </summary>
        /// <returns></returns>
        private async Task Load()
        {
            if (employeesList == null)
            {
                employees = await this._employeeService.GetEmployees();
                employeesList = employees;
            }
            else
                employees = employeesList;
            taskList = await this._employeeService.GetTasks();
            NotifyPropertyChanged("employees");
            NotifyPropertyChanged("tasks");
        }

        /// <summary>
        /// LoadDetails
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task LoadDetails(DateTime startDate, DateTime endDate, string employeeId)
        {
            timesheetList = await this._employeeService.GetEmployeeDetailsById(startDate, endDate, employeeId);
            NotifyPropertyChanged("timesheetList");
        }

        /// <summary>
        /// LoadInsert
        /// </summary>
        /// <param name="timesheetData"></param>
        /// <returns></returns>
        public async Task LoadInsert(TimesheetData timesheetData)
        {
             await this._employeeService.InsertUpdate(timesheetData);
        }
    }
}
