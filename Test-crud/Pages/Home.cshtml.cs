using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Test_crud.Pages
{
    public class HomeModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public List<UserInfo> listUsers = new List<UserInfo>();
        public string errorMessage = "";

        public HomeModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

            try {
                string connectionString = "Data Source=(localdb)\\local;Initial Catalog=test;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlQuery = "SELECT * FROM users_info";

                    using(SqlCommand cmd = new SqlCommand(sqlQuery,connection))
                    {
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                UserInfo user = new UserInfo
                                {
                                    User_id = reader.GetInt32(0),
                                    User_name = reader.GetString(1),
                                    User_email = reader.GetString(2),
                                    User_age = reader.GetInt32(3),
                                    User_sex = reader.GetString(4),
                                    User_is_engaged = reader.GetBoolean(5),
                                    Created_date = reader.GetDateTime(6)
                                };
                                listUsers.Add(user);
                            }
                        }

                    }
                }



            }
            catch (Exception e)
            {
                errorMessage = e.ToString();
                Console.WriteLine(e.ToString());
            }

        }
    }

    public class UserInfo
    {
        public int User_id { get; set; }
        public string User_name { get; set; }
        public string User_email { get; set; }
        public int? User_age { get; set; }
        public string User_sex { get; set; }
        public bool User_is_engaged { get; set; }
        public DateTime Created_date { get; set; }

    }
}