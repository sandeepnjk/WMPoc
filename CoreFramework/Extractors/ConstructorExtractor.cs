using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using CoreFramework.Models;

namespace CoreFramework.Extractors
{
    class ConstructorExtractor : iExtractor
    {
        public ComplexTypeModel extract(ComplexTypeModel compTypeAtHand, Type classAtHand)
        {
            ConstructorModel constructorAtHand = new ConstructorModel(classAtHand.Name);
            constructorAtHand.setNoArgConstructor(true);
            ConstructorInfo constructorInfo = classAtHand.GetConstructor(new Type[] { });

            //Class doesnt have no-arg constructor, extract other constructors
            constructorAtHand.setNoArgConstructor(false);
            ConstructorInfo[] constructors = classAtHand.GetConstructors();

            compTypeAtHand.setConstructorModel(constructorAtHand);            
            
            return compTypeAtHand;
        }

        private ComplexTypeModel extractIndividualConstructors(
                            ComplexTypeModel compTypeAtHand, ConstructorModel constructorAtHand, ConstructorInfo[] constructors)
        {
            for (int i = 0; i < constructors.Length; i++)
            {
                ConstructorInfo constructorInfo = constructors[i];
            }
                return compTypeAtHand;
        }

        private ConstructorModel buildIndividualConstructor(ConstructorModel constructorAtHand,
                                   ConstructorInfo constructorInfoAtHand)
        {
            //MethodModel methodConstructorAtHand = new MethodModel()
            return constructorAtHand;
        }
    }
}
