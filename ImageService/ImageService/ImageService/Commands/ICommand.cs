using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    /// <summary>
    /// an interface for commands
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// The Function That will Execute The command
        /// </summary>
        /// <param name= args> the args to the command </param>
        /// <param name= resultSuccesful> gets true if command executed successfuly and false otherwise </param>
        /// <return> return the result of the command or an error message </return>
        string Execute(string[] args, out bool result);
    }
}
