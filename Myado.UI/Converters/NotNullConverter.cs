using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Myado.UI.Converters
{
    public class NotNullConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool b = true;
            if(parameter != null)
            {
                bool.TryParse(parameter.ToString(), out b);
            }
            if (value != null && value.ToString()!= "")
            {
                return b;
            }
            return b == false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
