using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using GUI;
using Infrastructure.Enums;

namespace GUI.VMs
{
    /// <summary>
    /// A converter from message type to string 
    /// </summary>
    class ColorConverter : IValueConverter
    {
        /// <summary>
        /// Returns the color as a string according to the message type.
        /// </summary>
        /// <param name="value">The message type</param>
        /// <param name="targetType">The type of the value</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(targetType.Name != "Brush")
            {
                throw new InvalidOperationException("Type must be of message enum type");
            }
            // we return the proper color according to the type off the message: red-for fail; yellow-for warning; yellow green- for info.
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
