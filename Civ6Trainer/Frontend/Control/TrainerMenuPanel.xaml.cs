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

        public enum MenuCategory
        {
            Player,
            City,
            Army,
            Research,
            Debug,
        }

        public void ClearAll()
        {
            playerList.Children.Clear();
            cityList.Children.Clear();
            armyList.Children.Clear();
            researchList.Children.Clear();
            debugList.Children.Clear();
        }

        public TrainerMenuPageSelector AddItem(MenuCategory category, object tag = null, string contentText = "", string bubbleText = "")
        {
            TrainerMenuPageSelector item = new TrainerMenuPageSelector();
            item.IsMarked = false;
            item.Tag = tag;
            item.ContentText = contentText;
            item.BubbleText = bubbleText;
            item.MouseLeftButtonUp += item_MouseLeftButtonUp;
            switch (category)
            {
                case MenuCategory.Player:
                    playerList.Children.Add(item);
                    break;
                case MenuCategory.City:
                    cityList.Children.Add(item);
                    break;
                case MenuCategory.Army:
                    armyList.Children.Add(item);
                    break;
                case MenuCategory.Research:
                    researchList.Children.Add(item);
                    break;
                case MenuCategory.Debug:
                    debugList.Children.Add(item);
                    break;
            }
            return item;
        }

        private void item_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // 选中元素标记为选中
            (sender as TrainerMenuPageSelector).IsMarked = true;

            // 其余元素标记为未选中
            Action<TrainerMenuPageSelector> clearMark = (TrainerMenuPageSelector i) =>
            {
                if (i != sender)
                {
                    i.IsMarked = false;
                }
            };
            foreach (var i in playerList.Children)
            {
                clearMark(i as TrainerMenuPageSelector);
            }
            foreach (var i in cityList.Children)
            {
                clearMark(i as TrainerMenuPageSelector);
            }
            foreach (var i in armyList.Children)
            {
                clearMark(i as TrainerMenuPageSelector);
            }
            foreach (var i in researchList.Children)
            {
                clearMark(i as TrainerMenuPageSelector);
            }
            foreach (var i in debugList.Children)
            {
                clearMark(i as TrainerMenuPageSelector);
            }

            // 触发选中事件
            if (PageSelected != null)
            {
                PageSelected.Invoke(sender, EventArgs.Empty);
            }
        }
    }
}
