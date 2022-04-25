using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;


namespace ConsoleApp4
{
    internal class DoraemonAsset3D
    {
        public List<Vector3> vertices = new List<Vector3>();
        private List<uint> indices = new List<uint>();
        public List<DoraemonAsset3D> Child = new List<DoraemonAsset3D>();
        int _vertexBufferObject;
        int _elementBufferObject;
        int _vertexArrayObject;

        Shader _shader;
        Vector3 color = new Vector3(0f, 0f, 0f);

        Matrix4 model = Matrix4.Identity;
        public Vector3 _centerPosition;
        public Vector3 rotationCenter;
        public List<Vector3> _euler;
        bool trackPos = false;


        public DoraemonAsset3D(float red, float green, float blue)
        {
            setColor(red, green, blue);
            vertices = new List<Vector3>();
            setdefault();
        }

        public DoraemonAsset3D()
        {
            vertices = new List<Vector3>();
            setdefault();
        }

        public void multModel(Matrix4 change)
        {
            model *= change;
        }

        public void setTrack(bool track)
        {
            trackPos = track;
        }

        public void setColor(float red, float green, float blue)
        {
            color.X = red;
            color.Y = green;
            color.Z = blue;
        }

        public void setdefault()
        {
            _euler = new List<Vector3>();
            _euler.Add(new Vector3(1, 0, 0));   //sumbu X
            _euler.Add(new Vector3(0, 1, 0));   //sumbu y
            _euler.Add(new Vector3(0, 0, 1));   //sumbu z
            model = Matrix4.Identity;
            _centerPosition = new Vector3(0, 0, 0);
            Child = new List<DoraemonAsset3D>();
        }

        public void resetEuler()
        {
            _euler[0] = new Vector3(1, 0, 0);
            _euler[1] = new Vector3(0, 1, 0);
            _euler[2] = new Vector3(0, 0, 1);
        }

        public void addChild(DoraemonAsset3D newChild)
        {
            Child.Add(newChild);
        }

        public void load(string ShaderVert, string ShaderFrag, int Size_x, int Size_y)
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
            _shader = new Shader(ShaderVert, ShaderFrag);
            _shader.Use();

            //_view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);

            //_projection = Matrix4.CreatePerspectiveFieldOfView(
            // MathHelper.DegreesToRadians(46f), Size_x / (float)Size_y, 0.1f, 100.0f);

            foreach (var child in Child)
            {
                child.load(ShaderVert, ShaderFrag, Size_x, Size_y);
            }


        }

        public void render(Camera _camera)
        {
            _shader.Use();
            int vertexColorLocation = GL.GetUniformLocation(_shader.Handle, "ourColor");
            GL.Uniform4(vertexColorLocation, color[0], color[1], color[2], 1.0f);
            GL.BindVertexArray(_vertexArrayObject);

            _shader.SetVector3("ourColor", color);
            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());


            if (indices.Count != 0)
            {
                GL.DrawElements(PrimitiveType.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);
                //GL.DrawElements(PrimitiveType.LineStrip, indices.Count, DrawElementsType.UnsignedInt, 0);
            }
            else
            {
                //GL.DrawArrays(PrimitiveType.LineStrip, 0, vertices.Count);
            }

            if (trackPos)
            {
                float curr_x = model.M41;
                float curr_y = model.M42;
                float curr_z = model.M43;
                _centerPosition = new Vector3(curr_x, curr_y, curr_z);
            }

