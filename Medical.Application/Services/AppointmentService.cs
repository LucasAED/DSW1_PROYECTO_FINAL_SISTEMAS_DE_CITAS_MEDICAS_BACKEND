using Medical.Application.Interfaces;
using Medical.Domain.Entities;
using Medical.Domain.Interfaces;
using System.Globalization;

namespace Medical.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository, IDoctorRepository doctorRepository)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
        }

        private async Task ValidateAppointment(int doctorId, DateTime date, int? currentAppointmentId = null)
        {
            var doctor = await _doctorRepository.GetByIdAsync(doctorId);
            if (doctor == null) throw new Exception("El doctor no existe.");
            if (!doctor.IsAvailable) throw new Exception("El doctor no está disponible (Vacaciones/Baja).");

            // 1. AJUSTAR HORA PERÚ
            DateTime peruTime;
            try
            {
                var peruZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                peruTime = TimeZoneInfo.ConvertTime(date.ToUniversalTime(), peruZone);
            }
            catch
            {
                peruTime = date.ToUniversalTime().AddHours(-5);
            }

            // 2. VALIDACIÓN DE HORARIO INTELIGENTE (Soporta Madrugada)
            TimeSpan requestTime = peruTime.TimeOfDay;
            TimeSpan startShift = TimeSpan.Parse(doctor.ShiftStart);
            TimeSpan endShift = TimeSpan.Parse(doctor.ShiftEnd);

            bool isInsideShift = false;

            if (startShift <= endShift)
            {
                // TURNO NORMAL (Ej: 08:00 a 17:00) -> La hora debe estar EN MEDIO
                isInsideShift = requestTime >= startShift && requestTime <= endShift;
            }
            else
            {
                // TURNO AMANECIDA (Ej: 23:00 a 04:00) -> La hora debe ser MAYOR al inicio O MENOR al final
                isInsideShift = requestTime >= startShift || requestTime <= endShift;
            }

            if (!isInsideShift)
            {
                string amPm = peruTime.ToString("tt", CultureInfo.InvariantCulture);
                throw new Exception($"El doctor atiende de {doctor.ShiftStart} a {doctor.ShiftEnd}. Tu hora ({requestTime:hh\\:mm} {amPm}) está fuera de su turno.");
            }

            // 3. VALIDAR QUE NO HAYA OTRA CITA A LA MISMA HORA
            var allAppointments = await _appointmentRepository.GetAllAsync();
            bool isOccupied = allAppointments.Any(a =>
                a.DoctorId == doctorId &&
                a.Id != currentAppointmentId &&
                a.Status != "Cancelada" &&
                Math.Abs((a.AppointmentDate - date).TotalMinutes) < 20); // Margen de 20 min

            if (isOccupied) throw new Exception("El doctor ya tiene una cita reservada en ese horario.");
        }

        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
        {
            await ValidateAppointment(appointment.DoctorId, appointment.AppointmentDate);
            appointment.Status = "Programada";
            return await _appointmentRepository.AddAsync(appointment);
        }

        public async Task RescheduleAsync(int id, DateTime newDate)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null) throw new Exception("Cita no encontrada");
            await ValidateAppointment(appointment.DoctorId, newDate, id);
            await _appointmentRepository.UpdateDateAsync(id, newDate);
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync() { return await _appointmentRepository.GetAllAsync(); }
        public async Task DeleteAppointmentAsync(int id) { await _appointmentRepository.DeleteAsync(id); }
        public async Task ChangeStatusAsync(int id, string status) { await _appointmentRepository.UpdateStatusAsync(id, status); }
    }
}