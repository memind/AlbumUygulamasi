using DataAccessLayer.Context;
using Entities;
using System.Security.Cryptography.X509Certificates;

namespace UI
{
    public partial class AdminPanel : Form
    {
        AlbumStoreContext ctx;
        LoginForm baseForm;
        bool settingsStatus = false;
        public AdminPanel(LoginForm loginForm)
        {
            InitializeComponent();
            baseForm = loginForm;
        }

        private void AdminPanel_Load(object sender, EventArgs e)
        {
            this.Size = new Size(927, 327);
            FillDataByDefault();

            btnRemove.Enabled = false;
            btnUpdate.Enabled = false;
            btnSave.Enabled = false;

            gbAlbumPanel.Visible = false;
            btnLogOut.Visible = false;
            btnClose.Visible = false;

            nudDiscount.Increment = 0.01m;
            nudDiscount.DecimalPlaces = 2;
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            ctx = new AlbumStoreContext();


            if (rbSoldOut.Checked)
            {
                var table = ctx.Albums.Where(x => x.IsContinued == false).ToList();
                FilterTable(dgvTable, table);
            }

            if (rbOnGoing.Checked)
            {
                var table = ctx.Albums.Where(x => x.IsContinued == true).ToList();
                FilterTable(dgvTable, table);
            }

            if (rbLastTen.Checked)
            {
                var table = ctx.Albums.OrderByDescending(x => x.Id).Take(10).ToList();
                FilterTable(dgvTable, table);
            }

            if (rbDiscounted.Checked)
            {
                var table = ctx.Albums.Where(x => x.Discount > 0).ToList();
                FilterTable(dgvTable, table);
            }

            ClearGroupBox(gbFilter);
            CancelToStarting(this, btnRemove, btnUpdate, btnLogOut, btnClose, gbAlbumPanel);
        }

        private void btnClearFilter_Click(object sender, EventArgs e)
        {
            FillDataByDefault();
            ClearGroupBox(gbFilter);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ctx = new AlbumStoreContext();
            bool albumStatus;

            if (rbContinued.Checked)
                albumStatus = true;
            
            else if (rbDiscontinued.Checked)
                albumStatus = false;

            else
                albumStatus = true;

            Album album = new Album()
            {
                Artist = txtArtist.Text,
                Name = txtName.Text,
                ReleaseDate = dtpReleaseDate.Value,
                Price = decimal.Parse(nudPrice.Value.ToString()),
                Discount = float.Parse(nudDiscount.Value.ToString()),
                //IsContinued = rbOnGoing.Checked? true : false
                IsContinued = albumStatus
            };

            ctx.Albums.Add(album);
            ctx.SaveChanges();

            this.Size = new Size(927, 327);
            gbAlbumPanel.Visible = false;
            btnLogOut.Visible = false;
            btnClose.Visible = false;
            btnLogOut.Visible = false;
            btnClose.Visible = false;

            FillDataByDefault();
        }

        private void dgvTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnRemove.Enabled = true;
            btnUpdate.Enabled = true;
            FillUpdateDatas(dgvTable, txtName, txtArtist, dtpReleaseDate, nudPrice, nudDiscount, rbContinued, rbDiscontinued);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            ctx = new AlbumStoreContext();

