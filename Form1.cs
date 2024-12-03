using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LoginFormExample;
using MySql.Data.MySqlClient;


namespace LoginFormExmaple
{
    public partial class Form1 : Form
    {
        private string connectionString = "Server=localhost;Database=LOGINFORM;Uid=root;Pwd=2012A2015a$;";
        private List<User> users;

        public Form1()
        {
            InitializeComponent();

            try
            {
                users = GetUsersFromDatabase(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while initializing the users: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            button1.Click += button1_Click;
            textPassword.UseSystemPasswordChar = true;
          

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string username = textUsername.Text;
                string password = textPassword.Text;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Username and Password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var user = users.Find(u => u.Username == username && u.Password == password);

                if (user != null)
                {
                    MessageBox.Show("Login successful! Welcome!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    NavigateBasedOnRole(user);
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Unexpected Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            textUsername.Clear();
            textPassword.Clear();
        }

        private void NavigateBasedOnRole(User user)
        {
            try
            {
                if (user.Role == "Patient")
                {
                    MessageBox.Show("Redirecting to patient portal...", "Redirect", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (user.Role == "Doctor")
                {
                    MessageBox.Show("Redirecting to doctor portal...", "Redirect", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Role is not recognized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while navigating based on role: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<User> GetUsersFromDatabase()
        {
            var users = new List<User>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Username, Password, Role FROM Users";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(new User(
                                    reader["Username"].ToString(),
                                    reader["Password"].ToString(),
                                    reader["Role"].ToString()
                                ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while fetching users: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return users;
        }
    }

}
