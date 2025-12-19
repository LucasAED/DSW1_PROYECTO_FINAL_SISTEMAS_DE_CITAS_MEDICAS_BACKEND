using Medical.Application.Interfaces;
using Medical.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medical.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _service;

        public DoctorsController(IDoctorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var doctors = await _service.GetAllDoctorsAsync();
            return Ok(doctors);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Doctor doctor)
        {
            var newDoctor = await _service.RegisterDoctorAsync(doctor);
            return CreatedAtAction(nameof(GetAll), new { id = newDoctor.Id }, newDoctor);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteDoctorAsync(id);
            return Ok(new { message = "Doctor eliminado correctamente" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Doctor doctor)
        {
            if (id != doctor.Id && doctor.Id != 0) return BadRequest("ID no coincide");

            await _service.UpdateDoctorAsync(id, doctor);
            return Ok(new { message = "Doctor actualizado correctamente" });
        }
    }
}
