using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace Test_crud.Pages
{
    public class Index1Model : PageModel
    {
        public UserInfo userInfo = new();
        public string errorMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            try
            {
                string connectionString = "Data Source=(localdb)\\local;Initial Catalog=test;Integrated Security=True";
                Console.WriteLine(Request.Form["User_is_engaged"]);
                UserInfo userInfo = new UserInfo()
                {
                    User_name = Request.Form["User_name"],
                    User_age = Int32.Parse(Request.Form["User_age"]),
                    User_email = Request.Form["User_email"],
                    User_sex = Request.Form["User_sex"],
                    User_is_engaged = Request.Form["User_is_engaged"].IsNullOrEmpty() ? false : true,

                };

                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlCreate = "INSERT INTO users_info (user_name, user_email, user_age, user_sex, user_is_engaged) " +
                        "VALUES (@user_name, @user_email, @user_age, @user_sex, @user_is_engaged);";
                    using(SqlCommand command = new SqlCommand(sqlCreate, connection))
                    {
                        command.Parameters.AddWithValue("@user_name", userInfo.User_name);
                        command.Parameters.AddWithValue("@user_email", userInfo.User_email);
                        command.Parameters.AddWithValue("@user_age", userInfo.User_age);
                        command.Parameters.AddWithValue("@user_sex", userInfo.User_sex);
                        command.Parameters.AddWithValue("@user_is_engaged", userInfo.User_is_engaged);
                        command.ExecuteNonQuery();

                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                errorMessage = ex.Message;
                return;
            }
        }
    }
}
