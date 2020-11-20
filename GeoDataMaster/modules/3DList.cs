using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoDataMaster
{
    class _3DList
    {
        public List<Vectors3> vertices;
        public List<MeshFaces> indices;
    }
    public class Vectors3
    {
        public float x;
        public float y;
        public float z;
    }

    public class MeshFaces
    {
        public int v0;
        public int v1;
        public int v2;
    }
}
