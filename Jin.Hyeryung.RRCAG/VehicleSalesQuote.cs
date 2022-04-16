using System;
using System.Linq;
using System.Windows.Forms;

namespace Jin.Hyeryung.RRCAG
{
    public partial class VehicleSalesQuote : Form
    {
        public VehicleSalesQuote()
        {
            InitializeComponent();
            calculateBtn.Click += CalculateBtn_Click;
            customRadio.CheckedChanged += CustomRadio_CheckedChanged;
            standardRadio.CheckedChanged += StandardRadio_CheckedChanged;
            pearlizedRadio.CheckedChanged += PearlizedRadio_CheckedChanged;
            stereoCheck.CheckedChanged += StereoCheck_CheckedChanged;
            leatherCheck.CheckedChanged += LeatherCheck_CheckedChanged;
            computerCheck.CheckedChanged += ComputerCheck_CheckedChanged;
            vehiclesSalesTxtBox.TextChanged += VehiclesSalesTxtBox_TextChanged;
            tradeInValueTxtBox.TextChanged += TradeInValueTxtBox_TextChanged;
        }

        private void TradeInValueTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (tradeInValueTxtBox.Text == "")
                ClearTextBox();
            else
            {
                if (IsNumber(tradeInValueTxtBox.Text.ToCharArray()))
                    tradeInValue = int.Parse(tradeInValueTxtBox.Text);
            }
        }

        private void VehiclesSalesTxtBox_TextChanged(object sender, EventArgs e)
        {
            if (vehiclesSalesTxtBox.Text == "")
                ClearTextBox();
            else
            {
                if (IsNumber(vehiclesSalesTxtBox.Text.ToCharArray()))
                    vehiclePrice = int.Parse(vehiclesSalesTxtBox.Text);
            }
        }

        Business.SalesQuote salesQuote;

        private void ComputerCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (computerCheck.Checked)
            {
                accessories = Business.Accessories.ComputerNavigation;
                CalculateSales();
            }
        }

        private void LeatherCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (leatherCheck.Checked)
            {
                accessories = Business.Accessories.LeatherInterior;
                CalculateSales();
            }
        }

        private void StereoCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (stereoCheck.Checked)
            {
                accessories = Business.Accessories.StereoSystem;
                CalculateSales();
            }
        }

        private void PearlizedRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (pearlizedRadio.Checked)
            {
                exteriorFinish = Business.ExteriorFinish.Pearlized;
                CalculateSales();
            }
        }

        private void StandardRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (standardRadio.Checked)
            {
                exteriorFinish = Business.ExteriorFinish.Standard;
                CalculateSales();
            }
        }

        private void CustomRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (customRadio.Checked)
            {
                exteriorFinish = Business.ExteriorFinish.Custom;
                CalculateSales();
            }
        }

        Business.ExteriorFinish exteriorFinish;
        Business.Accessories accessories;
        bool isNoError = true;
        bool isNoError2 = true;
        decimal vehiclePrice = 0;
        decimal tradeInValue = 0;
        /// <summary>
        /// validate wrong input then display price quote
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalculateBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(vehiclesSalesTxtBox.Text))
                errorProvider1.SetError(vehiclesSalesTxtBox, "Vehicle price is a required field.");
            else
            {
                if (!IsNumber(vehiclesSalesTxtBox.Text.ToArray()))
                    errorProvider1.SetError(vehiclesSalesTxtBox, "Vehicle price cannot contain letters or special characters.");
                else
                {
                    vehiclePrice = int.Parse(vehiclesSalesTxtBox.Text);
                    if (vehiclePrice <= 0)
                        errorProvider1.SetError(vehiclesSalesTxtBox, "Vehicle price cannot be less than or equal to 0.");
                    else
                        isNoError = false;
                }
            }

            if (string.IsNullOrEmpty(tradeInValueTxtBox.Text))
                errorProvider1.SetError(tradeInValueTxtBox, "Trade-in value is a required field.");
            else
            {
                if (!IsNumber(tradeInValueTxtBox.Text.ToArray()))
                    errorProvider1.SetError(tradeInValueTxtBox, "Trade-in value cannot contain letters or special characters.");
                else
                {
                    tradeInValue = int.Parse(tradeInValueTxtBox.Text);
                    if (tradeInValue <= 0)
                        errorProvider1.SetError(tradeInValueTxtBox, "Trade-in cannot be less than or equal to 0.");
                    else if (tradeInValue > vehiclePrice)
                        errorProvider1.SetError(tradeInValueTxtBox, "Trade-in value cannot exceed the vehicle sale price.");
                    else
                        isNoError2 = false;
                }
            }
            if (isNoError == false && isNoError2 == false)
            {
                CalculateSales();
                //monthlyPymtTextBx.Text = salesQuote.
            }
        }

        /// <summary>
        /// Calculates Price Quote and desplay the result to the controls
        /// </summary>
        private void CalculateSales()
        {
            salesQuote = new Business.SalesQuote(vehiclePrice, tradeInValue, 1, accessories, exteriorFinish);
            optionsTxtBox.Text = String.Format("{0:n}", salesQuote.TotalOptions);
            vehicleSalesPriceTxtBox.Text = salesQuote.VehicleSalePrice.ToString("C2");
            subtotalTxtBox.Text = salesQuote.SubTotal.ToString("C2");
            salesTaxTxtBox.Text = String.Format("{0:n}", salesQuote.SalesTax);
            totalTxtBox.Text = salesQuote.Total.ToString("C2");
            tradeInTxtBox.Text = String.Format("-{0:n}", salesQuote.TradeInAmount);
            amountDueTxtBox.Text = salesQuote.AmountDue.ToString("C2");
            var result = Business.Financial.GetPayment(decimal.Parse(anualIntstUpDown.Value.ToString()), int.Parse(noYearUpDown.Value.ToString()), vehiclePrice);
            monthlyPymtTextBx.Text = result.ToString("C2");
        }

        /// <summary>
        /// Checks if all values are numbers
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private bool IsNumber(char[] values)
        {
            if (Enumerable.All(values, Char.IsNumber))
                return true;
            return false;
        }
        /// <summary>
        /// Reset to the initial state when reset button is clicked and confirmed with yes 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resetBtn_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Do you want to reset the form?", "Reset Form", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                ResetControls();
            }
        }

        /// <summary>
        /// Clears text boxes
        /// </summary>
        private void ClearTextBox()
        {
            optionsTxtBox.Text = "";
            vehicleSalesPriceTxtBox.Text = "";
            subtotalTxtBox.Text = "";
            salesTaxTxtBox.Text = "";
            totalTxtBox.Text = "";
            tradeInTxtBox.Text = "";
            amountDueTxtBox.Text = "";
            monthlyPymtTextBx.Text = "";
        }

        /// <summary>
        /// Reset controls to the initial state
        /// </summary>
        private void ResetControls()
        {
            stereoCheck.Checked = false;
            leatherCheck.Checked = false;
            computerCheck.Checked = false;
            standardRadio.Checked = true;
            tradeInTxtBox.Text = "0";
            vehiclesSalesTxtBox.Text = "";
            anualIntstUpDown.Value = 5;
            noYearUpDown.Value = 1;
            ClearTextBox();

        }
    }
}
