using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Deal_or_No_Deal_Game
{
    public partial class Form3 : Form
    {
        private double bankerValue;
        private static bool Deal = false;
        private static double assignable;
        public Form3(double zet)
        {
            //initiates the banker interaction, given the current value of the case held for calculations
            assignable = zet;
            InitializeComponent();
            MessageBox.Show("The banker wishes to make a deal");
            //creates an offer value to give to the player
            bankerValue = getBankerValue();
            string offer = Program.ConvertToMoney(bankerValue);
            MessageBox.Show("The banker offers " + offer);
            string bankerValueMessage = bankerValue.ToString();
            button3.Text = offer;
            button1.Text = "Deal!";
            button2.Text = "No Deal!";
        }

        public static double getBankerValue()
        {
            //divides total value by the amount of cases remaining including the one the user has
            
            double x = Program.totalValue();
            x = ((x + assignable) / (Program.getLength() + 1));
            int y = (int)x;
            return y;
        }

        public static bool GetDeal()
        {
            return Deal;
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this is the DEAL button
            Deal = true;
            string remainingValue = Program.ConvertToMoney(Program.totalValue());
            //string test = string.Create(CultureInfo.InvariantCulture, $"${Program.totalValue():N}");
            string winnings = Program.ConvertToMoney(bankerValue);
            MessageBox.Show("You have taken the Deal!");
            MessageBox.Show("Congratulations on winning " + winnings);
            MessageBox.Show("The remaining value left in the cases was " + remainingValue);
            MessageBox.Show("Click below to end the game.");
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // this is the NO DEAL button
            MessageBox.Show("You refuse the offer!");
            MessageBox.Show("Let's continue the game!");
            double allCases;
            allCases = Program.totalValue();
            if (Program.getLength() != 1)
            {
                string remainingValue = Program.ConvertToMoney(allCases);
                MessageBox.Show(remainingValue + " is left in the field.");
            }
            Close();
        }
    }
}
