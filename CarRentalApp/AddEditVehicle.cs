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
    public partial class AddEditVehicle : Form
    {
        private bool isEditMode;
        private ManageVehicleListing _manageVehicleListing;
        private readonly CarRentalEntities _db;
       


        public AddEditVehicle(ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            lblTitle.Text = "Add New Vehicle";
            isEditMode = false;
            _manageVehicleListing = manageVehicleListing; 
            _db = new CarRentalEntities();
            
        }
        public AddEditVehicle(TypesOfCar carToEdit, ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Vehicle";
            _manageVehicleListing = manageVehicleListing;
            isEditMode = true;
            _db = new CarRentalEntities();
            PopulateFieds(carToEdit);
                
        }

        private void PopulateFieds(TypesOfCar car)
        {
            lblId.Text = car.Id.ToString();
            tbMake.Text = car.make;
            tbModel.Text = car.model;
            tbYear.Text = car.Year.ToString();
            tbVIN.Text = car.VIN;
            tbLicenceNum.Text = car.LicencePlateNumber;
                   
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void AddEditVehicle_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (isEditMode)
            {
                //Esit code here
                var id = int.Parse(lblId.Text);
                var car = _db.TypesOfCars.FirstOrDefault(q => q.Id == id);
                car.model = tbModel.Text;
                car.make = tbMake.Text;
                car.Year = int.Parse(tbYear.Text);
                car.VIN = tbVIN.Text;
                car.LicencePlateNumber = tbLicenceNum.Text;

                
             
            }
            else
            {
                //Add Code Here
                var newCar = new TypesOfCar
                {
                    LicencePlateNumber = tbLicenceNum.Text,
                    make = tbMake.Text,
                    model = tbModel.Text,
                    VIN = tbVIN.Text,
                    Year = int.Parse(tbYear.Text)                   
                };
                _db.TypesOfCars.Add(newCar);
              
            }
            _db.SaveChanges();
            _manageVehicleListing.PopulateGrid();
            MessageBox.Show("Vechile Edited Successfully \n Please refresh to see changes ");

            Close();
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
