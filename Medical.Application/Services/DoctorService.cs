using Medical.Application.Interfaces;
using Medical.Domain.Entities;
using Medical.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;

        public DoctorService(IDoctorRepository repository)
        {
            _repository = repository;
        }

        public async Task<Doctor> RegisterDoctorAsync(Doctor doctor)
        {
            return await _repository.RegisterAsync(doctor);
        }

        public async Task<List<Doctor>> GetAllDoctorsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task DeleteDoctorAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task UpdateDoctorAsync(int id, Doctor doctor)
        {
            var existingDoctor = await _repository.GetByIdAsync(id);
            if (existingDoctor != null)
            {    
                existingDoctor.FullName = doctor.FullName;
                existingDoctor.Specialty = doctor.Specialty;
                existingDoctor.Cmp = doctor.Cmp;
                existingDoctor.Email = doctor.Email;
                existingDoctor.IsAvailable = doctor.IsAvailable;

                await _repository.UpdateAsync(existingDoctor);
            }
        }
    }
}
