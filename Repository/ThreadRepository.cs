using Repository.Interface;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Repository
{
    public class ThreadRepository<T> : IRepository<T> where T : RawThreadDTO
    {
        //public string SqlConnection { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string sqlConnection {  get; set; }
        public ThreadRepository(string sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public bool CreateRecord(T thread)
        {
            using (SqlConnection conn = new SqlConnection(sqlConnection))
            {
                SqlCommand cmd = new SqlCommand("InsertRawThread",conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ThWeight", thread.ThreadWeight);
                cmd.Parameters.AddWithValue("@BillAmount", thread.BillAmount);
                cmd.Parameters.AddWithValue("@BillNo", thread.BillNo);
                cmd.Parameters.AddWithValue("@BillDate",thread.BillDate);
                cmd.Parameters.AddWithValue("@CreatedBy",thread.CreatedBy);
                cmd.Parameters.AddWithValue("@CreatedDate",thread.CreatedDate);
                cmd.Parameters.AddWithValue("@CompanyName", thread.CompanyName);
                cmd.Parameters.AddWithValue("@Quality",thread.Quality);
                conn.Open();
                cmd.ExecuteNonQuery();  
                return true;
            }

        }

        public bool DeleteRecord(int id)
        {
            //DeleteRawThread
            //@Id
            using (SqlConnection conn = new SqlConnection(sqlConnection))
            {
                SqlCommand cmd = new SqlCommand("DeleteRawThread", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            
        }

        public T GetRecord(int id)
        {

            //GetRecord
            RawThreadDTO rawThreadDTO = new RawThreadDTO();
            using (SqlConnection conn = new SqlConnection(sqlConnection))
            {
                SqlCommand cmd = new SqlCommand("GetRecord", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    rawThreadDTO.Id = Convert.ToInt32(reader["ThreadID"]);
                    rawThreadDTO.CreatedBy = reader["CreatedBy"].ToString();
                    rawThreadDTO.BillDate = Convert.ToDateTime(reader["BillDate"]); 
                    rawThreadDTO.ThreadWeight = Convert.ToInt32(reader["ThWeight"]);
                    rawThreadDTO.BillNo = Convert.ToInt32(reader["BillNo"]);
                    rawThreadDTO.BillAmount = Convert.ToInt32(reader["BillAmount"]);
                    rawThreadDTO.CompanyName = reader["CompanyName"].ToString();
                    rawThreadDTO.Quality = reader["Quality"].ToString();
                    rawThreadDTO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);

                }
            }
            return (T)rawThreadDTO;
        }

        public bool UpdateRecord(T thread)
        {
            using (SqlConnection conn = new SqlConnection(sqlConnection))
            {
                SqlCommand cmd = new SqlCommand("UpdateRawThread", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ThWeight", thread.ThreadWeight);
                cmd.Parameters.AddWithValue("@BillAmount", thread.BillAmount);
                cmd.Parameters.AddWithValue("@BillNo", thread.BillNo);
                cmd.Parameters.AddWithValue("@BillDate", thread.BillDate);
                cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@CompanyName", thread.CompanyName);
                cmd.Parameters.AddWithValue("@Quality", thread.Quality);
                conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public IEnumerable<T> GetRecords()
        {
            List<RawThreadDTO> rawThreadDTOs = new List<RawThreadDTO>();
            using (SqlConnection conn = new SqlConnection(sqlConnection))
            {
                SqlCommand cmd = new SqlCommand("GetRecord", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", 0);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    RawThreadDTO rawThreadDTO = new RawThreadDTO();
                    rawThreadDTO.Id = Convert.ToInt32(reader["ThreadID"]);
                    rawThreadDTO.CreatedBy = reader["CreatedBy"].ToString();
                    rawThreadDTO.BillDate = Convert.ToDateTime(reader["BillDate"]);
                    rawThreadDTO.ThreadWeight = Convert.ToInt32(reader["ThWeight"]);
                    rawThreadDTO.BillNo = Convert.ToInt32(reader["BillNo"]);
                    rawThreadDTO.BillAmount = Convert.ToInt32(reader["BillAmount"]);
                    rawThreadDTO.CompanyName = reader["CompanyName"].ToString();
                    rawThreadDTO.Quality = reader["Quality"].ToString();
                    rawThreadDTO.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                    rawThreadDTOs.Add((T)rawThreadDTO);

                }
            }
            //return rawThreadDTOs.OfType<T>();
            return rawThreadDTOs.Select(m=>(T)m).ToList();
        }
    }
}
