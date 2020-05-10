using System;
using System.Collections.Generic;
using System.Text;

namespace Contestant.Models
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.All, AllowMultiple = false)]
    public class CustomGeneratedCodeAttribute : Attribute
    {
    }
}
