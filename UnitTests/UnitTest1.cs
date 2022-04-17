using AutoMapper;
using DocAppLibrary.Entities;
using NUnit.Framework;
using System.Collections.Generic;
using WebDoctorAppointment.Models;

namespace UnitTests
{
    public class Tests
    {
        [Test]
        public void MappingValidationTest()
        {
            var mapper = new MapperConfiguration(x =>
            {
                x.CreateMap<Doctor, DoctorViewModel>();
                x.CreateMap<DoctorViewModel, Doctor>().ForMember(dst => dst.Appointments, opt => opt.Ignore());
            }).CreateMapper();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            var doclist = new List<Doctor>();
            var doc = mapper.Map<List<DoctorViewModel>>(doclist);
            Assert.IsEmpty(doc);
        }
    }
}