using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

using CoreFramework.Models;

namespace CoreFramework.Builders
{
    class ConstructorBuilder
    {
        public object build(ComplexTypeModel compType)
        {
            object objToBuild = null;
            ConstructorModel constructorModelAtHand = compType.getConstructorModel();
            if (constructorModelAtHand.hasNoArgConstructor())
            {
                objToBuild = Activator.CreateInstance(compType.getRepresentationalTypeFromAssembly());
            } else {
                //Logic to handle where no no-arg constructors exist
                objToBuild = FormatterServices.GetUninitializedObject(
                                         compType.getRepresentationalTypeFromAssembly());
            }
            return objToBuild;   
        }
    }
}
