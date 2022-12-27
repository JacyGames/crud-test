using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace Test_crud.Pages
{
    public class HomeModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private string connectionString = "Data Source=(localdb)\\local;Initial Catalog=test;Integrated Security=True";

        public List<UserInfo> listUsers = new List<UserInfo>();
        public string errorMessage = "";
        public int rowsCount = 0;
        public int portionSize = 10;
        public int currentPage = 0;

        public HomeModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

            try
            {
                rowsCount = GetRowsCount();
                if (Request.Query["page"].IsNullOrEmpty())
                {
                    currentPage = 1;
                }
                else {
                    currentPage = Int32.Parse(Request.Query["page"]);
                }

                int offset = currentPage * portionSize - portionSize;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlQuery = "SELECT * FROM users_info ORDER BY created_date OFFSET @offset ROWS FETCH NEXT @rows ROWS ONLY";

                    using(SqlCommand cmd = new SqlCommand(sqlQuery,connection))
                    {
                        cmd.Parameters.AddWithValue("@offset", offset);
                        cmd.Parameters.AddWithValue("rows", portionSize);
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

        public int GetRowsCount()
        {
            int rowsCount = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlQuery = "SELECT COUNT(*) FROM users_info";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rowsCount = reader.GetInt32(0);
                        }
                    }

                }
            }

            return rowsCount;
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