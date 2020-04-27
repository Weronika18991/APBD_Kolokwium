using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using APBD_Kolokwium.DTOs.Responses;
using APBD_Kolokwium.Models;

namespace APBD_Kolokwium.Services
{
    public class SqlServerMedicamentService : IDbService
    {
        private static string ConnString = "Data Source=db-mssql;Initial Catalog=s18991;Integrated Security=True";


        public GetMedicamentResponse GetMedication(int id)
        {
            GetMedicamentResponse medicamentResponse = new GetMedicamentResponse();

            using (var con = new SqlConnection(ConnString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();

                try
                {
                    com.CommandText = "select * from Medicament where IdMedicament=@idMedicament";
                    com.Parameters.AddWithValue("idMedicament", id);

                    var dr = com.ExecuteReader();
                    if(!dr.HasRows)
                        throw new Exception("Nie ma takiego leku");
                    if (dr.Read())
                    {
                        medicamentResponse.IdMedicament = (int) dr["IdMedicament"];
                        medicamentResponse.Name = dr["Name"].ToString();
                        medicamentResponse.Description = dr["Description"].ToString();
                        medicamentResponse.MedicamentType = dr["Type"].ToString();
                        medicamentResponse.Prescriptions= new List<Prescription>();
                    }
                    else
                    {
                      //  dr.Close();
                        throw new Exception("Nie ma takiego leku");
                    }
                    dr.Close();
                    
                    List<Prescription> prescriptionsList = new List<Prescription>();

                    com.CommandText =
                        "select Prescription.IdPrescription,Date,DueDate,IdPatient,IdDoctor,Dose,Details from Prescription join Prescription_Medicament on Prescription.IdPrescription=Prescription_Medicament.IdPrescription where IdMedicament=@MedId order by Date desc";
                    com.Parameters.AddWithValue("MedId", id);

                    dr = com.ExecuteReader();

                    while (dr.Read())
                    {
                        prescriptionsList.Add(
                        new Prescription()
                        { 
                            IdPrescription = (int) dr["IdPrescription"],
                            Date = DateTime.Parse(dr["Date"].ToString()),
                            DueDate = DateTime.Parse(dr["DueDate"].ToString()),
                            IdPatient = (int) dr["IdPatient"],
                            IdDoctor = (int) dr["IdDoctor"],
                            Dose = (int)dr["Dose"],
                            Details = dr["Details"].ToString()
                        });
                    }
                    dr.Close();

                    medicamentResponse.Prescriptions = prescriptionsList;

                }
                catch (SqlException e)
                {
                    throw new Exception(e.Message);
                }
            }

            return medicamentResponse;
        }
        
        

        public string deletePatient(int id)
        {
            using (var con = new SqlConnection(ConnString))
            using(var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                com.Transaction = con.BeginTransaction();

                try
                {
                    com.CommandText = "select 1 from Patient where IdPatient=@idPatient";
                    com.Parameters.AddWithValue("idPatient", id);
                    var dr = com.ExecuteReader();
                    
                    if (!dr.HasRows)
                    {
                        dr.Close();
                        com.Transaction.Rollback();
                        throw new Exception("Nie ma takiego pacjenta!");
                    }
                    dr.Close();
                    com.Parameters.Clear();
                    
                    
                    com.CommandText = "select IdPrescription from Prescription where IdPatient=@idPatient";
                    com.Parameters.AddWithValue("idPatient", id);
                    
                    List<string> prescriptionId = new List<string>();

                    dr = com.ExecuteReader();
                    if (!dr.HasRows)
                    {
                        dr.Close();
                        com.Transaction.Rollback();
                        throw new Exception("Ten pacjent nie posiada recept!");
                    }

                    while (dr.Read())
                    {
                        prescriptionId.Add(dr["IdPrescription"].ToString());
                    }
                    dr.Close();

                    foreach (var presId in prescriptionId)
                    {
                        com.Parameters.Clear();
                        com.CommandText = "delete from Prescription_Medicament where IdPrescription=@prescriptionId";
                        com.Parameters.AddWithValue("prescriptionId", presId);
                        com.ExecuteNonQuery();
                    }
                    com.Parameters.Clear();

                    com.CommandText = "delete from Prescription where IdPatient=@patientId";
                    com.Parameters.AddWithValue("patientId", id);
                    com.ExecuteNonQuery();
                    
                    com.Parameters.Clear();
                    com.CommandText = "delete from Patient where IdPatient=@patientId";
                    com.Parameters.AddWithValue("patientId", id);
                    com.ExecuteNonQuery();
                    
                    com.Transaction.Commit();
                    return "usunieto pacjenta o id "+ id;

                }
                catch (SqlException e)
                {
                    com.Transaction.Rollback();
                    throw new Exception(e.Message);
                }
            }
            
        }
    }
}