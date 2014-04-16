using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Configuration;

using CoreFramework.Processor;
using CoreFramework.Models;
using CoreFramework.Generators;

namespace CoreFramework.Modes
{
    public class ServerModeDelegate
    {

        public static Assembly generatedAssemblyAtHand;

        public static bool doConfiguration()
        {
            Console.WriteLine("In server mode");
            string dllPath = ConfigurationManager.AppSettings["dllFilePath"];
            Console.WriteLine("DLL File is : " + dllPath);
            DLLModel dLLAtHand = null;
            if (dllPath != null && dllPath.EndsWith(".dll"))
            {
                dLLAtHand = new DLLModel(dllPath);
                dLLAtHand = dLLAtHand.processDll(dLLAtHand);
            }
            else
            {
                Console.WriteLine("DLL Path information is not properly configured.");
                Environment.Exit(0);
            }

            dLLAtHand = DLLProcessor.populateComplexTypes(dLLAtHand);
            dLLAtHand = DLLProcessor.populateUserSelectedClassesAndMethods(dLLAtHand);

            Assembly assemblyAtHand = new ControllerGenerator().generateControllersAndDll(dLLAtHand);
            if (assemblyAtHand != null)
            {
                generatedAssemblyAtHand = assemblyAtHand;
                dLLAtHand.generateXml();
                return true;
            }

            return false;
        }
    }
}
