using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class AddUser : Form
    {
        private readonly CarRentalEntities _db;
        private ManageUsers _manageUsers;

        public AddUser(ManageUsers manageUsers)
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            _manageUsers =manageUsers;
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            var roles = _db.Roles.ToList();
            cbRoles.DataSource = roles;
            cbRoles.ValueMember = "id";
            cbRoles.DisplayMember = "name";
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            Close();

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
           
            try
            {
                var username = tbUsername.Text;
                var roleId = (int)cbRoles.SelectedValue;
                var password = Utils.DefaultHashPassword();
                var user = new User
                {
                    username = username,
                    password = password,
                    isActive = true

                };
                _db.Users.Add(user);
                _db.SaveChanges();

                var userid = user.id;

                var useRole = new UserRole
                {
                    roleid = roleId,
                    userid = userid
                };

                _db.UserRoles.Add(useRole);
                _db.SaveChanges();

                MessageBox.Show("New user added Sucessfully");
                _manageUsers.poplatedGrid ();
                Close();
            }
            catch (Exception)
            {

                MessageBox.Show("An error has occured");
            }
        }
    }
}
