using System;
using System.Text;

namespace NETCoreRemoveServices.Core.Reflection
{
    internal class JException : Exception
    {
        /// <summary>
        /// Exception type.
        /// </summary>
        public JTypeInfo ParameterType { get; set; }

        /// <summary>
        /// Exception message.
        /// </summary>
        public new string Message { get; set; }

        /// <summary>
        /// Inner Exception information
        /// </summary>
        public JException JInnerException { get; set; }

        /// <summary>
        /// Initialize an empty JException object.
        /// </summary>
        public JException() { }

        /// <summary>
        /// Initialize JException object using the provided Exception
        /// </summary>
        /// <param name="e"></param>
        public JException(Exception e)
        {
            ParameterType = new JTypeInfo(e.GetType());
            Message = e.Message;
            if (e.InnerException != null)
            {
                JInnerException = new JException(e.InnerException);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Exception was thrown by the target remote machine:");
            JException currentException = this;
            do
            {
                sb.AppendLine($"{currentException.ParameterType.FullName} : {currentException.Message}");
                currentException = currentException.JInnerException;
            }
            while (currentException != null);
            return sb.ToString();
        }
    }
}