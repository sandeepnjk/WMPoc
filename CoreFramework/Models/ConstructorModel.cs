using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreFramework.Models
{
    public class ConstructorModel
    {
        private int numberOfConstructors;
        private string className;
        private bool boolHasNoArgConstructor;
        private HashSet<MethodModel> allConstructorsForClass = new HashSet<MethodModel>();

        public ConstructorModel(string className)
        {
            this.className = className;
        }

        public int getNumberOfConstructors()
        {
            return this.numberOfConstructors;
        }

        public void setNumberOfConstructors(int numberOfConstructors)
        {
            this.numberOfConstructors = numberOfConstructors;
        }

        public bool hasNoArgConstructor()
        {
            return this.boolHasNoArgConstructor;
        }

        public void setNoArgConstructor(bool boolHasNoArgConstructor)
        {
            this.boolHasNoArgConstructor = boolHasNoArgConstructor;
        }

        public string getClassOfContructor()
        {
            return this.className;
        }
    }
}
