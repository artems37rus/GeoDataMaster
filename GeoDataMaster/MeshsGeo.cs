using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoDataMaster
{
    public class MeshsGeo
    {
        public List<int> setMeshFile(string path)
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
            return numbers;
        }

        public MeshData getMeshData(int idx, string path)
        {
            Console.WriteLine(idx);
            FileStream meshfile = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader meshfile_r = new BinaryReader(meshfile);
            meshfile_r.BaseStream.Seek(idx, 0);

            meshfile_r.ReadBytes(4 * 5);

            int verticesCount = meshfile_r.ReadInt32();

            meshfile_r.ReadBytes(4);

            int indicesCount = meshfile_r.ReadInt32();

            meshfile_r.ReadBytes(4);

            MeshData res = new MeshData();
            res.vertices = new Vector3[verticesCount];
            res.indices = new MeshFace[indicesCount];

            for (int i = 0; i < verticesCount; i++)
            {
                res.vertices[i] = new Vector3();
                res.vertices[i].x = meshfile_r.ReadSingle();
                res.vertices[i].y = meshfile_r.ReadSingle();
                res.vertices[i].z = meshfile_r.ReadSingle();

                meshfile_r.ReadBytes(4 * 3);
            }

            for (int i = 0; i < indicesCount; i++)
            {
                res.indices[i] = new MeshFace();
                res.indices[i].v0 = meshfile_r.ReadInt32();
                res.indices[i].v1 = meshfile_r.ReadInt32();
                res.indices[i].v2 = meshfile_r.ReadInt32();

                meshfile_r.ReadBytes(4 * 2);
            }
            meshfile_r.Close();
            return res;
        }

        public void ComMeshs(string path)
        {
            FileStream meshs = new FileStream("meshs.geo", FileMode.Append, FileAccess.Write);
            BinaryWriter meshsgeo = new BinaryWriter(meshs);
            List<int> mesh_tk = setMeshFile(path);

            foreach (int irid in mesh_tk)
            {
                MeshData mesh_data = getMeshData(irid, path);
                if(mesh_data.vertices.Length > 32767)
                {
                    meshsgeo.Close();
                    return;
                }
            }

            Int16 flfld = Convert.ToInt16(path.Length);
            meshsgeo.Write(flfld);

            meshsgeo.Write(Encoding.Default.GetBytes(path));

            meshsgeo.Write(Convert.ToInt16(mesh_tk.Count));

            foreach (int irid in mesh_tk)
            {
                MeshData mesh_data = getMeshData(irid, path);

                meshsgeo.Write(Convert.ToInt16(mesh_data.vertices.Length));
                foreach (Vector3 vector in mesh_data.vertices)
                {
                    meshsgeo.Write(Convert.ToSingle(vector.x / 100.0));
                    meshsgeo.Write(Convert.ToSingle(vector.y / 100.0));
                    meshsgeo.Write(Convert.ToSingle(vector.z / 100.0));
                }

                meshsgeo.Write(mesh_data.indices.Length * 3);
                foreach (MeshFace meshFace in mesh_data.indices)
                {
                    meshsgeo.Write(Convert.ToInt16(meshFace.v0));
                    meshsgeo.Write(Convert.ToInt16(meshFace.v1));
                    meshsgeo.Write(Convert.ToInt16(meshFace.v2));
                }
                meshsgeo.Write(Convert.ToInt16(2309));
            }
            meshsgeo.Close();
        }
    }
}
