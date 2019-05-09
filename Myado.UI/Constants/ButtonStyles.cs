using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myado.UI.Constants
{
    /// <summary>
    /// (PanuonUI) Button styles for PUButton.
    /// </summary>
    public enum ButtonStyles
    {
        /// <summary>
        /// 一个常规按钮。
        /// </summary>
        General = 1,
        /// <summary>
        /// 一个带边框的空心按钮，当鼠标悬浮时才会显示背景色。
        /// <para>当鼠标移入时，该按钮的背景色将由Background变为指定的CoverBrush。</para>
        /// </summary>
        Hollow = 2,
        /// <summary>
        /// 一个带边框的空心按钮，当鼠标悬浮时才会显示前景色。
        /// <para>当鼠标移入时，该按钮的边框和前景色将由BorderBrush和Foreground变为指定的CoverBrush。</para>
        /// </summary>
        Outline = 3,
        /// <summary>
        /// 一个不带任何边框和背景色的文字按钮。
        /// <para>当鼠标移入时，该按钮的前景色将由Foreground变为指定的CoverBrush。</para>
        /// </summary>
        Link = 4,
    }
}
