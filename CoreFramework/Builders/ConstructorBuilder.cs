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
                Console.WriteLine("Constructor Info " + 
                    constructorModelAtHand.hasNoArgConstructor() + " " + 
                    constructorModelAtHand.getClassOfContructor() +
                    compType.getActualTypeName());
                objToBuild = Activator.CreateInstance(compType.getRepresentationalTypeFromAssembly());
            } else {
                //Logic to handle where no no-arg constructors exist
                objToBuild = FormatterServices.GetUninitializedObject(
                                         compType.getRepresentationalTypeFromAssembly());
                //var constructor = compType.getRepresentationalTypeFromAssembly().GetConstructor(Type.EmptyTypes);
                //constructor.Invoke(uninitializedObject, null);
            }
            Console.WriteLine(objToBuild);
            return objToBuild;   
        }
    }
}
