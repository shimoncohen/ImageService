using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Enums
{
    public static class EnumTranslator
    {
        /// <summary>
        /// translates a command enum type to a info enum type
        /// </summary>
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

        /// <summary>
        /// translates a info enum type to a command enum type
        /// </summary>
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

        /// <summary>
        /// returns a command enum's string representation
        /// </summary>
        public static string CommandToString(int num)
        {
            string name = "";
            switch (num)
            {
                case 0:
                    name = "NotCommand";
                    break;
                case 1:
                    name = "NewFileCommand";
                    break;
                case 2:
                    name = "GetConfigCommand";
                    break;
                case 3:
                    name = "LogCommand";
                    break;
                case 4:
                    name = "CloseCommand";
                    break;
            }
            return name;
        }

        /// <summary>
        /// returns a info enum's string representation
        /// </summary>
        public static string InfoToString(int num)
        {
            string name = "";
            switch (num)
            {
                case 0:
                    name = "NotInfo";
                    break;
                case 1:
                    name = "LogInfo";
                    break;
                case 2:
                    name = "LogHistoryInfo";
                    break;
                case 3:
                    name = "AppConfigInfo";
                    break;
                case 4:
                    name = "CloseHandlerInfo";
                    break;
            }
            return name;
        }

        /// <summary>
        /// returns a MessageType enum's string representation
        /// </summary>
        public static string MessageTypeToString(MessageTypeEnum type)
        {
            string name = "";
            switch (type)
            {
                case MessageTypeEnum.INFO:
                    name = "INFO";
                    break;
                case MessageTypeEnum.WARNING:
                    name = "WARNING";
                    break;
                case MessageTypeEnum.FAIL:
                    name = "FAIL";
                    break;
            }
            return name;
        }
    }
}
