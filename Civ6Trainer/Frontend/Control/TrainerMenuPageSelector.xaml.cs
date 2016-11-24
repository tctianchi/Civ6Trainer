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
    /// MenuItem.xaml 的交互逻辑
    /// </summary>
    public partial class TrainerMenuPageSelector : UserControl
    {
        public TrainerMenuPageSelector()
        {
            InitializeComponent();
        }

        public string ContentText
        {
            get
            {
                return contentBox.Text;
            }
            set
            {
                contentBox.Text = value;
            }
        }
        
        public string BubbleText
        {
            get
            {
                return bubbleText.Text;
            }
            set
            {
                bubbleText.Text = value;
                if (value == "")
                {
                    bubbleBox.Visibility = Visibility.Collapsed;
                }
                else
                {
                    bubbleBox.Visibility = Visibility.Visible;
                }
            }
        }

        public bool IsMarked
        {
            get
            {
                return marker.Visibility == Visibility.Visible;
            }
            set
            {
                marker.Visibility = value ? Visibility.Visible : Visibility.Hidden;
                recheckBackboardColor();
            }
        }

        private void wholePanel_MouseEvent(object sender, MouseEventArgs e)
        {
            recheckBackboardColor();
        }

        private void recheckBackboardColor()
        {
            if (wholePanel.IsMouseOver || IsMarked)
            {
                VisualStateManager.GoToElementState(wholePanel, "HighLight", true);
            }
            else
            {
                VisualStateManager.GoToElementState(wholePanel, "NormalLight", true);
            }
        }
    }
}
