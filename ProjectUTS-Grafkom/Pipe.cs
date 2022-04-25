using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using LearnOpenTK.Common;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ConsoleApp4
{
    class Pipe
    {
        private readonly string path = "../../../";
        List<DoraemonAsset3D> objectPipe = new List<DoraemonAsset3D>();
        public Vector2 _Size;

        public void load(int X, int Y)
        {
            // Object - Pipe
            // Front pipe
            var pipe1 = new DoraemonAsset3D(0.563f, 0.580f, 0.556f);
            pipe1.createCylinderSide(26f, 2.85f, 0f, 2.75f, 17.5f, 100);
            //pipe1.createPipe(26f, 2.85f, 0f, 2.75f, 17.5f);
            pipe1.rotate(pipe1._centerPosition, pipe1._euler[1], 90);
            pipe1.rotateY_all(MathHelper.DegreesToRadians(90f));
            objectPipe.Add(pipe1);

            // Back pipe
            var pipe2 = new DoraemonAsset3D(0.663f, 0.680f, 0.656f);
            pipe2.createCylinderSide(20.5f, 2.85f, 0f, 2.75f, 17.5f, 100);
            pipe2.rotate(pipe2._centerPosition, pipe2._euler[1], 90);
            pipe2.rotateY_all(MathHelper.DegreesToRadians(90f));
            objectPipe.Add(pipe2);

            // Top pipe
            var pipe3 = new DoraemonAsset3D(0.763f, 0.780f, 0.756f);
            pipe3.createCylinderSide(23.5f, 7.8f, 0f, 2.75f, 17.5f, 100);
            pipe3.rotate(pipe3._centerPosition, pipe3._euler[1], 90);
            pipe3.rotateY_all(MathHelper.DegreesToRadians(90f));
            objectPipe.Add(pipe3);



            foreach (DoraemonAsset3D i in objectPipe)
            {
                i.load(Constants.path + "shader.vert", Constants.path + "shader.frag", X, Y);
            }
        }


        public void render(Camera _camera)
        {
            objectPipe[0].render(_camera);
            objectPipe[1].render(_camera);
            objectPipe[2].render(_camera);
        }


    }
}
