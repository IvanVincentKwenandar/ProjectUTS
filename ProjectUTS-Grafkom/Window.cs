using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System;
using OpenTK.Mathematics;
using LearnOpenTK.Common;

namespace ConsoleApp4
{
    class Window : GameWindow
    {

        koroClass koroSensei = new koroClass();
        private Sally sallyRed = new Sally();
        private Sally sallyRefree = new Sally();
        public Doraemon doraemon = new Doraemon();


        // Additional Object
        Pipe pipe = new Pipe();

        // Camera
        private Camera _camera;
        private bool _firstMove = true;
        private Vector2 _lastPos;

        // Environment
        map lantai = new map();

        Building kiri1 = new Building();
        Building kiri2 = new Building();

        Building depan1 = new Building();
        Building depan2 = new Building();
        Building depan3 = new Building();
        Building depan4 = new Building();

        Building belakang1 = new Building();
        Building belakang2 = new Building();
        Building belakang3 = new Building();
        Building belakang4 = new Building();

        Building kanan1 = new Building();
        Building kanan2 = new Building();
        Building kanan3 = new Building();
        Building kanan4 = new Building();



        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            
        }

        protected override void OnLoad()
        {
            CursorGrabbed = true;

            GL.ClearColor(0.5f, 0.8f, 1f, 0.0f);
            GL.Enable(EnableCap.DepthTest);

            _camera = new Camera(new Vector3(0f,15f, 15f), Size.X / (float)Size.Y);

            //--------------------------Koro--------------------------
            koroSensei.load(Size.X, Size.Y);

            //--------------------------Sally--------------------------
            sallyRed.LoadSally(Size.X, Size.Y, new Vector3(0.7f, 0.0f, 0.0f), new Vector3(0f, 7.5f, -19.7f));
            sallyRed.SitPosition();


            sallyRefree.LoadSally(Size.X, Size.Y, new Vector3(0.0f, 0f, 0.0f), new Vector3(5f, 20f, 0.0f));
            sallyRefree.FlyPosition();

            //--------------------------Doraemon--------------------------
            doraemon.OnLoad(Size.X, Size.Y);

            //--------------------------Enviroment--------------------------
            lantai.OnLoad(0.0f, 0.0f, 0.0f, 30f);

            depan1.OnLoad(-30f, 7.5f, -22.5f, 7.5f, "tegak", 0);
            depan2.OnLoad(-30f, 7.5f, -7.5f, 7.5f, "tegak", 0);
            depan3.OnLoad(-30f, 7.5f, 7.5f, 7.5f, "tegak", 0);
            depan4.OnLoad(-30f, 7.5f, 22.5f, 7.5f, "tegak", 0);

            /* kiri1.OnLoad(-15f, 15f, -30.0f, 15f, "depan", 0);
             kiri2.OnLoad(15f, 15f, -30.0f, 15f, "depan", 0);*/

            kanan1.OnLoad(-22.5f, 7.5f, -30.0f, 7.5f, "depan", 0);
            kanan2.OnLoad(-7.5f, 7.5f, -30.0f, 7.5f, "depan", 0);
            kanan3.OnLoad(7.5f, 7.5f, -30.0f, 7.5f, "depan", 0);
            kanan4.OnLoad(22.5f, 7.5f, -30.0f, 7.5f, "depan", 0);

            belakang1.OnLoad(30f, 7.5f, -22.5f, 7.5f, "tegak", 0);
            belakang2.OnLoad(30f, 7.5f, -7.5f, 7.5f, "tegak", 0);
            belakang3.OnLoad(30f, 7.5f, 7.5f, 7.5f, "tegak", 0);
            belakang4.OnLoad(30f, 7.5f, 22.5f, 7.5f, "tegak", 0);
            // Additional Object
            pipe.load(Size.X, Size.Y);
            base.OnLoad();
        }


        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            float time = (float)args.Time; //Deltatime ==> waktu antara frame sebelumnya ke frame berikutnya, gunakan untuk animasi

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit); // DepthBufferBit juga harus di clear karena kita memakai depth testing.

            
            koroSensei.render(_camera);
            koroSensei.animate(args, time);

            sallyRed.RenderSally(_camera);
            sallyRed.SallySitFeetSwing(time);

            sallyRefree.RenderSally(_camera);
            sallyRefree.SallyFly(time);

            doraemon.OnRenderFrame(args, _camera);
            // Additional object
            pipe.render(_camera);

            // Environment
            lantai.render(_camera);

            /* kiri1.render(_camera);
             kiri2.render(_camera);*/

            depan1.render(_camera);
            depan2.render(_camera);
            depan3.render(_camera);
            depan4.render(_camera);

            belakang1.render(_camera);
            belakang2.render(_camera);
            belakang3.render(_camera);
            belakang4.render(_camera);

            kanan1.render(_camera);
            kanan2.render(_camera);
            kanan3.render(_camera);
            kanan4.render(_camera);




            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            float time = (float)args.Time; //Deltatime ==> waktu antara frame sebelumnya ke frame berikutnya, gunakan untuk animasi

            if (!IsFocused)
            {
                return; //Reject semua input saat window bukan focus.
            }

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }


            const float cameraSpeed = 3.5f;
            const float sensitivity = 0.25f;

            if (input.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * cameraSpeed * (float)args.Time; // Forward
            }

            if (input.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * cameraSpeed * (float)args.Time; // Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * cameraSpeed * (float)args.Time; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * cameraSpeed * (float)args.Time; // Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                _camera.Position += _camera.Up * cameraSpeed * (float)args.Time; // Up
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                _camera.Position -= _camera.Up * cameraSpeed * (float)args.Time; // Down
            }
            if(input.IsKeyReleased(Keys.F))
            {
                sallyRefree.canFly = !sallyRefree.canFly;
            }

            // Get the mouse state
            var mouse = MouseState;


            if (_firstMove) // This bool variable is initially set to true.
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            _camera.Fov -= e.OffsetY;
        }


        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
	    _camera.AspectRatio = Size.X / (float)Size.Y;
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            doraemon.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);
            doraemon.OnKeyUp(e);
        }


        /*protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButton.Left)
            {
                float _x = (MousePosition.X - Size.X / 2) / (Size.X / 2);
                float _y = -(MousePosition.Y - Size.Y / 2) / (Size.Y / 2);

                Console.WriteLine("x: " + _x + ", y: " + _y);
                koroSensei.updateMousePosition(_x, _y);

            }

        }*/



        /*public void rotateControl()
        {
            if (KeyboardState.IsKeyDown(Keys.W))
            {
                KORO[0].rotate(KORO[0].objectCenter, KORO[0]._euler[0], -5);
            }
            if (KeyboardState.IsKeyDown(Keys.S))
            {
                KORO[0].rotate(KORO[0].objectCenter, KORO[0]._euler[0], 5);
            }
            if (KeyboardState.IsKeyDown(Keys.A))
            {
                KORO[0].rotate(KORO[0].objectCenter, KORO[0]._euler[1], -5);
            }
            if (KeyboardState.IsKeyDown(Keys.D))
            {
                KORO[0].rotate(KORO[0].objectCenter, KORO[0]._euler[1], 5);
            }
            if (KeyboardState.IsKeyDown(Keys.Q))
            {
                KORO[0].rotate(KORO[0].objectCenter, KORO[0]._euler[2], -5);
            }
            if (KeyboardState.IsKeyDown(Keys.E))
            {
                KORO[0].rotate(KORO[0].objectCenter, KORO[0]._euler[2], 5);
            }
            if (KeyboardState.IsKeyDown(Keys.P))
            {
                KORO[0].reset();
            }
        }*/

        /*public void scalingControl()
        {
            if (KeyboardState.IsKeyDown(Keys.Up))
            {
                KORO[0].translate(0, 0, 0.1f);
            }
            if (KeyboardState.IsKeyDown(Keys.Down))
            {
                KORO[0].translate(0, 0, -0.1f);
            }
        }*/

    }
}
