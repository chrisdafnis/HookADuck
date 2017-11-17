using HookADuck.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HookADuck
{
    public partial class Form1 : Form
    {
        //Timer blinkTimer = new Timer();
        Timer timerChangePicture = new Timer();

        public Form1()
        {
            InitializeComponent();

            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;

            //Blink(labelPrizeMessage, false);
            timerChangePicture.Tick += TimerChangePicture_Tick;
            timerChangePicture.Interval = 12000;
            timerChangePicture.Stop();


        }

        private void TimerChangePicture_Tick(object sender, EventArgs e)
        {
            if (pictureBox.Image != (Image)HookADuck.Properties.Resources.rubberduckdoctor)
            {
                pictureBox.Image = HookADuck.Properties.Resources.rubberduckdoctor;
                labelPrizeMessage.Text = String.Empty;
                timerChangePicture.Stop();
                // stop text from blinking
                //blinkTimer.Stop();
            }
        }

        private void textBoxBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData.ToString() == "Return")
            {
                //labelPrizeMessage.Text = String.Empty;
                //SoftBlink(labelPrizeMessage, Color.White, Color.FromArgb(0, 182, 222), 0, false);

                string duckCode = textBoxBarcode.Text;
                if (textBoxBarcode.Text.Length == 22)
                {
                    duckCode = textBoxBarcode.Text.Remove(0, 17);
                    duckCode = duckCode.Remove(duckCode.Length - 1);
                }

                HookADuckDataClassesDataContext context = new HookADuckDataClassesDataContext();
                List<CheckDuckResult> prizes = context.CheckDuck(duckCode).ToList<CheckDuckResult>();

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
                labelPrizeMessage.Text = @"Sorry, " + name + @" Duck says “You haven’t won a prize this time. Well, its not all bad news, we have a Dakota pen for you”";
                //Blink(labelPrizeMessage, false);
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
                    //SoftBlink(labelPrizeMessage, Color.White, Color.FromArgb(0, 182, 222), 3000, false);
                    labelPrizeMessage.Text = "Congratulations, " + name + " Duck is a lucky duck and you're the winner of a " + description;
                    //Blink(labelPrizeMessage, true);
                    pictureBox.Image = GetResourceImage(image);
                    
                    // update the prize to set it as won
                    context.UpdatePrize(won[0].ID);
                }
                else
                {
                    string description = won[0].Prize.TrimEnd(' ');
                    string name = duck.Name.TrimEnd(' ');
                    string image = won[0].Image.TrimEnd(' ');

                    labelPrizeMessage.Text = "Sorry, " + name + " Duck says “My " + won[0].Prize.TrimEnd(' ') + " has already been won”";
                    //Blink(labelPrizeMessage, false);
                    //SoftBlink(labelPrizeMessage, Color.White, Color.FromArgb(0, 182, 222), 1000, false);
                    pictureBox.Image = GetResourceImage(image);
                }
            }

            textBoxBarcode.Clear();
            timerChangePicture.Stop();
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

        //private async void SoftBlink(Control ctrl, Color c1, Color c2, short CycleTime_ms, bool BkClr)
        //{
        //    if (ctrl != null)
        //    {
        //        var sw = new Stopwatch(); sw.Start();
        //        short halfCycle = (short)Math.Round(CycleTime_ms * 0.5);
        //        while (true)
        //        {
        //            await Task.Delay(1);
        //            var n = sw.ElapsedMilliseconds % CycleTime_ms;
        //            var per = (double)Math.Abs(n - halfCycle) / halfCycle;
        //            var red = (short)Math.Round((c2.R - c1.R) * per) + c1.R;
        //            var grn = (short)Math.Round((c2.G - c1.G) * per) + c1.G;
        //            var blw = (short)Math.Round((c2.B - c1.B) * per) + c1.B;
        //            var clr = Color.FromArgb(red, grn, blw);
        //            if (BkClr) ctrl.BackColor = clr; else ctrl.ForeColor = clr;
        //        }
        //    }
        //}

        //private void Blink(Control ctrl, bool tick)
        //{
        //    blinkTimer.Interval = 500;
        //    blinkTimer.Tag = ctrl;

        //    blinkTimer.Tick += BlinkTimer_Tick;

        //    if (tick)
        //    {
        //        blinkTimer.Start();
        //    }
        //    else
        //    {
        //        ctrl.Visible = true;
        //        blinkTimer.Stop();
        //    }
        //}

        //private void BlinkTimer_Tick(object sender, EventArgs e)
        //{
        //    Control ctrl = ((Timer)sender).Tag as Control;
        //    ctrl.Visible = !ctrl.Visible;
        //}
    }
}
