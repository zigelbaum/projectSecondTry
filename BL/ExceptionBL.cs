using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    internal class ExceptionBL : Exception
    {
        public ExceptionBL()
        {
        }

        public ExceptionBL(string message) : base(message)
        {
        }

        public ExceptionBL(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExceptionBL(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}