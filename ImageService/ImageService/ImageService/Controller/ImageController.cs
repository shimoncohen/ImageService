using ImageService.Commands;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using ImageService.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    /// <summary>
    /// the class of the controller.
    /// </summary>
    public class ImageController : IImageController
    {
        // The Modal Object
        private IImageServiceModal modal;
        // a dictionary of commands to execute
        private Dictionary<int, ICommand> commands;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name= modal> the controller receives the modal we created </param>
        public ImageController(IImageServiceModal mod)
        {
            modal = mod;                    // Storing the Modal Of The System
            // create the commands dictionary
            commands = new Dictionary<int, ICommand>()
            {
                // add commands to the dictionary by thier enum value
                {(int)CommandEnum.NewFileCommand, new NewFileCommand(mod)},
                {(int)CommandEnum.GetConfigCommand, new GetConfigCommand()},
                {(int)CommandEnum.LogCommand, new LogCommand()},
                {(int)CommandEnum.CloseCommand, new CloseCommand()}
            };
        }

        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccessful)
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
                System.Threading.Thread.Sleep(1);
                // save result from thread
                Tuple<string, bool> output = t.Result;
                resultSuccessful = output.Item2;
                return output.Item1;
            }
            else
            {
                resultSuccessful = false;
                return "Not a command";
            }
        }
    }
}
