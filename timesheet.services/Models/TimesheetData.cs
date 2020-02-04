using System;

namespace timesheet.data.Models
{
    /// <summary>
    /// TimesheetData Class
    /// </summary>
    public class TimesheetData
    {
        /// <summary>
        /// Timesheet Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Employee Id
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Task Id
        /// </summary>
        public int TaskId { get; set; }

        /// <summary>
        /// Hours worked
        /// </summary>
        public int NoofHrs { get; set; }

        /// <summary>
        /// Working day
        /// </summary>
        public DateTime workingDay { get; set; }
    }
}
