using Medical.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical.Application.Interfaces
{
    public interface IDoctorService
    {
        Task<Doctor> RegisterDoctorAsync(Doctor doctor);
        Task<List<Doctor>> GetAllDoctorsAsync();
        Task DeleteDoctorAsync(int id);
        Task UpdateDoctorAsync(int id, Doctor doctor);
    }
}