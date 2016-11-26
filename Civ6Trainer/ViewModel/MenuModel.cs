using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tctianchi.Civ6Trainer.ViewModel
{
    public class MenuModel
    {
        #region menu item

        public enum MenuCategory
        {
            Player,
            City,
            Army,
            Research,
            Debug,
        }

        public class MenuItemModel : INotifyPropertyChanged
        {
            // 大分类
            private MenuCategory category;
            public MenuCategory Category { get { return category; } set { category = value; onChange("Category"); } }

            // 是否选中
            private bool isMarked;
            public bool IsMarked { get { return isMarked; } set { isMarked = value; onChange("IsMarked"); } }

            // 左侧的标题文字
            private string contentText;
            public string ContentText { get { return contentText; } set { contentText = value; onChange("ContentText"); } }

            // 右侧的气泡文字
            private string bubbleText;
            public string BubbleText { get { return bubbleText; } set { bubbleText = value; onChange("BubbleText"); } }

            // 任意的附加信息
            private object tag;
            public object Tag { get { return tag; } set { tag = value; onChange("Tag"); } }

            #region on change

            public event PropertyChangedEventHandler PropertyChanged;
            private void onChange(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            #endregion
        }

        #endregion

        // singleton instance
        private static MenuModel instance = new MenuModel();

        // get singleton instance
        public static MenuModel Instance
        {
            get
            {
                return instance;
            }
        }

        private MenuModel()
        {
            PlayerList = new ObservableCollection<MenuItemModel>();
            CityList = new ObservableCollection<MenuItemModel>();
            ArmyList = new ObservableCollection<MenuItemModel>();
            ResearchList = new ObservableCollection<MenuItemModel>();
            DebugList = new ObservableCollection<MenuItemModel>();
        }

        public void ClearAll()
        {
            PlayerList.Clear();
            CityList.Clear();
            ArmyList.Clear();
            ResearchList.Clear();
            DebugList.Clear();
        }

        // 当menu被选中时
        public void OnMenuSelected(MenuItemModel selected)
        {
            // 把当前菜单标记为选中
            selected.IsMarked = true;

            // 其余元素标记为未选中
            Action<MenuItemModel> clearMark = (MenuItemModel i) =>
            {
                if (i != selected)
                {
                    i.IsMarked = false;
                }
            };
            foreach (var i in PlayerList)
            {
                clearMark(i);
            }
            foreach (var i in CityList)
            {
                clearMark(i);
            }
            foreach (var i in ArmyList)
            {
                clearMark(i);
            }
            foreach (var i in ResearchList)
            {
                clearMark(i);
            }
            foreach (var i in DebugList)
            {
                clearMark(i);
            }
        }

        // 以下是各个菜单列表的内容
        public ObservableCollection<MenuItemModel> PlayerList { get; private set; }
        public ObservableCollection<MenuItemModel> CityList { get; private set; }
        public ObservableCollection<MenuItemModel> ArmyList { get; private set; }
        public ObservableCollection<MenuItemModel> ResearchList { get; private set; }
        public ObservableCollection<MenuItemModel> DebugList { get; private set; }
    }
}
