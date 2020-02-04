using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using timesheet.data.Contracts;
using timesheet.data.Models;

namespace timesheet.data.Services
{
    public class EmployeeService : IEmployeeService
    {
        private string _baseurl = "https://localhost:44391/api/v1/";

        /// <summary>
        /// EmployeeService
        /// </summary>
        public EmployeeService()
        {
          
        }
        /// <summary>
        /// Getting all employees
        /// </summary>
        /// <returns></returns>
        public async Task<List<Employee>> GetEmployees()
        {
            using (HttpClient client = new HttpClient())
            {
                List<Employee> employees = new List<Employee>();
                HttpResponseMessage response = await client.GetAsync(_baseurl+"/employee/getall");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    employees= JsonConvert.DeserializeObject<List<Employee>>(json);
                }
                return employees;
            }
        }

        /// <summary>
        /// Getting all tasks
        /// </summary>
        /// <returns></returns>
        public async Task<List<Tasks>> GetTasks()
        {
            using (HttpClient client = new HttpClient())
            {
                List<Tasks> tasks = new List<Tasks>();
                HttpResponseMessage response = await client.GetAsync(_baseurl + "/task/getall");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    tasks = JsonConvert.DeserializeObject<List<Tasks>>(json);
                }
                return tasks;
            }
        }

        /// <summary>
        /// Getting employee details by Id and dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<List<TimesheetData>> GetEmployeeDetailsById(DateTime startDate,DateTime endDate,string employeeId)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("startDate", startDate.ToString()),
                new KeyValuePair<string, string>("endDate", endDate.ToString()),
                new KeyValuePair<string, string>("employeeId", employeeId.ToString())
            });
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsync(_baseurl + "/timesheetData/getall", content);
                List<TimesheetData> timeSheetData = new List<TimesheetData>();
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    timeSheetData = JsonConvert.DeserializeObject<List<TimesheetData>>(json);
                }
                return timeSheetData;
            }
        }

        /// <summary>
        /// Inserting or updating timesheet data
        /// </summary>
        /// <param name="timesheetData"></param>
        /// <returns></returns>
        public async Task<bool> InsertUpdate(TimesheetData timesheetData)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(timesheetData), UnicodeEncoding.UTF8, "application/json");
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsync(_baseurl + "/timesheetData/insertUpdate", stringContent);
                if (response.IsSuccessStatusCode)
                {
                     await response.Content.ReadAsStringAsync();
                }
            }
            return true;
        }
    }
}
