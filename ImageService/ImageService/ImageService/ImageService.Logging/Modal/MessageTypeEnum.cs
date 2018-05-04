using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging.Modal
{
    /// <summary>
    /// an enum set of possible states that the logger can recieve.
    /// </summary> 
    public enum MessageTypeEnum : int
    {
        INFO,
        WARNING,
        FAIL
    }
}
