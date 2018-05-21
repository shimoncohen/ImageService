using System;
using Infrastructure.Enums;

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
