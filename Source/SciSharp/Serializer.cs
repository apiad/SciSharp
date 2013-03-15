using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;


namespace SciSharp
{
    public static class Serializer
    {
        #region Format enum

        public enum Format
        {
            Binary,
            Soap
        }

        #endregion

        private static string pathPrefix = "";

        public static string PathPrefix
        {
            get { return pathPrefix; }
            set { pathPrefix = value; }
        }

        public static bool OverrideExistingFiles { get; set; }

        public static void Serialize<T>(this T item, Stream stream, Format format)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            IFormatter formatter = format == Format.Binary
                                       ? new BinaryFormatter()
                                       : (IFormatter) new SoapFormatter();

            formatter.Serialize(stream, item);
        }

        public static void Serialize<T>(this T item, string filename, Format format)
        {
            if (filename == null)
                throw new ArgumentNullException("filename");

            if (!Directory.Exists(pathPrefix))
                Directory.CreateDirectory(pathPrefix);

            string path = Path.Combine(pathPrefix, filename);

            if (File.Exists(path) && !OverrideExistingFiles)
                throw new InvalidOperationException("The file '{0}' already exists.".Formatted(path));

            using (FileStream stream = File.OpenWrite(path))
                Serialize(item, stream, format);
        }

        public static T Deserialize<T>(Stream stream, Format format)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            IFormatter formatter = format == Format.Binary
                                       ? new BinaryFormatter()
                                       : (IFormatter) new SoapFormatter();

            return (T) formatter.Deserialize(stream);
        }

        public static T Deserialize<T>(string filename, Format format)
        {
            if (filename == null)
                throw new ArgumentNullException("filename");

            string path = Path.Combine(pathPrefix, filename);

            if (!File.Exists(path))
                throw new InvalidOperationException("The file '{0}' doesn't exist.".Formatted(path));

            using (FileStream stream = File.OpenRead(path))
                return Deserialize<T>(stream, format);
        }

        public static IContext OpenContext()
        {
            return new SerializerContext(pathPrefix);
        }

        #region Nested type: SerializerContext

        private class SerializerContext : Context
        {
            private readonly string previousPathPrefix;

            public SerializerContext(string pathPrefix)
            {
                previousPathPrefix = pathPrefix;
            }

            protected override void SafeEnd()
            {
                pathPrefix = previousPathPrefix;
            }
        }

        #endregion
    }
}
