using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GeoDataMaster
{
    class Terain
    {
        static string fileNames = Environment.CurrentDirectory + "\\WorldId.xml";
        public void TerainConvert()
        {
            BrushList bry = new BrushList();

            XmlDocument document = new XmlDocument();
            document.Load(fileNames);
            XmlNodeList nodeList = document.GetElementsByTagName("data");

            foreach (XmlNode xmlnode in nodeList)
            {
                if (File.Exists("levels\\" + xmlnode.InnerText + "\\terrain\\land_map.h32"))
                {
                    FileStream fso = new FileStream("levels\\" + xmlnode.InnerText + "\\terrain\\land_map.h32", FileMode.Open, FileAccess.Read);
                    BinaryReader fds = new BinaryReader(fso);

                    FileStream fs = new FileStream(xmlnode.Attributes["id"].InnerText + ".geo", FileMode.OpenOrCreate, FileAccess.Write);
                    BinaryWriter fda = new BinaryWriter(fs);
                    byte yyt = 1;
                    int yyr = Convert.ToInt32(fso.Length / 3);
                    fda.Write(yyt);
                    fda.Write(yyr);
                    int i;
                    for (i = 0; fso.Length / 3 > i; i++)
                    {
                        fda.Write(fds.ReadInt16());
                        fds.ReadByte();
                    }
                    fda.Close();
                }
                else
                {
                    FileStream fs = new FileStream(xmlnode.Attributes["id"].InnerText + ".geo", FileMode.OpenOrCreate, FileAccess.Write);
                    BinaryWriter fda = new BinaryWriter(fs);
                    byte yyt = 1;
                    fda.Write(yyt);
                    fda.Write(yyt);
                    fda.Write(yyt);
                    fda.Close();
                }
                
                bry.Brush(xmlnode.Attributes["id"].InnerText, xmlnode.InnerText);

                Console.WriteLine("OK- " + xmlnode.Attributes["id"].InnerText);
                Console.WriteLine(bry.name_model.Count);
            }

            foreach (string name in bry.name_model)
            {
                MeshsGeo ghd = new MeshsGeo();

                Meshs rtry = new Meshs();

                if (File.Exists(name.Replace(".cgf", ".mesh")))
                {
                rtry.ReadMeshs(name);
                }
                else
                {
                ghd.ComMeshs(name);
                }

                Console.WriteLine("OK");
            }
        }
    }
}
