using System;

namespace ImageService.Logging.Modal.Event
{
    public class InfoEventArgs : EventArgs
    {
        public int InfoId { get; set; }

        public string[] Args { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name= infoEnum> the id of the information type </param>
        /// <param name= arguments> the args we receive </param>
        public InfoEventArgs(int infoEnum, string[] arguments)
        {
            InfoId = infoEnum;
            Args = arguments;
        }
    }
}
