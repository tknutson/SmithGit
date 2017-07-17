namespace WebsiteManagement
{
    using System;
    using System.IO;

    public class Logger
    {
        /***************************************************************************
         * This class requires use with a data-structure class object, and contains no data
         * properties (public or otherwise) of its own.  As such the methods of this class
         * may be used without instantiating the Logger() class.  Just use the methods
         * passing the LoggerInfo class object as the first parameter.
         * 
         * NOTE: the ValidatePath() method is called with the LoggerInfo class parameter
         *       passed as a "reference" variable so that the original LoggerInfo class
         *       object of the calling class will reflect any error or status flags.
         *       
         * ************************************************************************/
        public static void ValidatePath(ref LoggerInfo loggerInfo)
        {
            loggerInfo.ErrorFlag = false;

            if (loggerInfo.RootPath == "" || loggerInfo.FolderName == "")
            {
                loggerInfo.ErrorMsg = "Logger ValidatePath: RootPath and/or FolderName not provided";
                loggerInfo.ErrorFlag = true;
            }
            else
            {
                try
                {
                    // check if ROOT path is valid
                    if (!Directory.Exists(loggerInfo.RootPath))
                    {
                        loggerInfo.ErrorMsg = "Logger ValidatePath: RootPath (" + loggerInfo.RootPath.Trim() + ") not found";
                        loggerInfo.ErrorFlag = true;
                    }
                    else
                    {
                        // unconditionally attempt to create log FOLDER
                        // (CreateSubdirectory does nothing if it already exists)
                        DirectoryInfo appDI = new DirectoryInfo(loggerInfo.RootPath);
                        appDI.CreateSubdirectory(loggerInfo.FolderName);
                    }

                    loggerInfo.FullPathChecked = true;
                }
                catch (Exception ex)
                {
                    loggerInfo.ErrorMsg = "Logger ValidatePath: failed. " + ex.Message;
                    loggerInfo.ErrorFlag = true;
                }
            }

            return;
        }

        public static void Write(LoggerInfo loggerInfo, string logMsg)
        {
            // if we havent yet done so, validate complete file path to log file
            if (!loggerInfo.FullPathChecked)
            {
                ValidatePath(ref loggerInfo);
            }

            // once logger issue develops, stop trying to write to it
            if (!loggerInfo.ErrorFlag)
            {
                try
                {
                    // if log file not currently present, create it with passed message
                    // otherwise append message to existing log file
                    if (!File.Exists(loggerInfo.FullPath))
                    {
                        using (StreamWriter sw = File.CreateText(loggerInfo.FullPath))
                        {
                            sw.WriteLine(DateTime.Now.ToString() + ": " + logMsg);
                        }
                    }
                    else
                    {
                        using (StreamWriter sw = File.AppendText(loggerInfo.FullPath))
                        {
                            sw.WriteLine(DateTime.Now.ToString() + ": " + logMsg);
                        }
                    }

                    loggerInfo.ErrorMsg = "written";
                }
                catch (Exception ex)
                {
                    loggerInfo.ErrorMsg = "Logger Write(): failed. " + ex.Message;
                }
            }

            return;
        }

        public static void Write(LoggerInfo loggerInfo, string methodName, Exception thisException)
        {
            // if we havent yet done so, validate complete file path to log file
            if (!loggerInfo.FullPathChecked)
            {
                ValidatePath(ref loggerInfo);
            }

            // once logger issue develops, stop trying to write to it
            if (!loggerInfo.ErrorFlag)
            {
                if (thisException is System.Data.Odbc.OdbcException)
                {
                    var ex = (System.Data.Odbc.OdbcException)thisException;
                    Write(loggerInfo, methodName + " ODBC Exception Message: " + ex.Message);
                    Write(loggerInfo, methodName + " ODBC Exception Source: " + ex.Source);
                    Write(loggerInfo, methodName + " ODBC Exception TargetSite: " + ex.TargetSite);
                    Write(loggerInfo, methodName + " ODBC Exception Data: " + ex.Data);
                    Write(loggerInfo, methodName + " ODBC Exception Error Code: " + ex.ErrorCode);
                    Write(loggerInfo, methodName + " ODBC Exception Errors: " + ex.Errors);
                    Write(loggerInfo, methodName + " ODBC Exception StackTrace: " + ex.StackTrace);
                }
                else
                {
                    var ex = thisException;
                    Write(loggerInfo, methodName + " Exception Message: " + ex.Message);
                    Write(loggerInfo, methodName + " Exception Source: " + ex.Source);
                    Write(loggerInfo, methodName + " Exception TargetSite: " + ex.TargetSite);
                    Write(loggerInfo, methodName + " Exception Data: " + ex.Data);
                    Write(loggerInfo, methodName + " Exception StackTrace: " + ex.StackTrace);
                }
            }

            return;
        }

        public static void WriteDbg(LoggerInfo loggerInfo, string logMsg)
        {
            // if we havent yet done so, validate complete file path to log file
            if (!loggerInfo.FullPathChecked)
            {
                ValidatePath(ref loggerInfo);
            }

            // once logger issue develops, stop trying to write to it
            if (!loggerInfo.ErrorFlag)
            {
                // assuming DebugLogging has been activated
                if (loggerInfo.DebugActive)
                {
                    try
                    {
                        // if log file not currently present, create it with passed message
                        // otherwise append message to existing log file
                        if (!File.Exists(loggerInfo.FullPath))
                        {
                            using (StreamWriter sw = File.CreateText(loggerInfo.FullPath))
                            {
                                sw.WriteLine(DateTime.Now.ToString() + ": " + logMsg);
                            }
                        }
                        else
                        {
                            using (StreamWriter sw = File.AppendText(loggerInfo.FullPath))
                            {
                                sw.WriteLine(DateTime.Now.ToString() + ": " + logMsg);
                            }
                        }

                        loggerInfo.ErrorMsg = "dbg written";
                    }
                    catch (Exception ex)
                    {
                        loggerInfo.ErrorMsg = "Logger WriteDbg(): failed. " + ex.Message;
                    }
                }
            }

            return;
        }
    }

    public class LoggerInfo
    {
        /*******************************************************************************
         * This is a primitive Class that communicates thru its public property accessors
         * rather than thru the common StandardResponse object.
         * 
         * Classes that instantiate this object should:
         * (1) set the RootPath and FolderName properties (and optionally FileName), 
         * (2) call the .ValidatePath() method, 
         * (3) finally check the "LogError" property (boolean) to verify that the path 
         *     and folder provided were valid.  If ErrorFlag=true, the LogErrorMsg property (string) 
         *     will contain an error message.
         * 
         * Optional property "DebugActive" (boolean, default=false) may be set to control 
         * whether uses of method .writeDbg() will be honored or ignored.  Setting this
         * property eliminates the need to condition the use of the .WriteDbgMsgs() method
         * in all other classes that use Logger.
         * *****************************************************************************/
        private string _rootPath;
        private string _folderName;
        private string _fileName;
        private bool _debugActive;
        private bool _errorFlag;
        private string _errorMsg;
        private bool _fullPathChecked;

        // CONSTRUCTOR
        public LoggerInfo()
        {
            _rootPath = "";
            _folderName = "";
            _fileName = "log.txt";
            _debugActive = false;
            _errorFlag = true;          // set True until we checkpath or make first attempt to write to log
            _errorMsg = "no errors";

            _fullPathChecked = false;
        }

        // ACCESSORS
        public string RootPath
        {
            get { return _rootPath; }
            set { _rootPath = value; }
        }

        public string FolderName
        {
            get { return _folderName; }
            set { _folderName = value; }
        }

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public bool DebugActive
        {
            get { return _debugActive; }
            set { _debugActive = value; }
        }// defaults False if not set

        public bool ErrorFlag
        {
            get { return _errorFlag; }
            set { _errorFlag = value; }
        }

        public string ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; }
        }

        public string FullPath
        {
            get
            {
                var path = System.IO.Path.Combine(_rootPath, _folderName);
                return System.IO.Path.Combine(path, _fileName);
            }
        }// readOnly

        public bool FullPathChecked
        {
            get { return _fullPathChecked; }
            set { _fullPathChecked = value; }
        }
    }
}