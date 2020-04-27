using APBD_Kolokwium.DTOs.Responses;

namespace APBD_Kolokwium.Services
{
    public interface IDbService
    {
        public GetMedicamentResponse GetMedication(int id);
        public string deletePatient(int id);
        
    }
}