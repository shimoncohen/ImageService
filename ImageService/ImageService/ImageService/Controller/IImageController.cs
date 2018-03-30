using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    /// <summary>
    /// the interface of the controller.
    /// </summary>
    public interface IImageController
    {
        /// <summary>
        /// the function gets a command id an execute it
        /// </summary>
        /// <param name= commandID> the id of the command </param>
        /// <param name= args> the args to the command </param>
        /// <param name= resultSuccesful> gets true if command executed successfuly and false otherwise </param>
        /// <return> return the result of the command or an error message </return>
        string ExecuteCommand(int commandID, string[] args, out bool result);          // Executing the Command Requet
    }
}
