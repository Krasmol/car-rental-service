using System;
using System.Drawing;
using System.Windows.Forms;

namespace CarRentalSystem
{
    public partial class RegisterForm : Form
    {
        private DatabaseHelper dbHelper;
        private TextBox txtUsername, txtPassword, txtConfirmPassword, txtFirstName, txtLastName, txtPhone, txtLicense;
        private Button btnRegister, btnCancel;
        private Panel panelMain;
        private Label lblTitle;

        public RegisterForm(DatabaseHelper db)
        {
            dbHelper = db;
            InitializeComponent();
            CreateControls();
        }

        private void CreateControls()
        {
            this.Text = "Регистрация";
            this.Size = new System.Drawing.Size(500, 650); // УВЕЛИЧИЛ ВЫСОТУ
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 245, 249);

            // Главная панель
            panelMain = new Panel();
            panelMain.Location = new System.Drawing.Point(25, 20);
            panelMain.Size = new System.Drawing.Size(450, 600); // УВЕЛИЧИЛ ВЫСОТУ
            panelMain.BackColor = Color.White;
            panelMain.BorderStyle = BorderStyle.FixedSingle;

            // Заголовок
            lblTitle = new Label();
            lblTitle.Text = "📝 Регистрация";
            lblTitle.Font = new Font("Arial", 16, FontStyle.Bold);
            lblTitle.Location = new System.Drawing.Point(125, 20);
            lblTitle.Size = new System.Drawing.Size(200, 30);
            lblTitle.ForeColor = Color.FromArgb(41, 128, 185);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            int y = 70;

            // Логин и пароль
            CreateLabel("Логин*:", 50, y);
            txtUsername = CreateTextBox(50, y + 25); y += 60;

            CreateLabel("Пароль*:", 50, y);
            txtPassword = CreateTextBox(50, y + 25);
            txtPassword.PasswordChar = '*'; y += 60;

            CreateLabel("Подтверждение*:", 50, y);
            txtConfirmPassword = CreateTextBox(50, y + 25);
            txtConfirmPassword.PasswordChar = '*'; y += 70;

            // Личная информация
            CreateLabel("Имя*:", 50, y);
            txtFirstName = CreateTextBox(50, y + 25); y += 60;

            CreateLabel("Фамилия*:", 50, y);
            txtLastName = CreateTextBox(50, y + 25); y += 60;

            CreateLabel("Телефон:", 50, y);
            txtPhone = CreateTextBox(50, y + 25); y += 60;

            CreateLabel("Водительские права*:", 50, y);
            txtLicense = CreateTextBox(50, y + 25); y += 80; // УВЕЛИЧИЛ ОТСТУП

            // Кнопки
            btnRegister = new Button()
            {
                Text = "✅ Зарегистрироваться",
                Location = new System.Drawing.Point(80, y),
                Size = new System.Drawing.Size(150, 40),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRegister.FlatAppearance.BorderSize = 0;

            btnCancel = new Button()
            {
                Text = "❌ Отмена",
                Location = new System.Drawing.Point(240, y),
                Size = new System.Drawing.Size(120, 40),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            btnRegister.Click += BtnRegister_Click;
            btnCancel.Click += (s, e) => this.Close();
            // Подсказка
            Label lblHint = new Label();
            lblHint.Text = "* - обязательные поля";
            lblHint.Font = new Font("Arial", 8, FontStyle.Italic);
            lblHint.Location = new System.Drawing.Point(50, y + 50);
            lblHint.Size = new System.Drawing.Size(350, 20);
            lblHint.ForeColor = Color.FromArgb(149, 165, 166);
            lblHint.TextAlign = ContentAlignment.MiddleCenter;

            panelMain.Controls.AddRange(new Control[] {
                lblTitle, btnRegister, btnCancel, lblHint
            });

            this.Controls.Add(panelMain);
        }

        private void CreateLabel(string text, int x, int y)
        {
            Label label = new Label()
            {
                Text = text,
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(350, 20),
                Font = new Font("Arial", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            panelMain.Controls.Add(label);
        }

        private TextBox CreateTextBox(int x, int y)
        {
            TextBox textBox = new TextBox()
            {
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(350, 25), // УВЕЛИЧИЛ ШИРИНУ
                Font = new Font("Arial", 10),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(236, 240, 241)
            };
            panelMain.Controls.Add(textBox);
            return textBox;
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            var user = new User
            {
                Username = txtUsername.Text.Trim(),
                Password = txtPassword.Text,
                FirstName = txtFirstName.Text.Trim(),
                LastName = txtLastName.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                DriverLicense = txtLicense.Text.Trim()
            };

            if (dbHelper.RegisterUser(user))
            {
                MessageBox.Show("Регистрация прошла успешно! Теперь вы можете войти в систему.",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(txtUsername.Text.Trim()) ||
                string.IsNullOrEmpty(txtPassword.Text) ||
                string.IsNullOrEmpty(txtFirstName.Text.Trim()) ||
                string.IsNullOrEmpty(txtLastName.Text.Trim()) ||
                string.IsNullOrEmpty(txtLicense.Text.Trim()))
            {
                MessageBox.Show("Заполните все обязательные поля!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (txtPassword.Text.Length < 3)
            {
                MessageBox.Show("Пароль должен содержать минимум 3 символа!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}