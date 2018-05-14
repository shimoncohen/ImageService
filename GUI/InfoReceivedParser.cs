using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUI.Modal.Event;
using GUI.Enums;

namespace GUI
{
    class InfoReceivedParser
    {
        public static int parseInfoType(int info)
        {
            switch(info)
            {
                case (int)InfoEnums.AppConfigInfo:
                    return 1;
                case (int)InfoEnums.CloseHandlerInfo:
                    return 1;
                case (int)InfoEnums.LogHistoryInfo:
                    return 2;
                case (int)InfoEnums.LogInfo:
                    return 2;
                case (int)InfoEnums.NotInfo:
                    return -1;
                default:
                    return 0;
            }
        }
    }
}
