using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using L02.Models;
using L02.Repositories;

namespace L02.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : Controller
    {
        public StudentsController() { }

        [HttpGet]
        public IEnumerable<Student> Get()
        {
            return StudentRepo.Students;
        }

        [HttpGet("{id}")]
        public Student GetStudent(int id)
        {
            return StudentRepo.Students.FirstOrDefault(s => s.Id == id);
        }

        [HttpPost]
        public void AddStudent([FromBody] Student student)
        {
            StudentRepo.Students.Add(student);
        }

        [HttpPut]
        public void Put([FromBody] Student student)
        {
            Console.Write(student.Id);
            var stud = StudentRepo.Students.FirstOrDefault(s => s.Id == student.Id);

            stud.Nume = student.Nume;
            stud.Prenume = student.Prenume;
            stud.Facultate = student.Facultate;
            stud.AnStudiu = student.AnStudiu;
        }
        

        [HttpDelete("{id}")]
        public void DeleteStudent(int id)
        {
            StudentRepo.Students.Remove(StudentRepo.Students.FirstOrDefault(s => s.Id == id));
        }
        /*public IActionResult Index()
        {
            return View();
        }*/
    }
}
