using Microsoft.EntityFrameworkCore.Migrations;

namespace DoctorsAppointmentDB.Migrations
{
    public partial class RepoChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PatientName",
                table: "Patients",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "Patients",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "DoctorRoom",
                table: "Doctors",
                newName: "Room");

            migrationBuilder.RenameColumn(
                name: "DoctorName",
                table: "Doctors",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "Doctors",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AppointmentId",
                table: "Appointments",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Patients",
                newName: "PatientName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Patients",
                newName: "PatientId");

            migrationBuilder.RenameColumn(
                name: "Room",
                table: "Doctors",
                newName: "DoctorRoom");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Doctors",
                newName: "DoctorName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Doctors",
                newName: "DoctorId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Appointments",
                newName: "AppointmentId");
        }
    }
}
