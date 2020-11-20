using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace GeoDataMaster
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine
            ("Ведите пороль:"
            );

            switch (Console.ReadLine().GetHashCode())
            {
                case 1225954449:
                    enter();
                    break;
                default:
                    enter();
                    break;
            }
        }

        static public void enter()
        {
            Console.WriteLine
            ("Функции парсера:\nПарсить гео: pars\nПодготовка для парсера файлов: copy"
            );

            switch (Console.ReadLine())
            {
                case "pars":
                    Terain terain = new Terain();
                    terain.TerainConvert();
                    break;

                case "copy":
                    BrushList1 ddfd = new BrushList1();
                    ddfd.Brush();
                    break;
            }
            Console.ReadLine();
        }
    }

    public class BrushList
    {
        public List<string> name_model = new List<string>();
        public void Brush(string geo, string location)
        {
            if (File.Exists("levels\\" + location + "\\brush.lst"))
            {
                FileStream meshsgeo = new FileStream(geo + ".geo", FileMode.Append, FileAccess.Write);
                BinaryWriter meshsgeos = new BinaryWriter(meshsgeo);

                byte[] byter = new byte[1];
                byter[0] = 0;
                List<string> name_modeles = new List<string>();

                FileStream brush = new FileStream("levels\\" + location + "\\brush.lst", FileMode.Open, FileAccess.Read);
                BinaryReader brush_b = new BinaryReader(brush);

                brush_b.ReadBytes(3);//нужно добавить сигнатуру(проверку)

                int dw1 = brush_b.ReadInt32();
                int meshDataBlockSz = brush_b.ReadInt32();
                int titlesCount = brush_b.ReadInt32();

                for (int i = 0; i < titlesCount; i++)
                {
                    int nameLen = brush_b.ReadInt32();
                    brush_b.ReadBytes(nameLen - 4);
                }

                int meshInfoCount = brush_b.ReadInt32();

                for (int i = 0; i < meshInfoCount; i++)
                {
                    brush_b.ReadBytes(4);

                    byte[] fileNameBytes = brush_b.ReadBytes(128);
                    String fileName = new String(Encoding.ASCII.GetChars(fileNameBytes));
                    Regex rgx = new Regex(Encoding.ASCII.GetString(byter));
                    string fileNames = rgx.Replace(fileName, string.Empty);
                    name_modeles.Add(fileNames);

                    if (name_model.IndexOf(fileNames) == -1 && File.Exists(fileNames) && fileNames.IndexOf(".cga") == -1)
                    {
                        name_model.Add(fileNames);
                    }
                    ///////////////////////
                    //Console.WriteLine(fileNames);
                    //Reader(fileNames+"\n");
                    ///////////////////////

                    brush_b.ReadBytes(4 * 7);
                }

                int meshDataCount = brush_b.ReadInt32();

                for (int i = 0; i < meshDataCount; i++)
                {
                    brush_b.ReadBytes(4 * 2);

                    int meshIdx = brush_b.ReadInt32();

                    brush_b.ReadBytes(4 * 3);

                    float[] mesh_matrix = new float[3 * 4];

                    for (int j = 0; j < mesh_matrix.Length; j++)
                    {
                        mesh_matrix[j] = brush_b.ReadSingle();
                    }

                    brush_b.ReadBytes(4 * (meshDataBlockSz - 10));
                    ///////////////////////
                    //Console.WriteLine("meshIdx " + meshIdx + "\n" + "matrix "+ mesh_matrix[0] + " matrix " + mesh_matrix[1]  + " matrix " + mesh_matrix[2] + "\n" + "matrix " + mesh_matrix[3] + " matrix " + mesh_matrix[4] + " matrix " + mesh_matrix[5] + "\n" + "matrix " + mesh_matrix[6] + " matrix " + mesh_matrix[7] + " matrix " + mesh_matrix[8] + "\n" + "matrix " + mesh_matrix[9] + " matrix " + mesh_matrix[10] + " matrix " + mesh_matrix[11] + "\n");
                    //Reader("meshIdx " + meshIdx + "\n" + "matrix " + mesh_matrix[0] + " matrix " + mesh_matrix[1] + " matrix " + mesh_matrix[2] + "\n" + "matrix " + mesh_matrix[3] + " matrix " + mesh_matrix[4] + " matrix " + mesh_matrix[5] + "\n" + "matrix " + mesh_matrix[6] + " matrix " + mesh_matrix[7] + " matrix " + mesh_matrix[8] + "\n" + "matrix " + mesh_matrix[9] + " matrix " + mesh_matrix[10] + " matrix " + mesh_matrix[11] + "\n");
                    ///////////////////////


                    string modelName = name_modeles[meshIdx];
                    if(modelName.IndexOf(".cga") == -1)
                    {
                        meshsgeos.Write(Convert.ToInt16(modelName.Length));
                        meshsgeos.Write(Encoding.Default.GetBytes(modelName));
                        //Reader(modelName+"\n");

                        for (int s = 0; s < 3; s++)
                        {
                            meshsgeos.Write(Convert.ToSingle(mesh_matrix[s * 4 + 3]));
                            //Reader(Convert.ToString(mesh_matrix[s * 4 + 3])+"\n");
                        }

                        for (int s = 0; s < 3; s++)
                        {
                            meshsgeos.Write(Convert.ToSingle(mesh_matrix[s * 4 + 0]));
                            meshsgeos.Write(Convert.ToSingle(mesh_matrix[s * 4 + 1]));
                            meshsgeos.Write(Convert.ToSingle(mesh_matrix[s * 4 + 2]));
                        }

                        meshsgeos.Write(Convert.ToSingle(1.0));
                    }
                }
            }
        }

        public void Reader(string buff_S)
        {
            byte[] buff = Encoding.Default.GetBytes(buff_S);
            using (FileStream fs = new FileStream("1.txt", FileMode.Append, FileAccess.Write))
            {
                fs.Write(buff, 0, buff.Length);
                fs.Flush();
                fs.Close();
            }
        }
    }


    public class BrushList1
    {
        public List<string> name_model = new List<string>();
        public void Brush()
        {
            string fileNam = Environment.CurrentDirectory + "\\WorldId.xml";
            XmlDocument document = new XmlDocument();
            document.Load(fileNam);
            XmlNodeList nodeList = document.GetElementsByTagName("data");

            foreach (XmlNode xmlnode in nodeList)
            {

                if (File.Exists("levels\\" + xmlnode.InnerText + "\\brush.lst"))
                {
                    byte[] byter = new byte[1];
                    byter[0] = 0;

                    FileStream brush = new FileStream("levels\\" + xmlnode.InnerText + "\\brush.lst", FileMode.Open, FileAccess.Read);
                    BinaryReader brush_b = new BinaryReader(brush);

                    brush_b.ReadBytes(3);//нужно добавить сигнатуру(проверку)

                    int dw1 = brush_b.ReadInt32();
                    int meshDataBlockSz = brush_b.ReadInt32();
                    int titlesCount = brush_b.ReadInt32();

                    for (int i = 0; i < titlesCount; i++)
                    {
                        int nameLen = brush_b.ReadInt32();
                        brush_b.ReadBytes(nameLen - 4);
                    }

                    int meshInfoCount = brush_b.ReadInt32();

                    for (int i = 0; i < meshInfoCount; i++)
                    {
                        brush_b.ReadBytes(4);

                        byte[] fileNameBytes = brush_b.ReadBytes(128);
                        String fileName = new String(Encoding.ASCII.GetChars(fileNameBytes));
                        Regex rgx = new Regex(Encoding.ASCII.GetString(byter));
                        string fileNames = rgx.Replace(fileName, string.Empty);

                        if (name_model.IndexOf(fileNames) == -1 && File.Exists(fileNames))
                        {
                            name_model.Add(fileNames);
                        }

                        brush_b.ReadBytes(4 * 7);
                    }
                }
            }

            foreach(string nams in name_model)
            {
                if(nams.IndexOf(".cga") == -1)
                {
                    ///////////////////////
                    Console.WriteLine(nams);
                    Directory.CreateDirectory(@"Fast\" + Path.GetDirectoryName(nams));
                    File.Copy(nams, @"Fast\" + nams, true);
                    Reader(nams + "\n");
                    ///////////////////////
                }
                else
                {
                    Console.WriteLine(nams);
                    Directory.CreateDirectory(@"models\" + Path.GetDirectoryName(nams));
                    File.Copy(nams, @"models\" + nams, true);
                    Reader(nams + "\n");
                }
            }
        }

        public void Reader(string buff_S)
        {
            byte[] buff = Encoding.Default.GetBytes(buff_S);
            using (FileStream fs = new FileStream("1.txt", FileMode.Append, FileAccess.Write))
            {
                fs.Write(buff, 0, buff.Length);
                fs.Flush();
                fs.Close();
            }
        }
    }
}
