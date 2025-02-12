using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Project
{
    public static class Sphere
    {
        public static void genSphere(
        List<uint> outIndexes, //Выходной массив индексов
        List<vec3> outVertices, //Выходной массив координат
        List<Vector2> outTextures, //Выходной массив текстурных координат
        List<vec3> outNormals, //Выходной массив векторов нормалей
        float radius, //Радиус сферы
        int sectorCount, //Число меридианов
        int stackCount //Число параллелей
        )
        {
            //Очистка выходных массивов
            outIndexes.Clear();
            outNormals.Clear();
            outVertices.Clear();
            outTextures.Clear();
            //Вспомогательные переменные для хранения промежуточных данных
            float x, y, z, xy;
            float nx, ny, nz, lengthInv = 1.0f / radius;
            float s, t;
            float sectorStep = 2.0f * MathHelper.Pi / sectorCount;
            float stackStep = MathHelper.Pi / stackCount;
            float sectorAngle, stackAngle;
        //Цикл по каждой параллели
        for (int i = 0; i <= stackCount; ++i)
        {
                stackAngle = MathHelper.Pi / 2 - i * stackStep; // начиная от PI/2 и до -PI/2
                xy = radius * MathF.Cos(stackAngle); // r * cos(u)
                z = radius * MathF.Sin(stackAngle); // r * sin(u)
                                               // На каждую параллель добавляется (sectorCount+1) вершин
                                               // для первой и последней совпадают позиция и нормаль, но отличаются текстурные координаты
                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep; // от 0 до 2PI
                                                  // высчитываются координаты (x, y, z)
                    x = xy * MathF.Cos(sectorAngle); // r * cos(u) * cos(v)
                    y = xy * MathF.Sin(sectorAngle); // r * cos(u) * sin(v)
                    vec3 vert;
                    vert.X = x;
                    vert.Y = y;
                    vert.Z = z;
                    outVertices.Add( vert );
                    // высчитывается вектор нормали (nx, ny, nz)
                    nx = x * lengthInv;
                    ny = y * lengthInv;
                    nz = z * lengthInv;
                    vec3 norm;
                    norm.X = nx;
                    norm.Y = ny;
                    norm.Z = nz;
                    outNormals.Add(norm);
                    // высчитываются текстурные координаты (s, t) в диапазоне [0, 1]
                    s = (float)j / sectorCount;
                    t = (float)i / stackCount;
                    Vector2 vt;
                    vt.X = s;
                    vt.Y = t;
                    outTextures.Add(vt);
                }
                //Но координат мало - нужен порядок обхода, т.е. индексы
                int k1, k2;
                for (int k = 0; k < stackCount; ++k)
                {
                    k1 = k * (sectorCount + 1); // начало текущего меридиана
                    k2 = k1 + sectorCount + 1; // начало следующего меридиана
                    for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                    {
                        // Для первой и последней параллели по 1 треугольнику, для остальных – по два
                    // k1 => k2 => k1+1
                        if (k != 0)
                        {
                            outIndexes.Add((uint)k1);
                            outIndexes.Add((uint)k2);
                            outIndexes.Add((uint)k1 + 1);
                        }
                        // k1+1 => k2 => k2+1
                        if (k != (stackCount - 1))
                        {
                            outIndexes.Add((uint)k1 + 1);
                            outIndexes.Add((uint)k2);
                            outIndexes.Add((uint)k2 + 1);
                        }
                    }
                }
        }
        }
    }
}
