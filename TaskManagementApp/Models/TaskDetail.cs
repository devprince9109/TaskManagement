using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementApp.Models
{
    public class TaskDetail
    {


       
        public int Task_Id { get; set; }
        public string TaskTitle { get; set; }
        public string TaskDescription { get; set; }
        public DateTime TaskDueDate { get; set; }
        public string TaskStatus { get; set; }
        public string TaskRemarks { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string CreatedByName { get; set; }
        public string LastUpdatedByName { get; set; }
    }
}


