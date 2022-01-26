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

    public partial class ManageRentalRecords : Form
    {
        private readonly CarRentalEntities _db;
        public ManageRentalRecords()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void ManageRentalRecords_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateGrid();
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Error : {ex}");
            }
        }

        private void PopulateGrid()
        {

            var records = _db.CarRentalRecords.Select(q => new
            {
                Cusotmer = q.CustomerName,
                DateIn = q.DtaeRented,
                DateOut = q.DateReturned,
                Id = q.id,
                Cost = q.cost,
                Car = q.TypesOfCar.make +" "+q.TypesOfCar.model 
            }).ToList();

            gvRecordList.DataSource = records;
            gvRecordList.Columns["DateIn"].HeaderText = "Date In";
            gvRecordList.Columns["DateOut"].HeaderText = "Date Out";

            gvRecordList.Columns["Id"].Visible = false;

        }

        private void btnAddRecord_Click(object sender, EventArgs e)
        {
            var addRentalRecord = new AddRentelRecords
            {
                MdiParent = this.MdiParent
            };
            addRentalRecord.Show();

        }

        private void btnEditRecord_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id odf selected row
                var id = (int)gvRecordList.SelectedRows[0].Cells["id"].Value;


                //querry database for record
                var record = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);

                //Launch AddEditRecord window with data

                var addEditRentalRecord = new AddRentelRecords(record);
                addEditRentalRecord.MdiParent = this.MdiParent;
                addEditRentalRecord.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("Error : Please select column");
            }

        }

        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id odf selected row
                var id = (int)gvRecordList.SelectedRows[0].Cells["Id"].Value;

                //querry database for record
                var record = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);

                //delete vehicle from table
                _db.CarRentalRecords.Remove(record);
                _db.SaveChanges();

                PopulateGrid();


            }
            catch (Exception)
            {
                MessageBox.Show("Error : Please select column");

            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ManageRentalRecords mr = new ManageRentalRecords();
            mr.MdiParent = this.MdiParent;
            this.Close();
            mr.Show();

        }
    }
}
