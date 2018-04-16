using ImageService.Infrastructure;
using ImageService.Modal;
using ImageService.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Logging.Modal;

namespace ImageService.Commands
{
    /// <summary>
    /// the class of the NewFile command. creates a new file.
    /// </summary>
    public class NewFileCommand : ICommand
    {
        private IImageServiceModal modal;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name= modal> the modal we created </param>
        public NewFileCommand(IImageServiceModal m)
        {
            modal = m;            // Storing the Modal
        }

        public string Execute(string[] args, out bool result)
        {
            // The String Will Return the New Path if result = true, and will return the error message
            return modal.AddFile(args[0], out result);
        }
    }
}
