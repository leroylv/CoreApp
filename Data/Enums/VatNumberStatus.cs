using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApp.Data.Enums
{

        public enum VatNumberStatus
        {
            /// <summary>
            /// Unknown
            /// </summary>
            Unknown = 0,

            /// <summary>
            /// Empty
            /// </summary>
            Empty = 10,

            /// <summary>
            /// Valid
            /// </summary>
            Valid = 20,

            /// <summary>
            /// Invalid
            /// </summary>
            Invalid = 30
        }
}
