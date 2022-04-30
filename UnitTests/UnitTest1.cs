using System;
using AutoMapper;
using DocAppLibrary.Entities;
using NUnit.Framework;
using System.Collections.Generic;
using FluentAssertions;
using WebDoctorAppointment.Extensions;
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

        [TestCase("30.04.2022 16:39:44.123456", "30.04.2022 16:40", 1)]
        [TestCase("30.04.2022 16:39:01.123456", "30.04.2022 16:40", 1)]
        [TestCase("30.04.2022 16:39:44.123456", "30.04.2022 17:00", 60)]
        [TestCase("30.04.2022 16:39:01.123456", "30.04.2022 17:00", 60)]
        [TestCase("30.04.2022 16:39:44.123456", "30.04.2022 16:40", 10)]
        [TestCase("30.04.2022 16:35:01.123456", "30.04.2022 16:40", 10)]
        [TestCase("30.04.2022 16:29:01.123456", "30.04.2022 16:30", 10)]
        [TestCase("30.04.2022 16:39:44.123456", "30.04.2022 16:40:00", 0.5)]
        [TestCase("30.04.2022 16:39:01.123456", "30.04.2022 16:39:30", 0.5)]
        public void DateTime_RoundUp_Ok(string original, string expected, double minutes)
        {
            // Arrange
            var originalDate = DateTime.Parse(original);
            var expectedDate = DateTime.Parse(expected);

            // Act
            var actualDate = originalDate.RoundUp(TimeSpan.FromMinutes(minutes));

            // Assert
            actualDate.Should().Be(expectedDate);
        }
    }
}