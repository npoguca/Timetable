using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;
using System.IO;

namespace timetable
{
    class Program
    {
        static void Main(string[] args)
        {
            var forSerializationStudents = new Student();
            var forSerializationClasses = new Day();

            
            List<Student> students = new List<Student>();
            forSerializationStudents.Deserialize(out students);
            // students.Add(new Student("Nikita_Pogutsa", 4, 3, new DateTime[] { new DateTime(2016, 8, 10) }, new List<Attendance> {
            //     new Attendance(DayOfWeek.Monday,0),
            //     new Attendance(DayOfWeek.Tuesday,0),
            //     new Attendance(DayOfWeek.Thursday,1),
            //     new Attendance(DayOfWeek.Friday,1),

            // }));

            // students.Add(new Student("Nikita_Horev", 3, 4, new DateTime[]{
            //     new DateTime(2016,8,10),
            //     new DateTime(2016,8, 15),
            //     new DateTime(2016,8, 20),
            //     new DateTime(2016,8, 25),
            //     new DateTime(2016,8, 28),
            //     new DateTime(2016,8, 30)
            // }, new List<Attendance>{
            //     new Attendance(DayOfWeek.Monday,1),
            //     new Attendance(DayOfWeek.Tuesday,1),
            //     new Attendance(DayOfWeek.Thursday,1),
            //     new Attendance(DayOfWeek.Friday,1),


            // }));
            // students.Add(new Student("Natasha", 1, 5, new DateTime[]{
            //     new DateTime(2016,8,10),
            //     new DateTime(2016,8,11),
            //     new DateTime(2016,8,12),
            //     new DateTime(2016,8,13),
            //}, new List<Attendance>{
            //     new Attendance(DayOfWeek.Monday,0),
            //     new Attendance(DayOfWeek.Tuesday,0),
            //     new Attendance(DayOfWeek.Thursday,2),
            //     new Attendance(DayOfWeek.Friday,1),


            // }));
            // students.Add(new Student("Max", 3, 4, new DateTime[] { new DateTime(2016, 8, 8) }, new List<Attendance>{
            //     new Attendance(DayOfWeek.Monday,1),
            //     new Attendance(DayOfWeek.Tuesday,1),
            //     new Attendance(DayOfWeek.Thursday,1),
            //     new Attendance(DayOfWeek.Friday,1),


            // }));
            // students.Add(new Student("Lavr", 5, 1, new DateTime[] { new DateTime(2016, 8, 8) }, new List<Attendance>{
            //     new Attendance(DayOfWeek.Monday,1),
            //     new Attendance(DayOfWeek.Tuesday,0),
            //     new Attendance(DayOfWeek.Thursday,0),
            //     new Attendance(DayOfWeek.Friday,0),


            // }));
            List<Day> daysList = new List<Day>();
            forSerializationClasses.Deserialze(out daysList);

            List<Day> nextWeek = new List<Day>();

            //nextWeek.Add(new Day(DayOfWeek.Friday, new DateTime(2016, 8, 10)));
            //nextWeek.Add(new Day(DayOfWeek.Monday, new DateTime(2016, 8, 11)));
            //nextWeek.Add(new Day(DayOfWeek.Tuesday, new DateTime(2016, 8, 12)));
            nextWeek.Add(daysList[daysList.Count - 1]);
            nextWeek.Add(daysList[daysList.Count - 2]);
            nextWeek.Add(daysList[daysList.Count - 3]);





            var pretenders = new List<Student>();
            var contestants = new List<Student>(students);

            for (int i = 0; i < nextWeek.Count; i++)
            {
                Console.WriteLine("########" + nextWeek[i].dayOfWeek.ToString() + "########");

                while (pretenders.Count != 2)
                {
                    var pretender = new Student();
                    foreach (var item in contestants)
                    {
                        pretender = contestants.MinBy(x => x.GetSpecificAttendance(nextWeek[i].dayOfWeek));
                        if (!pretenders.Contains(pretender) || pretenders.Count == 0)
                        {
                            break;
                        }
                    }

                    if (!pretender.rainCheck.Contains(nextWeek[i].date))
                    {


                        pretenders.Add(pretender);
                        nextWeek[i].attendants.Add(pretender);
                        pretender.presentCount++;
                        Console.WriteLine(pretender.name + " Attended: " + pretender.GetSpecificAttendance(nextWeek[i].dayOfWeek).ToString());

                    }
                    contestants.Remove(pretender);



                }


               
                pretenders.Clear();
                contestants = new List<Student>(students);

            }
            //Students_serial
            forSerializationStudents.Serialize(students);
            //Classes_serial
            forSerializationClasses.Serialize(nextWeek); 
            Console.ReadLine();
        }


    }


    [Serializable]
    public class Student
    {
        public string name { get; set; }
        public int absentCount { get; set; }
        public int presentCount { get; set; }
        public DateTime[] rainCheck { get; set; }
        public List<Attendance> attendance { get; set; }
        public Student(string name, int absentCount, int presentCount, DateTime[] rainCheck, List<Attendance> attendance)
        {
            this.name = name;
            this.absentCount = absentCount;
            this.presentCount = presentCount;
            this.rainCheck = rainCheck;
            this.attendance = attendance;
        }
        public Student()
        { }
        public int GetSpecificAttendance(DayOfWeek dayOfWeek)
        {
            return this.attendance.Find(x => x.dayOfWeek == dayOfWeek).attended;


        }
        public void Serialize(List<Student> attendance)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Student>));
            using (var stream = File.Create(AppDomain.CurrentDomain.BaseDirectory + @"\attendance"))
            {
                xmlSerializer.Serialize(stream, attendance);
            }
        }
        public void Deserialize(out List<Student> studentList)
        {
            XmlSerializer xmlSerialzer = new XmlSerializer(typeof(List<Student>));
            using (var stream = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + @"\attendance"))
            {
                studentList = (List<Student>)(xmlSerialzer.Deserialize(stream));
            }
        }
    }
    [Serializable]
    public class Attendance
    {
        public DayOfWeek dayOfWeek { get; set; }
        public int attended { get; set; }
        public List<DateTime> datesAttended { get; set; }

        public Attendance(DayOfWeek dayOfWeek, int attended)
        {
            this.dayOfWeek = dayOfWeek;
            this.attended = attended;
        }
        public Attendance()
        { }
    }
    [Serializable]
    public class Day
    {
        public DayOfWeek dayOfWeek { get; set; }
        public DateTime date { get; set; }
        public List<Student> attendants { get; set; }
        public Day(DayOfWeek dayOfWeek, DateTime date)
        {
            this.dayOfWeek = dayOfWeek;
            this.date = date;
            attendants = new List<Student>();
        }
        public Day()
        {

        }
        public void Serialize(List<Day> days)
        {

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Day>));
            using (var stream = File.Create(AppDomain.CurrentDomain.BaseDirectory +  @"\classes"))
            {
                xmlSerializer.Serialize(stream, days);
            }
        }
        public void Deserialze(out List<Day> days)
        {
            XmlSerializer xmlDeserializer = new XmlSerializer(typeof(List<Day>));
            using (var stream = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + @"\classes"))
            {
                days = (List<Day>)(xmlDeserializer.Deserialize(stream));
            }
            
        }
    }
}
