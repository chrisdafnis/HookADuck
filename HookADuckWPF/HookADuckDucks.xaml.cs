using System.Collections.Generic;
using System.Linq;
using MahApps.Metro.SimpleChildWindow;

namespace HookADuck
{
    /// <summary>
    /// Interaction logic for HookADuckDucks.xaml
    /// </summary>
    public partial class HookADuckDucks : ChildWindow
    {
        public HookADuckDucks()
        {
            InitializeComponent();
            FillDataGrid();
        }

        private void FillDataGrid()
        {
            HookADuckDataClassesDataContext context = new HookADuckDataClassesDataContext();
            List<GetAllDucksResult> ducks = context.GetAllDucks().ToList<GetAllDucksResult>();
            gridDucks.ItemsSource = ducks;
        }
    }
}
