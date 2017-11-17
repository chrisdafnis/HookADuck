using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;

namespace HookADuck
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow 
    {
        public MainWindow()
        {
            InitializeComponent();
            var uri = new Uri(@"c:\users\chrisd.dakotais\documents\visual studio 2017\Projects\HookADuck\HookADuck\Images\rubberduck.jpg");
            rubberduck.Source = new BitmapImage(uri);
            uri = new Uri(@"c:\users\chrisd.dakotais\documents\visual studio 2017\Projects\HookADuck\HookADuck\Images\Dakota_logo.jpg");
            dakotalogo.Source = new BitmapImage(uri);
            uri = new Uri(@"c:\users\chrisd.dakotais\documents\visual studio 2017\Projects\HookADuck\HookADuck\Images\GS1_Corporate_logo.png");
            gs1.Source = new BitmapImage(uri);
            Barcode.Focus();
        }

        private async void Prizes_Button_Click(object sender, RoutedEventArgs e)
        {
            await this.ShowChildWindowAsync(new HookADuckPrizes());
        }

        private async void Ducks_Button_Click(object sender, RoutedEventArgs e)
        {
            await this.ShowChildWindowAsync(new HookADuckDucks());
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void CheckPrize(object sender, RoutedEventArgs e)
        {
            await this.ShowChildWindowAsync(new CheckPrize(sender));
        }

        private void TxtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            string etr = e.Key.ToString();

            if (etr == "Return")
            {
                HookADuckDataClassesDataContext context = new HookADuckDataClassesDataContext();
                List<CheckDuckResult> prizes = context.CheckDuck(Barcode.Text).ToList<CheckDuckResult>();

                RoutedEventArgs args = new RoutedEventArgs();
                switch (prizes.Count)
                {
                    case 0:
                        // Invalid Barcode scanned, not in the database
                        CheckPrize(null, args);
                        //MessageBox.Show("Invalid Barcode scanned, please try again");
                        break;
                    case 1:
                        CheckPrize(prizes[0], args);
                        //if (prizes[0].PrizeID == null)
                        //{
                        //    no prize assigned to wristband
                        //    CheckPrize(prizes[0], args);
                        //    MessageBox.Show("Sorry, no prize assigned to this wristband");
                        //}
                        //else
                        //{
                        //    wristband has won a prize
                        //    List<GetPrizeResult> prize = context.GetPrize(prizes[0].PrizeID).ToList<GetPrizeResult>();
                        //    if (prize[0].Won == 0)
                        //    {
                        //        string description = prize[0].Prize.TrimEnd(' ');
                        //        string name = prizes[0].Name.TrimEnd(' ');
                        //        MessageBox.Show("Congratulations, you have won a " + description + " from " + name);

                        //        string image = prize[0].Image;
                        //        var uri = new Uri(@"c:\users\chrisd.dakotais\documents\visual studio 2017\Projects\HookADuck\HookADuck\Images\" + image);
                        //        rubberduck.Source = new BitmapImage(uri);
                        //        update the prize to set it as won
                        //        context.UpdatePrize(prize[0].ID);
                        //    }
                        //    else
                        //    {
                        //        MessageBox.Show("Sorry, this prize has already been won");
                        //    }
                        //}

                        // update the duck to set it as scanned
                        context.UpdateDuck(Barcode.Text);
                        break;
                    default:
                        // error - more than one prize assigned
                        MessageBox.Show("Congratulations, wristband has won a prize");
                        break;
                }
            }
        }
    }
}
