using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreFramework.Builders
{
    class BuilderException : Exception
    {
        public BuilderException() : base() { }
        public BuilderException(string message) : base(message) {  }
        public BuilderException(string message, System.Exception inner) : base(message, inner) { }
    }
}
