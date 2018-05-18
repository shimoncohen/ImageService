﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrastructure.Enums
{
    public static class EnumTranslator
    {
        public static InfoEnums CommandToInfo(int num)
        {
            switch(num)
            {
                case (int)CommandEnum.GetConfigCommand:
                    return InfoEnums.AppConfigInfo;
                case (int)CommandEnum.LogCommand:
                    return InfoEnums.LogHistoryInfo;
            }
            return InfoEnums.NotInfo;
        }

        public static CommandEnum InfoToCommand(int num)
        {
            switch(num)
            {
                case (int)InfoEnums.AppConfigInfo:
                    return CommandEnum.GetConfigCommand;
                case (int)InfoEnums.LogHistoryInfo:
                    return CommandEnum.LogCommand;
            }
            return CommandEnum.NotCommand;
        }

        public static string CommandEnumToString(int command)
        {
            string answer = "";
            switch(command)
            {
                case 0:
                    answer = "NotCommand";
                    break;
                case 1:
                    answer = "NewFileCommand";
                    break;
                case 2:
                    answer = "GetConfigCommand";
                    break;
                case 3:
                    answer = "LogCommand";
                    break;
                case 4:
                    answer = "CloseCommand";
                    break;
            }
            return answer;
        }

        public static string InfoEnumToString(int command)
        {
            string answer = "";
            switch (command)
            {
                case 0:
                    answer = "NotInfo";
                    break;
                case 1:
                    answer = "LogInfo";
                    break;
                case 2:
                    answer = "LogHistoryInfo";
                    break;
                case 3:
                    answer = "AppConfigInfo";
                    break;
                case 4:
                    answer = "CloseHandlerInfo";
                    break;
            }
            return answer;
        }
    }
}
