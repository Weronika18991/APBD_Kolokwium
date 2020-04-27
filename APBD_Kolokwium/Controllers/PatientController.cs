using System;
using APBD_Kolokwium.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Kolokwium.Controllers
{
    [ApiController]
    [Route("api/patients")]
    public class PatientController:ControllerBase
    {
        
        private IDbService _service;

        public PatientController(IDbService service)
        {
            _service = service;
        }
        
        [HttpDelete("{id}")]
        public IActionResult deletePatient(int id)
        {
            try
            {
                string response = _service.deletePatient(id);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
        
    }
}