            foreach (var child in Child)
            {
                child.render(_camera);
            }

        }

        public void scale(float scaleX, float scaleY, float scaleZ)
        {
            model *= Matrix4.CreateScale(scaleX, scaleY, scaleZ);
            foreach (var child in Child)
            {
                child.scale(scaleX, scaleY, scaleZ);
            }
        }

        public void scaleStay(float scaleX, float scaleY, float scaleZ)
        {
            model *= Matrix4.CreateTranslation(-_centerPosition);
            model *= Matrix4.CreateScale(scaleX, scaleY, scaleZ);
            model *= Matrix4.CreateTranslation(_centerPosition);
            foreach (var child in Child)
            {
                child.scaleStay(scaleX, scaleY, scaleZ);
            }
        }

        public void translate(float tranX, float tranY, float tranZ)
        {
            model *= Matrix4.CreateTranslation(tranX, tranY, tranZ);
            foreach (var child in Child)
            {
                child.translate(tranX, tranY, tranZ);
            }
        }

        public void translate(float tranX, float tranY, float tranZ, bool save)
        {
            if (save)
            {
                _centerPosition.X += tranX;
                _centerPosition.Y += tranY;
                _centerPosition.Z += tranZ;
            }
            translate(tranX, tranY, tranZ);

        }

        public void rotateX_all(float angle)
        {
            model *= Matrix4.CreateRotationX(angle);
            foreach (var child in Child)
            {
                child.rotateX_all(angle);
            }
        }

        public void rotateY_all(float angle)
        {
            model *= Matrix4.CreateRotationY(angle);
            foreach (var child in Child)
            {
                child.rotateY_all(angle);
            }
        }

        public void rotateZ_all(float angle)
        {
            model *= Matrix4.CreateRotationZ(angle);
            foreach (var child in Child)
            {
                child.rotateZ_all(angle);
            }
        }

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

            rotationCenter = getRotationResult(pivot, vector, radAngle, rotationCenter);
            _centerPosition = getRotationResult(pivot, vector, radAngle, _centerPosition);

            foreach (var i in Child)
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

        public void createDot(float x, float y, float z)
        {
            _centerPosition = new Vector3(x, y, z);
            trackPos = true;
        }

        public void createBoxVertices(float x, float y, float z, float length)
        {
            //biar lebih fleksibel jangan inisialiasi posisi dan 
            //panjang kotak didalam tapi ditaruh ke parameter
            float _x = x;
            float _y = y;
            float _z = z;
            float _length = length;

            Vector3 temp_vector;
            //1. Inisialisasi vertex
            vertices.Add(new Vector3(_x - _length / 2.0f, _y + _length / 2.0f, _z - _length / 2.0f)); //Titik 1
            vertices.Add(new Vector3(_x + _length / 2.0f, _y + _length / 2.0f, _z - _length / 2.0f)); //Titik 2
            vertices.Add(new Vector3(_x - _length / 2.0f, _y - _length / 2.0f, _z - _length / 2.0f)); //Titik 3
            vertices.Add(new Vector3(_x + _length / 2.0f, _y - _length / 2.0f, _z - _length / 2.0f)); //Titik 4
            vertices.Add(new Vector3(_x - _length / 2.0f, _y + _length / 2.0f, _z + _length / 2.0f)); //Titik 5
            vertices.Add(new Vector3(_x + _length / 2.0f, _y + _length / 2.0f, _z + _length / 2.0f)); //Titik 6
            vertices.Add(new Vector3(_x - _length / 2.0f, _y - _length / 2.0f, _z + _length / 2.0f)); //Titik 7
            vertices.Add(new Vector3(_x + _length / 2.0f, _y - _length / 2.0f, _z + _length / 2.0f)); //Titik 8

            //2. Inisialisasi index vertex
            indices = new List<uint> {
                // Segitiga Depan 1
                0, 1, 2,
                // Segitiga Depan 2
                1, 2, 3,
                // Segitiga Atas 1
                0, 4, 5,
                // Segitiga Atas 2
                0, 1, 5,
                // Segitiga Kanan 1
                1, 3, 5,
                // Segitiga Kanan 2
                3, 5, 7,
                // Segitiga Kiri 1
                0, 2, 4,
                // Segitiga Kiri 2
                2, 4, 6,
                // Segitiga Belakang 1
                4, 5, 6,
                // Segitiga Belakang 2
                5, 6, 7,
                // Segitiga Bawah 1
                2, 3, 6,
                // Segitiga Bawah 2
                3, 6, 7
            };

        }

        public void createCylinderUp(float x, float y, float z, float radius, float height, float bagiBujur)
        {
            _centerPosition = new Vector3(x, y, z);
            float topY = (float)(y + height / 2.0f);
            float bottomY = (float)(y - height / 2.0f);

            float pi = (float)Math.PI;
            float bujurAngle = 2 * pi / bagiBujur;

            for (int i = 0; i < bagiBujur; i++)
            {
                float tempX = (float)(x + radius * MathHelper.Cos(bujurAngle * i));
                float tempZ = (float)(z + radius * MathHelper.Sin(bujurAngle * i));
                vertices.Add(new Vector3(tempX, topY, tempZ));
                vertices.Add(new Vector3(tempX, bottomY, tempZ));
            }

            indices = new List<uint>();

            for (uint i = 0; i < bagiBujur * 2; i++)
            {
                indices.Add(i);

                if (i == bagiBujur * 2 - 1)
                {
                    indices.Add(0);
                }
                else
                {
                    indices.Add(i + 1);
                }

                if (i == bagiBujur * 2 - 2)
                {
                    indices.Add(0);
                }
                else if (i == bagiBujur * 2 - 1)
                {
                    indices.Add(1);
                }
                else
                {
                    indices.Add(i + 2);
                }

            }
        }

        public void createCylinderSide(float x, float y, float z, float radius, float length, float bagiLintang)
        {
            _centerPosition = new Vector3(x, y, z);
            float leftX = (float)(x - length / 2.0f);
            float RightX = (float)(x + length / 2.0f);

            float pi = (float)Math.PI;
            float bujurAngle = 2 * pi / bagiLintang;

            for (int i = 0; i < bagiLintang; i++)
            {
                float tempY = (float)(y + radius * MathHelper.Cos(bujurAngle * i));
                float tempZ = (float)(z + radius * MathHelper.Sin(bujurAngle * i));
                vertices.Add(new Vector3(leftX, tempY, tempZ));
                vertices.Add(new Vector3(RightX, tempY, tempZ));
            }

            indices = new List<uint>();

            for (uint i = 0; i < bagiLintang * 2; i++)
            {
                indices.Add(i);

                if (i == bagiLintang * 2 - 1)
                {
                    indices.Add(0);
                }
                else
                {
                    indices.Add(i + 1);
                }

                if (i == bagiLintang * 2 - 2)
                {
                    indices.Add(0);
                }
                else if (i == bagiLintang * 2 - 1)
                {
                    indices.Add(1);
                }
                else
                {
                    indices.Add(i + 2);
                }

            }
        }

        public void createHalfHyperOneSheet(float x, float y, float z, float r, float length, float bagiX, uint bagiLintang)
        {
            _centerPosition = new Vector3(x, y, z);
            float pi = MathHelper.Pi;
            float angle = 2 * pi / bagiLintang;
            float deltaX = length / bagiX;

            for (float _X = -length; _X < 10; _X += deltaX)
            {
                float radius = (float)(MathHelper.Sqrt(MathHelper.Pow(r, 2) + MathHelper.Pow(_X, 2)));

                for (int i = 0; i < bagiLintang; i++)
                {
                    float tempY = (float)(y + radius * MathHelper.Sin(angle * i));
                    float tempZ = (float)(z + radius * MathHelper.Cos(angle * i));
                    vertices.Add(new Vector3(_X + x, tempY, tempZ));
                }
            }
            indices = new List<uint>();

            for (uint j = 0; j < bagiLintang * bagiX; j++)
            {
                indices.Add(j);
                indices.Add(j + bagiLintang);

                if (j % bagiLintang == bagiLintang - 1)
                {
                    indices.Add(j + 1 - bagiLintang);
                }
                else
                {
                    indices.Add(j + 1);
                }

                indices.Add(j + bagiLintang);

                if (j % bagiLintang == bagiLintang - 1)
                {
                    indices.Add(j + 1 - bagiLintang);
                    indices.Add(j + 1);
                }
                else
                {
                    indices.Add(j + 1);
                    indices.Add(j + 1 + bagiLintang);
                }
            }
        }

        public void createHalfHyperTwoSheet(float x, float y, float z, float r, float height, float bagiLintang, uint bagiBujur)
        {
            _centerPosition = new Vector3(x, y, z);
            float pi = MathHelper.Pi;
            float angle = 2 * pi / bagiBujur;
            float deltaY = height / bagiLintang;

            float topY = r * -1;
            float bottomY = topY - height;
            vertices.Add(new Vector3(x, topY + y, z));  //peak

            for (float _Y = topY - deltaY; _Y >= bottomY; _Y -= deltaY)
            {

                float radius = (float)(MathHelper.Sqrt(MathHelper.Pow(_Y, 2) - MathHelper.Pow(r, 2)));

                for (int i = 0; i < bagiBujur; i++)
                {
                    float tempX = (float)(x + (float)(radius * MathHelper.Cos(angle * i)));
                    float tempZ = (float)(z + (float)(radius * MathHelper.Sin(angle * i)));
                    vertices.Add(new Vector3(tempX, _Y + y, tempZ));
                }
            }
            indices = new List<uint>();

            for (uint i = 0; i < bagiBujur * bagiLintang; i++)
            {
                if (i < bagiBujur)
                {
                    indices.Add(i + 1);
                    indices.Add(0);
                    if (i % bagiBujur == bagiBujur - 1)
                    {
                        indices.Add(1);
                    }
                    else
                    {
                        indices.Add(i + 2);
                    }
                }
                else
                {
                    indices.Add(i - bagiBujur + 1);
                    indices.Add(i + 1);
                    if (i % bagiBujur != bagiBujur - 1)
                    {
                        indices.Add(i - bagiBujur + 2);
                    }
                    else
                    {
                        indices.Add(i - (2 * bagiBujur) + 2);
                    }

                    indices.Add(i + 1);
                    if (i % bagiBujur != bagiBujur - 1)
                    {
                        indices.Add(i - bagiBujur + 2);
                    }
                    else
                    {
                        indices.Add(i - (2 * bagiBujur) + 2);
                    }
                    if (i % bagiBujur != bagiBujur - 1)
                    {
                        indices.Add(i + 2);
                    }
                    else
                    {
                        indices.Add(i - bagiBujur + 2);
                    }

                }
            }





        }

        public void createCircleUp(float x, float y, float z, float r, float bagiBujur)
        {
            float pi = MathHelper.Pi;
            float angle = 2 * pi / bagiBujur;
            vertices.Add(new Vector3(x, y, z)); //center
            for (int i = 0; i < bagiBujur; i++)
            {
                float tempX = (float)(r * MathHelper.Cos(angle * i));
                float tempZ = (float)(r * MathHelper.Sin(angle * i));
                vertices.Add(new Vector3(tempX + x, y, tempZ + z));
            }
            indices = new List<uint>();
            for (uint i = 1; i <= bagiBujur; i++)
            {
                indices.Add(0);
                indices.Add(i);
                if (i != bagiBujur)
                {
                    indices.Add(i + 1);
                }
                else
                {
                    indices.Add(1);
                }
            }
        }

        public void createEllipsoid(float x, float y, float z, float radX, float radY, float radZ, float sectorCount, float stackCount)
        {
            _centerPosition = new Vector3(x, y, z);

            float pi = (float)Math.PI;
            Vector3 temp_vector;
            float sectorStep = 2 * pi / sectorCount;
            float stackStep = pi / stackCount;
            float stackAngle, tempX, tempY, tempZ;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = pi / 2 - i * stackStep;
                tempX = radX * (float)Math.Cos(stackAngle);
                tempY = radY * (float)Math.Sin(stackAngle);
                tempZ = radZ * (float)Math.Cos(stackAngle);

                for (int j = 0; j <= sectorCount; ++j)
                {
                    temp_vector.X = x + tempX * (float)Math.Cos(j * sectorStep);
                    temp_vector.Y = y + tempY;
                    temp_vector.Z = z + tempZ * (float)Math.Sin(j * sectorStep);
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

        public int factorial(int n)
        {
            if (n < 0)
            {
                Console.Out.WriteLine("Error. Negative Factorial.");
            }
            if (n == 0 || n == 1)
            {
                return 1;
            }
            return n * factorial(n - 1);
        }

        public List<int> pasc_triangle(int row)
        {
            List<int> result = new List<int>();
            for (int i = 0; i <= row; i++)
            {
                result.Add(factorial(row) / (factorial(i) * factorial(row - i)));
            }
            return result;
        }

        public void createBezierCurve(List<Vector3> guide)
        {
            int row = guide.Count;
            List<int> pascal = pasc_triangle(row - 1);

            for (float t = 0; t <= 1.0f; t += 0.01f)
            {
                Vector3 result = new Vector3(0, 0, 0);
                for (int i = 0; i < row; i++)
                {
                    float constant = (float)(pascal[i] * MathHelper.Pow((1 - t), row - 1 - i) * MathHelper.Pow(t, i));
                    result.X += (float)(constant * guide[i].X);
                    result.Y += (float)(constant * guide[i].Y);
                    result.Z += (float)(constant * guide[i].Z);
                }
                vertices.Add(result);
            }
        }

        public void createMouth(DoraemonAsset3D lipUp, DoraemonAsset3D lipBottom)
        {
            for (int i = 0; i < lipUp.vertices.Count; i++)
            {
                vertices.Add(lipUp.vertices[i]);
                vertices.Add(lipBottom.vertices[i]);
            }

            for (int i = 0; (i + 2) < vertices.Count; i++)
            {
                indices.Add((uint)i);
                indices.Add((uint)i + 1);
                indices.Add((uint)i + 2);
            }
        }

        public void createBatStatic(float x, float y, float z, float radBarrel, float radGrip, float length, uint bagiBujur, uint bagiLintang)
        {
            _centerPosition = new Vector3(x, y, z);
            float pi = MathHelper.Pi;
            float angle = 2 * pi / bagiBujur;
            float deltaY = length / bagiLintang;
            float _length = length / 3f;

            float botY = (float)(y - (_length / 2f));
            float MidGripY = (float)(y + (_length / 2f));
            float MidBarrelY = MidGripY + _length;
            float topY = MidBarrelY + _length;



            //grip
            for (float _Y = botY; _Y <= MidGripY; _Y += deltaY)
            {
                for (int i = 0; i < bagiBujur; i++)
                {
                    float tempX = (float)(radGrip * MathHelper.Cos(angle * i));
                    float tempZ = (float)(radGrip * MathHelper.Sin(angle * i));
                    vertices.Add(new Vector3(tempX + x, _Y, tempZ + z));
                }
            }
            //mid

            float deltaRad = (radBarrel - radGrip) * 3 / bagiLintang;

            for (float _Y = MidGripY, rad = radGrip; _Y <= MidBarrelY; _Y += deltaY, rad += deltaRad)
            {
                for (int i = 0; i < bagiBujur; i++)
                {
                    float tempX = (float)(rad * MathHelper.Cos(angle * i));
                    float tempZ = (float)(rad * MathHelper.Sin(angle * i));
                    vertices.Add(new Vector3(tempX + x, _Y, tempZ + z));
                }
            }
            //barrel
            for (float _Y = MidBarrelY; _Y <= topY; _Y += deltaY)
            {
                for (int i = 0; i < bagiBujur; i++)
                {
                    float tempX = (float)(radBarrel * MathHelper.Cos(angle * i));
                    float tempZ = (float)(radBarrel * MathHelper.Sin(angle * i));
                    vertices.Add(new Vector3(tempX + x, _Y, tempZ + z));
                }
            }




            indices = new List<uint>();

            for (uint j = 0; j < bagiBujur * bagiLintang; j++)
            {
                indices.Add(j);
                indices.Add(j + bagiBujur);

                if (j % bagiBujur == bagiBujur - 1)
                {
                    indices.Add(j + 1 - bagiBujur);
                }
                else
                {
                    indices.Add(j + 1);
                }

                indices.Add(j + bagiBujur);

                if (j % bagiBujur == bagiBujur - 1)
                {
                    indices.Add(j + 1 - bagiBujur);
                    indices.Add(j + 1);
                }
                else
                {
                    indices.Add(j + 1);
                    indices.Add(j + 1 + bagiBujur);
                }
            }
        }
    }
}

