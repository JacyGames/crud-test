using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace Test_crud.Pages
{
    public class EditUserModel : PageModel
    {

        public UserInfo userInfo = new();
        public string errorMessage = "";
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=(localdb)\\local;Initial Catalog=test;Integrated Security=True";
                string id = Request.Query["id"];
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlQuery = "SELECT * FROM users_info WHERE user_id=@id";
                    using(SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                       
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                userInfo.User_id = reader.GetInt32(0);
                                userInfo.User_name = reader.GetString(1);
                                userInfo.User_email = reader.GetString(2);
                                userInfo.User_age = reader.GetInt32(3);
                                userInfo.User_sex = reader.GetString(4);
                                userInfo.User_is_engaged = reader.GetBoolean(5);
                                userInfo.Created_date = reader.GetDateTime(6);
                            }
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
            }
            
        }

        public void OnPost()
        {
            try
            {
                string connectionString = "Data Source=(localdb)\\local;Initial Catalog=test;Integrated Security=True";
                string id = Request.Query["id"];
                UserInfo userInfo = new UserInfo()
                {
                    User_name = Request.Form["User_name"],
                    User_age = Int32.Parse(Request.Form["User_age"]),
                    User_email = Request.Form["User_email"],
                    User_sex = Request.Form["User_sex"],
                    User_is_engaged = Request.Form["User_is_engaged"].IsNullOrEmpty() ? false : true,

                };
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    string sqlQuery = "UPDATE users_info "
                        + "SET user_name=@user_name, user_age=@user_age, user_email=@user_email, user_sex=@user_sex, " +
                        "user_is_engaged=@user_is_engaged " +
                        "WHERE user_id=@id";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {

                        command.Parameters.AddWithValue("@user_name", userInfo.User_name);
                        command.Parameters.AddWithValue("@user_age", userInfo.User_age);
                        command.Parameters.AddWithValue("@user_email", userInfo.User_email);
                        command.Parameters.AddWithValue("@user_sex", userInfo.User_sex);
                        command.Parameters.AddWithValue("@user_is_engaged", userInfo.User_is_engaged);
                        command.Parameters.AddWithValue("@id", id);

                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
            Response.Redirect("/Home");
        }
    }
}
