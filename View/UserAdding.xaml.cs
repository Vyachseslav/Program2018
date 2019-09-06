using System;
using System.Data.SqlClient;
using System.Windows;

namespace Kachatel2018.View
{
    /// <summary>
    /// Логика взаимодействия для UserAdding.xaml
    /// </summary>
    public partial class UserAdding : Window
    {
        public delegate void SetMessage(string message);
        public event SetMessage WriteMessageToConsole;

        Model.User NewUser { get; set; }

        public UserAdding()
        {
            InitializeComponent();

            NewUser = new Model.User();
            userPanel.DataContext = NewUser;

            txtUserName.Focus();
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            // Добавление пользователя в prmUsers.
            if (txtUserName.Text == "")
            {
                MessageBox.Show("Вам необходимо добавить имя пользователя!", "Ошибка заполнения", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            AddNewUser(txtUserName.Text, int.Parse(txtRoleIds.Text));
        }

        /// <summary>
        /// Добавить пользователя в prmUsers.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="roleIds">Роль пользователя.</param>
        private void AddNewUser(string userName, int roleIds = 512)
        {
            userName = "PALITRA\\" + userName;
            using (SqlConnection connection = new SqlConnection(Connections.ConnectionStrings.SQLConnectionStirng))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                SqlCommand command = connection.CreateCommand();
                command.Transaction = transaction;
                try
                {
                    string zapr = String.Format("insert into prmUsers (userName, roleIds) values ('{0}', {1})", userName, roleIds);
                    
                    command.CommandText = zapr;
                    command.ExecuteNonQuery();
                    transaction.Commit(); // Подтверждаем транзакцию.
                    WriteMessage(String.Format("Добавлен пользователь {0} с правами {1}", userName, roleIds));
                    this.Close();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Ошибка при добавлении пользователя.\n" + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    WriteMessage(String.Format("Ошибка при добавлении пользователя.\n{0}", ex.Message));
                }
            }
        }

        private void WriteMessage(string mess)
        {
            if (WriteMessageToConsole != null)
            {
                WriteMessageToConsole.Invoke(mess);
            }
        }
    }
}
