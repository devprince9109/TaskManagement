using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TaskManagementApp.Models;

namespace TaskManagementApp.Controllers
{
    public class TaskController : Controller
    {
       
        private readonly BusinessLayer objbus;

        public TaskController()
        {
            objbus = new BusinessLayer();
        }
        public ActionResult Index()
        {
            List<TaskDetail> taskList = new List<TaskDetail>();
            taskList = objbus.GetAllTasks();
              return View(taskList);
           
        }

       
        public ActionResult CreateTask()
        {
            return View();
        }

       
        [HttpPost]
       
        public ActionResult CreateTask(TaskDetail task)
        {
            if (ModelState.IsValid)
            {
                
                    bool k = objbus.InsertTask(task);
                    if (k)
                    {
                        ViewBag.Message = "Task created successfully!";
                    }
                    else
                    {
                        ViewBag.Message = "Error!";

                    }
            }
            return View(task);
        }

        public ActionResult Edit(int Task_Id)
            {
            TaskDetail task = objbus.GetTaskById(Task_Id);
            if (task.Task_Id == 0)
            {
                return HttpNotFound();
            }
            return View(task);
        }

      
        [HttpPost]
        public ActionResult Edit(TaskDetail task)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool isUpdated = objbus.UpdateTask(task);
                    if (isUpdated)
                    {
                        ViewBag.Message = "Task updated successfully!";
                       
                    }
                    else
                    {
                        ViewBag.Message = "Error!";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }
            }
            return View(task);
        }

        
     
        public JsonResult DeleteTask(string Task_Id)
        {
            string res = "";
                bool isDeleted = objbus.DeleteTask(Task_Id);
                if (isDeleted)
                {
                    res= "Task deleted successfully!";
                }

            return Json(res, JsonRequestBehavior.AllowGet);
           
            
        }
    }
}