            if (DataRowIsSelected(dgvTable))
            {
                Album? deletingAlbum = ctx.Albums.Find(dgvTable.SelectedCells[5].Value);

                DialogResult removeResult = new DialogResult();
                removeResult = MessageBox.Show(text: $"Do you want to continue removing '{deletingAlbum.Artist} - {deletingAlbum.Name}' album?", "Deleting Album", MessageBoxButtons.YesNo);

                if (deletingAlbum != null)
                {
                    if (removeResult == DialogResult.Yes)
                    {
                        ctx.Albums.Remove(deletingAlbum);
                    }

                    else
                        CancelToStarting(this, btnRemove, btnUpdate, btnLogOut, btnClose, gbAlbumPanel);
                }

                ctx.SaveChanges();
                FillDataByDefault();

                btnRemove.Enabled = false;
                btnUpdate.Enabled = false;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            this.Size = new Size(927, 571);
            btnSave.Enabled = true;
            btnAdd.Enabled = false;
            gbAlbumPanel.Visible = true;
            FillUpdateDatas(dgvTable, txtName, txtArtist, dtpReleaseDate, nudPrice, nudDiscount, rbContinued, rbDiscontinued);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearGroupBox(gbAlbumPanel);
            this.Size = new Size(927, 571);
            btnSave.Enabled = false;
            btnAdd.Enabled = true;
            gbAlbumPanel.Visible = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (DataRowIsSelected(dgvTable))
            {
                Album? updatingAlbum = ctx.Albums.Find(dgvTable.SelectedRows[0].Cells[5].Value);
                DialogResult saveResult = new DialogResult();
                saveResult = MessageBox.Show(text: $"Do you want to continue updating '{updatingAlbum.Artist} - {updatingAlbum.Name}' album?", "Updating Album", MessageBoxButtons.YesNo);

                if (updatingAlbum != null)
                {
                    if (saveResult == DialogResult.Yes)
                    {
                        updatingAlbum.Artist = txtArtist.Text;
                        updatingAlbum.Name = txtName.Text;
                        updatingAlbum.ReleaseDate = dtpReleaseDate.Value;
                        updatingAlbum.Price = Convert.ToDecimal(nudPrice.Value.ToString());
                        updatingAlbum.Discount = (float)nudDiscount.Value;

                        if (rbContinued.Checked)
                            updatingAlbum.IsContinued = true;

                        else if (rbDiscontinued.Checked)
                            updatingAlbum.IsContinued = false;

                        else updatingAlbum.IsContinued = true;

                        ctx.SaveChanges();
                        CancelToStarting(this, btnRemove, btnUpdate, btnLogOut, btnClose, gbAlbumPanel);
                    }
                }
            }

            CancelToStarting(this, btnRemove, btnUpdate, btnLogOut, btnClose, gbAlbumPanel);
            FillDataByDefault();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult exitResult = new DialogResult();
            exitResult = MessageBox.Show(text: $"Are you sure you want to close the program?", "Closing", MessageBoxButtons.YesNo);

            if (exitResult == DialogResult.Yes)
                this.Close();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            SettingsStatus(btnLogOut, btnClose, this, gbAlbumPanel, btnRemove, btnUpdate);
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            DialogResult logOutResult = new DialogResult();
            logOutResult = MessageBox.Show(text: $"Are you sure you want to log out?", "Log Out", MessageBoxButtons.YesNo);

            if (logOutResult == DialogResult.Yes)
                this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult closeResult = new DialogResult();
            closeResult = MessageBox.Show(text: $"Are you sure you want to close the program?", "Closing", MessageBoxButtons.YesNo);

            if (closeResult == DialogResult.Yes)
                Application.Exit();
        }

        private void AdminPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            baseForm.Show();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearGroupBox(gbAlbumPanel);
            FillDataByDefault();
            SettingsStatus(btnLogOut, btnClose, this, gbAlbumPanel, btnRemove, btnUpdate);
            CancelToStarting(this, btnRemove, btnUpdate, btnLogOut, btnClose, gbAlbumPanel);
        }

        /// <summary>
        /// Method that checks if any data is selected.
        /// </summary>
        /// <param name="datagrid"></param>
        /// <returns>Bool data selected</returns>
        private bool DataRowIsSelected(DataGridView datagrid)
        {
            bool result;

            if (datagrid.SelectedRows.Count > 0)
                result = true;

            else
                result = false;

            return result;
        }

        /// <summary>
        /// The method that prints the information of the selected data to the controls in the GroupBox.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="name"></param>
        /// <param name="artist"></param>
        /// <param name="dtp"></param>
        /// <param name="price"></param>
        /// <param name="nud"></param>
        /// <param name="rb1"></param>
        /// <param name="rb2"></param>
        private void FillUpdateDatas(DataGridView table, TextBox name, TextBox artist, DateTimePicker dtp, NumericUpDown price, NumericUpDown nud, RadioButton rb1, RadioButton rb2)
        {
            if (table.SelectedRows.Count == 1)
            {
                name.Text = table.SelectedRows[0].Cells[6].Value.ToString();
                artist.Text = table.SelectedRows[0].Cells[0].Value.ToString();
                dtp.Value = (DateTime)table.SelectedRows[0].Cells[1].Value;
                price.Value = (decimal)table.SelectedRows[0].Cells[2].Value;
                nud.Value = Convert.ToDecimal(table.SelectedRows[0].Cells[3].Value.ToString());
                if ((bool)table.SelectedRows[0].Cells[4].Value == true)
                {
                    rb1.Checked = true;
                    rb2.Checked = false;
                }
                else if ((bool)table.SelectedRows[0].Cells[4].Value == false)
                {
                    rb1.Checked = false;
                    rb2.Checked = true;
                }
            }

        }

