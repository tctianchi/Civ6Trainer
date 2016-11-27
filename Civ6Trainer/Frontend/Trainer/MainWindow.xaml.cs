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
using tctianchi.Civ6Trainer.Backend;
using tctianchi.Civ6Trainer.ViewModel;

namespace tctianchi.Civ6Trainer.Frontend.Trainer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        protected Dictionary<MenuModel.MenuCategory, UserControl> _pages =
            new Dictionary<MenuModel.MenuCategory, UserControl>()
            {
                { MenuModel.MenuCategory.Hello, new HelloPage() },
                { MenuModel.MenuCategory.Resource, new ResourcePage() },
                { MenuModel.MenuCategory.City, new CityPage() },
                { MenuModel.MenuCategory.Army, new ArmyPage() },
                { MenuModel.MenuCategory.Research, new ResearchPage() },
                { MenuModel.MenuCategory.Debug1, new Debug1Page() },
            };

        public MainWindow()
        {
            InitializeComponent();

            // 绑定
            menuPanel.DataContext = MenuModel.Instance;
            MenuModel.Instance.SwitchPageRequired += MenuModel_SwitchPageRequired;

            // 右侧显示默认页面
            MenuModel.Instance.ShowMessage(Properties.Resources.UITextPleasePressRefresh);
        }

        private void menuPanel_RefreshClicked(object sender, EventArgs e)
        {
            Random rand = new Random();

            if (MenuModel.Instance.PlayerList.Count > 5)
            {
                MenuModel.Instance.ClearAll();
            }
            if (MenuModel.Instance.PlayerList.Count > 0)
            {
                MenuModel.Instance.PlayerList[0].ContentText += "y";
                MenuModel.Instance.PlayerList[0].BubbleText += "x";
            }
            MenuModel.Instance.PlayerList.Add(new MenuModel.MenuItemModel()
            {
                Category = MenuModel.MenuCategory.Resource,
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
            MenuModel.Instance.ShowMessage(Properties.Resources.UITextPleaseSelectAMenuItem);
        }

        private void menuPanel_PageSelected(object sender, EventArgs e)
        {
            TrainerMenuPageSelector selector = sender as TrainerMenuPageSelector;
            MenuModel.MenuItemModel model = selector.DataContext as MenuModel.MenuItemModel;
            MenuModel.Instance.OnMenuSelected(model);
        }

        private void MenuModel_SwitchPageRequired(object sender, MenuModel.SwitchPageEventArgs e)
        {
            UserControl page = _pages[e.Category];
            page.DataContext = e.DataContext;
            contentFrame.Navigate(page);
        }
    }
}
