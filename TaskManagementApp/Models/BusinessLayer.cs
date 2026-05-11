using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace TaskManagementApp.Models
{
    public class BusinessLayer
    {
        private readonly DataLayer _dl;
        private readonly string _connString;

        public BusinessLayer()
        {
            _dl = new DataLayer();
            _connString = _dl.GetConnString();
        }

        // 1. GET ALL TASKS & SEARCH
        public List<TaskDetail> GetAllTasks(string searchKeyword = "")
        {
           

            DataTable dt = _dl.GetDataTableByProcedure("sp_GetAllTasks", null);
            List<TaskDetail> list = new List<TaskDetail>();

            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new TaskDetail
                {
                    Task_Id = Convert.ToInt32(dr["Task_Id"]),
                    TaskTitle = dr["TaskTitle"].ToString(),
                    TaskDescription = dr["TaskDescription"].ToString(),
                    TaskDueDate = Convert.ToDateTime(dr["TaskDueDate"]),
                    TaskStatus = dr["TaskStatus"].ToString(),
                    TaskRemarks = dr["TaskRemarks"].ToString(),
                    CreatedOn = Convert.ToDateTime(dr["CreatedOn"]),
                    LastUpdatedOn = Convert.ToDateTime(dr["LastUpdatedOn"])
                });
            }
            return list;
        }

        // 2. GET SINGLE TASK BY ID
        public TaskDetail GetTaskById(int id)
        {
            TaskDetail task = new TaskDetail();
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Task_Id", id)
            };

            DataTable dt = _dl.GetDataTableByProcedure("sp_GetTaskById", parameters);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                task.Task_Id = Convert.ToInt32(dr["Task_Id"]);
                task.TaskTitle = dr["TaskTitle"].ToString();
                task.TaskDescription = dr["TaskDescription"].ToString();
                task.TaskDueDate = Convert.ToDateTime(dr["TaskDueDate"]);
                task.TaskStatus = dr["TaskStatus"].ToString();
                task.TaskRemarks = dr["TaskRemarks"].ToString();
            }
            return task;
        }

        // 3. INSERT TASK (Transaction के साथ)
        public bool InsertTask(TaskDetail task)
        {
            using (SqlConnection con = new SqlConnection(_connString))
            {
                if (con.State != ConnectionState.Open) con.Open();
                SqlTransaction trans = con.BeginTransaction();

                try
                {
                    SqlParameter[] sp = {
                        new SqlParameter("@Title", task.TaskTitle),
                        new SqlParameter("@Desc", task.TaskDescription),
                        new SqlParameter("@DueDate", task.TaskDueDate),
                        new SqlParameter("@Status", task.TaskStatus),
                        new SqlParameter("@Remarks",task.TaskRemarks),
                        new SqlParameter("@CreatedBy", "Candidate User")
                    };

                    int rows = _dl.ExecuteProcedureWithTransAndCon("sp_InsertTask", sp, trans, con);

                    trans.Commit(); 
                    return rows > 0;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return false;
                }
            }
        }

     
        public bool UpdateTask(TaskDetail task)
        {
            using (SqlConnection con = new SqlConnection(_connString))
            {
                if (con.State != ConnectionState.Open) con.Open();
                SqlTransaction trans = con.BeginTransaction();

                try
                {
                    SqlParameter[] sp = {
                        new SqlParameter("@Task_Id", task.Task_Id),
                        new SqlParameter("@Title", task.TaskTitle),
                        new SqlParameter("@Desc",task.TaskDescription),
                        new SqlParameter("@DueDate", task.TaskDueDate),
                        new SqlParameter("@Status", task.TaskStatus),
                        new SqlParameter("@Remarks",task.TaskRemarks),
                        new SqlParameter("@UpdatedBy", "User")
                    };

                    int rows = _dl.ExecuteProcedureWithTransAndCon("sp_UpdateTask", sp, trans, con);

                    trans.Commit();
                    return rows > 0;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("Error in Update: " + ex.Message);
                }
            }
        }

        
        public bool DeleteTask(string id)
        {
            using (SqlConnection con = new SqlConnection(_connString))
            {
                if (con.State != ConnectionState.Open) con.Open();
                SqlTransaction trans = con.BeginTransaction();

                try
                {
                    SqlParameter[] sp = {
                        new SqlParameter("@Task_Id", id)
                    };

                    int rows = _dl.ExecuteProcedureWithTransAndCon("sp_DeleteTask", sp, trans, con);

                    trans.Commit();
                    return rows > 0;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("Error in Delete: " + ex.Message);
                }
            }
        }
    }
}