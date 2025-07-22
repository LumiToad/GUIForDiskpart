using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Diagnostics;
using System.Windows;

namespace GUIForDiskpart.Utils
{
    public static class ErrorUtils
    {
        #region CompileTimeErrors

        private const string VIRTUAL_METHOD_REQUIRED = "Virtual Method METHOD not implemented in File: FILE, Line LINE!";

        #endregion

        #region UserErrors

        public const string MBR_ACTIVE_STATUS_WARNING = "Setting an MBR partition active or inactive can result in the computer no longer booting correctly!";

        #endregion UserErrors

        public static MessageBoxResult ShowMSGBoxWarning(string title, string text, MessageBoxButton btnType = MessageBoxButton.OKCancel)
        {
            return MessageBox.Show(
                text,
                title,
                btnType
                );
        }

        public static void NotImplementedException()
        {
            StackFrame? sf = new StackTrace(true).GetFrame(1);
            string error = VIRTUAL_METHOD_REQUIRED.Replace("METHOD", sf.GetMethod().Name);
            error = error.Replace("FILE", sf.GetFileName());
            error = error.Replace("LINE", sf.GetFileLineNumber().ToString());
            throw new NotImplementedException(error);
        }
    }
}
