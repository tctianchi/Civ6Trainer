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
using tctianchi.Civ6Trainer.Frontend.Control;

namespace tctianchi.Civ6Trainer.Frontend.Trainer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        protected class PageTag
        {
            public TrainerMenuPanel.MenuCategory Category { get; set; }
            public object Tag { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnNav_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            this.contentFrame.Navigate(new Uri("Frontend/Trainer/" + btn.Tag.ToString() + ".xaml", UriKind.Relative));
        }

        private void menuPanel_RefreshClicked(object sender, EventArgs e)
        {
            Random rand = new Random();

            menuPanel.ClearAll();
            menuPanel.AddItem(
                TrainerMenuPanel.MenuCategory.Player,
                new PageTag { Tag = "player 0 tag" },
                "玩家0");
            menuPanel.AddItem(
                TrainerMenuPanel.MenuCategory.City,
                new PageTag { Tag = "city 0 tag" },
                "城市长长长长长长长长长长长长长长长长",
                "1");
            menuPanel.AddItem(
                TrainerMenuPanel.MenuCategory.City,
                new PageTag { Tag = "city 1 tag" },
                $"城市{rand.Next()}",
                "12");
            menuPanel.AddItem(
                TrainerMenuPanel.MenuCategory.City,
                new PageTag { Tag = "city 2 tag" },
                $"城市{rand.Next()}",
                "123");
            menuPanel.AddItem(
                TrainerMenuPanel.MenuCategory.City,
                new PageTag { Tag = "city 3 tag" },
                "城市长长长长长长长长长长长长长长长长");
            for (int i = 0; i < 20; i++)
            {
                menuPanel.AddItem(
                    TrainerMenuPanel.MenuCategory.Army,
                    new PageTag { Tag = $"army {i} tag" },
                    $"部队{rand.Next()}");
            }
            menuPanel.AddItem(
                TrainerMenuPanel.MenuCategory.Science,
                new PageTag { Tag = "science 0 tag" },
                "自然科学");
            menuPanel.AddItem(
                TrainerMenuPanel.MenuCategory.Science,
                new PageTag { Tag = "science 1 tag" },
                "社会科学");
            menuPanel.AddItem(
                TrainerMenuPanel.MenuCategory.Debug,
                new PageTag { Tag = "debug 0 tag" },
                "测试1");
        }

        private void menuPanel_PageSelected(object sender, EventArgs e)
        {
            TrainerMenuPageSelector selector = sender as TrainerMenuPageSelector;
            PageTag tag = selector.Tag as PageTag;
            MessageBox.Show(tag.Tag as string);
        }
    }
}
