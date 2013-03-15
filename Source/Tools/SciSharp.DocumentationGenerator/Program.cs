using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using SciSharp;


namespace ScientificTools.DocumentationGenerator
{
    internal class Program
    {
        private static readonly Assembly[] Assemblies = new[]
                                                        {
                                                            typeof (Engine).Assembly,
                                                        };

        private static void Main(string[] args)
        {
            StreamWriter file = File.CreateText(@"..\..\..\Documentation\UserManual\Reference.tex");

            foreach (Assembly assembly in Assemblies)
                GenerateAssembly(assembly, file);

            file.WriteLine("%==================================================");

            file.Close();
        }

        private static void GenerateAssembly(Assembly assembly, StreamWriter file)
        {
            Console.WriteLine("Assembly: {0}", assembly.GetName().Name);

            file.WriteLine("%==================================================");
            //file.WriteLine(@" \assemblydef{" + assembly.GetName().Name + "}");

            foreach (Type type in assembly.GetExportedTypes().OrderBy(type => type.FullName))
                GenerateType(type, file);
        }

        private static void GenerateType(Type type, StreamWriter file)
        {
            Console.WriteLine(" Type: {0}", GetTypeName(type, true));

            if (type.IsEnum)
                GenerateEnumType(type, file);
            else
                GenerateDataType(type, file);
        }

        private static void GenerateEnumType(Type type, StreamWriter file)
        {
            file.WriteLine("%--------------------------------------------------");
            file.WriteLine("    Enum \\typedef{" + GetTypeName(type, true) + "}{" + GetFullName(type) + "}");

            file.WriteLine("        \\item[Members]");

            foreach (string name in Enum.GetNames(type))
                file.WriteLine("        \\item[] {0}", "\\code{" + name + "}");

            file.WriteLine("    \\typedefend");
        }

        private static string GetFullName(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() != type)
                return GetFullName(type.GetGenericTypeDefinition());

            return type.FullName;
        }

        private static void GenerateDataType(Type type, StreamWriter file)
        {
            file.WriteLine("%--------------------------------------------------");
            file.WriteLine("    \\typedef{" + GetTypeName(type, true) + "}{" + type.FullName + "}");

            if (type.BaseType != typeof (Object))
            {
                file.WriteLine("%- - - - - - - - - - - - - - - - - - - - - - - - - -");
                file.WriteLine("        \\item[Base] {0}", GetTypeReference(type.BaseType));
            }

            if (type.GetProperties().Length > 0)
            {
                file.WriteLine("%- - - - - - - - - - - - - - - - - - - - - - - - - -");
                file.WriteLine("        \\item[Properties]");

                foreach (PropertyInfo property in type.GetProperties().OrderBy(prop => prop.Name))
                    GenerateProperty(type, property, file);
            }

            if (type.GetMethods().Length > 0)
            {
                file.WriteLine("%- - - - - - - - - - - - - - - - - - - - - - - - - -");
                file.WriteLine("        \\item[Methods]");

                foreach (MethodInfo method in type.GetMethods().OrderBy(m => m.Name))
                    GenerateMethod(type, method, file);
            }

            file.WriteLine("    \\typedefend");
        }

        private static string GetTypeName(Type type, bool full)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() != type)
                return GetTypeName(type.GetGenericTypeDefinition(), full);

            string baseName = "";

            if (type.DeclaringType != null && !type.IsGenericParameter)
            {
                baseName = GetTypeName(type.DeclaringType, full);
            }

            string name = full && String.IsNullOrEmpty(baseName) ? type.FullName : type.Name;

            if (type.GetGenericArguments().Length > 0 && (type.DeclaringType == null || !type.DeclaringType.GetGenericArguments().Select(t => t.Name).SequenceEqual(type.GetGenericArguments().Select(t => t.Name))))
            {
                name = name.Substring(0, name.Length - 2) + "<{0}>".Formatted(String.Join(",", GetGenericTypeNames(type)));
            }

            if (!String.IsNullOrEmpty(baseName))
                name = baseName + "." + name;

            return name;
        }

        private static IEnumerable<string> GetGenericTypeNames(Type type)
        {
            foreach (Type argument in type.GetGenericArguments())
                yield return GetTypeName(argument, false);
        }

        private static void GenerateProperty(Type type, PropertyInfo property, StreamWriter file)
        {
            if (property.DeclaringType != type)
                return;

            if (property.Name == "_")
                return;

            Console.WriteLine("   Property: {0}", property.Name);

            file.WriteLine("        \\item[] {0} {2} {1}".Formatted(
                GetTypeReference(property.PropertyType),
                property.CanRead && property.CanWrite
                    ? "\\code{\\{ get; set \\}}"
                    : (property.CanRead ? "\\code{\\{ get; \\}}" : "\\code{\\{ set; \\}}"),
                "\\propertydef{" + type.Name + "}" + "{" + property.Name + "}"));
        }

        private static void GenerateMethod(Type type, MethodInfo method, StreamWriter file)
        {
            if (method.DeclaringType != type)
                return;

            if (method.IsSpecialName)
                return;

            Console.WriteLine("   Method: {0}", method.Name);

            file.WriteLine("        \\item[] {0} {1}({2})".Formatted(
                GetTypeReference(method.ReturnType),
                " \\methoddef{" + GetFullName(type) + "}" + "{" + method.Name + "}",
                string.Join(", ", GetParameters(method)))
                );
        }

        private static IEnumerable<string> GetParameters(MethodInfo method)
        {
            foreach (ParameterInfo parameterInfo in method.GetParameters())
                yield return "{0} {1}".Formatted(GetTypeReference(parameterInfo.ParameterType), "\\code{" + parameterInfo.Name + "}");
        }

        private static string GetTypeReference(Type type)
        {
            if (type == null)
                return "Not Available";

            string name = GetTypeName(type, false);

            if (name.EndsWith("&"))
                name = name.Substring(0, name.Length - 1);

            return Assemblies.Contains(type.Assembly) ? "\\typeref{" + name + "}{" + type.FullName + "}" : "\\code{" + name + "}";
        }
    }
}
