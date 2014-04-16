using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreFramework.Models;

namespace CoreFramework.Extractors
{
    interface iExtractor
    {
       ComplexTypeModel extract(ComplexTypeModel compTypeAtHand, Type classAtHand);
    }
}
