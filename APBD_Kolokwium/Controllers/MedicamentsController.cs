using System;
using System.Data.SqlClient;
using APBD_Kolokwium.DTOs.Responses;
using APBD_Kolokwium.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Kolokwium.Controllers
{
    [ApiController]
    [Route("api/medicaments")]
    public class MedicamentsController: ControllerBase
    {
        
        private IDbService _service;

        public MedicamentsController(IDbService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public IActionResult GetMedication(int id)
        {
            try
            {
                GetMedicamentResponse response = _service.GetMedication(id);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
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