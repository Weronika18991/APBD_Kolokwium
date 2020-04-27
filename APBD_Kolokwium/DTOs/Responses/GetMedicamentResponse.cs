using System.Collections.Generic;
using System.ComponentModel;
using APBD_Kolokwium.Models;

namespace APBD_Kolokwium.DTOs.Responses
{
    public class GetMedicamentResponse
    {
        public int IdMedicament { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MedicamentType { get; set; }
        public List<Prescription> Prescriptions { get; set; }
    }
}