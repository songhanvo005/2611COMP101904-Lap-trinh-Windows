
using System;
using System.Data;
using System.Data.SqlClient;

namespace Lab12.DAL
{
    public class KetQuaDAL
    {
        // 1. Lấy danh sách Lớp
        public DataTable GetAllLop()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(Database.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_Lop_GetAll", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        // 2. Lấy danh sách Môn Học
        public DataTable GetAllMonHoc()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(Database.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_MonHoc_GetAll", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        // 3. Lấy danh sách điểm sinh viên theo Lớp và Môn học
        public DataTable GetDanhSachDiem(string maLop, string maMon)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(Database.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_KetQua_GetByLopMon", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaLop", maLop);
                    cmd.Parameters.AddWithValue("@MaMon", maMon);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        // 4. Lưu danh sách điểm sử dụng Transaction (đảm bảo tính toàn vẹn dữ liệu)
        public bool SaveDanhSachDiem(DataTable dtChanges, string maMon)
        {
            using (SqlConnection conn = new SqlConnection(Database.ConnectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction(); // Bắt đầu Transaction

                try
                {
                    foreach (DataRow row in dtChanges.Rows)
                    {
                        if (row.RowState == DataRowState.Deleted) continue;

                        string maSV = row["MaSV"].ToString();
                        float diem = Convert.ToSingle(row["Diem"]);
                        string ghiChu = row["GhiChu"] != DBNull.Value ? row["GhiChu"].ToString() : string.Empty;

                        using (SqlCommand cmd = new SqlCommand("sp_KetQua_Save", conn, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@MaSV", maSV);
                            cmd.Parameters.AddWithValue("@MaMon", maMon);
                            cmd.Parameters.AddWithValue("@Diem", diem);
                            cmd.Parameters.AddWithValue("@GhiChu", ghiChu);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit(); // Xác nhận lưu thành công toàn bộ
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback(); // Hoàn tác nếu có lỗi xảy ra
                    throw;
                }
            }
        }
    }
}