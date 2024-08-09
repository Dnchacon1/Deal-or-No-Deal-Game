using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Deal_or_No_Deal_Game
{
    public partial class Form2 : Form
    {
        //The goal is to check if the player has a held case, if not, copy that value and case number into the class-level vars
        private double heldCaseValue;
        private int heldCase;
        private bool firstCase = false;
        //Tracking of rounds to determine when banker calls
        private int roundsPlayed = 0;
        private int[] RoundLimiter = new int[9];//creates an array used to determine which rounds the banker should call
        private int RoundLimiterPos = 0; //tracks index used for RoundLimiter
        //creates a list with all the case values to display to the user. Numbers that are pulled are replaced with
        //a "-----" to signify they have been removed from the game as a possible value. This list excludes the case taken in the beginning
        private List<string> stringBoxNumbers = new List<string>();
        private List<double> copyOfValues = new List<double>();

        public Form2()
        {
            // this array contains the rounds that must be reached for the banker to make an offer
            RoundLimiter[0] = 5; RoundLimiter[1] = 5; RoundLimiter[2] = 4; RoundLimiter[3] = 2; RoundLimiter[4] = 2;
            RoundLimiter[5] = 2; RoundLimiter[6] = 2; RoundLimiter[7] = 2; RoundLimiter[8] = 0;

            //build the strings list to contain all the values in the game. This is list is used to fill the listBoxes
            foreach (double number in Program.getValueList())
            {
                string x = Program.ConvertToMoney(number);
                stringBoxNumbers.Add(x);
                //stringBoxNumbers.Add(number.ToString());
            }

            InitializeComponent();
            MessageBox.Show("Pick the case you wish to hold.");
            updateList1();
            updateList2();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        public void updateList1()
        {
            //List on the LH side
            int pos = 0;
            foreach (string c in stringBoxNumbers)
            {
                if (pos == 13) { break; }
                listBox1.Items[pos] = c;
                pos++;
            }

        }


        //the right hand side was a lot more difficult to figure out
        public void updateList2()
        {
            //List on the RH side
            int pos = 0;
            int i = 13;
            foreach (string c in stringBoxNumbers)
            {
                if (pos >= (stringBoxNumbers.Count) - 13)
                {
                    break;
                }
                listBox2.Items[pos] = stringBoxNumbers[i];
                pos++;
                i++;
            }
        }

        private void changeHeldCaseNumber(int x)
        {
            //changes the big button's case number
            button27.Text = "Your Case: " + x;
        }

        private void bankerCall()
        {
            //pulls up form 3 (banker interaction)
            double x = Form3.getBankerValue();
            Form3 form3 = new Form3(heldCaseValue);
            form3.Show();
        }

        private string firstCaseSelection(double x, int y, int w)
        {
            //given the value of the case (x), the index of the case (y), and the case number (w)
            //remove that case's value from the value list, change button 27, return responsive text, set class-level references
            heldCaseValue = x; //class level reference
            firstCase = true; //class level reference
            heldCase = y; //class level reference (case index)
            string word = "You have chosen to keep case number " + w + ".";
            Program.removeValue(y); //removes value from list checked in 'casing'
            changeHeldCaseNumber(w); //changes button 27 to display to the user what case they hold
            return word;
        }

        private void casing(int caseNumber)
        {
            updateList1();
            updateList2();
            //pull a random value out of the Values list
            double value = Program.randomValue();
            //convert that value to string to display to user (appears in USD format)
            string word = Program.ConvertToMoney(value);
            //string word = value.ToString();
            //record index of the value pulled from earlier
            int caseHeld = Program.getIndex(value);
            //determines local index of the value taken from the "values" list
            int boxListIndex = stringBoxNumbers.IndexOf(word);
            //converts heldCaseValue and selected case's var to readable money values
            string caseHoldings = Program.ConvertToMoney(heldCaseValue);

            //Checks if this is the showdown part of the game
            if (Program.getLength() == 1)
            {
                MessageBox.Show("You currnetly have the option to trade out the last case or keep you current case to take home.");
                DialogResult response = MessageBox.Show("Will you keep your case to take home?", "Case Choice", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (response == DialogResult.Yes)
                {
                    MessageBox.Show("You decide to keep your case!");
                    MessageBox.Show("Your take home amount is " + caseHoldings);
                    MessageBox.Show("The other case contained " + word + "!");
                    MessageBox.Show("Click this to end the game.");
                    Application.Exit();
                }
                else
                {
                    MessageBox.Show("You decide to swap cases!");
                    MessageBox.Show("The case you have taken home has " + word + "!");
                    MessageBox.Show("The case other case contained " + caseHoldings + "!");
                    MessageBox.Show("Click this to end the game.");
                    Application.Exit();
                }
            }
            //checks if this is the first case
            else if (firstCase == false)
            {
                word = firstCaseSelection(value, caseHeld, caseNumber);
                MessageBox.Show(word);
            }
            //if it is NOT time to call the banker, displays value of the case and removes case and value from the game 
            else if (roundsPlayed < (RoundLimiter[RoundLimiterPos]))
            {
                Program.removeValue(caseHeld);
                MessageBox.Show(word);
                //updates lists shown to user, removing the number that was just pulled
                stringBoxNumbers[boxListIndex] = "-----";
                updateList1();
                updateList2();
            }
            //checks if banker calls this round, then calls 'em. App closes if deal is taken, returns here otherwise
            else if (roundsPlayed == RoundLimiter[RoundLimiterPos])
            {
                //displays value of the case, removes case, and value from the game
                Program.removeValue(caseHeld);
                MessageBox.Show(word);
                stringBoxNumbers[boxListIndex] = "-----";
                bankerCall();
                RoundLimiterPos++; roundsPlayed = 0;
            }
            updateList1();
            updateList2();
        }

        //each button click will call the casing method, sending its respective case number. RoundsPlayed increased by one per case
        //removes button from game
        private void button1_Click(object sender, EventArgs e)
        {
            casing(1);
            button1.Visible = false;
            roundsPlayed++;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            casing(2);
            button2.Visible = false;
            roundsPlayed++;
        }

        //Button 27 is the large button showing the user the case number they hold
        private void button27_Click(object sender, EventArgs e)
        {
            // Show the pop-up
            MessageBox.Show("This is the case you're holding.");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            casing(3);
            button3.Visible = false;
            roundsPlayed++;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            casing(4);
            button4.Visible = false;
            roundsPlayed++;
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            casing(5);
            button5.Visible = false;
            roundsPlayed++;
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            casing(6);
            button6.Visible = false;
            roundsPlayed++;
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            casing(7);
            button7.Visible = false;
            roundsPlayed++;
        }

        private void button14_Click_1(object sender, EventArgs e)
        {
            //this is case 8
            casing(8);
            button14.Visible = false;
            roundsPlayed++;
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            //this is case 9
            casing(9);
            button13.Visible = false;
            roundsPlayed++;
        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            //this is case 10
            casing(10);
            button12.Visible = false;
            roundsPlayed++;
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            //this is case 11
            casing(11);
            button11.Visible = false;
            roundsPlayed++;
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            //this is case 12
            casing(12);
            button10.Visible = false;
            roundsPlayed++;
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            //this is case 13
            casing(13);
            button9.Visible = false;
            roundsPlayed++;
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            //this is case 14
            casing(14);
            button8.Visible = false;
            roundsPlayed++;
        }

        private void button15_Click_1(object sender, EventArgs e)
        {
            casing(15);
            button15.Visible = false;
            roundsPlayed++;
        }

        private void button16_Click_1(object sender, EventArgs e)
        {
            casing(16);
            button16.Visible = false;
            roundsPlayed++;
        }

        private void button17_Click_1(object sender, EventArgs e)
        {
            casing(17);
            roundsPlayed++;
            button17.Visible = false;
        }

        private void button18_Click_1(object sender, EventArgs e)
        {
            casing(18);
            roundsPlayed++;
            button18.Visible = false;
        }

        private void button19_Click_1(object sender, EventArgs e)
        {
            casing(19);
            roundsPlayed++;
            button19.Visible = false;
        }

        private void button20_Click_1(object sender, EventArgs e)
        {
            casing(20);
            roundsPlayed++;
            button20.Visible = false;
        }

        private void button21_Click_1(object sender, EventArgs e)
        {
            casing(21);
            roundsPlayed++;
            button21.Visible = false;
        }

        private void button26_Click_1(object sender, EventArgs e)
        {
            //this is case 22
            casing(22);
            roundsPlayed++;
            button26.Visible = false;
        }

        private void button25_Click_1(object sender, EventArgs e)
        {
            //this is case 23
            casing(23);
            roundsPlayed++;
            button25.Visible = false;
        }

        private void button24_Click_1(object sender, EventArgs e)
        {
            casing(24);
            roundsPlayed++;
            button24.Visible = false;
        }

        private void button23_Click_1(object sender, EventArgs e)
        {
            //this is case 25
            casing(25);
            roundsPlayed++;
            button23.Visible = false;
        }

        private void button22_Click_1(object sender, EventArgs e)
        {
            //this is case 26
            casing(26);
            roundsPlayed++;
            button22.Visible = false;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this is the left side box

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this is the right side box


        }

        private void button28_Click(object sender, EventArgs e)
        {
            DialogResult input = MessageBox.Show("Are you sure you wish to exti?", "Exiting?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (input == DialogResult.Yes) { Application.Exit(); }

        }
    }
}
