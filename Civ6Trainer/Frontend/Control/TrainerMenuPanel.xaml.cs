using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace tctianchi.Civ6Trainer.Frontend.Control
{
    /// <summary>
    /// MenuPanel.xaml 的交互逻辑
    /// </summary>
    public partial class TrainerMenuPanel : UserControl
    {
        public event EventHandler<EventArgs> RefreshClicked;
        public event EventHandler<EventArgs> PageSelected;
        
        public TrainerMenuPanel()
        {
            InitializeComponent();
        }

        private void TrainerMenuPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (RefreshClicked != null)
            {
                RefreshClicked.Invoke(this, EventArgs.Empty);
            }
        }
        
        private void item_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (PageSelected != null)
            {
                PageSelected.Invoke(sender, EventArgs.Empty);
            }
        }
    }
}
