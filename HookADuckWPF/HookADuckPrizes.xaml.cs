using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using MahApps.Metro.SimpleChildWindow;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace HookADuck
{
    /// <summary>
    /// Interaction logic for HookADuckPrizes.xaml
    /// </summary>
    public partial class HookADuckPrizes : ChildWindow
    {
        public HookADuckPrizes()
        {
            InitializeComponent();
            FillDataGrid();
        }

        private void FillDataGrid()
        {
            HookADuckDataClassesDataContext context = new HookADuckDataClassesDataContext();
            List<GetAllPrizesResult> prizes = context.GetAllPrizes().ToList<GetAllPrizesResult>();
            gridPrizes.ItemsSource = prizes;

            gridPrizes.ItemContainerGenerator.StatusChanged += (s, e) =>
            {
                if (gridPrizes.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                {
                    foreach (GetAllPrizesResult prize in gridPrizes.ItemsSource)
                    {
                        var row = gridPrizes.ItemContainerGenerator.ContainerFromItem(prize) as DataGridRow;
                        if (prize.Won == 1)
                        {
                            if (row != null)
                            {
                                row.Background = Brushes.Pink;
                            }
                        }
                    }
                }
            };
        }
    }
}
