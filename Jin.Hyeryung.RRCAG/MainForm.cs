using System;
using System.Windows.Forms;

namespace Jin.Hyeryung.RRCAG
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            salesQuoteMenuItem.Click += SalesQuoteMenuItem_Click;
            carWashMenuItem.Click += CarWashMenuItem_Click;
            exitMenuItem.Click += ExitMenuItem_Click;
            vehicleMenuItem.Click += VehicleMenuItem_Click;
            aboutMenuItem.Click += AboutMenuItem_Click;
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void VehicleMenuItem_Click(object sender, EventArgs e)
        {
            ACE.BIT.ADEV.Forms.VehicleDataForm vehicleData = new ACE.BIT.ADEV.Forms.VehicleDataForm();
            vehicleData.MdiParent = this;
            vehicleData.Show();
            vehicleData.Activate();
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CarWashMenuItem_Click(object sender, EventArgs e)
        {
            ACE.BIT.ADEV.Forms.CarWashForm carWash = new ACE.BIT.ADEV.Forms.CarWashForm();
            carWash.MdiParent = this;
            carWash.Show();
            carWash.Activate();
        }

        private void SalesQuoteMenuItem_Click(object sender, EventArgs e)
        {
            VehicleSalesQuote vehicleSalesQuote = new VehicleSalesQuote();
            vehicleSalesQuote.MdiParent = this;
            vehicleSalesQuote.Show();
            vehicleSalesQuote.Activate();
        }


    }
}
