using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Wyklad3.Models;
using Wyklad3.Services;

namespace Wyklad3.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private IDbService _dbService;
        string myDatabase = "Data Source=db-mssql;Initial Catalog=s18286;Integrated Security=True";
        public StudentsController(IDbService service)
        {
            _dbService = service;
        }

        //2. QueryString
        [HttpGet]
        public IActionResult GetStudents([FromServices]IDbService service)
        {
            var list = new List<StudentInfoDTO>();

            using (SqlConnection connection = new SqlConnection(myDatabase))
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "select s.FirstName, s.LastName, s.BirthDate, st.Name, e.Semester from Student s " +
                    "join Enrollment e on e.IdEnrollment = s.IdEnrollment join Studies st on st.IdStudy = e.IdStudy";

                connection.Open();

                SqlDataReader sqlDataReader = command.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    var studentInfoDTO = new StudentInfoDTO
                    {
                        BirthDate = sqlDataReader["BirthDate"].ToString(),
                        LastName = sqlDataReader["LastName"].ToString(),
                        FirstName = sqlDataReader["FirstName"].ToString(),
                        Name = sqlDataReader["Name"].ToString(),
                        Semester = sqlDataReader["Semester"].ToString()
                    };
                    list.Add(studentInfoDTO);
                }
            }
            return Ok(list);
        }

        //[FromRoute], [FromBody], [FromQuery]
        //1. URL segment
        [HttpGet("{id}")]
        public IActionResult GetStudent([FromRoute]string id) //action method
        {
            var list = new List<StudentInfoDTO>();

            using (SqlConnection connection = new SqlConnection(myDatabase))
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                string sqlQuery = "select s.FirstName, s.LastName, s.BirthDate, st.Name, e.Semester from Student s " +
                    "join Enrollment e on e.IdEnrollment = s.IdEnrollment join Studies st on st.IdStudy = e.IdStudy " +
                    "where s.IndexNumber='"+ id + "'";
                Console.WriteLine(sqlQuery);
                command.CommandText = sqlQuery;

                connection.Open();

                SqlDataReader sqlDataReader = command.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    var studentInfoDTO = new StudentInfoDTO
                    {
                        BirthDate = sqlDataReader["BirthDate"].ToString(),
                        LastName = sqlDataReader["LastName"].ToString(),
                        FirstName = sqlDataReader["FirstName"].ToString(),
                        Name = sqlDataReader["Name"].ToString(),
                        Semester = sqlDataReader["Semester"].ToString()
                    };
                    list.Add(studentInfoDTO);
                }
            }
            return Ok(list);
        }

        //3. Body - cialo zadan
        [HttpPost]
        public IActionResult CreateStudent([FromBody]Student student)
        {
            student.IndexNumber=$"s{new Random().Next(1, 20000)}";
            //...
            return Ok(student); //JSON
        }


    }
}