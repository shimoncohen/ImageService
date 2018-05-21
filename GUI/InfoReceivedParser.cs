using System;
using Infrastructure.Enums;

namespace GUI
{
    /// <summary>
    /// A parser from infoEnum to an int that represents the model we want to act.
    /// 1 is the settings model; 2 is the logs model; -1 is not an info.
    /// </summary>
    class InfoReceivedParser
    {
        /// <summary>
        /// The function returns an int that represents the corresponding model. 1 for settings model and 2 for logs model.
        /// </summary>
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
