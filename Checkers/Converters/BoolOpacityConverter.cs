using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Checkers.Converters
{
    public class BoolOpacityConverter : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is bool)
			{
				if ((bool)value)
				{
					return .5;
				}
			}
			return 1.0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			if (value is double)
			{
				if ((double)value == .5)
				{
					return true;
				}
			}
			return false;
		}

	}
}
