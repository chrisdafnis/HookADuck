using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace HookADuck
{
    /// <summary>
    /// Interaction logic for CheckPrize.xaml
    /// </summary>
    public partial class CheckPrize : ChildWindow
    {
        public CheckPrize()
        {
            InitializeComponent();
        }

        public CheckPrize(object sender)
        {
            InitializeComponent();
            CheckDuckResult duck = sender as CheckDuckResult;
            if (duck == null)
            {
                labelMessage.Content = "Invalid Barcode scanned, please try again";
            }
            else if (duck.PrizeID == null)
            {
                string name = duck.Name.TrimEnd(' ');
                //MessageBox.Show("Congratulations, you have won a " + description + " from " + name);
                labelMessage.Content = @"Sorry, " + name + @" says 'I don't have a prize!'";
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
                    string image = won[0].Image;
                    //MessageBox.Show("Congratulations, you have won a " + description + " from " + name);
                    labelMessage.Content = "Congratulations, you have won a " + description + " from " + name;

                    var uri = new Uri(@"c:\users\chrisd.dakotais\documents\visual studio 2017\Projects\HookADuck\HookADuck\Images\" + image);
                    prizeImage.Source = new BitmapImage(uri);
                    // update the prize to set it as won
                    context.UpdatePrize(won[0].ID);
                }
                else
                {
                    string description = won[0].Prize.TrimEnd(' ');
                    string name = duck.Name.TrimEnd(' ');
                    string image = won[0].Image;
                    //MessageBox.Show("Congratulations, you have won a " + description + " from " + name);
                    labelMessage.Content = "Sorry, " + name + " says this prize has already been won!";

                    var uri = new Uri(@"c:\users\chrisd.dakotais\documents\visual studio 2017\Projects\HookADuck\HookADuck\Images\" + image);
                    prizeImage.Source = new BitmapImage(uri);
                }
            }
        }

        private void OK_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
