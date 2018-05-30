using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    interface IServer
    {
        /// <summary>
        /// Starts the servers functionality
        /// </summary>
        void Start(string[] str);

        /// <summary>
        /// Stops the server from runningS
        /// </summary>
        void Stop();
    }
}
