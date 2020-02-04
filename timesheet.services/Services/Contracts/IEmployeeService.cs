using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using timesheet.data.Models;

namespace timesheet.data.Contracts
{
    public interface IEmployeeService
    {
        /// <summary>
        /// Getting all employees
        /// </summary>
        /// <returns></returns>
        Task<List<Employee>> GetEmployees();

        /// <summary>
        /// Getting all tasks
        /// </summary>
        /// <returns></returns>
        Task<List<Tasks>> GetTasks();

        /// <summary>
        /// Getting employee details by Id and dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="employeeId"></param>
        Task<List<TimesheetData>> GetEmployeeDetailsById(DateTime startDate,DateTime enDate, string employeeId);

        /// <summary>
        /// Inserting or updating timesheet data
        /// </summary>
        /// <param name="timesheetData"></param>
        /// <returns></returns>
        Task<bool> InsertUpdate(TimesheetData timesheetData);
    }
}
