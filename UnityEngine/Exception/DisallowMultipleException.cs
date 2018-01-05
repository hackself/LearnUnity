// -----------------------------------------------------------------
// File:    DisallowMultipleException
// Author:  ruanban
// Date:    1/5/2018 2:44:56 PM
// Description:
//      
// -----------------------------------------------------------------

using System;
namespace UnityEngine
{
    internal class DisallowMultipleException : Exception
    {
        private Type type;
        public DisallowMultipleException(Type type)
        {
            this.type = type;
        }

        public override string ToString()
        {
            return string.Format("{0} can only be add once!", this.type.Name);
        }
    }
}
