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
        private UserControl helloPage = new HelloPage();
        private UserControl resourcePage = new ResourcePage();
        private UserControl cityPage = new CityPage();
        private UserControl armyPage = new ArmyPage();
        private UserControl researchPage = new ResearchPage();
        private UserControl debug1Page = new Debug1Page();

        public MainWindow()
        {
            InitializeComponent();

            // 左侧菜单绑定
            menuPanel.DataContext = ViewModel.MenuModel.Instance;

            // 右侧显示默认页面
            ShowMessage(Properties.Resources.UITextPleasePressRefresh);
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
            ShowMessage(Properties.Resources.UITextPleaseSelectAMenuItem);
        }

        private void menuPanel_PageSelected(object sender, EventArgs e)
        {
            TrainerMenuPageSelector selector = sender as TrainerMenuPageSelector;
            ViewModel.MenuModel.MenuItemModel model = selector.DataContext as ViewModel.MenuModel.MenuItemModel;
            ViewModel.MenuModel.Instance.OnMenuSelected(model);
        }

        public void ShowMessage(string message)
        {
            contentFrame.Navigate(helloPage);
            helloPage.DataContext = ViewModel.HelloPageModel.Instance;
            ViewModel.HelloPageModel.Instance.PromptText = message;
        }
    }
}
