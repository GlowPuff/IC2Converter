using System.Globalization;
using System.Windows.Data;

namespace IC2_Mass_Mission_Converter
{
	public class InvertBoolConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return !(bool)value;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture ) => throw new NotImplementedException();
	}

	public class EnumCheckedConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return value?.Equals( parameter );
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			return value?.Equals( true ) == true ? parameter : Binding.DoNothing;
		}
	}
}
