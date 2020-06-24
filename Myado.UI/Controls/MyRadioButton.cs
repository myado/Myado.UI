using Myado.UI.Constants;
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

namespace Myado.UI.Controls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Myado.UI.Controls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Myado.UI.Controls;assembly=Myado.UI.Controls"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:MyRadioButton/>
    ///
    /// </summary>
    public class MyRadioButton : RadioButton
    {
        static MyRadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyRadioButton), new FrameworkPropertyMetadata(typeof(MyRadioButton)));
        }

        /// <summary>
        /// 获取或设置按钮的基本样式。默认值为标准样式（General）。
        /// </summary>
        public RadioButtonStyles RadioButtonStyle
        {
            get { return (RadioButtonStyles)GetValue(RadioButtonStyleProperty); }
            set { SetValue(RadioButtonStyleProperty, value); }
        }
        public static readonly DependencyProperty RadioButtonStyleProperty = DependencyProperty.Register("RadioButtonStyle", typeof(RadioButtonStyles), typeof(MyRadioButton), new PropertyMetadata(RadioButtonStyles.General));

        /// <summary>
        /// 获取或设置鼠标悬浮时遮罩层的背景颜色（在Outline和Link样式下为前景色），默认值为#26FFFFFF（在Hollow、Outline和Link样式下为灰黑色）。
        /// </summary>
        public Brush CoverBrush
        {
            get { return (Brush)GetValue(CoverBrushProperty); }
            set { SetValue(CoverBrushProperty, value); }
        }
        public static readonly DependencyProperty CoverBrushProperty = DependencyProperty.Register("CoverBrush", typeof(Brush), typeof(MyRadioButton));


        /// <summary>
        /// 获取或设置鼠标悬浮时遮罩层的背景颜色（在Outline和Link样式下为前景色），默认值为#26FFFFFF（在Hollow、Outline和Link样式下为灰黑色）。
        /// </summary>
        public double BorderCornerRadius
        {
            get { return (double)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }
        public static readonly DependencyProperty BorderCornerRadiusProperty = DependencyProperty.Register("BorderCornerRadius", typeof(double), typeof(MyRadioButton));


        /// <summary>
        /// 获取或设置鼠标点击时按钮的效果。默认为无特殊效果（Classic）。
        /// </summary>
        public ClickStyles ClickStyle
        {
            get { return (ClickStyles)GetValue(ClickStyleProperty); }
            set { SetValue(ClickStyleProperty, value); }
        }

        public static readonly DependencyProperty ClickStyleProperty =
            DependencyProperty.Register("ClickStyle", typeof(ClickStyles), typeof(MyRadioButton), new PropertyMetadata(ClickStyles.Classic));
    }
}
