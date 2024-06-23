using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnipFMC.Common
{
    public static class RaintelsLogManager
    {
        private static readonly NLog.Logger _Logger = NLog.LogManager.GetCurrentClassLogger();

        public static void Info(string className, string methodName, string message)
        {
            try
            {
                string logmessage = $"{className}|{methodName}|{message}";
                _Logger.Info(logmessage);
            }
            catch (Exception)
            {

            }

        }

        public static void Error(Exception ex, string className, string methodName, string message)
        {
            try
            {
                string logmessage = $"{className}|{methodName}|{message}";
                _Logger.Error(ex, logmessage);
            }
            catch (Exception)
            {

            }

        }
        public static void Debug(string className, string methodName, string message)
        {
            try
            {
                string logmessage = $"{className}|{methodName}|{message}";
                _Logger.Debug(logmessage);
            }
            catch (Exception)
            {

            }

        }
    }
}
