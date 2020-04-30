using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Checkers.Converters
{
    class StarsFilledConverter : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is int && parameter is string)
			{
				int p;
				try
				{
					p = Int32.Parse((string)parameter);
				} catch (FormatException)
				{
					return false;
				}
				return (int)value >= p;
			}
			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}

	}
}
