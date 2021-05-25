using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace sppenyakitlambung.Services
{
    public static class LoggingService
    {
        /// <summary>
        /// Logs/displays an error message using the console and/or an optional dialog action.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="preText">Text to place before the exception message (optional).</param>
        /// <param name="title">The title of a console or dialog (optional).</param>
        /// <param name="useLogToDialogAction">If <see cref="LogToDialogAction"/> is set, will attempt to use the action in order to display an error dialog.</param>
        /// <param name="callerMemberName">The name of the method that called the logging service.
        /// Displayed as the initial text of the full error message (can be overridden with optional text).</param>
        /// <returns>The full error message if <see cref="DetailedErrors"/> is true, otherwise returns just the message of the exception.</returns>
        public static string LogErrorMessage(Exception exception, string preText = "", string title = "", bool useLogToDialogAction = true, [CallerMemberName] string callerMemberName = "")
        {
            string fullErrorMessage = ParseException(exception, preText, callerMemberName);
            Console.Out.WriteLine(fullErrorMessage);
            Debug.Write(fullErrorMessage);

            if (LogToConsoleAction != null)
            {
                LogToConsoleAction.Invoke(fullErrorMessage, title);
            }

            if (useLogToDialogAction && LogToDialogAction != null)
            {
                try
                {
                    LogToDialogAction.Invoke(fullErrorMessage, title);
                }
                catch (Exception e)
                {
                    string dialogError = $"{Environment.NewLine}{Environment.NewLine}LogToDialogAction{Environment.NewLine}{ParseException(e)}{Environment.NewLine}{Environment.NewLine}";
                    Console.WriteLine(dialogError);
                    Debug.Write(dialogError);
                }
            }

            return fullErrorMessage;
        }

        /// <summary>
        /// Builds an error string from the given exception with optional text that appears before the exception text.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="preText">Text to place before the exception message (optional).</param>
        /// <param name="callerMemberName">The name of the method that called the logging service.
        /// Displayed as the initial text of the full error message (can be overridden with optional text).</param>
        /// <returns>The full error message if <see cref="DetailedErrors"/> is true, otherwise returns just the message of the exception.</returns>
        public static string ParseException(Exception exception, string preText = "", [CallerMemberName] string callerMemberName = "")
        {
            string fullErrorMessage = $"{Environment.NewLine}{Environment.NewLine}";

            if (DetailedErrors)
            {
                fullErrorMessage = $"{Environment.NewLine}{(string.IsNullOrWhiteSpace(callerMemberName) ? "" : "Function: " + callerMemberName)}{Environment.NewLine}{preText}{Environment.NewLine}";

                while (exception != null)
                {
                    string type = exception.GetType().ToString();
                    string message = exception.Message;

                    if (type == "Java.Net.UnknownHostException")
                    {
                        message += $"{Environment.NewLine}Possible Resolution: Wifi needs reboot or device needs reboot.{Environment.NewLine}";
                    }

                    fullErrorMessage += $"{Environment.NewLine}Type...{Environment.NewLine}{type}{Environment.NewLine}Message...{Environment.NewLine}{message}{Environment.NewLine}{(exception.TargetSite != null ? $"Target Site...{Environment.NewLine}{exception.TargetSite}{Environment.NewLine}" : "")}{(string.IsNullOrWhiteSpace(exception.StackTrace) ? "" : $"Stack Trace...{Environment.NewLine}{exception.StackTrace}{Environment.NewLine}")}";

                    if (exception.InnerException != null)
                    {
                        fullErrorMessage += $"{Environment.NewLine}-------------------------------------{Environment.NewLine}";
                    }

                    exception = exception.InnerException;
                }
            }
            else
            {
                fullErrorMessage += exception.Message;
            }

            return fullErrorMessage + Environment.NewLine + Environment.NewLine;
        }

        public static bool DetailedErrors;

        /// <summary>
        /// An action that accepts text that logs to a console provided through the function passed to the action.
        /// /// The first argument is the message, the second argument is the title.
        /// </summary>
        public static Action<string, string> LogToConsoleAction;

        /// <summary>
        /// An action that accepts text that logs to a dialog provided through the function passed to the action.
        /// The first argument is the message, the second argument is the title.
        /// </summary>
        public static Action<string, string> LogToDialogAction;
    }
}
