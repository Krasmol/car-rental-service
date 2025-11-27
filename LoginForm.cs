using System;
using System.Drawing;
using System.Windows.Forms;

namespace CarRentalSystem
{
    public partial class LoginForm : Form
    {
        private DatabaseHelper dbHelper;
        private TextBox txtUsername, txtPassword;
        private Button btnLogin, btnRegister;
        private Label lblTitle, lblSubtitle;
        private Panel panelMain;

        public LoginForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            CreateControls();
        }

        private void CreateControls()
        {
            this.Text = "Сервис аренды автомобилей - Вход";
            this.Size = new System.Drawing.Size(450, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 245, 249);

            // Главная панель
            panelMain = new Panel();
            panelMain.Location = new System.Drawing.Point(30, 30);
            panelMain.Size = new System.Drawing.Size(390, 340);
            panelMain.BackColor = Color.White;
            panelMain.BorderStyle = BorderStyle.FixedSingle;

            // Заголовок
            lblTitle = new Label();
            lblTitle.Text = "🚗 CAR RENTAL";
            lblTitle.Font = new Font("Arial", 18, FontStyle.Bold);
            lblTitle.Location = new System.Drawing.Point(80, 25);
            lblTitle.Size = new System.Drawing.Size(230, 35);
            lblTitle.ForeColor = Color.FromArgb(41, 128, 185);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            lblSubtitle = new Label();
            lblSubtitle.Text = "Аренда автомобилей";
            lblSubtitle.Font = new Font("Arial", 11, FontStyle.Regular);
            lblSubtitle.Location = new System.Drawing.Point(80, 60);
            lblSubtitle.Size = new System.Drawing.Size(230, 25);
            lblSubtitle.ForeColor = Color.FromArgb(127, 140, 141);
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;

            // Поля ввода
            int y = 110;
            CreateLabel("Логин:", 50, y, Color.FromArgb(52, 73, 94));
            txtUsername = new TextBox()
            {
                Location = new System.Drawing.Point(50, y + 25),
                Size = new System.Drawing.Size(290, 30),
                Font = new Font("Arial", 10),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(236, 240, 241)
            };
            y += 70;

            CreateLabel("Пароль:", 50, y, Color.FromArgb(52, 73, 94));
            txtPassword = new TextBox()
            {
                Location = new System.Drawing.Point(50, y + 25),
                Size = new System.Drawing.Size(290, 30),
                Font = new Font("Arial", 10),
                BorderStyle = BorderStyle.FixedSingle,
                PasswordChar = '*',
                BackColor = Color.FromArgb(236, 240, 241)
            };
            y += 70;

            // Кнопки
            btnLogin = new Button()
            {
                Text = "Войти",
                Location = new System.Drawing.Point(50, y),
                Size = new System.Drawing.Size(140, 40),
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;

            btnRegister = new Button()
            {
                Text = "Регистрация",
                Location = new System.Drawing.Point(200, y),
                Size = new System.Drawing.Size(140, 40),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRegister.FlatAppearance.BorderSize = 0;

            btnLogin.Click += BtnLogin_Click;
            btnRegister.Click += BtnRegister_Click;

            // Тестовые данные
            Label lblTest = new Label();
            lblTest.Font = new Font("Arial", 8, FontStyle.Italic);
            lblTest.Location = new System.Drawing.Point(50, y + 50);
            lblTest.Size = new System.Drawing.Size(290, 20);
            lblTest.ForeColor = Color.FromArgb(149, 165, 166);
            lblTest.TextAlign = ContentAlignment.MiddleCenter;

            panelMain.Controls.AddRange(new Control[] {
                lblTitle, lblSubtitle, txtUsername, txtPassword, btnLogin, btnRegister, lblTest
            });

            this.Controls.Add(panelMain);
        }

        private void CreateLabel(string text, int x, int y, Color color)
        {
            Label label = new Label()
            {
                Text = text,
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(100, 20),
                Font = new Font("Arial", 9, FontStyle.Bold),
                ForeColor = color
            };
            panelMain.Controls.Add(label);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dbHelper.Login(username, password))
            {
                MessageBox.Show($"Добро пожаловать, {dbHelper.CurrentUser.FirstName}!",
                    "Успешный вход", MessageBoxButtons.OK, MessageBoxIcon.Information);

                UserMainForm mainForm = new UserMainForm(dbHelper);
                mainForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm(dbHelper);
            registerForm.ShowDialog();
        }
    }
}