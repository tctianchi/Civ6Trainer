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
                    bubbleBox.Width = new GridLength(0);
                }
                else
                {
                    bubbleBox.Width = new GridLength(50);
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
            }
        }

        private void wholePanel_MouseEvent(object sender, MouseEventArgs e)
        {
            if (wholePanel.IsMouseOver)
            {
                VisualStateManager.GoToElementState(wholePanel, "MouseEnter", true);
            }
            else
            {
                VisualStateManager.GoToElementState(wholePanel, "MouseLeave", true);
            }
        }
    }
}
