using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp4
{
    class AssetKoro
    {

        private readonly string path = "../../../Shaders/";

        public List<Vector3> vertices = new List<Vector3>();
        private List<uint> indices = new List<uint>();

        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private int _elementBufferObject;

        private Shader _shader;

        private Matrix4 model = Matrix4.Identity;   // Model Matrix      ==> Matrix ini yang akan berubah saat terjadi transformasi
        private Matrix4 view;                       // View Matrix       ==> Matrix ini menentukan arah pandang 'kamera'
        private Matrix4 projection;                 // Projection Matrix ==> Matrix ini menentukan jenis projection, kamera game cenderung menggunakan kamera perspective.

        private Vector3 color;                      // Warna objek, dikirim ke shader lewat uniform.

        public List<Vector3> _euler = new List<Vector3>();  // Sudut lokal, relatif terhadap objek yang bersangkutan.
        public Vector3 objectCenter = Vector3.Zero;         // Titik tengah objek

        public List<AssetKoro> child = new List<AssetKoro>();   // Sistem Hierarchical Object ==> List untuk menampung objek - objek child.
        int indexs;
        int[] _pascal;

        //File
        Vector3[] vertices2;
        Vector3[] colors;
        Vector2[] texturecoords;
        List<Tuple<int, int, int>> faces = new List<Tuple<int, int, int>>();


        public AssetKoro()
        {

        }


        public AssetKoro(Vector3 color)
        {
            this.color = color;
            _euler.Add(Vector3.UnitX); // Masukkan sudut Euler di Constructor.
            _euler.Add(Vector3.UnitY);
            _euler.Add(Vector3.UnitZ);
        }

        public void reset()
        {
            model = Matrix4.Identity;
            //objectCenter = (0, 0, 0);

            foreach (var i in child)
            {
                i.reset();
            }
        }

        public void load(int sizeX, int sizeY)
        {
            _vertexBufferObject = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * Vector3.SizeInBytes, vertices.ToArray(), BufferUsageHint.StaticDraw);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            if (indices.Count != 0)
            {
                _elementBufferObject = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
                GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(uint), indices.ToArray(), BufferUsageHint.StaticDraw);
            }

            view = Matrix4.CreateTranslation(0, 0, -13.0f);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), sizeX / (float)sizeY, 0.01f, 100f);

            _shader = new Shader(path + "shader.vert", path + "shader.frag");
            _shader.Use();

            foreach (var i in child)
            {
                i.load(sizeX, sizeY);
            }
        }

        public void render(Camera _camera)
        {
            _shader.Use();
            GL.BindVertexArray(_vertexArrayObject);

            _shader.SetVector3("objColor", color);

            //model *= Matrix4.CreateTranslation(MathF.Cos((float)time * 0.01f), MathF.Sin((float)time * 0.01f), MathF.Cos((float)time * 0.01f));

            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());

            if (indices.Count != 0)
            {
                GL.DrawElements(PrimitiveType.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                GL.DrawArrays(PrimitiveType.Lines, 0, vertices.Count);
            }

            foreach (var i in child)
            {
                i.render(_camera);
            }
        }

        /*public void render2(int _lines, double time, Matrix4 temp)
        {
            _shader.Use();
            GL.BindVertexArray(_vertexArrayObject);
            Matrix4 model = Matrix4.Identity * Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(time));
            model = model * Matrix4.CreateTranslation(0.0f, 0.5f, 0.0f);



            *//*model = model * Matrix4.CreateScale(0.5f);*/

        /*Matrix4 model = Matrix4.Identity;
        model = temp;*//*
        _shader.SetMatrix4("model", model);
        _shader.SetMatrix4("view", _view);
        _shader.SetMatrix4("projection", _projection);
        _shader.SetVector3("ourColor", color);

        if (indices.Count != 0)
        {
            GL.DrawElements(PrimitiveType.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);

        }
        else
        {

            GL.DrawArrays(PrimitiveType.LinesAdjacency, 0, vertices.Count);


        }

    }*/

        /// <summary>
        /// Berfungsi untuk me-reset sudut euler (sudut relatif terhadap objek)
        /// </summary>
        public void resetEuler()
        {
            _euler[0] = new Vector3(1, 0, 0);
            _euler[1] = new Vector3(0, 1, 0);
            _euler[2] = new Vector3(0, 0, 1);
        }

        #region solidObjects

        public void createCuboid(float x_, float y_, float z_, float length)
        {
            var tempVertices = new List<Vector3>();
            Vector3 temp_vector;

            //Titik 1
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 2
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 3
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 4
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 5
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 6
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 7
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 8
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            var tempIndices = new List<uint>
            {
				//Back
				1, 2, 0,
                2, 1, 3,
				
				//Top
				5, 0, 4,
                0, 5, 1,

				//Right
				5, 3, 1,
                3, 5, 7,

				//Left
				0, 6, 4,
                6, 0, 2,

				//Front
				4, 7, 5,
                7, 4, 6,

				//Bottom
				3, 6, 2,
                6, 3, 7
            };
            vertices = tempVertices;
            indices = tempIndices;
        }

        public void createTieD(float x_, float y_, float z_, float length)
        {
            var tempVertices = new List<Vector3>();
            Vector3 temp_vector;

            //Titik 1
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 2
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 3
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 4
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 5
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 6
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 7
            temp_vector.X = x_ - length / 10f;
            temp_vector.Y = y_ - length / 10f;
            temp_vector.Z = z_ + length / 10f;
            tempVertices.Add(temp_vector);

            //Titik 8
            temp_vector.X = x_ + length / 10f;
            temp_vector.Y = y_ - length / 10f;
            temp_vector.Z = z_ + length / 10f;
            tempVertices.Add(temp_vector);

            var tempIndices = new List<uint>
            {
               0, 1, 7,
               0, 1, 6

            };

            vertices = tempVertices;
            indices = tempIndices;
        }

        /*internal void load(object x, object y)
        {
            throw new NotImplementedException();
        }*/

        public void createTieD2(float x_, float y_, float z_, float length)
        {
            var tempVertices = new List<Vector3>();
            Vector3 temp_vector;

            //Titik 1
            temp_vector.X = x_ - length / 10f;
            temp_vector.Y = y_ + length / 10f;
            temp_vector.Z = z_ - length / 10f;
            tempVertices.Add(temp_vector);

            //Titik 2
            temp_vector.X = x_ + length / 10f;
            temp_vector.Y = y_ + length / 10f;
            temp_vector.Z = z_ - length / 10f;
            tempVertices.Add(temp_vector);

            //Titik 3
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 4
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 5
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 6
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 7
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 8
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            var tempIndices = new List<uint>
            {
               0, 6, 7,
               1, 6, 7

            };

            vertices = tempVertices;
            indices = tempIndices;
        }

        public void createTieL(float x_, float y_, float z_, float length)
        {
            var tempVertices = new List<Vector3>();
            Vector3 temp_vector;

            //Titik 1
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 2
            temp_vector.X = x_ + length / 3.0f;
            temp_vector.Y = y_ + length / 3.0f;
            temp_vector.Z = z_ - length / 3.0f;
            tempVertices.Add(temp_vector);

            //Titik 3
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 4
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 5
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 6
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 7
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 8
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            var tempIndices = new List<uint>
            {
                0, 1, 6
            };
            vertices = tempVertices;
            indices = tempIndices;
        }

        public void createTieR(float x_, float y_, float z_, float length)
        {
            var tempVertices = new List<Vector3>();
            Vector3 temp_vector;

            //Titik 1
            temp_vector.X = x_ - length / 3.0f;
            temp_vector.Y = y_ + length / 3.0f;
            temp_vector.Z = z_ - length / 3.0f;
            tempVertices.Add(temp_vector);

            //Titik 2
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 3
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 4
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 5
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 6
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 7
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 8
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            var tempIndices = new List<uint>
            {
                0, 1, 7
            };

            vertices = tempVertices;
            indices = tempIndices;
        }

        public void createCap(float X, float Y, float Z, float rad)
        {
            Vector3 temp_vector;

            vertices = new List<Vector3>();

            for (int i = 0; i < 360; i++)
            {
                double degInRad = i * Math.PI / 180;
                temp_vector.X = rad * (float)Math.Cos(degInRad) + X;
                temp_vector.Y = Y;
                temp_vector.Z = rad * (float)Math.Sin(degInRad) + Z;
                vertices.Add(temp_vector);
            }

            for (int i = 0; i < 360; i++)
            {
                double degInRad = i * Math.PI / 180;
                temp_vector.X = rad * (float)Math.Cos(degInRad) + X;
                temp_vector.Y = Y - 0.3f;
                temp_vector.Z = rad * (float)Math.Sin(degInRad) + Z;
                vertices.Add(temp_vector);
            }

            for (uint i = 0; i < 359; i++)
            {
                indices.Add(i);
                indices.Add(i + 1);
                indices.Add(i + 360);
            }

            for (uint i = 0; i < 359; i++)
            {
                indices.Add(i + 1);
                indices.Add(i + 360);
                indices.Add(i + 360 + 1);
            }
        }

        public void createCapTop(float x_, float y_, float z_, float length)
        {
            var tempVertices = new List<Vector3>();
            Vector3 temp_vector;

            //Titik 1
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 2
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 3
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 4
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ - length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 5
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 6
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ + length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 7
            temp_vector.X = x_ - length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            //Titik 8
            temp_vector.X = x_ + length / 2.0f;
            temp_vector.Y = y_ - length / 2.0f;
            temp_vector.Z = z_ + length / 2.0f;
            tempVertices.Add(temp_vector);

            var tempIndices = new List<uint>
            {
                3, 6, 2,
                6, 3, 7
            };

            vertices = tempVertices;
            indices = tempIndices;
        }


        public void createSphere(float x, float y, float z, float rad, float sectorCount, float stackCount)
        {
            objectCenter = new Vector3(x, y, z);

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = pi / stackCount;
            float sectorAngle, stackAngle, tempX, tempY, tempZ;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = pi / 2 - i * stackStep;
                tempX = rad * (float)Math.Cos(stackAngle);
                tempY = rad * (float)Math.Sin(stackAngle);
                tempZ = rad * (float)Math.Cos(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x + tempX * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = y + tempY;
                    temp_vector.Z = z + tempZ * (float)Math.Sin(sectorAngle);

                    vertices.Add(temp_vector);
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);

                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        indices.Add(k1);
                        indices.Add(k2);
                        indices.Add(k1 + 1);

                    }

                    if (i != stackCount - 1)
                    {
                        indices.Add(k1 + 1);
                        indices.Add(k2);
                        indices.Add(k2 + 1);
                    }
                }
            }
        }
        public void createEllipsoid(float x, float y, float z, float radX, float radY, float radZ, float sectorCount, float stackCount)
        {
            objectCenter = new Vector3(x, y, z);

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = pi / stackCount;
            float sectorAngle, stackAngle, tempX, tempY, tempZ;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = pi / 2 - i * stackStep;
                tempX = radX * (float)Math.Cos(stackAngle);
                tempY = radY * (float)Math.Sin(stackAngle);
                tempZ = radZ * (float)Math.Cos(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x + tempX * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = y + tempY;
                    temp_vector.Z = z + tempZ * (float)Math.Sin(sectorAngle);

                    vertices.Add(temp_vector);
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);

                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        indices.Add(k1);
                        indices.Add(k2);
                        indices.Add(k1 + 1);

                    }

                    if (i != stackCount - 1)
                    {
                        indices.Add(k1 + 1);
                        indices.Add(k2);
                        indices.Add(k2 + 1);
                    }
                }
            }
        }


        public void createEllipsoid2(float x, float y, float z, float radX, float radY, float radZ, float sectorCount, float stackCount)
        {
            objectCenter = new Vector3(x, y, z);

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = pi / stackCount * 0.5f;
            float sectorAngle, stackAngle, tempX, tempY, tempZ;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = pi / 0.5f - i * stackStep;
                tempX = radX * (float)Math.Cos(stackAngle);
                tempY = radY * (float)Math.Sin(stackAngle);
                tempZ = radZ * (float)Math.Cos(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x + tempX * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = y + tempY;
                    temp_vector.Z = z + tempZ * (float)Math.Sin(sectorAngle);

                    vertices.Add(temp_vector);
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);

                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        indices.Add(k1);
                        indices.Add(k2);
                        indices.Add(k1 + 1);

                    }

                    if (i != stackCount - 1)
                    {
                        indices.Add(k1 + 1);
                        indices.Add(k2);
                        indices.Add(k2 + 1);
                    }
                }
            }
        }

        public void createCollarTop(float x, float y, float z, float radX, float radY, float radZ, float sectorCount, float stackCount)
        {
            objectCenter = new Vector3(x, y, z);

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * pi / sectorCount * (-0.5f);
            float stackStep = pi / stackCount * (-0.5f);
            float sectorAngle, stackAngle, tempX, tempY, tempZ;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = pi / 2 - i * stackStep;
                tempX = radX * (float)Math.Cos(stackAngle);
                tempY = radY * (float)Math.Sin(stackAngle);
                tempZ = radZ * (float)Math.Cos(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x + tempX * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = y + tempY;
                    temp_vector.Z = z + tempZ * (float)Math.Sin(sectorAngle);

                    vertices.Add(temp_vector);
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);

                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        indices.Add(k1);
                        indices.Add(k2);
                        indices.Add(k1 + 1);

                    }

                    if (i != stackCount - 1)
                    {
                        indices.Add(k1 + 1);
                        indices.Add(k2);
                        indices.Add(k2 + 1);
                    }
                }
            }
        }

        public void createHand(float x, float y, float z, float radX, float radY, float radZ, float sectorCount, float stackCount)
        {
            objectCenter = new Vector3(x, y, z);

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = pi / stackCount * -(0.5f);
            float sectorAngle, stackAngle, tempX, tempY, tempZ;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = pi / 2 - i * stackStep;
                tempX = radX * (float)Math.Cos(stackAngle);
                tempY = radY * (float)Math.Sin(stackAngle);
                tempZ = radZ * (float)Math.Cos(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x + tempX * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = y + tempY;
                    temp_vector.Z = z + tempZ * (float)Math.Sin(sectorAngle);

                    vertices.Add(temp_vector);
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);

                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    if (i != 0)
                    {
                        indices.Add(k1);
                        indices.Add(k2);
                        indices.Add(k1 + 1);

                    }

                    if (i != stackCount - 1)
                    {
                        indices.Add(k1 + 1);
                        indices.Add(k2);
                        indices.Add(k2 + 1);
                    }
                }
            }
        }

        public void createBody(float x, float y, float z, float rad_X, float rad_Y, float rad_Z, float sectorCount, float stackCount)
        {
            //objectCenter = new Vector3(x, y, z);
            float _pi = 3.1416f;

            Vector3 tempvector_elipsoid;

            for (float v = 0; v <= _pi / 2; v += _pi / 180000 * 2)
            {
                for (float u = -_pi; u <= _pi; u += _pi / 50)
                {
                    tempvector_elipsoid.X = x + rad_X * (float)Math.Cos(v) * (float)Math.Cos(u); //x
                    tempvector_elipsoid.Y = y + rad_Y * 2.0f * (float)Math.Sin(v); //y
                    tempvector_elipsoid.Z = z + rad_Z * (float)Math.Cos(v) * (float)Math.Sin(u); //z
                    vertices.Add(tempvector_elipsoid);
                }

            }


        }

        /*public void createHand(float x, float y, float z, float rad_X, float rad_Y, float rad_Z, float sectorCount, float stackCount)
        {
            float _pi = 3.1416f;

            Vector3 tempvector_elipsoid;

            for (float v = 0; v <= _pi / 2; v += _pi / 180000 * 2)
            {
                for (float u = -_pi; u <= _pi; u += _pi / 50)
                {
                    tempvector_elipsoid.X = x + rad_X * (float)Math.Cos(v) * (float)Math.Cos(u); //x
                    tempvector_elipsoid.Y = y + rad_Y * 2.0f * (float)Math.Sin(v); //y
                    tempvector_elipsoid.Z = z + rad_Z * (float)Math.Cos(v) * (float)Math.Sin(u); //z
                    vertices.Add(tempvector_elipsoid);
                }

            }


        }*/

        Vector3 setBezier(float t, float x, float y, float height, float ext = 0.5f)
        {
            Vector3 p = new Vector3(0f, 0f, 0f);

            float[] k = new float[7];

            k[0] = (float)Math.Pow((1 - t), 6) * (float)Math.Pow(t, 0) * 1;
            k[1] = (float)Math.Pow((1 - t), 5) * (float)Math.Pow(t, 1) * 6;
            k[2] = (float)Math.Pow((1 - t), 4) * (float)Math.Pow(t, 2) * 15;
            k[3] = (float)Math.Pow((1 - t), 3) * (float)Math.Pow(t, 3) * 20;
            k[4] = (float)Math.Pow((1 - t), 2) * (float)Math.Pow(t, 4) * 15;
            k[5] = (float)Math.Pow((1 - t), 1) * (float)Math.Pow(t, 5) * 6;
            k[6] = (float)Math.Pow((1 - t), 0) * (float)Math.Pow(t, 6) * 1;


            // Titik 1
            p.X += k[0] * x;
            p.Y += k[0] * y - height;

            // Titik 2
            p.X += k[1] * (x + 0.25f);
            p.Y += k[1] * y;

            // Titik 3
            p.X += k[2] * (x + 0.25f);
            p.Y += k[2] * y;

            // Titik 4
            p.X += k[3] * (x + 0.25f);
            p.Y += k[3] * y;

            // Titik 5
            p.X += k[4] * (x + 1f);
            p.Y += k[4] * y;

            // Titik 6
            p.X += k[5] * (x + 1.75f);
            p.Y += k[5] * y;

            // Titik 7
            p.X += k[6] * x;
            p.Y += k[6] * y + height;

            return p;
        }

        Vector3 setBezierStrip(float t, float x, float y, float height, float ext)
        {
            Vector3 p = new Vector3(0f, 0f, 0f);

            float[] k = new float[3];

            k[0] = (float)Math.Pow((1 - t), 2) * (float)Math.Pow(t, 0) * 1;
            k[1] = (float)Math.Pow((1 - t), 1) * (float)Math.Pow(t, 1) * 2;
            k[2] = (float)Math.Pow((1 - t), 0) * (float)Math.Pow(t, 2) * 1;


            // Titik 1
            p.X += k[0] * x;
            p.Y += k[0] * y - height;

            // Titik 2
            p.X += k[1] * (x + ext);
            p.Y += k[1] * y;

            // Titik 3
            p.X += k[2] * x;
            p.Y += k[2] * y + height;


            return p;
        }

        Vector3 setBezierFoot(float t, float x, float y, float height, float ext, float ext2)
        {
            Vector3 p = new Vector3(0f, 0f, 0f);

            float[] k = new float[4];

            k[0] = (float)Math.Pow((1 - t), 3) * (float)Math.Pow(t, 0) * 1;
            k[1] = (float)Math.Pow((1 - t), 2) * (float)Math.Pow(t, 1) * 3;
            k[2] = (float)Math.Pow((1 - t), 1) * (float)Math.Pow(t, 2) * 3;
            k[3] = (float)Math.Pow((1 - t), 0) * (float)Math.Pow(t, 3) * 1;


            // Titik 1
            p.X += k[0] * x;
            p.Y += k[0] * y - height;

            // Titik 2
            p.X += k[1] * (x + ext);
            p.Y += k[1] * y;

            // Titik 3
            p.X += k[2] * (x + ext2);
            p.Y += k[2] * y;

            // Titik 4
            p.X += k[3] * x;
            p.Y += k[3] * y + height;


            return p;
        }



        public void createHandDown(float x, float y, float z, float rad, float height, float ext)
        {
            Vector3 temp_vector;
            float _pi = (float)Math.PI;

            for (float v = -height / 2; v <= (height / 2); v += 0.0001f)
            {
                Vector3 p = setBezier(((v + (height / 2)) / height), x, y, height, ext);
                for (float u = -_pi; u <= _pi; u += (_pi / 30))
                {

                    temp_vector.X = p.X + rad * (float)Math.Cos(u);
                    temp_vector.Y = p.Y + rad * (float)Math.Sin(u);
                    temp_vector.Z = z + v;


                    vertices.Add(temp_vector);
                }
            }
        }


        public void createHandStrip(float x, float y, float z, float rad, float height, float ext)
        {
            Vector3 temp_vector;
            float _pi = (float)Math.PI;

            for (float v = -height / 2; v <= (height / 2); v += 0.0001f)
            {
                Vector3 p = setBezierStrip(((v + (height / 2)) / height), x, y, height, ext);
                for (float u = -_pi; u <= _pi; u += (_pi / 30))
                {

                    temp_vector.X = p.X + rad * (float)Math.Cos(u);
                    temp_vector.Y = p.Y + rad * (float)Math.Sin(u);
                    temp_vector.Z = z + v;


                    vertices.Add(temp_vector);
                }
            }
        }


        public void createFoot(float x, float y, float z, float rad, float height, float ext, float ext2)
        {
            Vector3 temp_vector;
            float _pi = (float)Math.PI;

            for (float v = -height / 2; v <= (height / 2); v += 0.0001f)
            {
                Vector3 p = setBezierFoot(((v + (height / 2)) / height), x, y, height, ext, ext2);
                for (float u = -_pi; u <= _pi; u += (_pi / 30))
                {

                    temp_vector.X = p.X + rad * (float)Math.Cos(u);
                    temp_vector.Y = p.Y + rad * (float)Math.Sin(u);
                    temp_vector.Z = z + v;


                    vertices.Add(temp_vector);
                }
            }
        }

        public void createPipe(float x, float y, float z, float rad, float height)
        {
            Vector3 temp_vector;
            float _pi = (float)Math.PI;

            for (float v = -height / 2; v <= (height / 2); v += 0.01f)
            {
                for (float u = -_pi; u <= _pi; u += (_pi / 30))
                {

                    temp_vector.X = x + rad * (float)Math.Cos(u);
                    temp_vector.Y = y + rad * (float)Math.Sin(u);
                    temp_vector.Z = z + v;


                    vertices.Add(temp_vector);
                }
            }
        }


        public void createTorus(float x, float y, float z, float radMajor, float radMinor, float sectorCount, float stackCount)
        {
            objectCenter = new Vector3(x, y, z);

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            stackCount *= 2;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = 2 * pi / stackCount;
            float sectorAngle, stackAngle, tempX, tempY, tempZ;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = pi / 2 - i * stackStep;
                tempX = radMajor + radMinor * (float)Math.Cos(stackAngle);
                tempY = radMinor * (float)Math.Sin(stackAngle);
                tempZ = radMajor + radMinor * (float)Math.Cos(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x + tempX * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = y + tempY;
                    temp_vector.Z = z + tempZ * (float)Math.Sin(sectorAngle);

                    vertices.Add(temp_vector);
                }
            }

            uint k1, k2;
            for (int i = 0; i < stackCount; ++i)
            {
                k1 = (uint)(i * (sectorCount + 1));
                k2 = (uint)(k1 + sectorCount + 1);

                for (int j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    indices.Add(k1);
                    indices.Add(k2);
                    indices.Add(k1 + 1);

                    indices.Add(k1 + 1);
                    indices.Add(k2);
                    indices.Add(k2 + 1);
                }
            }
        }

        #endregion

        #region transforms
        public void rotate(Vector3 pivot, Vector3 vector, float angle)
        {
            var radAngle = MathHelper.DegreesToRadians(angle);

            var arbRotationMatrix = new Matrix4
                (
                new Vector4((float)(Math.Cos(radAngle) + Math.Pow(vector.X, 2.0f) * (1.0f - Math.Cos(radAngle))), (float)(vector.X * vector.Y * (1.0f - Math.Cos(radAngle)) + vector.Z * Math.Sin(radAngle)), (float)(vector.X * vector.Z * (1.0f - Math.Cos(radAngle)) - vector.Y * Math.Sin(radAngle)), 0),
                new Vector4((float)(vector.X * vector.Y * (1.0f - Math.Cos(radAngle)) - vector.Z * Math.Sin(radAngle)), (float)(Math.Cos(radAngle) + Math.Pow(vector.Y, 2.0f) * (1.0f - Math.Cos(radAngle))), (float)(vector.Y * vector.Z * (1.0f - Math.Cos(radAngle)) + vector.X * Math.Sin(radAngle)), 0),
                new Vector4((float)(vector.X * vector.Z * (1.0f - Math.Cos(radAngle)) + vector.Y * Math.Sin(radAngle)), (float)(vector.Y * vector.Z * (1.0f - Math.Cos(radAngle)) - vector.X * Math.Sin(radAngle)), (float)(Math.Cos(radAngle) + Math.Pow(vector.Z, 2.0f) * (1.0f - Math.Cos(radAngle))), 0),
                Vector4.UnitW
                );

            model *= Matrix4.CreateTranslation(-pivot);
            model *= arbRotationMatrix;
            model *= Matrix4.CreateTranslation(pivot);

            for (int i = 0; i < 3; i++)
            {
                _euler[i] = Vector3.Normalize(getRotationResult(pivot, vector, radAngle, _euler[i], true));
            }

            objectCenter = getRotationResult(pivot, vector, radAngle, objectCenter);

            foreach (var i in child)
            {
                i.rotate(pivot, vector, angle);
            }
        }

        public Vector3 getRotationResult(Vector3 pivot, Vector3 vector, float angle, Vector3 point, bool isEuler = false)
        {
            Vector3 temp, newPosition;

            if (isEuler)
            {
                temp = point;
            }
            else
            {
                temp = point - pivot;
            }

            newPosition.X =
                temp.X * (float)(Math.Cos(angle) + Math.Pow(vector.X, 2.0f) * (1.0f - Math.Cos(angle))) +
                temp.Y * (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) - vector.Z * Math.Sin(angle)) +
                temp.Z * (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) + vector.Y * Math.Sin(angle));

            newPosition.Y =
                temp.X * (float)(vector.X * vector.Y * (1.0f - Math.Cos(angle)) + vector.Z * Math.Sin(angle)) +
                temp.Y * (float)(Math.Cos(angle) + Math.Pow(vector.Y, 2.0f) * (1.0f - Math.Cos(angle))) +
                temp.Z * (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) - vector.X * Math.Sin(angle));

            newPosition.Z =
                temp.X * (float)(vector.X * vector.Z * (1.0f - Math.Cos(angle)) - vector.Y * Math.Sin(angle)) +
                temp.Y * (float)(vector.Y * vector.Z * (1.0f - Math.Cos(angle)) + vector.X * Math.Sin(angle)) +
                temp.Z * (float)(Math.Cos(angle) + Math.Pow(vector.Z, 2.0f) * (1.0f - Math.Cos(angle)));

            if (isEuler)
            {
                temp = newPosition;
            }
            else
            {
                temp = newPosition + pivot;
            }
            return temp;
        }

        public void rotateY_all(float angle)
        {
            model *= Matrix4.CreateRotationY(angle);
            foreach (var child in child)
            {
                child.rotateY_all(angle);
            }
        }

        public void translate(float x, float y, float z)
        {
            model *= Matrix4.CreateTranslation(x, y, z);
            objectCenter.X += x;
            objectCenter.Y += y;
            objectCenter.Z += z;

            foreach (var i in child)
            {
                i.translate(x, y, z);
            }
        }

        public void scale(float scaleX, float scaleY, float scaleZ)
        {
            model *= Matrix4.CreateTranslation(-objectCenter);
            model *= Matrix4.CreateScale(scaleX, scaleY, scaleZ);
            objectCenter *= new Vector3(scaleX, scaleY, scaleZ);
            model *= Matrix4.CreateTranslation(objectCenter);

            foreach (var i in child)
            {
                i.scale(scaleX, scaleY, scaleZ);
            }
        }




        #endregion

        #region readFile
        public static AssetKoro LoadFromFile(string filename)
        {
            AssetKoro obj = new AssetKoro();
            try
            {
                using (StreamReader reader = new StreamReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
                {
                    obj = LoadFromString(reader.ReadToEnd());
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found: {0}", filename);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading file: {0}", filename);
            }

            return obj;
        }

        public static AssetKoro LoadFromString(string obj)
        {
            // Seperate lines from the file
            List<String> lines = new List<string>(obj.Split('\n'));

            // Lists to hold model data
            List<Vector3> verts = new List<Vector3>();
            List<Vector3> colors = new List<Vector3>();
            List<Vector2> texs = new List<Vector2>();
            List<Tuple<int, int, int>> faces = new List<Tuple<int, int, int>>();

            // Read file line by line
            foreach (String line in lines)
            {
                if (line.StartsWith("v ")) // Vertex definition
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Vector3 vec = new Vector3();

                    if (temp.Count((char c) => c == ' ') == 2) // Check if there's enough elements for a vertex
                    {
                        String[] vertparts = temp.Split(' ');

                        // Attempt to parse each part of the vertice
                        bool success = float.TryParse(vertparts[0], out vec.X);
                        success &= float.TryParse(vertparts[1], out vec.Y);
                        success &= float.TryParse(vertparts[2], out vec.Z);

                        // Dummy color/texture coordinates for now
                        colors.Add(new Vector3((float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z)));
                        texs.Add(new Vector2((float)Math.Sin(vec.Z), (float)Math.Sin(vec.Z)));

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Console.WriteLine("Error parsing vertex: {0}", line);
                        }
                    }

                    verts.Add(vec);
                }
                else if (line.StartsWith("f ")) // Face definition
                {
                    // Cut off beginning of line
                    String temp = line.Substring(2);

                    Tuple<int, int, int> face = new Tuple<int, int, int>(0, 0, 0);

                    if (temp.Count((char c) => c == ' ') == 2) // Check if there's enough elements for a face
                    {
                        String[] faceparts = temp.Split(' ');

                        int i1, i2, i3;

                        // Attempt to parse each part of the face
                        bool success = int.TryParse(faceparts[0], out i1);
                        success &= int.TryParse(faceparts[1], out i2);
                        success &= int.TryParse(faceparts[2], out i3);

                        // If any of the parses failed, report the error
                        if (!success)
                        {
                            Console.WriteLine("Error parsing face: {0}", line);
                        }
                        else
                        {
                            // Decrement to get zero-based vertex numbers
                            face = new Tuple<int, int, int>(i1 - 1, i2 - 1, i3 - 1);
                            faces.Add(face);
                        }
                    }
                }
            }

            // Create the ObjVolume
            AssetKoro vol = new AssetKoro();
            vol.vertices2 = verts.ToArray();
            vol.faces = new List<Tuple<int, int, int>>(faces);
            vol.colors = colors.ToArray();
            vol.texturecoords = texs.ToArray();

            return vol;
        }
        #endregion
    }
}
