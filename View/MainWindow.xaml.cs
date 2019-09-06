using Kachatel2018.Model;
using Kachatel2018.ViewModel;
using Microsoft.Win32;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Kachatel2018
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Эффекты для окна.
        /// </summary>
        private UI.Effects WindowEffects { get; set; }

        /// <summary>
        /// Сообщения для консоли.
        /// </summary>
        public SystemMessages ConsoleMessage { get; set; }

        /// <summary>
        /// Настройки приложения.
        /// </summary>
        private static View.SettingsWindow _settingsWindow { get; set; }

        private string _pathToExcel="";
        public string PathToExcelDocument
        {
            get { return _pathToExcel; }
            set
            {
                _pathToExcel = value;
                if (_pathToExcel == "")
                {
                    stackLoaders.IsEnabled = true;
                    expanderFirstLoad.IsExpanded = false;
                    expanderSecondLoad.IsExpanded = false;
                    btnClear.IsEnabled = true;
                }
                else
                {
                    stackLoaders.IsEnabled = true;
                    expanderFirstLoad.IsExpanded = false;
                    expanderSecondLoad.IsExpanded = false;
                    btnClear.IsEnabled = true;
                }                    
            }
        }

        public DataTable ExcelData { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            WindowEffects = new UI.Effects();
            ConsoleMessage = new SystemMessages();
            ConsoleMessage.MessageList.Add(new SystemMessage() { Title = "Система готова к работе." });
            listInfo.DataContext = ConsoleMessage.MessageList;

            ExcelData = new DataTable();
            PathToExcelDocument = "";

            winSize = new NormalWindowSize();
        }

        private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnLoading_Click(object sender, RoutedEventArgs e)
        {
            WindowEffects.ApplyEffect(this);

            // Выбрать Excel-файл и загрузить данные из него.
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Title = "Выбрать шаблон для АСУ МС";
            dlg.FileName = "Document";
            dlg.DefaultExt = ".xlsm";
            dlg.Filter = "Excel (macros) |*.xlsm|Excel documents (.xlsx)|*.xlsx|Excel documents 2003 (.xls)|*.xls";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string fileName = dlg.FileName;
                PathToExcelDocument = fileName; // Записать путь к документу.
                
                AddUser_WriteMessageToConsole(String.Format("Подключен документ: {0}", PathToExcelDocument));

                LoadDataFromExcel();
                WindowEffects.ClearEffect(this);
            }
            else
            {
                WindowEffects.ClearEffect(this);
                return;
            }            
        }

        private void btnLookData_Click(object sender, RoutedEventArgs e)
        {
            WindowEffects.ApplyEffect(this);
            CheckData dataWindow = new CheckData(ExcelData);
            dataWindow.Owner = this;
            dataWindow.ShowDialog();
            WindowEffects.ClearEffect(this);
        }

        private void btnSettingUDL_Click(object sender, RoutedEventArgs e)
        {
            // Настроить Metr6.udl.
            System.Diagnostics.Process.Start("Settings\\ServerUDL\\Metr6.udl");

            AddUser_WriteMessageToConsole("Произведена настройка Metr6.udl.");
        }

        /// <summary>
        /// Загрузить данные из Excel.
        /// </summary>
        private void LoadDataFromExcel()
        {
            ExcelData.Clear();
            ReadExcelFiles readingExcel = new ReadExcelFiles();
            readingExcel.WriteMessageToConsole += AddUser_WriteMessageToConsole;
            ExcelData = readingExcel.GetExcelDataOnly(PathToExcelDocument);

            ViewModel.ExcelFiles file = new ViewModel.ExcelFiles(PathToExcelDocument);
        }

        private void btnLoadData_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                ImportToDataBase importing = new ImportToDataBase(ExcelData);
                importing.SetProgress += GetProgress;
                importing.WriteMessageToConsole += AddUser_WriteMessageToConsole;
                importing.LoadIntoKoks();

                this.Dispatcher.Invoke(new Action(() => this.progress.Value = 0));
            });
        }

        /// <summary>
        /// Установить процент выполнения действия.
        /// </summary>
        /// <param name="value"></param>
        public void GetProgress(double value)
        {
            progress.Dispatcher.Invoke(new Action(() => progress.Value = value));
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            InstallSize();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnLastEKZID_Click(object sender, RoutedEventArgs e)
        {
            DataBase bd = new DataBase();
            int idekz = bd.GetLastEkzemplyarID();

            AddUser_WriteMessageToConsole(String.Format("Последний ID в таблице EKZ: {0}\nНачните нумерацию в EXCEL c номера {1}", idekz, (idekz + 1)));
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            PathToExcelDocument = "";
            ExcelData.Clear();

            AddUser_WriteMessageToConsole("Память была очищена.");
        }

        private void btnClearKOKS_Click(object sender, RoutedEventArgs e)
        {
            // Очистить КОКС.           

            string messageText = "Вы действительно хотите очистить таблицу KOKS?";
            string messageTitle = "Удаление";

            if (MessageBox.Show(messageText, messageTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    DataBase clearKoks = new DataBase();
                    clearKoks.WriteMessageToConsole += AddUser_WriteMessageToConsole;
                    clearKoks.ClearTableKoks();
                }
                catch (Exception ex)
                {
                    rtbInformationToUser.AppendText(String.Format(ex.Message));
                    AddUser_WriteMessageToConsole(ex.Message);
                }
            }
            else
            {
                return;
            }
        }

        private void btnCreateKOKS_Click(object sender, RoutedEventArgs e)
        {
            // Создать Кокс.
            StartProgram startProg = new StartProgram();
            startProg.StartSQLScript(@"P:\Производственный отдел\АРМ Качателя\sql\03_createKOKS_2017.sql");            

            AddUser_WriteMessageToConsole("Запуск скрипта 'Создать КОКС'.");
        }

        private void btnDrivers_Click(object sender, RoutedEventArgs e)
        {
            WindowEffects.ApplyEffect(this);
            AdditionalWindow additions = new AdditionalWindow();
            additions.Owner = this;
            additions.ShowDialog();
            WindowEffects.ClearEffect(this);
        }

        private void btnAddUser_Click_1(object sender, RoutedEventArgs e)
        {
            // Добавление пользователя в prmUsers.
            WindowEffects.ApplyEffect(this);
            View.UserAdding addUser = new View.UserAdding();
            addUser.Owner = this;
            addUser.WriteMessageToConsole += AddUser_WriteMessageToConsole;
            addUser.ShowDialog();
            WindowEffects.ClearEffect(this);
        }

        private void AddUser_WriteMessageToConsole(string message)
        {
            this.Dispatcher.Invoke(new Action(()=> ConsoleMessage.MessageList.Add(new SystemMessage() { Title = message })));
        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            // Удаление всех пользователей фирмы Палитра систем.
            string messageText = "Вы действительно хотите удалить пользователей из таблицы prmUsers?";
            string messageTitle = "Удаление";

            if (MessageBox.Show(messageText, messageTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(Connections.ConnectionStrings.SQLConnectionStirng))
                    {
                        connection.Open();
                        System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand();
                        command.CommandText = @"delete from prmUsers where userName like '%PALITRA\%'";
                        command.Connection = connection;

                        command.ExecuteNonQuery();
                    }

                    rtbInformationToUser.AppendText(String.Format("\nТаблица prmUsers очищена."));
                    AddUser_WriteMessageToConsole("Таблица prmUsers очищена.");
                }
                catch (Exception ex)
                {
                    rtbInformationToUser.AppendText(String.Format(ex.Message));
                    AddUser_WriteMessageToConsole(ex.Message);
                }                
            }
            else
            {
                return;
            }
        }

        #region Работы с размерами формы

        private bool IsMaximized = false;
        private NormalWindowSize winSize { get; set; }

        private void InstallSize()
        {
            Rect rec = SystemParameters.WorkArea;
            if (IsMaximized)
            {
                this.Height = winSize.Heigth;
                this.Width = winSize.Width;
                this.Left = winSize.Left;
                this.Top = winSize.Top;
                IsMaximized = false;
                return;
            }

            if (!IsMaximized)
            {
                winSize.Heigth = this.Height;
                winSize.Width = this.Width;
                winSize.Left = this.Left;
                winSize.Top = this.Top;


                this.Width = rec.Size.Width;
                this.Height = rec.Size.Height;
                this.Top = rec.Top;
                this.Left = rec.Left;

                IsMaximized = true;
                return;
            }
        }

        #endregion

        private void mainWin_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (menuBTN.IsChecked == true)
            {
                menuBTN.IsChecked = false;
            }
        }

        private void btnUpdateDataBase_Click(object sender, RoutedEventArgs e)
        {
            // Обновить базу до последней версии.
            // Нужно указать путь к скрипту PowerShell.

            MessageBox.Show("Скоро будет реализовано. Простите!");
        }

        private void BtnCheckKoks_Click(object sender, RoutedEventArgs e)
        {
            // Проверка кокса.
            StartProgram startProg = new StartProgram();
            startProg.StartSQLScript(@"P:\Производственный отдел\АРМ Качателя\sql\проверка кокс.sql");

            AddUser_WriteMessageToConsole("Запуск скрипта 'Проверка КОКС'.");
        }

        private void BtnCreateproc_Click(object sender, RoutedEventArgs e)
        {
            // Создать служебные процедуры.
            StartProgram startProg = new StartProgram();
            startProg.StartSQLScript(@"P:\Производственный отдел\АРМ Качателя\sql\служпроц.sql");

            AddUser_WriteMessageToConsole("Запуск скрипта 'Создать служебные процедуры'.");
        }

        private void BtnLoadDMSStruct_Click(object sender, RoutedEventArgs e)
        {
            // Закачать структуру DMS.
            StartProgram startProg = new StartProgram();
            startProg.StartSQLScript(@"P:\Производственный отдел\АРМ Качателя\sql\06_fltr.sql");

            AddUser_WriteMessageToConsole("Запуск скрипта 'Закачать структуру DMS'.");
        }

        private void BtnLoadHandbooks_Click(object sender, RoutedEventArgs e)
        {
            // Закачать справочники.
            StartProgram startProg = new StartProgram();
            startProg.StartSQLScript(@"P:\Производственный отдел\АРМ Качателя\sql\07_Spr_2017.sql");

            AddUser_WriteMessageToConsole("Запуск скрипта 'Закачать справочники'.");
        }

        private void BtnLoadTypes_Click(object sender, RoutedEventArgs e)
        {
            // Закачать типы.
            StartProgram startProg = new StartProgram();
            startProg.StartSQLScript(@"P:\Производственный отдел\АРМ Качателя\sql\08_TIPS.sql");

            AddUser_WriteMessageToConsole("Запуск скрипта 'Закачать типы'.");
        }

        private void BtnLoadEkz_Click(object sender, RoutedEventArgs e)
        {
            // Закачать экземпляры.
            StartProgram startProg = new StartProgram();
            startProg.StartSQLScript(@"P:\Производственный отдел\АРМ Качателя\sql\09_EKZ_ALL_2017.sql");

            AddUser_WriteMessageToConsole("Запуск скрипта 'Закачать экземпляры'.");
        }

        private void BtnMergeExcel_Click(object sender, RoutedEventArgs e)
        {
            StartProgram startProg = new StartProgram();
            startProg.StartSQLScript(@"P:\Производственный отдел\АРМ Качателя\Эталонный_шаблон\Соединение нескольких Excel файлов в один\Merge Excel Files.exe");

            AddUser_WriteMessageToConsole("Запуск программы 'Соединение Excel файлов'.");
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            // окно настроек приложения.
            if (_settingsWindow == null)
            {
                _settingsWindow = new View.SettingsWindow();
                _settingsWindow.Owner = this;
                WindowEffects.ApplyEffect(this);
                _settingsWindow.ShowDialog();
                WindowEffects.ClearEffect(this);
            }
            else
            {
                WindowEffects.ApplyEffect(this);
                _settingsWindow.Visibility = Visibility;
                WindowEffects.ClearEffect(this);
            }
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            // Окно справки.
            View.HelpWindow help = new View.HelpWindow();
            help.Show();
        }

        private void BtnConfigSqlInser_Click(object sender, RoutedEventArgs e)
        {
            // Сформировать SQL-скрипт на вставку в KOKS.
            if (ExcelData.Rows.Count < 1)
            {
                MessageBox.Show("Пожалуйста, выберите Excel файл.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Куда сохраняем выбираем.
            string pathToSave = String.Empty;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Выберите путь для сохранения ...";
            saveFileDialog.Filter = "SQL file (*.sql)|*.sql|Text file (*.txt)|*.txt|C# file (*.cs)|*.cs";
            
            if (saveFileDialog.ShowDialog() == true)
                pathToSave = saveFileDialog.FileName;
            else
                return;

            Task.Factory.StartNew(()=>
            {
                ImportToDataBase import = new ImportToDataBase(ExcelData);
                import.SetProgress += GetProgress;
                import.WriteMessageToConsole += AddUser_WriteMessageToConsole;
                import.LoadIntoFile(pathToSave);

                this.Dispatcher.Invoke(new Action(() => this.progress.Value = 0));
            });
        }

        private void BtnClearConsole_Click(object sender, RoutedEventArgs e)
        {
            // Очистить консоль.
            ConsoleMessage.MessageList.Clear();
        }

        private void TxtAbout_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // О программе.
            About about = new About();
            about.Owner = this;
            WindowEffects.ApplyEffect(this);
            about.ShowDialog();
            WindowEffects.ClearEffect(this);
        }
    }
}
