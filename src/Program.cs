using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DoctorsAppointmentDB
{
    public class Program
    {
        static void Main()
        {
            new Program().Run();
        }
        public void Run()
        {
            using var context = new DocVisitContext();
            using var uow = new UnitOfWork<DocVisitContext>(context);

            var appointment = uow.GetRepository<Appointment>();

            RemoveAll(uow);
            AddPatients(uow);
            AddDoctors(uow);

            var doc = context.Doctors.ToList();
            var pnt = context.Patients.ToList();
            appointment.Create(new Appointment
            {
                Doctor = doc[0],
                Patient = pnt[0],
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddMinutes(30)
            });
            appointment.Create(new Appointment
            {
                Doctor = doc[0],
                Patient = pnt[1],
                StartTime = DateTime.Now.AddMinutes(15),
                EndTime = DateTime.Now.AddMinutes(45)
            });
            appointment.Create(new Appointment
            {
                Doctor = doc[1],
                Patient = pnt[0],
                StartTime = DateTime.Now.AddMinutes(46),
                EndTime = DateTime.Now.AddMinutes(76)
            });

            MakeAllTests(uow);

        }

        public void MakeAllTests(IUnitOfWork _uow)
        {
            var unitOfWork = (UnitOfWork<DocVisitContext>)_uow;
            TestCreateAp(_uow);
            TestCheckIntersection(unitOfWork);
            TestGetAllAppointments(unitOfWork);

            Console.WriteLine();
            Console.WriteLine("Проверочные операции выполнены.");
        }
        public void TestCreateAp(IUnitOfWork _uow)
        {
            Console.WriteLine();
            Console.WriteLine("Проверка добавления назначений:");
            var data = _uow.GetRepository<Doctor>().Query();
            int counter = 0;
            foreach (var d in data.Include(a => a.Appointments).ToList())
            {
                Console.WriteLine($"Врач: {d.Name}, каб.: {d.Room}");
                counter = 0;
                foreach (var app in d.Appointments)
                {
                    Console.WriteLine($"Пациент: {app.Patient.Name}, {app.StartTime} - {app.EndTime}");
                    counter++;
                }
                if (counter == 0)
                    Console.WriteLine("У данного врача нет назначений");
                Console.WriteLine();
            }
        }
        public void TestCheckIntersection(UnitOfWork<DocVisitContext> _uow)
        {
            Console.WriteLine();
            Console.WriteLine("Проверка метода CheckIntersection:");
            var appRepo = new AppointmentRepository(_uow.Context);
            var app = appRepo.Query().ToList();
            int counter = 0;
            foreach (var a in app)
                if (appRepo.CheckIntersection(a.Id, a.StartTime, a.EndTime))
                {
                    Console.WriteLine($"У {a.Doctor.Name} и {a.Patient.Name} есть пересечение в период {a.StartTime} - {a.EndTime}");
                    counter++;
                }
            if (counter == 0)
            {
                Console.WriteLine($"Пересечений по времени нет.");
            }
        }
        private void TestGetAllAppointments(UnitOfWork<DocVisitContext> _uow)
        {
            Console.WriteLine();
            var t1 = DateTime.Today.AddDays(1);
            var t2 = DateTime.Today.AddDays(2);
            Console.WriteLine($"Проверка поиска назначений за период: {t1} - {t2} :");
            var appRepo = new AppointmentRepository(_uow.Context);
            var data = appRepo.GetAllAppointments(t1, t2);
            int counter = 0;
            foreach (var d in data)
            {
                Console.WriteLine($"Врач: {d.Doctor.Name}, каб.: {d.Doctor.Room}\n\t" +
                  $"Пациент: {d.Patient.Name}, {d.StartTime} - {d.EndTime}");
                Console.WriteLine();
                counter++;
            }
            if (counter == 0)
                Console.WriteLine("В заданный промежуток времени назначений нет.");
        }

        public void AddDoctors(IUnitOfWork _uow)
        {
            _uow.GetRepository<Doctor>().Create(new Doctor { Name = "Сергей Сергеев", Room = 11 });
            _uow.GetRepository<Doctor>().Create(new Doctor { Name = "Иван Иванов", Room = 22 });
            _uow.GetRepository<Doctor>().Create(new Doctor { Name = "Семен Семенов", Room = 33 });
        }
        public void AddPatients(IUnitOfWork _uow)
        {
            var repo = _uow.GetRepository<Patient>();
            repo.Create(new Patient { Name = "Серёжа" });
            repo.Create(new Patient { Name = "Ваня" });
            repo.Create(new Patient { Name = "Сёма" });
        }

        public static void RemoveAll(IUnitOfWork _uow)
        {
            Console.WriteLine("Очистка базы данных");
            _uow.GetRepository<Appointment>().Delete(a => true);
            _uow.GetRepository<Doctor>().Delete(a => true);
            _uow.GetRepository<Patient>().Delete(a => true);
        }

        // Красиво написанный AddAppointments. Понадобится ли?

        //private void AddAppointments(int docId, int pntId, DateTime startTime, DateTime endTime)
        //{
        //    using var context = new DocVisitContext();
        //    var appointment = GetRepository<Appointment>(context);
        //    var doc = context.Doctors.Find(docId);
        //    var pnt = context.Patients.Find(pntId);
        //}


        // Построение запроса

        //var data = context.Appointments.Select(a => new
        //{
        //    a.Doctor.Id,
        //    a.Doctor.Name,
        //    a.Doctor.Room,
        //    pname = a.Patient.Name,
        //    a.StartTime,
        //    a.EndTime
        //}).ToList();
    }
}
