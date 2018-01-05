// -----------------------------------------------------------------
// File:    DisallowMultipleComponentAttribute
// Author:  ruanban
// Date:    1/5/2018 2:33:41 PM
// Description:
//      
// -----------------------------------------------------------------
using System;

namespace UnityEngine
{
    [AttributeUsage(System.AttributeTargets.Class)]
    class DisallowMultipleComponentAttribute : Attribute
    {
    }
}
