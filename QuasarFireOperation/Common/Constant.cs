using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuasarFireOperation.Common
{
    public class Constant
    {
        public struct StatusResponse
        {
            public const int SUCCESS_SENT   = 1;
            public const int ERROR_LOCATION = 2;
            public const int ERROR_MESSAGE  = 3;
            public const int ERROR_PETICION = 3;
        }
    }
}
