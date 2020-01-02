using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace autoPrint
{
    [Serializable]
    public class PrintingState
    {
        private static String FilePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ThomasEWillson", "AutoPrint", "printingState.xml");

        private static String FolderPath => Path.GetDirectoryName(FilePath);

        public Point CurrentLocation { get; set; } = new Point(0, 0);

        public int CurrentLineHeight { get; set; } = 0;

        public static PrintingState Load()
        {
            if (!File.Exists(FilePath))
            {
                return new PrintingState();
            }

            var serializer = new XmlSerializer(typeof(PrintingState));

            using(var file = File.OpenRead(FilePath))
            {
                return (PrintingState) serializer.Deserialize(file);
            }
        }

        public static void Save(PrintingState printingState)
        {
            Directory.CreateDirectory(FolderPath);
            var serializer = new XmlSerializer(typeof(PrintingState));

            using(var writer = new StreamWriter(FilePath, false, Encoding.UTF8))
            {
                serializer.Serialize(writer, printingState);
            }
        }
    }

}