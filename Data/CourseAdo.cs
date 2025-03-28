using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleRestAPI.Data;
using Microsoft.Data.SqlClient;
using SimpleRestAPI.Models;

namespace SimpleRestAPI.Data
{
    public class CourseAdo : ICours

    {
        private readonly IConfiguration _configuration;
        private string connStr = string.Empty;

        public CourseAdo(IConfiguration configuration)
        {
            _configuration = configuration;
            this.connStr = configuration.GetConnectionString("DefaultConnection"); //untuk mengambil connection stringnya
                                                                                   //jadi connection stringnya membaca dari appsetting.json
        }
        public Course AddCours(Course course)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"INSERT INTO Courses (CourseName, CourseDescription, Duration, CategoryId)
                      VALUES (@CourseName, @CourseDescription, @Duration, @CategoryId);
                      SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(strSql, conn);

                try
                {
                    cmd.Parameters.AddWithValue("@CourseName", course.CourseName);
                    cmd.Parameters.AddWithValue("@CourseDescription", course.CourseDescription);
                    cmd.Parameters.AddWithValue("@Duration", course.Duration);
                    cmd.Parameters.AddWithValue("@CategoryId", course.CategoryId);

                    conn.Open();
                    int courseId = Convert.ToInt32(cmd.ExecuteScalar());
                    course.CourseId = courseId;
                    return course;
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
            throw new NotImplementedException();
        }

        public void DeleteCours(int coursId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"DELETE FROM Courses WHERE CourseId = @CourseId";
                SqlCommand cmd = new SqlCommand(strSql, conn);

                try
                {
                    cmd.Parameters.AddWithValue("@CourseId", coursId);
                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result == 0)
                    {
                        throw new Exception("Course not found");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        public IEnumerable<ViewCourseWithCategory> GetCours()
        {

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"SELECT Courses.CourseId, Courses.CourseName, Courses.CourseDescription, Courses.Duration, Categories.CategoryId, Categories.CategoryName
                                    FROM Categories INNER JOIN
                                    Courses ON Categories.CategoryId = Courses.CategoryId";
                SqlCommand cmd = new SqlCommand(strSql, conn);
                List<ViewCourseWithCategory> courses = new List<ViewCourseWithCategory>();

                try
                {
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        ViewCourseWithCategory course = new ViewCourseWithCategory();
                        course.CourseId = Convert.ToInt32(dr["CourseId"]);
                        course.CourseName = dr["CourseName"].ToString();
                        course.CourseDescription = dr["CourseDescription"].ToString();
                        course.Duration = Convert.ToInt32(dr["Duration"]);
                        course.CategoryId = Convert.ToInt32(dr["CategoryId"]);
                        course.CategoryName = dr["CategoryName"].ToString();
                        courses.Add(course);
                    }
                    return courses;
                }
                catch (SqlException sqlEx)
                {
                    throw new Exception(sqlEx.Message);
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

        public ViewCourseWithCategory GetCoursById(int coursId)
        {
            /* 
           SELECT TOP (1000) [CourseId]
           ,[CourseName]
           ,[CourseDescription]
           ,[Duration]
           ,[CategoryId]
           ,[CategoryName]
           FROM [MyRestDb].[dbo].[ViewCoruseWithCategory]
           */
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                //select from viewcoursewithcategory
                string strSql = @"SELECT CourseId, CourseName, CourseDescription, Duration, CategoryId, CategoryName
                  FROM ViewCoruseWithCategory
                  WHERE CourseId = @CourseId";

                SqlCommand cmd = new SqlCommand(strSql, conn);
                cmd.Parameters.AddWithValue("@CourseId", coursId);

                ViewCourseWithCategory course = new ViewCourseWithCategory();

                try
                {
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        course.CourseId = Convert.ToInt32(dr["CourseId"]);
                        course.CourseName = dr["CourseName"].ToString();
                        course.CourseDescription = dr["CourseDescription"].ToString();
                        course.Duration = Convert.ToInt32(dr["Duration"]);
                        course.CategoryId = Convert.ToInt32(dr["CategoryId"]);
                        course.CategoryName = dr["CategoryName"].ToString();
                    }
                    return course;
                }
                catch (SqlException sqlEx)
                {
                    throw new Exception(sqlEx.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public Course UpdateCours(Course course)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string strSql = @"UPDATE Courses
                      SET CourseName = @CourseName,
                          CourseDescription = @CourseDescription,
                          Duration = @Duration,
                          CategoryId = @CategoryId
                      WHERE CourseId = @CourseId";

                SqlCommand cmd = new SqlCommand(strSql, conn);

                try
                {
                    cmd.Parameters.AddWithValue("@CourseName", course.CourseName);
                    cmd.Parameters.AddWithValue("@CourseDescription", course.CourseDescription);
                    cmd.Parameters.AddWithValue("@Duration", course.Duration);
                    cmd.Parameters.AddWithValue("@CategoryId", course.CategoryId);
                    cmd.Parameters.AddWithValue("@CourseId", course.CourseId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return course;
                }
                catch (SqlException sqlEx)
                {
                    throw new Exception(sqlEx.Message);
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