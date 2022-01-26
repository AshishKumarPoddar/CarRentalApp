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
    public partial class AddRentelRecords : Form
    {
        private bool isEditMode;
        private readonly CarRentalEntities _db;
        public AddRentelRecords()
        {
            InitializeComponent();
            lblTitle.Text = "Add New Rental";
            this.Text = "Add new Rental";
            isEditMode = false;
            _db = new CarRentalEntities();
        }

        public AddRentelRecords(CarRentalRecord recordToEdit)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Rental Record";
            this.Text = "Edit Rental Record";
            if (recordToEdit == null)
            {
                MessageBox.Show("Please ensure that  you selected a valid record to edit");
                Close();

            }
            else
            {

                isEditMode = true;
                _db = new CarRentalEntities();
                PopulateFieds(recordToEdit);
            }

        }

        private void PopulateFieds(CarRentalRecord recordToEdit)
        {
            tbCustomerName.Text = recordToEdit.CustomerName;
            dtRented.Value = (DateTime)recordToEdit.DtaeRented;
            dtReturned.Value = (DateTime)recordToEdit.DateReturned;
            tbCost.Text = recordToEdit.cost.ToString();
            lblRecordId.Text = recordToEdit.id.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Select * from TypesOfCars
           // var cars = carRentalEntities.TypesOfCars.ToList();
            
            var cars = _db.TypesOfCars
                .Select(q => new {Id = q.Id,Name = q.make+" "+q.model})
                .ToList();
            cbTypeOfCar.DisplayMember = "Name";
            cbTypeOfCar.ValueMember = "Id";
            cbTypeOfCar.DataSource = cars;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string customerName = tbCustomerName.Text;
                var dateOut = dtRented.Value;
                var dateIn = dtReturned.Value;
                double cost = Convert.ToDouble(tbCost.Text);

                var carType = cbTypeOfCar.Text;
                var isValid = true;
                var errorMessage = "";

                if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(carType))
                {
                    isValid = false;
                    errorMessage += " Error: Please enter missimg data\n\r";
                }

                if (dateOut > dateIn)
                {
                    isValid = false;
                   errorMessage += "Error: Illegal Date selection\n\r";
                }
                //if (isValid == true)
                if (isValid)
                {
                    //Declare an Object of the record to be added
                    var rentalRecord = new CarRentalRecord();
                    if (isEditMode)
                    {
                        //If in edit made, then get Id and retrive the record from the database and place 
                        //the result in the record object
                        var id = int.Parse(lblRecordId.Text);
                        rentalRecord = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);
                    } 
                    //populate record object with values from the form 
                        rentalRecord.CustomerName = customerName;
                        rentalRecord.DtaeRented = dateOut;
                        rentalRecord.DateReturned = dateIn;
                        rentalRecord.cost = (decimal)cost;
                        rentalRecord.TypeofCarId = (int)cbTypeOfCar.SelectedValue;
                    //if not in edit mode then add record oblect with the values from the form
                    if (!isEditMode)
                        _db.CarRentalRecords.Add(rentalRecord);
                    //Save changes made to the entity
                    _db.SaveChanges();

                        MessageBox.Show($"Coustomer Name : {customerName}\n\r" + $"Date Rented :{dateOut}\n\r" + $" Date Returned :{dateIn}\n\r " + $"Cost : {cost}\n\r " + $" Car Type : {carType}\n\r " + "Thank you for Bussiness");
                    
                  
                   
                }
                else
                    MessageBox.Show(errorMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
              //  throw;
            }
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
