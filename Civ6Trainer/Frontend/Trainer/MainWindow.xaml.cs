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
        private Dictionary<MenuModel.MenuCategory, UserControl> _pages =
            new Dictionary<MenuModel.MenuCategory, UserControl>()
            {
                { MenuModel.MenuCategory.Hello, new HelloPage() },
                { MenuModel.MenuCategory.Resource, new ResourcePage() },
                { MenuModel.MenuCategory.City, new CityPage() },
                { MenuModel.MenuCategory.Army, new ArmyPage() },
                { MenuModel.MenuCategory.Tech, new TechPage() },
                { MenuModel.MenuCategory.Civic, new CivicPage() },
                { MenuModel.MenuCategory.Debug1, new Debug1Page() },
            };

        public MainWindow()
        {
            InitializeComponent();

            // 绑定
            menuPanel.DataContext = MenuModel.Instance;
            MenuModel.Instance.SwitchPageRequired += MenuModel_SwitchPageRequired;

            // 右侧显示默认页面
            if (!TrainerFacade.Instance.Init())
            {
                menuPanel.IsEnabled = false;
                return;
            }
            MenuModel.Instance.ShowMessage(Properties.Resources.UITextPleasePressRefresh);
        }

        private void menuPanel_RefreshClicked(object sender, EventArgs e)
        {
            TrainerFacade.Instance.Refresh();
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
            contentFrame.Navigate(page);
            page.DataContext = e.DataContext;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // 当点击窗口空白处时，使所有TextBox失去焦点
            gridSplitter.Focus();
        }
    }
}
