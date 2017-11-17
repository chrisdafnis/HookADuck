using HookADuck.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HookADuck
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            timerChangePicture.Stop();
        }

        private void textBoxBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData.ToString() == "Return")
            {
                HookADuckDataClassesDataContext context = new HookADuckDataClassesDataContext();
                List<CheckDuckResult> prizes = context.CheckDuck(textBoxBarcode.Text).ToList<CheckDuckResult>();

                EventArgs args = new EventArgs();
                switch (prizes.Count)
                {
                    case 0:
                        // Invalid Barcode scanned, not in the database
                        CheckPrize(null/*, args*/);
                        //MessageBox.Show("Invalid Barcode scanned, please try again");
                        break;
                    case 1:
                        CheckPrize(prizes[0]/*, args*/);
                        //if (prizes[0].PrizeID == null)
                        //{
                        //    //    no prize assigned to wristband
                        //    CheckPrize(prizes[0]/*, args*/);
                        //    //MessageBox.Show("Sorry, no prize assigned to this wristband");
                        //}
                        //else
                        //{
                        //    //    wristband has won a prize
                        //    List<GetPrizeResult> prize = context.GetPrize(prizes[0].PrizeID).ToList<GetPrizeResult>();
                        //    if (prize[0].Won == 0)
                        //    {
                        //        string description = prize[0].Prize.TrimEnd(' ');
                        //        string name = prizes[0].Name.TrimEnd(' ');
                        //        //MessageBox.Show("Congratulations, you have won a " + description + " from " + name);

                        //        string image = prize[0].Image.TrimEnd(' ');
                        //        var uri = new Uri(@"\Images\" + image);
                        //        //rubberduck.Source = new BitmapImage(uri);
                        //        //update the prize to set it as won
                        //        context.UpdatePrize(prize[0].ID);
                        //    }
                        //    else
                        //    {
                        //        MessageBox.Show("Sorry, this prize has already been won");
                        //    }
                        //}

                        // update the duck to set it as scanned
                        context.UpdateDuck(textBoxBarcode.Text);
                        break;
                    default:
                        // error - more than one prize assigned
                        MessageBox.Show("Congratulations, wristband has won a prize");
                        break;
                }
            }
        }

        private void CheckPrize(object sender)
        {
            CheckDuckResult duck = sender as CheckDuckResult;
            if (duck == null)
            {
                labelPrizeMessage.Text = "Invalid Barcode scanned, please try again";
                pictureBox.Image = GetResourceImage("sad_duck");
            }
            else if (duck.PrizeID == null)
            {
                string name = duck.Name.TrimEnd(' ');
                labelPrizeMessage.Text = @"Sorry, " + name + @" says 'I don't have a prize!'";
                pictureBox.Image = GetResourceImage("sad_duck");
            }
            else
            {
                // wristband has won a prize
                HookADuckDataClassesDataContext context = new HookADuckDataClassesDataContext();
                List<GetPrizeResult> won = context.GetPrize(duck.PrizeID).ToList<GetPrizeResult>();
                if (won[0].Won == 0)
                {
                    string description = won[0].Prize.TrimEnd(' ');
                    string name = duck.Name.TrimEnd(' ');
                    string image = won[0].Image.TrimEnd(' ');

                    labelPrizeMessage.Text = "Congratulations, you have won a " + description + " from " + name;
                    pictureBox.Image = GetResourceImage(image);
                    
                    // update the prize to set it as won
                    context.UpdatePrize(won[0].ID);
                }
                else
                {
                    string description = won[0].Prize.TrimEnd(' ');
                    string name = duck.Name.TrimEnd(' ');
                    string image = won[0].Image.TrimEnd(' ');

                    labelPrizeMessage.Text = "Sorry, " + name + " says my " + won[0].Prize.TrimEnd(' ') + " has already been won!";
                    pictureBox.Image = GetResourceImage(image);
                }
            }

            textBoxBarcode.Clear();
            timerChangePicture.Start();
        }

        private Image GetResourceImage(string imageName)
        {
            Image returnImage = null;

            switch (imageName)
            {
                case "sad_duck":
                    {
                        returnImage = HookADuck.Properties.Resources.sad_duck;
                    }
                    break;
                case "champagne":
                    {
                        returnImage = HookADuck.Properties.Resources.champagne;
                    }
                    break;
                case "chocolates":
                    {
                        returnImage = HookADuck.Properties.Resources.chocolates;
                    }
                    break;
                case "fitbit":
                    {
                        returnImage = HookADuck.Properties.Resources.fitbit;
                    }
                    break;
                case "redwine":
                    {
                        returnImage = HookADuck.Properties.Resources.redwine;
                    }
                    break;
                case "toiletries":
                    {
                        returnImage = HookADuck.Properties.Resources.toiletries;
                    }
                    break;
                case "whitewine":
                    {
                        returnImage = HookADuck.Properties.Resources.whitewine;
                    }
                    break;
            }

            return returnImage;
        }

        private void timerChangePicture_Tick(object sender, EventArgs e)
        {
            if (pictureBox.Image != (Image)HookADuck.Properties.Resources.rubberduckdoctor)
            {
                pictureBox.Image = HookADuck.Properties.Resources.rubberduckdoctor;
                labelPrizeMessage.Text = String.Empty;
                timerChangePicture.Stop();
            }
        }
    }
}
