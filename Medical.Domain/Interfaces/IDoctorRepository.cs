using Medical.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical.Domain.Interfaces
{
    public interface IDoctorRepository
    {
        Task<Doctor> RegisterAsync(Doctor doctor);
        Task<List<Doctor>> GetAllAsync();
        Task<Doctor> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task UpdateAsync(Doctor doctor);
    }
}
