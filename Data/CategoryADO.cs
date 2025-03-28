using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;


namespace SimpleRestAPI.Data
{
    public class CategoryADO : ICategory
    {
        private readonly IConfiguration _configuration;
        private string connStr = string.Empty;

        public CategoryADO(IConfiguration configuration)
        {
            _configuration = configuration;
            connStr = configuration.GetConnectionString("DefaultConnection"); //untuk mengambil connection stringnya
                                                                              //jadi connection stringnya membaca dari appsetting.json
        }

        public Category AddCategory(Category category)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"INSERT INTO Categories (CategoryName) 
                                VALUES (@categoryName); 
                                SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                try
                {
                    
                    cmd.Parameters.AddWithValue("@CategoryName", category.Categoryname);
                    conn.Open();
                    category.categoryId = Convert.ToInt32(cmd.ExecuteScalar()); // ExecuteScalar() supaya ada return value (tapi hanya 1)
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if(rowsAffected == 0)
                    {
                        throw new Exception("Insert failed");
                    }
                    return category;

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
            }
        }

        public void DeleteCategory(int categoryId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"DELETE FROM Categories WHERE CategoryId = @CategoryId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                try
                {
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result == 0)
                    {   
                        throw new Exception("Category not found");
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
            }
        }

        public IEnumerable<Category> GetCategories()
        {
            List<Category> categories = new List<Category>(); // pakai list karena kategory yang mau 
                                                              // ditampilkan lebih dari 1 (array) makanya ditampung didalam list
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"SELECT * FROM Categories ORDER BY CategoryName";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                conn.Open(); //mengambil data dari tabel
                SqlDataReader dr = cmd.ExecuteReader(); //membaca data pake datareader
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Category category = new();
                        category.categoryId = Convert.ToInt32(dr["CategoryId"]);
                        category.Categoryname = dr["CategoryName"].ToString();
                        categories.Add(category); //dimapping ke kelas jadi objeknya ditambah dlebih dari 1
                    }
                }
                dr.Close();
                cmd.Dispose();
                conn.Close();
            }
            return categories;
        }

        public Category GetCategoryById(int categoryId)
        {
            Category category = new();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"SELECT * FROM Categories 
                                WHERE CategoryId =@categoryId"; //penggunaan parameter dilakukan untuk menjaga keamanan (menghindari SQL Injection)
                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                conn.Open(); //mengambil data dari tabel
                SqlDataReader dr = cmd.ExecuteReader(); //membaca data pake datareader
                if (dr.HasRows)
                {
                    dr.Read();
                    category.categoryId = Convert.ToInt32(dr["CategoryId"]);
                    category.Categoryname = dr["CategoryName"].ToString();
                }
                else
                {
                    throw new Exception("Category not found");
                }
            }
            return category;
        }

        public Category UpdateCategory(Category category)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"UPDATE Categories SET 
                                CategoryName= @CategoryName
                                WHERE categoryId = @CategoryId";      
                SqlCommand cmd = new SqlCommand(strSql, conn);
                try
                {  
                    cmd.Parameters.AddWithValue("@CategoryName", category.Categoryname);
                    cmd.Parameters.AddWithValue("@CategoryId", category.categoryId);
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();//ExecuteNonQuery() menghasilkan nilai integer 
                                                       // untuk menentukan truefalse dibawah pada if
                    if(result == 0)
                    {
                        throw new Exception ("Category not found");
                    }
                    return category;
                }
                catch (Exception ex)
                {   
                    throw new Exception(ex.Message);
                }
                finally
                {   
                    cmd.Dispose();
                    conn.Close();
                }
            }
        }
    }
}