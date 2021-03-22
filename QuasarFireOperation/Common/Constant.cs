using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuasarFireOperation.Common
{
    public class Constant
    {
        public enum StatusResponse
        {
            SUCCESS_SENT   = 1,
            ERROR_LOCATION,
            ERROR_MESSAGE,
            ERROR_PETICION
        }

        public enum NumberRow
        { 
            ONE_ROW = 1,
            TWO_ROWS,
            THREE_ROWS
        }

        public enum NumberSatellite
        { 
            ONE,
            TWO,
            THREE
        }


    }
}
