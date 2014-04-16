using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CoreFramework.Models
{
    public interface iModel
    {
        XmlReader generateXml();
    }
}
