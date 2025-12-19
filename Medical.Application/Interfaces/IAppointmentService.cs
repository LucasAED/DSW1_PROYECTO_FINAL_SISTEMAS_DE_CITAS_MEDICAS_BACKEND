using Medical.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical.Application.Interfaces
{
    public interface IAppointmentService
    {
        Task<Appointment> CreateAppointmentAsync(Appointment appointment);
        Task<List<Appointment>> GetAllAppointmentsAsync();
        Task DeleteAppointmentAsync(int id);
        Task RescheduleAsync(int id, DateTime newDate);
        Task ChangeStatusAsync(int id, string status);
    }
}