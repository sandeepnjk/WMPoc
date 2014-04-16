using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using CoreFramework.Models;

namespace CoreFramework.Extractors
{
    class StructExtractor : iExtractor
    {

        public ComplexTypeModel extract(ComplexTypeModel compTypeAtHand, Type classAtHand)
        {


            return compTypeAtHand;
        }
    }
}
