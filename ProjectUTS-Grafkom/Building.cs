using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace ConsoleApp4
{
    public class Building
    {
        // Because we're adding a texture, we modify the vertex array to include texture coordinates.
        // Texture coordinates range from 0.0 to 1.0, with (0.0, 0.0) representing the bottom left, and (1.0, 1.0) representing the top right.
        // The new layout is three floats to create a vertex, then two floats to create the coordinates.
        private readonly float[] _vertices =
        {
            // Position         Texture coordinates
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
        };

        private readonly uint[] _indices =
        {
            0, 1, 3,
            1, 2, 3
        };

        private readonly string path = "../../../Shaders/";

        private readonly string path2 = "../../../Resources/";

        private int _elementBufferObject;

        private int _vertexBufferObject;

        private int _vertexArrayObject;

        protected Matrix4 model;

        private Shader _shader;

        float x;
        float y;
        float z;
        float length;


        // For documentation on this, check Texture.cs.
        private Texture _texture;

        public Building() { }

        public void OnLoad(float x, float y, float z, float length, String pos, int choice)
        {

            this.x = x;
            this.y = y;
            this.z = z;
            this.length = length;

            draw(pos);

            //GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            model = Matrix4.Identity;
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

            // The shaders have been modified to include the texture coordinates, check them out after finishing the OnLoad function.
            _shader = new Shader(path + "shaderMap.vert", path + "shaderMap.frag");
            _shader.Use();

            // Because there's now 5 floats between the start of the first vertex and the start of the second,
            // we modify the stride from 3 * sizeof(float) to 5 * sizeof(float).
            // This will now pass the new vertex array to the buffer.
            var vertexLocation = _shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            // Next, we also setup texture coordinates. It works in much the same way.
            // We add an offset of 3, since the texture coordinates comes after the position data.
            // We also change the amount of data to 2 because there's only 2 floats for texture coordinates.
            var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            
            if (choice == 0)
            {
                _texture = Texture.LoadFromFile(path2 + "white.png");
            }
            else if (choice == 1)
            {
                _texture = Texture.LoadFromFile(path2 + "depan2.jpg");
            }
            else if(choice == 2)
            {
                _texture = Texture.LoadFromFile(path2 + "wall1.jpg");
            }else if(choice == 3)
            {
                _texture = Texture.LoadFromFile(path2 + "wall2.jpg");
            }
            
            
           
            _texture.Use(TextureUnit.Texture0);
        }

        public void render(Camera _camera)
        {

            GL.BindVertexArray(_vertexArrayObject);

            _texture.Use(TextureUnit.Texture0);
            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            _shader.Use();

            GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);


        }


        public void draw(String pos)
        {
            if(pos == "datar")
            {
                // kanan atas
                _vertices[0] = x + length;
                _vertices[1] = y;
                _vertices[2] = z + length;

                // kanan bawah
                _vertices[5] = x - length;
                _vertices[6] = y;
                _vertices[7] = z + length;

                // kiri bawah
                _vertices[10] = x - length;
                _vertices[11] = y;
                _vertices[12] = z - length;

                // kiri atas
                _vertices[15] = x + length;
                _vertices[16] = y;
                _vertices[17] = z - length;
            }
            else if(pos == "tegak")
            {
                // kanan atas
                _vertices[0] = x;
                _vertices[1] = y + length;
                _vertices[2] = z + length;

                // kanan bawah
                _vertices[5] = x;
                _vertices[6] = y - length;
                _vertices[7] = z + length;

                // kiri bawah
                _vertices[10] = x;
                _vertices[11] = y - length;
                _vertices[12] = z - length;

                // kiri atas
                _vertices[15] = x;
                _vertices[16] = y + length;
                _vertices[17] = z - length;
            }
            else if(pos == "depan")
            {
                // kanan atas
                _vertices[0] = x + length;
                _vertices[1] = y + length;
                _vertices[2] = z;

                // kanan bawah
                _vertices[5] = x - length;
                _vertices[6] = y + length;
                _vertices[7] = z;

                // kiri bawah
                _vertices[10] = x - length;
                _vertices[11] = y - length;
                _vertices[12] = z;

                // kiri atas
                _vertices[15] = x + length;
                _vertices[16] = y - length;
                _vertices[17] = z;
            }
        }
    }
}