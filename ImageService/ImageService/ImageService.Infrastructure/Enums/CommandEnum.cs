using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrastructure.Enums
{
    /// <summary>
    /// set of enums for possible commands that can be sent to the controller and the logger.
    /// </summary>
    public enum CommandEnum : int
    {
        NewFileCommand,
        CloseCommand
    }
}
