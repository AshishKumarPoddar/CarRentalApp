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
    public partial class ManageVehicleListing : Form
    {
      
        private readonly CarRentalEntities _db;
        public ManageVehicleListing()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void ManageVehicleListing_Load(object sender, EventArgs e)
        {
            //Select * From TypeOfCars
            //var cars = _db.TypesOfCars.ToList();

            //Select Id as CarId,name as CarName from TypeOfCars
            // var cars = _db.TypesOfCars.Select(q => new { CarId = q.Id, CarName = q.make }).ToList();

            try
            {
                PopulateGrid();
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Error : {ex}");
            }
            
        }

        public void PopulateGrid()
        {
            var cars = _db.TypesOfCars
                 .Select(q => new
                 {
                     Make = q.make,
                     Model = q.model,
                     VIN = q.VIN,
                     Year = q.Year,
                     LicensePlateNumber = q.LicencePlateNumber,
                     Id = q.Id
                 })
                 .ToList();
            gvVehicleList.DataSource = cars;
            gvVehicleList.Columns[4].HeaderText = "License Plate Number";
            gvVehicleList.Columns[5].Visible = false;

            //gvVehicleList.Columns[0].HeaderText = "ID";
            //gvVehicleList.Columns[1].HeaderText = "NAME";

        }

        private void btnAddCar_Click(object sender, EventArgs e)
        {
            var addEditVehicle = new AddEditVehicle(this);
            addEditVehicle.MdiParent = this.MdiParent;
            addEditVehicle.Show();
        }

        private void btnDeleteCar_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id odf selected row
                var id = (int)gvVehicleList.SelectedRows[0].Cells["Id"].Value;

                //querry database for record
                var car = _db.TypesOfCars.FirstOrDefault(q => q.Id == id);

                DialogResult dr = MessageBox.Show("Are You Sure You Want To Delete This Record? ", "Delete", MessageBoxButtons.YesNoCancel);
                if(dr == DialogResult.Yes)
                {
                    //delete vehicle from table
                    _db.TypesOfCars.Remove(car);
                    _db.SaveChanges();
                }
                PopulateGrid();

                MessageBox.Show("vechile Deleted Successfully \n Please refresh to see changes ");


            }
            catch (Exception)
            {
                MessageBox.Show("Error : Please select column");

            }
           
        }
        
        private void btnEditCar_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id odf selected row
                var id = (int)gvVehicleList.SelectedRows[0].Cells["id"].Value;


                //querry database for record
                var car = _db.TypesOfCars.FirstOrDefault(q => q.Id == id);

                //Launch AddEditVechile window with data
                var addEditVehicle = new AddEditVehicle(car,this);
                addEditVehicle.MdiParent = this.MdiParent;
                addEditVehicle.Show();
            }
            catch(Exception )
            {
                MessageBox.Show("Error : Please select column");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateGrid();
            gvVehicleList.Update();
            gvVehicleList.Refresh();
        }
    }
}
