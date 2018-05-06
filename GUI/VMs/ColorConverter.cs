using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using GUI;

namespace GUI.VMs
{
    class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(targetType.Name != "Brush")
            {
                throw new InvalidOperationException("Type must be of message enum type");
            }
            MessageTypeEnum type = (MessageTypeEnum)value;
            switch(type)
            {
                case MessageTypeEnum.INFO:
                    return "YellowGreen";
                case MessageTypeEnum.WARNING:
                    return "Yellow";
                case MessageTypeEnum.FAIL:
                    return "Red";
                default:
                    return "Transparent";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
