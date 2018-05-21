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
    }
}
