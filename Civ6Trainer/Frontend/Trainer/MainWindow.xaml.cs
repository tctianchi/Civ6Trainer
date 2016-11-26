using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private FrameworkElement helloPage;
        private FrameworkElement resourcePage;
        private FrameworkElement cityPage;
        private FrameworkElement armyPage;
        private FrameworkElement researchPage;
        private FrameworkElement debug1Page;

        public MainWindow()
        {
            InitializeComponent();
            menuPanel.DataContext = ViewModel.MenuModel.Instance;
            //helloPage = Activator.CreateInstance(Type.GetType("Frontend/Trainer/HelloPage.xaml")) as FrameworkElement;

        }

        private void menuPanel_RefreshClicked(object sender, EventArgs e)
        {
            Random rand = new Random();

            if (ViewModel.MenuModel.Instance.PlayerList.Count > 5)
            {
                ViewModel.MenuModel.Instance.ClearAll();
            }
            if (ViewModel.MenuModel.Instance.PlayerList.Count > 0)
            {
                ViewModel.MenuModel.Instance.PlayerList[0].ContentText += "y";
                ViewModel.MenuModel.Instance.PlayerList[0].BubbleText += "x";
            }
            ViewModel.MenuModel.Instance.PlayerList.Add(new ViewModel.MenuModel.MenuItemModel()
            {
                Category = ViewModel.MenuModel.MenuCategory.Player,
                ContentText = "玩家12",
                BubbleText = "12",
            });
            //menuPanel.AddItem(
            //    TrainerMenuPanel.MenuCategory.City,
            //    new PageTag { Tag = "city 0 tag" },
            //    "城市长长长长长长长长长长长长长长长长",
            //    "1");
            //menuPanel.AddItem(
            //    TrainerMenuPanel.MenuCategory.City,
            //    new PageTag { Tag = "city 1 tag" },
            //    $"城市{rand.Next()}",
            //    "12");
            //menuPanel.AddItem(
            //    TrainerMenuPanel.MenuCategory.City,
            //    new PageTag { Tag = "city 2 tag" },
            //    $"城市{rand.Next()}",
            //    "123");
            //menuPanel.AddItem(
            //    TrainerMenuPanel.MenuCategory.City,
            //    new PageTag { Tag = "city 3 tag" },
            //    "城市长长长长长长长长长长长长长长长长");
            //for (int i = 0; i < 2; i++)
            //{
            //    menuPanel.AddItem(
            //        TrainerMenuPanel.MenuCategory.Army,
            //        new PageTag { Tag = $"army {i} tag" },
            //        $"部队{rand.Next()}");
            //}
            //menuPanel.AddItem(
            //    TrainerMenuPanel.MenuCategory.Research,
            //    new PageTag { Tag = "science 0 tag" },
            //    "自然科学");
            //menuPanel.AddItem(
            //    TrainerMenuPanel.MenuCategory.Research,
            //    new PageTag { Tag = "science 1 tag" },
            //    "社会科学");
            //menuPanel.AddItem(
            //    TrainerMenuPanel.MenuCategory.Debug,
            //    new PageTag { Tag = "debug 0 tag" },
            //    "测试1");
        }

        private void menuPanel_PageSelected(object sender, EventArgs e)
        {
            TrainerMenuPageSelector selector = sender as TrainerMenuPageSelector;
            ViewModel.MenuModel.MenuItemModel model = selector.DataContext as ViewModel.MenuModel.MenuItemModel;
            ViewModel.MenuModel.Instance.OnMenuSelected(model);
        }

        public void ShowMessage(string message)
        {
            //Button btn = sender as Button;
            //contentFrame.Navigate(new Uri("Frontend/Trainer/" + btn.Tag.ToString() + ".xaml", UriKind.Relative));
            contentFrame.Navigate(new Uri("Frontend/Trainer/HelloPage.xaml", UriKind.Relative));
            
        }
    }
}
