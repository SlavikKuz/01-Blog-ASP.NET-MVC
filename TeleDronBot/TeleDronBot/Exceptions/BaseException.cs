using System;
using System.Collections.Generic;
using System.Text;

namespace TeleDronBot.Exceptions
{
    class BaseException : Exception
    {
        public BaseException(string message, Exception ex) : base(message, ex) { }
        public BaseException(string message) : base(message) { }
        public BaseException() { }
    }
}
