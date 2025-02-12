using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public static class OBJ
    {
        public static void load(string path, List<vec3> outVertices, List<Vector2> outTextures, List<vec3> outNormals)
        {
            string buf;
            List<vec3> vertices = new List<Vector3>(), normals = new List<Vector3>();
            List<Vector2> textures = new List<Vector2>();
            List<uint> verticesInd = new List<uint>(), normalsInd = new List<uint>(), texturesInd = new List<uint>();
            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.Peek() > -1)
                {
                    buf = sr.ReadLine().Trim();
                    while (buf.Contains("  ")) { buf = buf.Replace("  ", " "); }
                    string[] tmp = buf.Split(' ');

                    if (tmp[0] == "v")
                    {
                        vertices.Add(ParseVector3(tmp));
                    }
                    else if (tmp[0] == "vt")
                    {
                        textures.Add(ParseVector2(tmp));
                    }
                    else if (tmp[0] == "vn")
                    {
                        normals.Add(ParseVector3(tmp));
                    }
                    else if (tmp[0] == "f")
                    {
                        for (int i = 1; i < 4; i++)
                        {
                            string[] tmp2 = tmp[i].Split("/");
                            verticesInd.Add(uint.Parse(tmp2[0]));
                            texturesInd.Add(uint.Parse(tmp2[1]));
                            normalsInd.Add(uint.Parse(tmp2[2]));
                        }
                        if (tmp.Length > 3) 
                        {
                            for (int i = 3; i < tmp.Length; i++)
                            {
                                string[] tmp2 = tmp[1].Split("/");
                                verticesInd.Add(uint.Parse(tmp2[0]));
                                texturesInd.Add(uint.Parse(tmp2[1]));
                                normalsInd.Add(uint.Parse(tmp2[2]));

                                tmp2 = tmp[i - 1].Split("/");
                                verticesInd.Add(uint.Parse(tmp2[0]));
                                texturesInd.Add(uint.Parse(tmp2[1]));
                                normalsInd.Add(uint.Parse(tmp2[2]));

                                tmp2 = tmp[i].Split("/");
                                verticesInd.Add(uint.Parse(tmp2[0]));
                                texturesInd.Add(uint.Parse(tmp2[1]));
                                normalsInd.Add(uint.Parse(tmp2[2]));
                            }
                        }
                    }
                }

                sr.Close();
            }
            for (int i = 0; i < verticesInd.Count; i++)
            {
                vec3 vertex = vertices[(int)(verticesInd[i] - 1)];
                outVertices.Add(vertex);
                Vector2 texture = textures[(int)(texturesInd[i] - 1)];
                outTextures.Add(texture);
                vec3 normal = normals[(int)(normalsInd[i] - 1)];
                outNormals.Add(normal);
            }
        }

        public static vec3 ParseVector3(string[] array)
        {
            return new vec3(float.Parse(array[1].Replace(".", ",")), float.Parse(array[2].Replace(".", ",")), float.Parse(array[3].Replace(".", ",")));
        }

        public static Vector2 ParseVector2(string[] array)
        {
            return new Vector2(float.Parse(array[1].Replace(".", ",")), float.Parse(array[2].Replace(".", ",")));
        }

    }
}
