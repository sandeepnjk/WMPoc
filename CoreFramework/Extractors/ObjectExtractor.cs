using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CoreFramework.Models;
using System.Reflection;

namespace CoreFramework.Extractors
{
    class ObjectExtractor : iExtractor
    {
        public ComplexTypeModel extract(ComplexTypeModel compTypeAtHand, Type classAtHand)
        {
            iExtractor[] extractors = new iExtractor[] { new FieldExtractor()
                , new ConstructorExtractor(), new StructExtractor()};

            for (int i = 0; i < extractors.Length; i++)
            {
                compTypeAtHand = extractors[i].extract(compTypeAtHand, classAtHand);
            }
            return compTypeAtHand;
        }
    }
}
