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
    public partial class ManageUsers : Form
    {
        private readonly CarRentalEntities _db;
        public ManageUsers()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("AddUser"))
            {
                var addUser = new AddUser(this);
                addUser.MdiParent = this.MdiParent;
                addUser.Show();
            }
     
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id odf selected row
                var id = (int)gvUserList.SelectedRows[0].Cells["id"].Value;


                //querry database for record
                var user = _db.Users.FirstOrDefault(q => q.id == id);
              
                var hashed_password = Utils.DefaultHashPassword();
                user.password = hashed_password;
                _db.SaveChanges();

                MessageBox.Show($"{user.username}'s Password has been reset!");
            }
            catch (Exception)
            {
                MessageBox.Show("Error : Please select column");
            }
        }

        private void btnDeactivateUser_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id odf selected row
                var id = (int)gvUserList.SelectedRows[0].Cells["id"].Value;


                //querry database for record
                var user = _db.Users.FirstOrDefault(q => q.id == id);
                 
                // if (user.isActive == true)
                // user.isActive == false'
                //else
                // user.isActive = true;

                user.isActive = user.isActive == true ? false : true ;

                _db.SaveChanges();

                MessageBox.Show($"{user.username}'s actice has changed!");
                poplatedGrid();
            }
            catch (Exception)
            {
                MessageBox.Show("Error : Please select column");
            }
        }

        private void ManageUsers_Load(object sender, EventArgs e)
        {
            poplatedGrid();
        }

        public void poplatedGrid()
        {
            var user = _db.Users.Select(q => new
            {
                q.id,
                q.username,
                q.UserRoles.FirstOrDefault().Role.name,
                q.isActive
            })
                .ToList();
            gvUserList.DataSource = user;
            gvUserList.Columns["username"].HeaderText = "Username";
            gvUserList.Columns["name"].HeaderText = "Role Name";
            gvUserList.Columns["isActive"].HeaderText = "Active";

            gvUserList.Columns["id"].Visible = false;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            poplatedGrid();
        }
    }
}