        /// <summary>
        /// Method to reset table to default settings.
        /// </summary>
        private void FillDataByDefault()
        {
            ctx = new AlbumStoreContext();
            dgvTable.DataSource = null;
            dgvTable.Rows.Clear();

            var table = ctx.Albums.ToList();
            dgvTable.DataSource = table;

            dgvTable.Columns["Name"].DisplayIndex = 1;
            dgvTable.Columns["Artist"].DisplayIndex = 2;
            dgvTable.Columns["ReleaseDate"].DisplayIndex = 3;
            dgvTable.Columns["Price"].DisplayIndex = 4;
            dgvTable.Columns["Discount"].DisplayIndex = 5;
            dgvTable.Columns["IsContinued"].DisplayIndex = 6;
            dgvTable.Columns["Id"].Visible = false;

            dgvTable.ClearSelection();
        }

        /// <summary>
        /// Method that performs filtering.
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="table"></param>
        private void FilterTable(DataGridView dgv, List<Album> table)
        {

            dgv.DataSource = table;

            dgv.Columns["Id"].DisplayIndex = 1;
            dgv.Columns["Name"].DisplayIndex = 2;
            dgv.Columns["Artist"].DisplayIndex = 3;
            dgv.Columns["Id"].Visible = false;
            dgv.Columns["ReleaseDate"].Visible = false;
            dgv.Columns["Price"].Visible = false;
            dgv.Columns["Discount"].Visible = false;
            dgv.Columns["IsContinued"].Visible = false;

            dgv.ClearSelection();
        }

        /// <summary>
        /// Method to clear GroupBox.
        /// </summary>
        /// <param name="gb"></param>
        private void ClearGroupBox(GroupBox gb)
        {
            foreach (Control ctrl in gb.Controls)
            {
                if (ctrl is TextBox)
                    ctrl.Text = "";

                if (ctrl is MaskedTextBox)
                    ctrl.Text = "";

                if (ctrl is RadioButton)
                    ((RadioButton)ctrl).Checked = false;

                if (ctrl is DateTimePicker)
                    ((DateTimePicker)ctrl).Value = DateTime.Now;

                if (ctrl is NumericUpDown)
                    ((NumericUpDown)ctrl).Value = 0;
            }
        }

        /// <summary>
        /// Method that checks the status of the Settings.
        /// </summary>
        /// <param name="logOut"></param>
        /// <param name="close"></param>
        /// <param name="form"></param>
        /// <param name="albumPanel"></param>
        /// <param name="remove"></param>
        /// <param name="update"></param>
        private void SettingsStatus(Button logOut, Button close, Form form, GroupBox albumPanel, Button remove, Button update)
        {
            if (settingsStatus)
            {
                settingsStatus = false;

                ClearGroupBox(gbAlbumPanel);
                FillDataByDefault();
                CancelToStarting(this, btnRemove, btnUpdate, btnLogOut, btnClose, gbAlbumPanel);
            }

            else
            {
                logOut.Visible = true;
                close.Visible = true;
                settingsStatus = true;

                form.Size = new Size(927, 571);
            }
        }

        /// <summary>
        /// Method that returns default settings.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="remove"></param>
        /// <param name="update"></param>
        /// <param name="logOut"></param>
        /// <param name="close"></param>
        /// <param name="albumPanel"></param>
        private void CancelToStarting(AdminPanel form, Button remove, Button update, Button logOut, Button close, GroupBox albumPanel)
        {
            albumPanel.Visible = false;
            form.Size = new Size(927, 327);
            remove.Enabled = false;
            update.Enabled = false;
            logOut.Visible = false;
            close.Visible = false;
        }
    }
}
