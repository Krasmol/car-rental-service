using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CarRentalSystem
{
    public partial class UserMainForm : Form
    {
        private DatabaseHelper dbHelper;
        private TabControl tabControl;
        private DataGridView dgvAvailableCars, dgvMyRentals;
        private Button btnRentCar, btnReturnCar, btnRefresh, btnLogout;
        private Label lblWelcome, lblBalance;
        private Panel panelHeader;

        public UserMainForm(DatabaseHelper db)
        {
            dbHelper = db;
            InitializeComponent();
            CreateControls();
            LoadData();
            StyleDataGridViews();
        }

        private void CreateControls()
        {
            this.Text = "Сервис аренды автомобилей";
            this.Size = new System.Drawing.Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 245, 249);

            // Панель заголовка
            panelHeader = new Panel();
            panelHeader.Location = new System.Drawing.Point(0, 0);
            panelHeader.Size = new System.Drawing.Size(1000, 80);
            panelHeader.BackColor = Color.FromArgb(52, 73, 94);
            panelHeader.BorderStyle = BorderStyle.FixedSingle;

            // Приветствие
            lblWelcome = new Label();
            lblWelcome.Text = $"🚗 Добро пожаловать, {dbHelper.CurrentUser.FirstName} {dbHelper.CurrentUser.LastName}!";
            lblWelcome.Font = new Font("Arial", 14, FontStyle.Bold);
            lblWelcome.Location = new System.Drawing.Point(20, 20);
            lblWelcome.Size = new System.Drawing.Size(500, 30);
            lblWelcome.ForeColor = Color.White;

            // Баланс
            lblBalance = new Label();
            lblBalance.Text = "💳 Баланс: --- руб";
            lblBalance.Font = new Font("Arial", 11, FontStyle.Regular);
            lblBalance.Location = new System.Drawing.Point(20, 50);
            lblBalance.Size = new System.Drawing.Size(300, 20);
            lblBalance.ForeColor = Color.FromArgb(236, 240, 241);

            // TabControl
            tabControl = new TabControl();
            tabControl.Location = new System.Drawing.Point(20, 100);
            tabControl.Size = new System.Drawing.Size(960, 450);
            tabControl.Font = new Font("Arial", 10, FontStyle.Regular);

            // Вкладка 1: Доступные автомобили
            TabPage tabAvailableCars = new TabPage("🚗 Доступные автомобили");
            tabAvailableCars.BackColor = Color.White;
            dgvAvailableCars = new DataGridView();
            dgvAvailableCars.Location = new System.Drawing.Point(10, 10);
            dgvAvailableCars.Size = new System.Drawing.Size(930, 390);
            dgvAvailableCars.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAvailableCars.ReadOnly = true;
            dgvAvailableCars.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAvailableCars.BackgroundColor = Color.White;
            tabAvailableCars.Controls.Add(dgvAvailableCars);

            // Вкладка 2: Мои аренды
            TabPage tabMyRentals = new TabPage("📋 Мои аренды");
            tabMyRentals.BackColor = Color.White;
            dgvMyRentals = new DataGridView();
            dgvMyRentals.Location = new System.Drawing.Point(10, 10);
            dgvMyRentals.Size = new System.Drawing.Size(930, 390);
            dgvMyRentals.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMyRentals.ReadOnly = true;
            dgvMyRentals.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMyRentals.BackgroundColor = Color.White;
            tabMyRentals.Controls.Add(dgvMyRentals);

            tabControl.TabPages.Add(tabAvailableCars);
            tabControl.TabPages.Add(tabMyRentals);
            // Кнопки
            btnRentCar = new Button()
            {
                Text = "🚗 Арендовать выбранный авто",
                Location = new System.Drawing.Point(20, 570),
                Size = new System.Drawing.Size(200, 40),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRentCar.FlatAppearance.BorderSize = 0;

            btnReturnCar = new Button()
            {
                Text = "🔄 Вернуть авто",
                Location = new System.Drawing.Point(230, 570),
                Size = new System.Drawing.Size(140, 40),
                BackColor = Color.FromArgb(230, 126, 34),
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnReturnCar.FlatAppearance.BorderSize = 0;

            btnRefresh = new Button()
            {
                Text = "🔄 Обновить",
                Location = new System.Drawing.Point(380, 570),
                Size = new System.Drawing.Size(120, 40),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;

            btnLogout = new Button()
            {
                Text = "🚪 Выйти",
                Location = new System.Drawing.Point(860, 570),
                Size = new System.Drawing.Size(120, 40),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;

            btnRentCar.Click += BtnRentCar_Click;
            btnReturnCar.Click += BtnReturnCar_Click;
            btnRefresh.Click += (s, e) => { LoadData(); StyleDataGridViews(); };
            btnLogout.Click += BtnLogout_Click;

            panelHeader.Controls.AddRange(new Control[] { lblWelcome, lblBalance });
            this.Controls.AddRange(new Control[] {
                panelHeader, tabControl, btnRentCar, btnReturnCar, btnRefresh, btnLogout
            });
        }

        private void StyleDataGridViews()
        {
            foreach (DataGridView dgv in new[] { dgvAvailableCars, dgvMyRentals })
            {
                dgv.BorderStyle = BorderStyle.None;
                dgv.EnableHeadersVisualStyles = false;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185);
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
                dgv.RowHeadersVisible = false;
                dgv.DefaultCellStyle.Font = new Font("Arial", 9);
                dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
                dgv.GridColor = Color.FromArgb(224, 224, 224);
            }
        }

        private void LoadData()
        {
            dgvAvailableCars.DataSource = dbHelper.GetAvailableCarsData();
            dgvMyRentals.DataSource = dbHelper.GetUserRentalsData(dbHelper.CurrentUser.UserID);
        }

        private void BtnRentCar_Click(object sender, EventArgs e)
        {
            if (dgvAvailableCars.CurrentRow == null)
            {
                MessageBox.Show("Выберите автомобиль для аренды!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int carId = Convert.ToInt32(dgvAvailableCars.CurrentRow.Cells["ID"].Value);
            var car = dbHelper.GetCars().FirstOrDefault(c => c.CarID == carId);

            if (car == null)
            {
                MessageBox.Show("Ошибка: автомобиль не найден!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Форма для ввода количества дней
            using (var daysForm = new Form())
            {
                daysForm.Text = "Аренда автомобиля";
                daysForm.Size = new System.Drawing.Size(350, 200);
                daysForm.StartPosition = FormStartPosition.CenterParent;
                daysForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                daysForm.BackColor = Color.White;
                daysForm.Font = new Font("Arial", 10);

                Label lblInfo = new Label()
                {
                    Text = $"🚗 {car.Brand} {car.Model}\n💵 Цена: {car.PricePerDay} руб/день",
                    Location = new System.Drawing.Point(20, 20),
                    Size = new System.Drawing.Size(300, 50),
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    ForeColor = Color.FromArgb(52, 73, 94)
                };
                Label lblDays = new Label()
                {
                    Text = "Количество дней:",
                    Location = new System.Drawing.Point(20, 80),
                    Size = new System.Drawing.Size(150, 20),
                    ForeColor = Color.FromArgb(52, 73, 94)
                };
                TextBox txtDays = new TextBox()
                {
                    Location = new System.Drawing.Point(170, 80),
                    Size = new System.Drawing.Size(50, 25),
                    Text = "1",
                    Font = new Font("Arial", 10),
                    BorderStyle = BorderStyle.FixedSingle
                };
                Button btnOk = new Button()
                {
                    Text = "✅ Арендовать",
                    Location = new System.Drawing.Point(80, 120),
                    Size = new System.Drawing.Size(120, 35),
                    BackColor = Color.FromArgb(46, 204, 113),
                    ForeColor = Color.White,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat
                };
                Button btnCancel = new Button()
                {
                    Text = "❌ Отмена",
                    Location = new System.Drawing.Point(210, 120),
                    Size = new System.Drawing.Size(100, 35),
                    BackColor = Color.FromArgb(231, 76, 60),
                    ForeColor = Color.White,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat
                };

                btnOk.Click += (s, e2) => {
                    if (int.TryParse(txtDays.Text, out int days) && days > 0)
                    {
                        var rental = new Rental
                        {
                            CarID = carId,
                            ClientID = dbHelper.CurrentUser.UserID,
                            Days = days,
                            TotalPrice = car.PricePerDay * days,
                            Status = "Активна"
                        };

                        if (dbHelper.AddRental(rental))
                        {
                            MessageBox.Show($"✅ Аренда оформлена!\n💵 Стоимость: {rental.TotalPrice} руб",
                                "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                            StyleDataGridViews();
                            daysForm.DialogResult = DialogResult.OK;
                            daysForm.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Введите корректное количество дней!", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                btnCancel.Click += (s, e2) => daysForm.Close();

                daysForm.Controls.AddRange(new Control[] { lblInfo, lblDays, txtDays, btnOk, btnCancel });
                daysForm.ShowDialog();
            }
        }

        private void BtnReturnCar_Click(object sender, EventArgs e)
        {
            if (dgvMyRentals.CurrentRow == null)
            {
                MessageBox.Show("Выберите аренду для возврата!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int rentalId = Convert.ToInt32(dgvMyRentals.CurrentRow.Cells["ID"].Value);
            var rental = dbHelper.GetRentals().FirstOrDefault(r => r.RentalID == rentalId);

            if (rental == null || rental.Status != "Активна")
            {
                MessageBox.Show("Нельзя вернуть завершенную аренду!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Вы уверены, что хотите вернуть автомобиль?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (dbHelper.CompleteRental(rentalId))
                {
                    MessageBox.Show("✅ Автомобиль возвращен!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    StyleDataGridViews();
                }
            }
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти из системы?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dbHelper.Logout();
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Close();
            }
        }
    }
}