using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

using CoreFramework.Models;

namespace CoreFramework.Builders
{
    interface iBuilder
    {
        T build<T>(ComplexTypeModel compType, IDictionary<string, object> jsonAsObject, T objectAtHand);
    }
}
