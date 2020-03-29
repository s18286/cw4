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
            var list = new List<Student>();

            using (SqlConnection connection = new SqlConnection(myDatabase))
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "select * from Student";

                connection.Open();

                SqlDataReader sqlDataReader = command.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    Console.WriteLine(sqlDataReader["IndexNumber"].ToString());
                    var student = new Student
                    {
                        IndexNumber = sqlDataReader["IndexNumber"].ToString(),
                        LastName = sqlDataReader["LastName"].ToString(),
                        FirstName = sqlDataReader["FirstName"].ToString()
                    };
                    list.Add(student);
                }
            }
            return Ok(list);
        }

        //[FromRoute], [FromBody], [FromQuery]
        //1. URL segment
        [HttpGet("{id}")]
        public IActionResult GetStudent([FromRoute]int id) //action method
        {
            if (id == 1)
            {
                return Ok("Jan");
            }

            return NotFound("Student was not found");
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