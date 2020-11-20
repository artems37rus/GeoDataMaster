using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoDataMaster
{
    public class MeshData
    {
        public Vector3[] vertices;
        public MeshFace[] indices;
    }

    public class Vector3
    {
        public float x;
        public float y;
        public float z;
    }

    public class MeshFace
    {
        public int v0;
        public int v1;
        public int v2;
    }
}
