using ImageService.Commands;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        private IImageServiceModal m_modal;                      // The Modal Object
        // a dictionary of commands to execute
        private Dictionary<int, ICommand> commands;

        public ImageController(IImageServiceModal modal)
        {
            m_modal = modal;                    // Storing the Modal Of The System
            // create the commands dictionary
            commands = new Dictionary<int, ICommand>()
            {
                // add commands to the dictionary by thier enum value
                {(int)CommandEnum.NewFileCommand, new NewFileCommand(modal)}
            };
        }

        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            ICommand command;
            // extract command from dictionary if exists
            if (commands.TryGetValue(commandID, out command))
            {
                // if exists then execute the command
                return command.Execute(args, out resultSuccesful);
            }
            else
            {
                resultSuccesful = false;
                return "No such command";
            }
            // if command returned an exception
            /*catch (Exception e)
            {
                resultSuccesful = false;
                return e.ToString();
            }*/
        }
    }
}
