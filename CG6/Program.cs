global using vec3 = OpenTK.Mathematics.Vector3;
using Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;
using Vector2 = OpenTK.Mathematics.Vector2;
using static System.Net.Mime.MediaTypeNames;
using System.CodeDom.Compiler;
using System.Xml.Linq;


namespace Project
{
    struct VBO {
        public int vboV;
        public int vboN;
        public int vboT;
    };
    public class Game : GameWindow
    {
        const int N = 12;

        VBO[] buffers = new VBO[N];
        int[] arrays = new int[N];

        int vao, vao_t, vao2;
        VBO vbo, vbo2;
        int vbo_v, vbo_n, vbo_t;
        vec3 position = new vec3(50.0f, 0.0f, 0.0f);
        vec3 front = new vec3(0.0f, 0.0f, 0.0f);
        vec3 up = new vec3(0.0f, 1.0f, 0.0f);
        Shader light_shader, object_shader, material_shader;
        Matrix4 model = Matrix4.Identity;
        Matrix4 trans = Matrix4.Identity;
        Matrix4 scale = Matrix4.Identity;
        Matrix4 rotation = Matrix4.Identity;
        vec3[] points, pointsO, normals, normalsO, points2 =
            {
            new vec3( -0.5f, -0.5f, -0.5f),
            new vec3( 0.5f, -0.5f, -0.5f ),
            new vec3( 0.5f,  0.5f, -0.5f ),
            new vec3( 0.5f,  0.5f, -0.5f ),
            new vec3(-0.5f,  0.5f, -0.5f ),
            new vec3(-0.5f, -0.5f, -0.5f ),
            new vec3(-0.5f, -0.5f,  0.5f ),
            new vec3( 0.5f, -0.5f,  0.5f ),
            new vec3( 0.5f,  0.5f,  0.5f ),
            new vec3( 0.5f,  0.5f,  0.5f ),
            new vec3(-0.5f,  0.5f,  0.5f ),
            new vec3(-0.5f, -0.5f,  0.5f ),
            new vec3(-0.5f,  0.5f,  0.5f ),
            new vec3(-0.5f,  0.5f, -0.5f ),
            new vec3(-0.5f, -0.5f, -0.5f ),
            new vec3(-0.5f, -0.5f, -0.5f ),
            new vec3(-0.5f, -0.5f,  0.5f ),
            new vec3(-0.5f,  0.5f,  0.5f ),
            new vec3( 0.5f,  0.5f,  0.5f ),
            new vec3( 0.5f,  0.5f, -0.5f ),
            new vec3( 0.5f, -0.5f, -0.5f ),
            new vec3( 0.5f, -0.5f, -0.5f ),
            new vec3( 0.5f, -0.5f,  0.5f ),
            new vec3( 0.5f,  0.5f,  0.5f ),
            new vec3(-0.5f, -0.5f, -0.5f ),
            new vec3( 0.5f, -0.5f, -0.5f ),
            new vec3( 0.5f, -0.5f,  0.5f ),
            new vec3( 0.5f, -0.5f,  0.5f ),
            new vec3(-0.5f, -0.5f,  0.5f ),
            new vec3(-0.5f, -0.5f, -0.5f ),
            new vec3(-0.5f,  0.5f, -0.5f ),
            new vec3( 0.5f,  0.5f, -0.5f ),
            new vec3( 0.5f,  0.5f,  0.5f ),
            new vec3( 0.5f,  0.5f,  0.5f ),
            new vec3(-0.5f,  0.5f,  0.5f ),
            new vec3(-0.5f,  0.5f, -0.5f ),
            },
        normals2 =
        {
        new vec3(0.0f,  0.0f, -1.0f),
        new vec3(0.0f,  0.0f, -1.0f),
        new vec3(0.0f,  0.0f, -1.0f),
        new vec3(0.0f,  0.0f, -1.0f),
        new vec3(0.0f,  0.0f, -1.0f),
        new vec3(0.0f,  0.0f, -1.0f),
        new vec3(0.0f,  0.0f,  1.0f),
        new vec3(0.0f,  0.0f,  1.0f),
        new vec3(0.0f,  0.0f,  1.0f),
        new vec3(0.0f,  0.0f,  1.0f),
        new vec3(0.0f,  0.0f,  1.0f),
        new vec3(0.0f,  0.0f,  1.0f),
        new vec3(1.0f,  0.0f,  0.0f),
        new vec3(1.0f,  0.0f,  0.0f),
        new vec3(1.0f,  0.0f,  0.0f),
        new vec3(1.0f,  0.0f,  0.0f),
        new vec3(1.0f,  0.0f,  0.0f),
        new vec3(1.0f,  0.0f,  0.0f),
        new vec3(1.0f,  0.0f,  0.0f),
        new vec3(1.0f,  0.0f,  0.0f),
        new vec3(1.0f,  0.0f,  0.0f),
        new vec3(1.0f,  0.0f,  0.0f),
        new vec3(1.0f,  0.0f,  0.0f),
        new vec3(1.0f,  0.0f,  0.0f),
        new vec3(0.0f, -1.0f,  0.0f),
        new vec3(0.0f, -1.0f,  0.0f),
        new vec3(0.0f, -1.0f,  0.0f),
        new vec3(0.0f, -1.0f,  0.0f),
        new vec3(0.0f, -1.0f,  0.0f),
        new vec3(0.0f, -1.0f,  0.0f),
        new vec3(0.0f,  1.0f,  0.0f),
        new vec3(0.0f,  1.0f,  0.0f),
        new vec3(0.0f,  1.0f,  0.0f),
        new vec3(0.0f,  1.0f,  0.0f),
        new vec3(0.0f,  1.0f,  0.0f),
        new vec3(0.0f,  1.0f,  0.0f),
        };
        Vector2[] textures =
        {
            new Vector2(0.0f,  0.0f),
            new Vector2(10.0f,  0.0f),
            new Vector2(10.0f,  10.0f),
            new Vector2(10.0f,  10.0f),
            new Vector2(0.0f,  10.0f),
            new Vector2(0.0f,  0.0f),
            new Vector2(0.0f,  0.0f),
            new Vector2(10.0f,  0.0f),
            new Vector2(10.0f,  10.0f),
            new Vector2(10.0f,  10.0f),
            new Vector2(0.0f,  10.0f),
            new Vector2(0.0f,  0.0f),
            new Vector2(10.0f,  0.0f),
            new Vector2(10.0f,  10.0f),
            new Vector2(0.0f,  10.0f),
            new Vector2(0.0f,  10.0f),
            new Vector2(0.0f,  0.0f),
            new Vector2(10.0f,  0.0f),
            new Vector2(10.0f,  0.0f),
            new Vector2(10.0f,  10.0f),
            new Vector2(0.0f,  10.0f),
            new Vector2(0.0f,  10.0f),
            new Vector2(0.0f,  0.0f),
            new Vector2(10.0f,  0.0f),
            new Vector2(0.0f,  10.0f),
            new Vector2(10.0f,  10.0f),
            new Vector2(10.0f,  0.0f),
            new Vector2(10.0f,  0.0f),
            new Vector2(0.0f,  0.0f),
            new Vector2(0.0f,  10.0f),
            new Vector2(0.0f,  10.0f),
            new Vector2(10.0f,  10.0f),
            new Vector2(10.0f,  0.0f),
            new Vector2(10.0f,  0.0f),
            new Vector2(0.0f,  0.0f),
            new Vector2(0.0f,  10.0f),
        };
        Vector2[] textures2 =
        {
            new Vector2(0.0f,  0.0f),
            new Vector2(1.0f,  0.0f),
            new Vector2(1.0f,  1.0f),
            new Vector2(1.0f,  1.0f),
            new Vector2(0.0f,  1.0f),
            new Vector2(0.0f,  0.0f),
            new Vector2(0.0f,  0.0f),
            new Vector2(1.0f,  0.0f),
            new Vector2(1.0f,  1.0f),
            new Vector2(1.0f,  1.0f),
            new Vector2(0.0f,  1.0f),
            new Vector2(0.0f,  0.0f),
            new Vector2(1.0f,  0.0f),
            new Vector2(1.0f,  1.0f),
            new Vector2(0.0f,  1.0f),
            new Vector2(0.0f,  1.0f),
            new Vector2(0.0f,  0.0f),
            new Vector2(1.0f,  0.0f),
            new Vector2(1.0f,  0.0f),
            new Vector2(1.0f,  1.0f),
            new Vector2(0.0f,  1.0f),
            new Vector2(0.0f,  1.0f),
            new Vector2(0.0f,  0.0f),
            new Vector2(1.0f,  0.0f),
            new Vector2(0.0f,  1.0f),
            new Vector2(1.0f,  1.0f),
            new Vector2(1.0f,  0.0f),
            new Vector2(1.0f,  0.0f),
            new Vector2(0.0f,  0.0f),
            new Vector2(0.0f,  1.0f),
            new Vector2(0.0f,  1.0f),
            new Vector2(1.0f,  1.0f),
            new Vector2(1.0f,  0.0f),
            new Vector2(1.0f,  0.0f),
            new Vector2(0.0f,  0.0f),
            new Vector2(0.0f,  1.0f),
        };
        List<vec3> vertices = new List<vec3>(), normalsL = new List<vec3>();
        List<Vector2> texturesL = new List<Vector2>();
        List<uint> indexesL = new List<uint>();
        Vector2[] texturesO;
        bool flag = false;
        vec3[] pointLightPositions = {
            //new vec3( 3.65f,  17.8f,  -14.0f),
            new vec3( 2.7f,  -15.2f,  12.0f),
            new vec3( 2.7f,  -15.2f,  12.0f),
        };
        vec3[] spotLightPositions = {
            new vec3( 3.65f,  17.8f,  -14.0f),
            //new vec3( 0f,  10f,  0f),
        };
        Camera _camera;
        bool _firstMove = true;
        Vector2 _lastPos;

