using DataAccessLayer.Context;
using Entities;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class LoginForm : Form
    {
        AlbumStoreContext ctx;
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult closeResult = new DialogResult();
            closeResult = MessageBox.Show(text: $"Are you sure you want to close the program?", "Closing", MessageBoxButtons.YesNo);

            if (closeResult == DialogResult.Yes)
                Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (LoginControl(txtName.Text, txtPass.Text, out string errorMessage))
            {
                AdminPanel panel = new AdminPanel(this);
                ClearAndHide(panel);
            }

            else
                lblError.Text = errorMessage;
        }

        private void btnNewUser_Click(object sender, EventArgs e)
        {
            RegisterForm form = new RegisterForm(this);
            ClearAndHide(form);
        }

        /// <summary>
        /// Method to check login information.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        /// <param name="error"></param>
        /// <returns>bool login successful</returns>
        private bool LoginControl(string name, string pass, out string error)
        {
            bool result = false;
            error = "";

            if (name != "" && pass != "")
            {
                ctx = new AlbumStoreContext();

                var user = ctx.Admins.Where(x => x.Name == name).ToList();

                if (user.Count != 0)
                {
                    if (user.FirstOrDefault().Password == SHA256Crypt(pass))
                    {
                        result = true;
                        error = "";
                    }

                    else
                    {
                        result = false;
                        error = "Incorrect Name Or Password!";
                    }
                }

                else
                {
                    result = false;
                    error = "Incorrect Name Or Password!";
                }
            }

            else
            {
                result = false;
                error = "Please Fill In All The Field!";
            }

            return result;
        }

        /// <summary>
        /// Method that encrypts the password with SHA-256.
        /// </summary>
        /// <param name="sifre"></param>
        /// <returns>Crypted Password</returns>
        public string SHA256Crypt(string pass)
        {
            using (SHA256 hash = SHA256Managed.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(pass)).Select(l => l.ToString("X2")));
            }
        }

        /// <summary>
        /// Method to clear login screen controls.
        /// </summary>
        /// <param name="form"></param>
        private void ClearAndHide(Form form)
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TextBox | ctrl is MaskedTextBox)
                    ctrl.Text = "";
            }

            form.Show();
            this.Hide();
        }
    }
}