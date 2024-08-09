using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Markup;
using System.Linq;


namespace Deal_or_No_Deal_Game
{
    public static class Program
    {
        //builds static list of all values for the cases. Referenced during whole game
        private static List<double> values = new List<double>
        {
        0.01, 1, 5, 10, 25, 50, 75, 100, 200, 300, 400, 500,
        750, 1_000, 5_000, 10_000, 25_000, 50_000, 75_000, 100_000,
        200_000, 300_000, 400_000, 500_000, 750_000, 1_000_000
        };

        private static List<int> cases;

        //this line is to test if git bash is actually tracking changes in the file contents
        public static void Main()
        {
            //builds cases list, used to reference which case has what value
            initiateCases();
            //adds indexes and numbers corresponding to that index for every value in "value" list
            setCases();
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        public static double getValue(int x)
        {
            return values[x];
        }

        public static string ConvertToMoney(double x)
        {
            //converts given double to a readable dollar amount
            //this code below was a bitch to figure out without gpt
            //this pulls a predetermined format for large numbers to build a new string with commas and a period for the decimal
            //that first $ is to allow for variables to be inserted into a string
            return string.Create(CultureInfo.InvariantCulture, $"${x:N}");
        }

        public static List<double> getValueList()
        {
            return values;
        }

        public static List<string> listToString()
        {
            List<String> test = new List<string>();
            foreach (double s in values)
            {
                test.Add(s.ToString());
            }
            return test;
        }
        public static int getLength()
        {
            return values.Count;
        }

        public static double totalValue()
        {
            return values.Sum();
        }

        public static double randomValue()
        {
            if (values == null || values.Count == 0)
            {
                {
                    throw new InvalidOperationException("The values list is empty");
                }
            }
            Random rnd = new Random();
            int index = rnd.Next(0, values.Count);
            double returnable = values[index];
            return returnable;
        }
        public static void removeValue(int x)
        {
            values.RemoveAt(x);
        }

        public static int getIndex(double x)
        {
            return values.IndexOf(x);
        }

        public static void initiateCases()
        {
            cases = new List<int>();
        }

        public static void setCases()
        {
            int i = 1;
            cases.Clear();
            foreach (var item in values)
            {
                cases.Add(i++);
            }
        }

        }
    }
/*
 * the following below is the instructions for the build. Rules are also located in Form1 Design
 * 
The interface for this program must use EasyGUI
There are 26 cases, numbered 1 - 26.
Inside each case is one of the following values: 0.01, 1, 5, 10, 25, 50, 75, 100, 200, 300, 
400, 500, 750, 1,000, 5,000, 10,000, 25,000, 50,000, 75,000, 100,000, 200,000, 300,000, 400,000, 
500,000, 750,000, 1,000,000.  This should be randomized each game.
The first thing the player must do is select one of the cases
From there the player will select the opening of  a descending number of cases (5, 5, 4, 2, 2, 2, 2, 2 ) 
each round, or until they decide to quit playing.  At the end of each round, the player should be told the 
values of the remaining cases on the board and given an offer to quit.  This might be as simple as, average
what’s left possible, divide by how many, or more complex as you see fit.
If the player quits playing, they should be notified of what was in their case, as well as the values of 
all cases remaining on the board.
If the player makes it down to the last 2 cases, they should be given the option to keep their original 
case, or switch to the last case on the board.  Then informed of the values of each that they have and that was left behind.
*/