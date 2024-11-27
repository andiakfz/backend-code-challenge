using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Shared
{
    public class Error
    {
        public int ErrorCode { get; }
        public string ErrorMessage { get; }
        public List<object>? ErrorDetails { get; set; }

        public Error(int code, string message)
        {
            ErrorCode = code;
            ErrorMessage = message;
        }
    }
}
