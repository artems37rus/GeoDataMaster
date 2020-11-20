using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoDataMaster
{
    class Meshs
    {
        public void ReadMeshs(string path)
        {
            MeshsGeo mes = new MeshsGeo();

            FileStream meshs = new FileStream("meshs.geo", FileMode.Append, FileAccess.Write);
            BinaryWriter meshsgeo = new BinaryWriter(meshs);

            List<Vector3> vertex = new List<Vector3>();
            List<MeshFace> faces = new List<MeshFace>();

            StreamReader sr = new StreamReader(path.Replace(".cgf", ".mesh"));

            string line;
            int cor = 0;
            bool cors = false;

            meshsgeo.Write(Convert.ToInt16(path.Length));
            meshsgeo.Write(Encoding.Default.GetBytes(path));
            meshsgeo.Write(Convert.ToInt16(setMeshFile(path)));

            while (!sr.EndOfStream)
            {
                Vector3 vector = new Vector3();
                MeshFace meshfa = new MeshFace();

                line = sr.ReadLine();
                
                int inde = line.IndexOf("(");
                int inder = line.IndexOf(")");
                if(inde != -1 && inder != -1)
                {
                    if(cors == false) { cor++; cors = true; }

                    string got = line.Substring(inde+1, inder-1);
                    int xind = got.IndexOf(",");
                    string gotx = got.Substring(0, xind);
                    if (cor == 1) { vector.x = Single.Parse(gotx.Replace(".", ",")); }
                    if (cor == 3) { meshfa.v0 = Convert.ToInt32(gotx); }
                    /////////////////////////////X
                    string goty = got.Substring(xind+1);
                    int yind = goty.IndexOf(",");
                    string gotyy = goty.Substring(0, yind);
                    if (cor == 1) { vector.y = Single.Parse(gotyy.Replace(".", ",")); }
                    if (cor == 3) { meshfa.v1 = Convert.ToInt32(gotyy); }
                    /////////////////////////////y
                    string gotz = goty.Substring(yind + 1);
                    if (cor == 1) { vector.z = Single.Parse(gotz.Replace(".", ",")); }
                    if (cor == 3) { meshfa.v2 = Convert.ToInt32(gotz); }
                    //////////////////////////////z
                    if (cor == 1) { vertex.Add(vector); }
                    if (cor == 3) { faces.Add(meshfa); }
                    //Console.WriteLine(got + "/"+ cor);
                }
                else
                {
                    cors = false;
                    if (cor == 3)
                    {
                        cor = 0;
                        meshsgeo.Write(Convert.ToInt16(vertex.Count));
                        for (int i = 0; vertex.Count > i; i++)
                        {
                            meshsgeo.Write(Convert.ToSingle(vertex[i].x / 100.0));
                            meshsgeo.Write(Convert.ToSingle(vertex[i].y / 100.0));
                            meshsgeo.Write(Convert.ToSingle(vertex[i].z / 100.0));
                        }
                        meshsgeo.Write(Convert.ToInt32(faces.Count * 3));
                        for (int i = 0; faces.Count > i; i++)
                        {
                            meshsgeo.Write(Convert.ToInt16(faces[i].v0));
                            meshsgeo.Write(Convert.ToInt16(faces[i].v1));
                            meshsgeo.Write(Convert.ToInt16(faces[i].v2));
                        }
                        meshsgeo.Write(Convert.ToInt16(2309));
                        vertex.Clear();
                        faces.Clear();
                        //return;
                    }
                }
            }
            cors = false;
            if (cor == 3)
            {
                cor = 0;
                meshsgeo.Write(Convert.ToInt16(vertex.Count));
                for (int i = 0; vertex.Count > i; i++)
                {
                    meshsgeo.Write(Convert.ToSingle(vertex[i].x / 100.0));
                    meshsgeo.Write(Convert.ToSingle(vertex[i].y / 100.0));
                    meshsgeo.Write(Convert.ToSingle(vertex[i].z / 100.0));
                }
                meshsgeo.Write(Convert.ToInt32(faces.Count * 3));
                for (int i = 0; faces.Count > i; i++)
                {
                    meshsgeo.Write(Convert.ToInt16(faces[i].v0));
                    meshsgeo.Write(Convert.ToInt16(faces[i].v1));
                    meshsgeo.Write(Convert.ToInt16(faces[i].v2));
                }
                meshsgeo.Write(Convert.ToInt16(2309));
                vertex.Clear();
                faces.Clear();
                //return;
            }
            sr.Close();
            meshsgeo.Close();
        }

        public int setMeshFile1(string path)
        {
            FileStream meshfile = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader meshfile_r = new BinaryReader(meshfile);

            List<int> numbers = new List<int>();

            meshfile_r.ReadBytes(16);

            int tableOffset = meshfile_r.ReadInt32();
            meshfile_r.BaseStream.Seek(tableOffset, 0);

            int chunksCount = meshfile_r.ReadInt32();
            for (int i = 0; i < chunksCount; i++)
            {
                int chunkType = meshfile_r.ReadInt32();

                if (chunkType == -859045888)
                {
                    int chunkVersion = meshfile_r.ReadInt32();
                    int chunkOffset = meshfile_r.ReadInt32();
                    int chunkId = meshfile_r.ReadInt32();
                    numbers.Add(chunkOffset);
                }
                else
                {
                    meshfile_r.ReadBytes(4 * 3);
                }
            }
            meshfile_r.Close();
            return numbers.Count;
        }


        public int setMeshFile(string path)
        {
            BrushList ggfe = new BrushList();
            ggfe.Reader(path);
            StreamReader sr = new StreamReader(path.Replace(".cgf", ".mesh"));

            string line;
            int cor = 0;
            int fg = 0;
            bool cors = false;

            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();

                int inde = line.IndexOf("(");
                int inder = line.IndexOf(")");
                if (inde != -1 && inder != -1)
                {
                    if (cors == false) { cor++; cors = true; }

                    string got = line.Substring(inde + 1, inder - 1);
                    int xind = got.IndexOf(",");
                    string gotx = got.Substring(0, xind);
                    /////////////////////////////X
                    string goty = got.Substring(xind + 1);
                    int yind = goty.IndexOf(",");
                    string gotyy = goty.Substring(0, yind);
                    /////////////////////////////y
                    string gotz = goty.Substring(yind + 1);
                    /////////////////////////////z
                }
                else
                {
                    cors = false;
                    if(cor == 3)
                    {
                        cor = 0;
                        fg++;
                    }
                }
            }
            if (cors)
            {
                fg++;
            }
            sr.Close();
            return fg;
        }
    }
}
