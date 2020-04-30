using Checkers.Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Checkers.Converters
{
    public class IntBoolConverter : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			if (value is int)
			{
				return (int)value > 0;
			}
			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			if (parameter is int)
			{
				return (int)parameter;
			}
			return 0;
		}

	}
}
