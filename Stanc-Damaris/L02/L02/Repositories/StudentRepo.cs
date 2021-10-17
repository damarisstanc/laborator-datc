using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using L02.Models;

namespace L02.Repositories
{
    public class StudentRepo
    {
        public static List<Student> Students = new List<Student>(){
        new Student() {Id=1, Nume="Anume", Prenume="Aprenume", Facultate="AC", AnStudiu=2},
        new Student() {Id=2, Nume="Bnume", Prenume="Bprenume", Facultate="ETC", AnStudiu=1},
        new Student() {Id=3, Nume="Cnume", Prenume="Cprenume", Facultate="ARH", AnStudiu=2},
        new Student() {Id=4, Nume="Dnume", Prenume="Dprenume", Facultate="AC", AnStudiu=4},
        new Student() {Id=5, Nume="Enume", Prenume="Eprenume", Facultate="ETC", AnStudiu=3},
        new Student() {Id=6, Nume="Fnume", Prenume="Fprenume", Facultate="MEC", AnStudiu=4},
        };
    }
}
