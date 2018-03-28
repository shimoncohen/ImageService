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
        // The Modal Object
        private IImageServiceModal m_modal;
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
                // create a thread to execute the command
                Task<Tuple<string, bool>> t = new Task<Tuple<string, bool>> (() => {
                    // execute the command
                    bool result;
                    return Tuple.Create(command.Execute(args, out result), result);
                });
                // activate the thread
                t.Start();
                // save result from thread
                Tuple<string, bool> output = t.Result;
                resultSuccesful = output.Item2;
                return output.Item1;
            }
            else
            {
                resultSuccesful = false;
                return "Not a command";
            }
        }
    }
}
