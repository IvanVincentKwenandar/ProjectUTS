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
    class Sally3DAssets
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

        public List<Sally3DAssets> child = new List<Sally3DAssets>();   // Sistem Hierarchical Object ==> List untuk menampung objek - objek child.


        public Sally3DAssets(Vector3 color)
        {
            this.color = color;
            _euler.Add(Vector3.UnitX); // Masukkan sudut Euler di Constructor.
            _euler.Add(Vector3.UnitY);
            _euler.Add(Vector3.UnitZ);

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

            view = Matrix4.CreateTranslation(0, 0, -8.0f);
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

            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());

            if (indices.Count != 0)
            {
                GL.DrawElements(PrimitiveType.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                GL.DrawArrays(PrimitiveType.LineStrip, 0, vertices.Count);
            }

            foreach (var i in child)
            {
                i.render(_camera);
            }
        }

        public void SetPivot(Vector3 Offset)
        {
            objectCenter += Offset;
        }

        /// <summary>
        /// Berfungsi untuk me-reset sudut euler (sudut relatif terhadap objek)
        /// </summary>
        public void resetEuler()
        {
            _euler.Clear();
            _euler.Add(Vector3.UnitX);
            _euler.Add(Vector3.UnitY);
            _euler.Add(Vector3.UnitZ);
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

        public void CreateEllipsoid(float x, float y, float z, float radX, float radY, float radZ, float sectorCount, float stackCount)
        {
            objectCenter = new Vector3(x, y, z);

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = pi / stackCount;
            float sectorAngle, stackAngle, tX, tY, tZ;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = pi / 2 - i * stackStep;
                tX = radX * (float)Math.Cos(stackAngle);
                tY = radY * (float)Math.Cos(stackAngle);
                tZ = radZ * (float)Math.Sin(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;

                    temp_vector.X = x + tX * (float)Math.Cos(sectorAngle);
                    temp_vector.Y = y + tY * (float)Math.Sin(sectorAngle);
                    temp_vector.Z = z + tZ ;

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
        /*
                public void CreateEllipsoid(float x, float y, float z, float radiusX, float radiusY, float radiusZ)
                {
                    objectCenter = new Vector3(x, y, z);
                    var tempVertex = new Vector3();
                    for (float u = -MathF.PI; u < MathF.PI; u += MathF.PI / 1000.0f)
                    {
                        for (float v = -MathF.PI / 2.0f; v < MathF.PI / 2.0f; v += MathF.PI / 100.0f)
                        {
                            tempVertex.X = x + radiusX * MathF.Cos(v) * MathF.Cos(u);
                            tempVertex.Y = y + radiusY * MathF.Cos(v) * MathF.Sin(u);
                            tempVertex.Z = z + radiusZ * MathF.Sin(v);
                            vertices.Add(tempVertex);
                        }
                    }
                }*/
        public void CreateParaboloid(float x, float y, float z, float radiusX, float radiusY, float radiusZ)
        {
            objectCenter = new Vector3(x, y, z);
            var tempVertex = new Vector3();
            for (float u = -MathF.PI; u < MathF.PI; u += MathF.PI / 1000.0f)
            {
                for (float v = 0; v < MathF.PI / 2.0f; v += MathF.PI / 1000.0f)
                {
                    tempVertex.X = x + radiusX * MathF.Cos(v) * MathF.Cos(u);
                    tempVertex.Y = y + radiusY * MathF.Cos(v) * MathF.Sin(u);
                    tempVertex.Z = z + radiusZ * 2 * MathF.Sin(v);
                    vertices.Add(tempVertex);
                }
            }
        }

      
        public void CreateHalfEllipsoid(float x, float y, float z, float radiusX, float radiusY, float radiusZ)
        {
            objectCenter = new Vector3(x, y, z);
            var tempVertex = new Vector3();
            for (float u = -MathF.PI; u < MathF.PI / 314; u += MathF.PI / 1000.0f)
            {
                for (float v = -MathF.PI / 2.0f; v < MathF.PI / 2.0f; v += MathF.PI / 100.0f)
                {
                    tempVertex.X = x + radiusX * MathF.Cos(v) * MathF.Cos(u);
                    tempVertex.Y = y + radiusY * MathF.Cos(v) * MathF.Sin(u);
                    tempVertex.Z = z + radiusZ * MathF.Sin(v);
                    vertices.Add(tempVertex);
                }
            }
        }
        public void CreateOneThreeEllipsoid(float x, float y, float z, float radiusX, float radiusY, float radiusZ)
        {
            objectCenter = new Vector3(x, y, z);
            var tempVertex = new Vector3();
            for (float u = -MathF.PI/4; u < MathF.PI / 2; u += MathF.PI / 1000.0f)
            {
                for (float v = -MathF.PI / 2.0f; v < MathF.PI / 2.0f; v += MathF.PI / 100.0f)
                {
                    tempVertex.X = x + radiusX *  MathF.Sin(v);
                    tempVertex.Y = y + radiusY * MathF.Cos(v) * MathF.Cos(u);
                    tempVertex.Z = z + radiusZ * MathF.Cos(v) * MathF.Sin(u);
                    vertices.Add(tempVertex);
                }
            }
        }
        public void createCylinder(float x, float y, float z, float radius, float height)
        {
            objectCenter = new Vector3(x, y, z);
            Vector3 tempVertex = new Vector3();
            float _pi = (float)Math.PI;


            for (float v = -height / 2; v <= (height / 2); v += 0.0005f)
            {
                for (float u = -_pi; u <= _pi; u += (_pi / 30))
                {

                    tempVertex.X = x + radius * (float)Math.Cos(u);
                    tempVertex.Y = y + v;
                    tempVertex.Z = z + radius * (float)Math.Sin(u) ;

                    vertices.Add(tempVertex);

                }
            }
        }


           

        


        Vector3 setBezier(float t, float x, float y, float height, float extended)
        {
            //Console.WriteLine(t);
            Vector3 p = new Vector3(0f, 0f, 0f);
            float[] k = new float[3];

            k[0] = (float)Math.Pow((1 - t), 3 - 1 - 0) * (float)Math.Pow(t, 0) * 1;
            k[1] = (float)Math.Pow((1 - t), 3 - 1 - 1) * (float)Math.Pow(t, 1) * 2;
            k[2] = (float)Math.Pow((1 - t), 3 - 1 - 2) * (float)Math.Pow(t, 2) * 1;


            //titik 1
            p.X += k[0] * x;
            p.Y += k[0] * y - height;

            //titik 2
            p.X += k[1] * (x + extended);
            p.Y += k[1] * y;

            //titik 3
            p.X += k[2] * x;
            p.Y += k[2] * y + height;

            //Console.WriteLine(p.X + " "+ p.Y);

            return p;

        }

        public void createBezierCylinder(float x, float y, float z, float rad, float panjang, float ext)
        {
            objectCenter = new Vector3(x, y, z);
            Vector3 temp_vector;
            float _pi = (float)Math.PI;

            for (float v = -panjang / 2; v <= (panjang / 2); v += 0.0001f)
            {
                Vector3 p = setBezier(((v + (panjang / 2)) / panjang), x, y, panjang, ext);
                for (float u = -_pi; u <= _pi; u += (_pi / 30))
                {

                    temp_vector.X = p.X + rad * (float)Math.Cos(u);
                    temp_vector.Y = p.Y + rad * (float)Math.Sin(u);
                    temp_vector.Z = z + v;


                    vertices.Add(temp_vector);
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
    }
}
