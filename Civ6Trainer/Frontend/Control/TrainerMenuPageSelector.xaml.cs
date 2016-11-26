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
            SetBinding(BubbleTextProperty, new Binding()
            {
                Path = new PropertyPath("BubbleText"),
            });
            SetBinding(IsMarkedProperty, new Binding()
            {
                Path = new PropertyPath("IsMarked"),
            });
            bubbleTextPostCheck();
            isMarkedPostCheck();
        }

        // 左侧标题栏文字。已绑定在DataContext的ContentText
        #region ContentText

        // 没有需要补充的

        #endregion

        // 右侧气泡。对外绑定在DataContext的BubbleText。对内绑定在ForwaredBubbleText
        #region BubbleText

        public static readonly DependencyProperty BubbleTextProperty =
            DependencyProperty.Register("BubbleText", typeof(string), typeof(TrainerMenuPageSelector),
            new PropertyMetadata("", new PropertyChangedCallback(OnBubbleTextChanged)));

        public static void OnBubbleTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrainerMenuPageSelector self = d as TrainerMenuPageSelector;
            string value = (string)e.NewValue;
            self.bubbleTextPostCheck();
        }

        public string ForwaredBubbleText
        {
            get
            {
                return (string)GetValue(BubbleTextProperty);
            }
        }

        private void bubbleTextPostCheck()
        {
            bubbleText.Text = ForwaredBubbleText;
            if (bubbleText.Text == "")
            {
                bubbleBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                bubbleBox.Visibility = Visibility.Visible;
            }
        }

        #endregion

        // 是否处于选中状态。对外绑定在DataContext的IsMarked。对内没有绑定关系
        #region IsMarked

        public static readonly DependencyProperty IsMarkedProperty =
            DependencyProperty.Register("IsMarked", typeof(bool), typeof(TrainerMenuPageSelector),
            new PropertyMetadata(false, new PropertyChangedCallback(IsMarkedChanged)));

        public static void IsMarkedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrainerMenuPageSelector self = d as TrainerMenuPageSelector;
            bool value = (bool)e.NewValue;
            self.isMarkedPostCheck();
        }
        
        public bool IsMarked
        {
            get
            {
                return (bool)GetValue(IsMarkedProperty);
            }
        }

        #endregion

        #region backboard color

        private void wholePanel_MouseEvent(object sender, MouseEventArgs e)
        {
            isMarkedPostCheck();
        }

        private void isMarkedPostCheck()
        {
            marker.Visibility = IsMarked ? Visibility.Visible : Visibility.Hidden;
            if (wholePanel.IsMouseOver || IsMarked)
            {
                VisualStateManager.GoToElementState(wholePanel, "HighLight", true);
            }
            else
            {
                VisualStateManager.GoToElementState(wholePanel, "NormalLight", true);
            }
        }

        #endregion
    }
}
