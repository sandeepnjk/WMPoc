using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Configuration;

using CoreFramework.Models;
using CoreFramework.Extractors;
using CoreFramework.Utils;


namespace CoreFramework.Processor
{
    public class DLLProcessor
    {
        private enum RegKind
        {
            RegKind_Default = 0,
            RegKind_Register = 1,
            RegKind_None = 2
        }

        [DllImport("oleaut32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        private static extern void LoadTypeLibEx(String strTypeLibName, RegKind regKind,
            [MarshalAs(UnmanagedType.Interface)] out Object typeLib);

        public DLLModel processDLL(DLLModel dLLAtHand)
        {
            if (!dLLAtHand.isDLLManaged())
            {
                dLLAtHand = convertDLLToManaged(dLLAtHand);
            }

            Console.WriteLine(dLLAtHand.getFullyQualifiedPath());
            Assembly dLLAssembly = Assembly.LoadFile(dLLAtHand.getFullyQualifiedPath());

            foreach (Type memberType in dLLAssembly.GetTypes())
            {
                int i = 0;
                if (memberType.IsClass)
                {
                    i++;
                    ClassModel classAtHand = new ClassModel(memberType.FullName);
                    this.extractMethodsFromClass(classAtHand, memberType);
                    classAtHand.setDllThisClassBelongsTo(dLLAtHand);
                    dLLAtHand.getAllClassesInThisDll().Add(classAtHand.getClassName(), classAtHand);
                }
            }

            return dLLAtHand;
        }

        public static bool checkIfDLLIsManaged(String pathOfDLL)
        {
            using (Stream fileStream = new FileStream(pathOfDLL, FileMode.Open, FileAccess.Read))
            using (BinaryReader binaryReader = new BinaryReader(fileStream))
            {
                if (fileStream.Length < 64)
                {
                    return false;
                }

                //PE Header starts @ 0x3C (60). Its a 4 byte header.
                fileStream.Position = 0x3C;
                uint peHeaderPointer = binaryReader.ReadUInt32();
                if (peHeaderPointer == 0)
                {
                    peHeaderPointer = 0x80;
                }

                // Ensure there is at least enough room for the following structures:
                //     24 byte PE Signature & Header
                //     28 byte Standard Fields         (24 bytes for PE32+)
                //     68 byte NT Fields               (88 bytes for PE32+)
                // >= 128 byte Data Dictionary Table
                if (peHeaderPointer > fileStream.Length - 256)
                {
                    return false;
                }

                // Check the PE signature.  Should equal 'PE\0\0'.
                fileStream.Position = peHeaderPointer;
                uint peHeaderSignature = binaryReader.ReadUInt32();
                if (peHeaderSignature != 0x00004550)
                {
                    return false;
                }

                // skip over the PEHeader fields
                fileStream.Position += 20;

                const ushort PE32 = 0x10b;
                const ushort PE32Plus = 0x20b;

                // Read PE magic number from Standard Fields to determine format.
                var peFormat = binaryReader.ReadUInt16();
                if (peFormat != PE32 && peFormat != PE32Plus)
                {
                    return false;
                }

                // Read the 15th Data Dictionary RVA field which contains the CLI header RVA.
                // When this is non-zero then the file contains CLI data otherwise not.
                ushort dataDictionaryStart = (ushort)(peHeaderPointer + (peFormat == PE32 ? 232 : 248));
                fileStream.Position = dataDictionaryStart;

                uint cliHeaderRva = binaryReader.ReadUInt32();
                if (cliHeaderRva == 0)
                {
                    return false;
                }

                return true;
            }
        }

        private DLLModel convertDLLToManaged(DLLModel dLLModel)
        {
            Object typeLib;
            LoadTypeLibEx(dLLModel.getFullyQualifiedPath(), RegKind.RegKind_None, out typeLib);

            if (typeLib == null)
            {
                Console.WriteLine("LoadTypeLibEx failed.");
            }

            TypeLibConverter converter = new TypeLibConverter();
            ConversionEventHandler eventHandler = new ConversionEventHandler();

            string newDllName = dLLModel.getDllFileName()
                + "_converted.dll";
            AssemblyBuilder asm = converter.ConvertTypeLibToAssembly(typeLib, newDllName, 0, eventHandler, null, null, null, null);

            asm.Save(newDllName);
            DLLModel newdLLModel = new DLLModel(AppDomain.CurrentDomain.BaseDirectory + newDllName);
            return newdLLModel;
        }

        private ClassModel extractMethodsFromClass(ClassModel classAtHand, Type classType)
        {
            int i = 0;
            foreach (MethodInfo methodInfo in classType.GetMethods())
            {
                i++;
                string nameOfMethodAtHand = methodInfo.Name;
                string classThisMethodBelongsTo = classAtHand.getClassName();
                Type returnTypeOfMethodAtHand = methodInfo.ReturnType;
                MethodModel methodAtHand = new MethodModel(nameOfMethodAtHand,
                          classAtHand, returnTypeOfMethodAtHand);
                methodAtHand.setShortenedMethodName(classAtHand.getClassName()
                    + "." +methodAtHand.getMethodName() + "(");
                this.extractParametersFromMethod(methodAtHand, methodInfo);
                methodAtHand.setShortenedMethodName(methodAtHand.getShortenedName() + ")");
                classAtHand.getAllMethodsInThisClass().Add(methodAtHand.getShortenedName(), methodAtHand);
            }
            return classAtHand;
        }

        private MethodModel extractParametersFromMethod(MethodModel methodAtHand, MethodInfo methodInfo)
        {
            ParameterInfo[] paramInfo = methodInfo.GetParameters();
            for (int i = 0; i < paramInfo.Length; i++)
            {                
                string paramName = paramInfo[i].Name;
                Type paramType = paramInfo[i].ParameterType;
                string paramShortName = paramType + " " + paramName;
                ParameterModel paramAtHand = new ParameterModel(paramName, paramType, i + 1, methodAtHand);
                methodAtHand.getAllParametersInThisMethod().Add(i + 1, paramAtHand);
                if (i != paramInfo.Length - 1)
                {
                    paramShortName = paramShortName + ",";
                }
                methodAtHand.setShortenedMethodName(methodAtHand.getShortenedName() + paramShortName);                
            }
            methodAtHand.setNumberOfMethodParameters(paramInfo.Length);
            return methodAtHand;
        }

        public static DLLModel populateUserSelectedClassesAndMethods(DLLModel dllAtHand)
        {
            string userAddedClassesAndMethods = PropertyUtil.getUserSelectedMethods();
            if (userAddedClassesAndMethods == null)
            {
                Console.WriteLine("Incorrect configuration, methodsToAdd property not present.");
                Environment.Exit(0);
            }
            string[] colonSeparatedStrings = userAddedClassesAndMethods.Split(';');
            for (int i = 0; i < colonSeparatedStrings.Length; i++)
            {
                string methodAtHandString = colonSeparatedStrings[i];
                string tmpStringForSplit = new string(methodAtHandString.ToCharArray());
                string[] controllerAndActionAliases = tmpStringForSplit.Split(PropertyUtil.methodAndAliasSeparator);
                Boolean aliasesExist = false;
                string controllerAlias = null;
                string actionAlias = null;

                if (controllerAndActionAliases.Length > 1)
                {
                    //Aliases Exist, time to fetch them
                    aliasesExist = true;
                    string[] individualAliasNames = controllerAndActionAliases[1].Split(PropertyUtil.aliasSeparator);
                    controllerAlias = individualAliasNames[0];
                    actionAlias = individualAliasNames[1];
                    
                    //Lest we pass the method name along with the aliases
                    methodAtHandString = controllerAndActionAliases[0];
                }
                Dictionary<string, ClassModel> classesInDll = dllAtHand.getAllClassesInThisDll();
                foreach (KeyValuePair<string, ClassModel> pair in classesInDll)
                {
                    ClassModel classAtHand = pair.Value;
                    if (classAtHand.getAllMethodsInThisClass().ContainsKey(methodAtHandString))
                    {
                        int indexOfMethodToBeFetched = classAtHand.getAllMethodsInThisClass().IndexOfKey(methodAtHandString);
                        MethodModel methodAtHand = classAtHand.getAllMethodsInThisClass().ElementAt(indexOfMethodToBeFetched).Value;
                        if (aliasesExist)
                        {
                            methodAtHand.setAliasName(actionAlias);
                            classAtHand.setAliasName(controllerAlias);
                        }
                        classAtHand.getUserSelectedMethodsInThisClass().Add(methodAtHandString,
                            methodAtHand);
                        //Now add this class to the DLL's user selected methods, no duplicates
                        if (!dllAtHand.getUserSelectedClassesInThisDll().ContainsKey(classAtHand.getClassName()))
                        {
                            dllAtHand.getUserSelectedClassesInThisDll().Add(classAtHand.getClassName(), classAtHand);
                        }
                    }
                }
            }
            return dllAtHand;
        }

        public static DLLModel populateComplexTypes(DLLModel dllAtHand)
        {
            Assembly dLLAssembly = Assembly.LoadFile(dllAtHand.getFullyQualifiedPath());
            IEnumerable<TypeInfo> myTypes = dLLAssembly.DefinedTypes;
            Dictionary<string, ComplexTypeModel> complexTypesInDll = new Dictionary<string, ComplexTypeModel>();
            foreach (TypeInfo typeInfo in myTypes)
            {
                ComplexTypeModel complexTypeAtHand = createComplexModel(typeInfo.FullName, dLLAssembly);
                complexTypeAtHand.setDllFileThisTypeBelongsTo(dllAtHand.getDllFileName());
                complexTypesInDll = dllAtHand.getAllComplexTypesInDLL();
                if (!complexTypesInDll.ContainsKey(complexTypeAtHand.getActualTypeName()))
                {
                    complexTypesInDll.Add(complexTypeAtHand.getActualTypeName(), complexTypeAtHand);
                }
            }
            ObjectProcessor.getComplexTypesAcrossDlls().Add(dllAtHand.getDllFileName(), complexTypesInDll);
            return dllAtHand;
        }

        private static ComplexTypeModel createComplexModel(string typeName, Assembly dLLAssembly)
        {
            Type classType = dLLAssembly.GetType(typeName);
            ComplexTypeModel complexTypeAtHand = new ComplexTypeModel(typeName);
            iExtractor extractor = new ObjectExtractor();
            complexTypeAtHand = extractor.extract(complexTypeAtHand, classType);
            complexTypeAtHand.setRepresentationalTypeFromAssembly(classType);

            return complexTypeAtHand;
        }

        public class ConversionEventHandler : ITypeLibImporterNotifySink
        {
            public void ReportEvent(ImporterEventKind eventKind, int eventCode, string eventMsg)
            {
                // handle warning event here...
            }

            public Assembly ResolveRef(object typeLib)
            {
                // resolve reference here and return a correct assembly... 
                return null;
            }
        }
    }
}
