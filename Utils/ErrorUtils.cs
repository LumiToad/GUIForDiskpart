using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Diagnostics;

namespace GUIForDiskpart.Utils
{
    public static class ErrorUtils
    {
        private const string VIRTUAL_METHOD_REQUIRED = "Virtual Method METHOD not implemented in File: FILE, Line LINE!";

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
