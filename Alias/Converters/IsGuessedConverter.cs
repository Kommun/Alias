using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Alias.Converters
{
    public class IsGuessedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var b = string.IsNullOrEmpty(parameter as string) ? (bool?)null : System.Convert.ToBoolean(parameter);
                var isGuessed = value as bool?;
                if (b == isGuessed)
                {
                    if (b == true)
                        return "Teal";
                    else if (b == false)
                        return "Firebrick";
                    else
                        return "#F0F0F0";
                }
                else
                    return "#5C5C5C";
            }
            catch
            {
                return "#5C5C5C";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
