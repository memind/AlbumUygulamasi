using DataAccessLayer.Context;
using Entities;
using System.Data;
using System.Text.RegularExpressions;

namespace UI
{
    public partial class RegisterForm : Form
    {
        AlbumStoreContext ctx;
        LoginForm baseForm;
        Regex upperCase;
        Regex lowerCase;
        Regex symbols;
        public RegisterForm(LoginForm loginForm)
        {
            InitializeComponent();
            baseForm = loginForm;
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            upperCase = new Regex(@".*[A-Z].*[A-Z].*");
            lowerCase = new Regex(@".*[a-z].*[a-z].*[a-z].*");
            symbols = new Regex((@".*[!:+*]+.*[!:+*]+.*"));
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult closeResult = new DialogResult();
            closeResult = MessageBox.Show(text: $"Are you sure you want to close the program?", "Closing", MessageBoxButtons.YesNo);

            if (closeResult == DialogResult.Yes)
                this.Close();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RegisterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            baseForm.Show();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (NameControl(txtName.Text, out string nameErrorMessage))
            {
                if (PasswordControl(txtPass.Text, txtPassRep.Text, out string passwordErrorMessage))
                {
                    CreateAnAdmin(txtName.Text, txtPass.Text);
                    this.Close();
                    baseForm.Show();
                }

                else
                {
                    lblError.Text = passwordErrorMessage;
                }
            }

            else 
            {
                lblError.Text = nameErrorMessage;
            }  
        }

        /// <summary>
        /// A method that checks whether the password complies with the specified criteria.
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="passRep"></param>
        /// <param name="error"></param>
        /// <returns>"error" ile tutulan hata mesaji</returns>
        private bool PasswordControl(string pass, string passRep, out string error)
        {
            bool result = false;
            error = "";

            if (txtPass.Text == txtPassRep.Text) // Girilen sifreler ayni mi sorgusu
            {
                if (txtPass.TextLength >= 8) // 8 karaktere esit ya da cok mu sorgusu
                {
                    if (upperCase.IsMatch(txtPass.Text)) // Buyuk harfler icin regEx'e uygun mu sorgusu
                    {
                        if (lowerCase.IsMatch(txtPass.Text)) // Kucuk harfler icin regEx'e uygun mu sorgusu
                        {
                            if (symbols.IsMatch(txtPass.Text)) // Semboller icin regEx'e uygun mu sorgusu
                            {
                                return true;
                                error = "";
                            }
                            else
                            {
                                result = false;
                                error = "Password Must Contain At Least 2 of {! * : +}";
                            }
                        }

                        else
                        {
                            result = false;
                            error = "Password Must Contain At Least 3 Lowercase Letters";
                        }
                    }

                    else
                    {
                        result = false;
                        error = "Password Must Contain At Least 2 Uppercase Letters";
                    }
                }

                else
                {
                    result = false;
                    error = "Password Must Be At Least 8 Characters!";
                }
            }

            else
            {
                result = false;
                error = "Entered Passwords Must Be The Same!";
            }

            return result;
        }

        /// <summary>
        /// Method that checks whether the entered name is registered in the system.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="error"></param>
        /// <returns>"error" ile tutulan hata mesaji</returns>
        private bool NameControl(string name, out string error)
        {
            ctx = new AlbumStoreContext();
            bool result = false;

            var userControl = ctx.Admins.Where(x => x.Name == name).FirstOrDefault();

            if (userControl == null)
            {
                result = true;
                error = "";
            }

            else
            {
                result = false;
                error = "Enter An Unregistered Name!";
            }

            return result;
        }

        /// <summary>
        /// The method that creates and registers the admin user.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pass"></param>
        private void CreateAnAdmin(string name, string pass)
        {
            ctx = new AlbumStoreContext();

            Admin user = new Admin() { Name = name, Password = baseForm.SHA256Crypt(pass)};
            ctx.Admins.Add(user);

            ctx.SaveChanges();
        }
    }
}