        List<vec3>[] list_vertices = new List<vec3>[N], list_normals = new List<vec3>[N]; List<Vector2>[] list_textures = new List<Vector2>[N];
        vec3[][] mas_vertices = new vec3[N][], mas_normals = new vec3[N][]; Vector2[][] mas_textures = new Vector2[N][];
        Texture[] diffuseMaps = new Texture[N];
        float rotate;
        bool selectMode = false, stop = false; int numtex = 0;
        int Width = 800, Height = 800;
        int[] a = new int[4];
        int speed_mode = 0; vec3 lightcolor, diffusecolor, ambientcolor;


        public Game(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title }) { }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (!IsFocused)
            {
                return;
            }

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            const float cameraSpeed = 8.0f;
            const float sensitivity = 0.2f;

            if (input.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward
            }
            if (input.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down
            }

            var mouse = MouseState;

            if (_firstMove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity;
            }
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.Space:
                    flag = !flag;
                    break;
            }
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.208f, 0.318f, 0.361f, 0.0f);
            Sphere.genSphere(indexesL, vertices, texturesL, normalsL, 6f, 30, 30);
            points = new vec3[indexesL.Count];
            normals = new vec3[indexesL.Count];

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = vertices[(int)indexesL[i]];
                normals[i] = normalsL[(int)indexesL[i]];
            }
            vertices.Clear(); texturesL.Clear(); normalsL.Clear();
            pointsO = new vec3[vertices.Count];
            normalsO = new vec3[normalsL.Count];
            texturesO = new Vector2[(int)texturesL.Count];
            for (int i = 0; i < pointsO.Length; i++)
            {
                pointsO[i] = vertices[i];
                normalsO[i] = normalsL[i];
            }
            for (int i = 0; i < texturesO.Length; i++)
            {
                texturesO[i] = texturesL[i];
            }


            for (int i = 0; i < list_vertices.Length; i++)
            {
                list_vertices[i] = new List<vec3>();
                list_normals[i] = new List<vec3>();
                list_textures[i] = new List<Vector2>();
            }
            //вентилятор
            OBJ.load("../../../Resourses/fan.obj", list_vertices[0], list_textures[0], list_normals[0]);
            OBJ.load("../../../Resourses/blades.obj", list_vertices[1], list_textures[1], list_normals[1]);
            OBJ.load("../../../Resourses/parts.obj", list_vertices[2], list_textures[2], list_normals[2]);
            OBJ.load("../../../Resourses/image.obj", list_vertices[3], list_textures[3], list_normals[3]);
            //стол
            OBJ.load("../../../Resourses/table.obj", list_vertices[4], list_textures[4], list_normals[4]);
            OBJ.load("../../../Resourses/table_legs.obj", list_vertices[5], list_textures[5], list_normals[5]);
            //лампа
            OBJ.load("../../../Resourses/lamp.obj", list_vertices[6], list_textures[6], list_normals[6]);
            //окно
            OBJ.load("../../../Resourses/frame.obj", list_vertices[7], list_textures[7], list_normals[7]);


            for (int i = 0; i < list_vertices.Length; i++)
            {
                mas_vertices[i] = new vec3[list_vertices[i].Count];
                mas_normals[i] = new vec3[list_normals[i].Count];
                mas_textures[i] = new Vector2[list_textures[i].Count];
            }

            for (int i = 0; i < list_vertices.Length; i++)
            {
                mas_vertices[i] = list_vertices[i].ToArray();
                mas_normals[i] = list_normals[i].ToArray();
                mas_textures[i] = list_textures[i].ToArray();
                if (i == 0 || i == 2)
                {
                    for (int j = 0; j < mas_textures[i].Length; j++)
                        mas_textures[i][j] = Vector2.Multiply(mas_textures[i][j], 0.1f);
                }
            }


            
            for (int i = 0; i < N; i++)
            {
                arrays[i] = GL.GenVertexArray();
                GL.BindVertexArray(arrays[i]);

                buffers[i].vboV = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffers[i].vboV);
                GL.BufferData(BufferTarget.ArrayBuffer, mas_vertices[i].Length * vec3.SizeInBytes, mas_vertices[i], BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);

                buffers[i].vboN = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffers[i].vboN);
                GL.BufferData(BufferTarget.ArrayBuffer, mas_normals[i].Length * vec3.SizeInBytes, mas_normals[i], BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                GL.EnableVertexAttribArray(1);

                buffers[i].vboT = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffers[i].vboT);
                GL.BufferData(BufferTarget.ArrayBuffer, mas_textures[i].Length * Vector2.SizeInBytes, mas_textures[i], BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
                GL.EnableVertexAttribArray(2);
            }
            

            {
                vao_t = GL.GenVertexArray();
                GL.BindVertexArray(vao_t);

                vbo_v = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_v);
                GL.BufferData(BufferTarget.ArrayBuffer, points.Length * vec3.SizeInBytes, points, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);

                vbo_n = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_n);
                GL.BufferData(BufferTarget.ArrayBuffer, normals.Length * vec3.SizeInBytes, normals, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                GL.EnableVertexAttribArray(1);

                vbo_t = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo_t);
                GL.BufferData(BufferTarget.ArrayBuffer, textures.Length * Vector2.SizeInBytes, textures, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
                GL.EnableVertexAttribArray(2);
            }

            {
                vao = GL.GenVertexArray();
                GL.BindVertexArray(vao);

                vbo.vboV = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.vboV);
                GL.BufferData(BufferTarget.ArrayBuffer, points2.Length * vec3.SizeInBytes, points2, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);

                vbo.vboN = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.vboN);
                GL.BufferData(BufferTarget.ArrayBuffer, normals2.Length * vec3.SizeInBytes, normals2, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                GL.EnableVertexAttribArray(1);

                vbo.vboT = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.vboT);
                GL.BufferData(BufferTarget.ArrayBuffer, textures2.Length * Vector2.SizeInBytes, textures2, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
                GL.EnableVertexAttribArray(2);
            }

            {
                vao2 = GL.GenVertexArray();
                GL.BindVertexArray(vao2);

                vbo2.vboV = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo2.vboV);
                GL.BufferData(BufferTarget.ArrayBuffer, points2.Length * vec3.SizeInBytes, points2, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);

                vbo2.vboN = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo2.vboN);
                GL.BufferData(BufferTarget.ArrayBuffer, normals2.Length * vec3.SizeInBytes, normals2, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                GL.EnableVertexAttribArray(1);

                vbo2.vboT = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vbo2.vboT);
                GL.BufferData(BufferTarget.ArrayBuffer, textures.Length * Vector2.SizeInBytes, textures, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
                GL.EnableVertexAttribArray(2);
            }

            diffuseMaps[0] = Texture.LoadFromFile("../../../Resourses/Rastvorennyy chernyy.png");
            diffuseMaps[1] = Texture.LoadFromFile("../../../Resourses/Valia Everest.jpg");
            diffuseMaps[2] = Texture.LoadFromFile("../../../Resourses/white.jpg");
            diffuseMaps[3] = Texture.LoadFromFile("../../../Resourses/blackmetal.png");
            diffuseMaps[4] = Texture.LoadFromFile("../../../Resourses/logo.png");
            diffuseMaps[5] = Texture.LoadFromFile("../../../Resourses/1.png");
            diffuseMaps[6] = Texture.LoadFromFile("../../../Resourses/diffuse.jpg");
            diffuseMaps[7] = Texture.LoadFromFile("../../../Resourses/reflect.png");
            light_shader = new Shader("../../../Shaders/light_shader.vert", "../../../Shaders/light_shader.frag");
            object_shader = new Shader("../../../Shaders/object_shader.vert", "../../../Shaders/object_shader.frag");
            material_shader = new Shader("../../../Shaders/material_shader.vert", "../../../Shaders/material_shader.frag");

            _camera = new Camera(Vector3.UnitZ * 5, Size.X / (float)Size.Y);

            CursorState = CursorState.Grabbed;
        }
        protected override void OnUnload()
        {
            base.OnUnload();
            light_shader.Dispose();
            object_shader.Dispose();
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            if (!stop)
            {
                lightcolor = new vec3();
                lightcolor.X = (float)Math.Sin(GLFW.GetTime() * 2.0f);
                lightcolor.Y = (float)Math.Sin(GLFW.GetTime() * 0.7f);
                lightcolor.Z = (float)Math.Sin(GLFW.GetTime() * 1.3f);
                diffusecolor = lightcolor * new vec3(0.5f);
                ambientcolor = diffusecolor * new vec3(0.2f);
            }
            object_shader.Use();
            {
                object_shader.SetVec3("viewPos", _camera.Position);

                object_shader.SetVec3("dirLight.direction", 0.0f, 0.0f, 0.0f);
                object_shader.SetVec3("dirLight.ambient", new vec3(0.5f));
                object_shader.SetVec3("dirLight.diffuse", new vec3(0.5f));
                object_shader.SetVec3("dirLight.specular", new vec3(0.5f));

                object_shader.SetVec3("pointLights[0].position", pointLightPositions[0]);
                object_shader.SetVec3("pointLights[0].ambient", ambientcolor);
                object_shader.SetVec3("pointLights[0].diffuse", diffusecolor);
                object_shader.SetVec3("pointLights[0].specular", new vec3(1.0f, 1.0f, 1.0f));
                object_shader.SetFloat("pointLights[0].constant", 1.0f);
                object_shader.SetFloat("pointLights[0].linear", 0.09f);
                object_shader.SetFloat("pointLights[0].quadratic", 0.032f);

                object_shader.SetVec3("pointLights[1].position", pointLightPositions[1]);
                object_shader.SetVec3("pointLights[1].ambient", new vec3(1.0f, 1.0f, 1.0f));
                object_shader.SetVec3("pointLights[1].diffuse", new vec3(1.0f, 1.0f, 0.0f));
                object_shader.SetVec3("pointLights[1].specular", new vec3(1.0f, 1.0f, 1.0f));
                object_shader.SetFloat("pointLights[1].constant", 1.0f);
                object_shader.SetFloat("pointLights[1].linear", 0.09f);
                object_shader.SetFloat("pointLights[1].quadratic", 0.032f);

                object_shader.SetVec3("spotLights[0].position", spotLightPositions[0]);
                object_shader.SetVec3("spotLights[0].direction", new vec3(0f, 2f, -2f));
                object_shader.SetVec3("spotLights[0].ambient", ambientcolor);
                object_shader.SetVec3("spotLights[0].diffuse", diffusecolor);
                object_shader.SetVec3("spotLights[0].specular", new vec3(1.0f, 1.0f, 1.0f));
                object_shader.SetFloat("spotLights[0].constant", 1.0f);
                object_shader.SetFloat("spotLights[0].linear", 0.09f);
                object_shader.SetFloat("spotLights[0].quadratic", 0.032f);
                object_shader.SetFloat("spotLights[0].cutOff", MathF.Cos(MathHelper.DegreesToRadians(12.5f)));
                object_shader.SetFloat("spotLights[0].outerCutOff", MathF.Cos(MathHelper.DegreesToRadians(15.5f)));

                object_shader.SetMatrix4("view", _camera.GetViewMatrix());
                object_shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            }
            material_shader.Use();
            {
                material_shader.SetVec3("viewPos", _camera.Position);

                material_shader.SetVec3("dirLight.direction", 0.0f, 0.0f, 0.0f);
                material_shader.SetVec3("dirLight.ambient", new vec3(0.5f));
                material_shader.SetVec3("dirLight.diffuse", new vec3(0.5f));
                material_shader.SetVec3("dirLight.specular", new vec3(0.5f));

                material_shader.SetVec3("pointLights[0].position", pointLightPositions[0]);
                material_shader.SetVec3("pointLights[0].ambient", ambientcolor);
                material_shader.SetVec3("pointLights[0].diffuse", diffusecolor);
                material_shader.SetVec3("pointLights[0].specular", new vec3(1.0f, 1.0f, 1.0f));
                material_shader.SetFloat("pointLights[0].constant", 1.0f);
                material_shader.SetFloat("pointLights[0].linear", 0.09f);
                material_shader.SetFloat("pointLights[0].quadratic", 0.032f);

                material_shader.SetVec3("pointLights[1].position", pointLightPositions[1]);
                material_shader.SetVec3("pointLights[1].ambient", new vec3(1.0f, 1.0f, 1.0f));
                material_shader.SetVec3("pointLights[1].diffuse", new vec3(1.0f, 1.0f, 0.0f));
                material_shader.SetVec3("pointLights[1].specular", new vec3(1.0f, 1.0f, 1.0f));
                material_shader.SetFloat("pointLights[1].constant", 1.0f);
                material_shader.SetFloat("pointLights[1].linear", 0.09f);
                material_shader.SetFloat("pointLights[1].quadratic", 0.032f);

                material_shader.SetVec3("spotLights[0].position", spotLightPositions[0]);
                material_shader.SetVec3("spotLights[0].direction", 0.0f, 2.0f, -2.0f);
                material_shader.SetVec3("spotLights[0].ambient", ambientcolor);
                material_shader.SetVec3("spotLights[0].diffuse", diffusecolor);
                material_shader.SetVec3("spotLights[0].specular", 1.0f, 1.0f, 1.0f);
                material_shader.SetFloat("spotLights[0].constant", 1.0f);
                material_shader.SetFloat("spotLights[0].linear", 0.09f);
                material_shader.SetFloat("spotLights[0].quadratic", 0.032f);
                material_shader.SetFloat("spotLights[0].cutOff", MathF.Cos(MathHelper.DegreesToRadians(12.5f)));
                material_shader.SetFloat("spotLights[0].outerCutOff", MathF.Cos(MathHelper.DegreesToRadians(15.5f)));

                material_shader.SetMatrix4("view", _camera.GetViewMatrix());
                material_shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            }
            Show();
            Context.SwapBuffers();
            object_shader.Use(); Select();
            change_speed();
        }

        protected void change_speed()
        {
            switch (speed_mode)
            {
                case 0: rotate += 3.0f;
                        break;
                case 1: rotate += 6.0f;
                        break;
                case 2: rotate += 9.0f; 
                        break;
                case 3:
                        break;

            }
        }

        protected void Show()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            light_shader.Use();

            light_shader.SetMatrix4("view", _camera.GetViewMatrix());
            light_shader.SetMatrix4("projection", _camera.GetProjectionMatrix());

            lamps();

            object_shader.Use();
            floor(); walls(); obj_fan(); obj_table(); lamp(); obj_window();

        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            _camera.Fov -= e.OffsetY;
            if (_camera.Fov >= 90)
                _camera.Fov = 90;
            else if (_camera.Fov <= 1)
                _camera.Fov = 1;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (a[0]==25)
            {
                if (speed_mode == 3)
                    speed_mode = 0;
                else
                    speed_mode += 1;
            }
            if (a[0]==51)
            {
                stop = !stop;
            }
        }

        private void obj_window()
        {
            GL.BindVertexArray(arrays[7]);
            material_shader.Use();

            material_shader.SetVec3("material.ambient", 1f, 1f, 1f);
            material_shader.SetVec3("material.diffuse", 0.55f, 0.55f, 0.55f);
            material_shader.SetVec3("material.specular", 0.70f, 0.70f, 0.70f);
            material_shader.SetFloat("material.shininess", 0.25f * 128);
            //окно    
            model = Matrix4.Identity; 
            trans = Matrix4.CreateTranslation(0f, 75f, -86.7f);
            scale = Matrix4.CreateScale(0.345f);
            model *= trans; model *= scale;
            material_shader.SetMatrix4("model", model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, mas_vertices[7].Length);
        }

        private void obj_fan()
        {
            GL.BindVertexArray(arrays[0]);
            object_shader.SetInt("material.specular", 1);
            object_shader.SetFloat("material.shininess", 32f);
            object_shader.SetInt("material.diffuse", 0);
            //вентилятор
            diffuseMaps[2].Use(TextureUnit.Texture0);

            model = Matrix4.Identity;
            trans = Matrix4.CreateTranslation(-0.6f, 2.02f, -2.0f);
            scale = Matrix4.CreateScale(8.5f);
            rotation = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(-45));
            model *= rotation; model *= trans; model *= scale;
            if (selectMode)
            {

                light_shader.Use();
                light_shader.SetVec3("Color", 0.1f, 0, 0);//25
                light_shader.SetMatrix4("model", model);
            }
            else
                object_shader.SetMatrix4("model", model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, mas_vertices[0].Length);
            //запчасти
            GL.BindVertexArray(arrays[2]);
            diffuseMaps[3].Use(TextureUnit.Texture0);

            model = Matrix4.Identity;
            trans = Matrix4.CreateTranslation(-0.6f, 2.02f, -2.0f);
            scale = Matrix4.CreateScale(8.5f);
            rotation = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(-45));
            model *= rotation;
            model *= trans; model *= scale;
            if (selectMode)
            {

                light_shader.Use();
                light_shader.SetVec3("Color", 0.1f, 0, 0);//25
                light_shader.SetMatrix4("model", model);
            }
            else
                object_shader.SetMatrix4("model", model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, mas_vertices[2].Length);

            //лого
            GL.Enable(EnableCap.AlphaTest);     //разрешаем альфа-тест
            GL.AlphaFunc(AlphaFunction.Greater, 0.0f);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.DstAlpha, BlendingFactor.SrcAlpha);

            GL.BindVertexArray(arrays[3]);
            diffuseMaps[4].Use(TextureUnit.Texture0);

            model = Matrix4.Identity;
            trans = Matrix4.CreateTranslation(-0.6f, 2.02f, -2.0f);
            scale = Matrix4.CreateScale(8.5f);
            rotation = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(-45));
            model *= rotation;
            model *= trans; model *= scale;
            if (selectMode)
            {

                light_shader.Use();
                light_shader.SetVec3("Color", 0.1f, 0, 0);//25
                light_shader.SetMatrix4("model", model);
            }
            else
                object_shader.SetMatrix4("model", model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, mas_vertices[3].Length);
            GL.Disable(EnableCap.Blend);
            //лопасти
            GL.BindVertexArray(arrays[1]);
            diffuseMaps[2].Use(TextureUnit.Texture0);

            model = Matrix4.Identity;
            trans = Matrix4.CreateTranslation(-0.6f, 2.02f, -2.0f);
            scale = Matrix4.CreateScale(8.5f);
            rotation = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotate));
            model *= rotation;
            rotation = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(-45));
            model *= rotation;
            model *= trans; model *= scale;
            if (selectMode)
            {

                light_shader.Use();
                light_shader.SetVec3("Color", 0.1f, 0, 0);//25
                light_shader.SetMatrix4("model", model);
            }
            else
                object_shader.SetMatrix4("model", model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, mas_vertices[1].Length);
            rotation = Matrix4.Identity;
        }

        private void obj_table()
        {
            GL.BindVertexArray(arrays[4]);

            object_shader.SetInt("material.specular", 1);
            object_shader.SetFloat("material.shininess", 32f);
            object_shader.SetInt("material.diffuse", 0);

            //table
            diffuseMaps[0].Use(TextureUnit.Texture0);

            model = Matrix4.Identity;
            trans = Matrix4.CreateTranslation(0, -7, -30);
            scale = Matrix4.CreateScale(0.5f);
            model *= trans; model *= rotation; model *= scale;
            object_shader.SetMatrix4("model", model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, mas_vertices[4].Length);

            //table_legs
            material_shader.Use();

            material_shader.SetVec3("material.ambient", 0f, 0f, 0f);
            material_shader.SetVec3("material.diffuse", 0.01f, 0.01f, 0.01f);
            material_shader.SetVec3("material.specular", 0.50f, 0.50f, 0.50f);
            material_shader.SetFloat("material.shininess", 0.078125f * 128);

            GL.BindVertexArray(arrays[5]);

            model = Matrix4.Identity;
            trans = Matrix4.CreateTranslation(0, -7, -30);
            scale = Matrix4.CreateScale(0.5f);
            model *= trans; model *= rotation; model *= scale;
            if (selectMode)
            {
                light_shader.Use();
                light_shader.SetVec3("Color", 0.3f, 0, 0);//76
                light_shader.SetMatrix4("model", model);
            }
            else
                material_shader.SetMatrix4("model", model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, mas_vertices[5].Length);
        }

        private void lamp()
        {
            GL.BindVertexArray(arrays[6]);

            //lamp
            object_shader.Use();

            object_shader.SetInt("material.specular", 1);
            object_shader.SetFloat("material.shininess", 32f);
            object_shader.SetInt("material.diffuse", 0);
            diffuseMaps[2].Use(TextureUnit.Texture0);

            model = Matrix4.Identity;
            trans = Matrix4.CreateTranslation(14, 28.8f, -35);
            scale = Matrix4.CreateScale(0.4f);
            model *= trans; model *= rotation; model *= scale;
            if (selectMode)
            {

                light_shader.Use();
                light_shader.SetVec3("Color", 0.2f, 0, 0);//51
                light_shader.SetMatrix4("model", model);
            }
            else
                object_shader.SetMatrix4("model", model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, mas_vertices[6].Length);
        }

        private void floor()
        {
            GL.BindVertexArray(vao2);

            object_shader.SetInt("material.specular", 1);
            object_shader.SetFloat("material.shininess", 32f);
            object_shader.SetInt("material.diffuse", 0);
            diffuseMaps[1].Use(TextureUnit.Texture0);

            model = Matrix4.Identity;
            trans = Matrix4.CreateTranslation(0, -4, 0);
            scale = Matrix4.CreateScale(60.0f, 1.0f, 60.0f);
            model *= trans; model *= rotation; model *= scale;
            object_shader.SetMatrix4("model", model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, points2.Length);
        }

        private void walls()
        {
            GL.BindVertexArray(vao);

            object_shader.SetInt("material.specular", 1);
            object_shader.SetFloat("material.shininess", 32f);
            object_shader.SetInt("material.diffuse", 0);

            diffuseMaps[6].Use(TextureUnit.Texture0);
            diffuseMaps[7].Use(TextureUnit.Texture1);

            model = Matrix4.Identity;
            trans = Matrix4.CreateTranslation(-30, 0.4f, 0);
            scale = Matrix4.CreateScale(1.0f, 50.0f, 60.0f);
            model *= trans; model *= rotation; model *= scale;
            object_shader.SetMatrix4("model", model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, points2.Length);

            model = Matrix4.Identity;
            trans = Matrix4.CreateTranslation(30, 0.4f, 0);
            scale = Matrix4.CreateScale(1.0f, 50.0f, 60.0f);
            model *= trans; model *= rotation; model *= scale;
            object_shader.SetMatrix4("model", model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, points2.Length);

            model = Matrix4.Identity;
            trans = Matrix4.CreateTranslation(0, 0.4f, 30);
            scale = Matrix4.CreateScale(60.0f, 50.0f, 1.0f);
            model *= trans; model *= rotation; model *= scale;
            object_shader.SetMatrix4("model", model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, points2.Length);

            model = Matrix4.Identity;
            trans = Matrix4.CreateTranslation(1, 0.4f, -30);
            scale = Matrix4.CreateScale(20.0f, 50.0f, 1.0f);
            model *= trans; model *= rotation; model *= scale;
            object_shader.SetMatrix4("model", model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, points2.Length);

            model = Matrix4.Identity;
            trans = Matrix4.CreateTranslation(-1, 0.4f, -30);
            scale = Matrix4.CreateScale(20.0f, 50.0f, 1.0f);
            model *= trans; model *= rotation; model *= scale;
            object_shader.SetMatrix4("model", model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, points2.Length);

            model = Matrix4.Identity;
            trans = Matrix4.CreateTranslation(0, 0.2f, -30);
            scale = Matrix4.CreateScale(20.0f, 20.0f, 1.0f);
            model *= trans; model *= rotation; model *= scale;
            object_shader.SetMatrix4("model", model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, points2.Length);

            model = Matrix4.Identity;
            trans = Matrix4.CreateTranslation(0, 4.25f, -30);
            scale = Matrix4.CreateScale(20.0f, 9.5f, 1.0f);
            model *= trans; model *= rotation; model *= scale;
            object_shader.SetMatrix4("model", model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, points2.Length);
        }

        private void lamps()
        {
            GL.BindVertexArray(vao_t);
            
            model = Matrix4.Identity;
            trans = Matrix4.CreateTranslation(pointLightPositions[0]);
            scale = Matrix4.CreateScale(0.11f);
            model *= scale * trans;
            light_shader.SetVec3("Color", lightcolor);
            light_shader.SetMatrix4("model", model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, points.Length);
            
            model = Matrix4.Identity;
            trans = Matrix4.CreateTranslation(pointLightPositions[1]);
            scale = Matrix4.CreateScale(0.2f);
            model *= scale * trans;
            light_shader.SetVec3("Color", 1, 1, 1);
            light_shader.SetMatrix4("model", model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, points.Length);

            //фонарь в лампе
            model = Matrix4.Identity;
            trans = Matrix4.CreateTranslation(spotLightPositions[0]);
            scale = Matrix4.CreateScale(0.11f);
            model *= scale * trans;
            if (selectMode)
            {
                light_shader.SetVec3("Color", 0.2f, 0, 0);//51
                light_shader.SetMatrix4("model", model);
            }
            else
            {
                light_shader.SetVec3("Color", lightcolor);
                light_shader.SetMatrix4("model", model);
            }
            GL.DrawArrays(PrimitiveType.Triangles, 0, points.Length);
        }

        private void Select()
        {
            selectMode = true;
            Show();
            selectMode = false;
            
            GL.ReadPixels(Width / 2, Height / 2, 1, 1, PixelFormat.Rgba, PixelType.UnsignedByte, a);
            //Console.WriteLine(a[0]);
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
            Width = e.Width;
            Height = e.Height;
        }

    }

    internal class Program
    {
        static void Main(string[] args)
        {
            using (Game game = new Game(800, 800, "Окошечко"))
            {
                game.Run();
            }

        }
    }
}
