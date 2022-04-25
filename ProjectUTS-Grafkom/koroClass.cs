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
    class koroClass
    {
        private readonly string path = "../../../";
        List<AssetKoro> KORO = new List<AssetKoro>();

        List<AssetKoro> Pivot = new List<AssetKoro>();


        Dictionary<string, int> textures = new Dictionary<string, int>();
        //Dictionary<string, ShaderProgram> shaders = new Dictionary<string, ShaderProgram>();
        //string activeShader = "default";

        public Vector2 _Size;

        float _posX;
        float _posY;

        // Fly pivot
        int flyAngle = 1;

        // Switch-case animate
        float counter = 0;
        float inner_counter = 0;

        // Head
        float angleHead;
        float angleHPoint = 1f;

        // Right Hand
        float angle;
        float angleSum = 1f;

        // Left Hand
        float angle2;
        float angleSum2 = 1f;

        public koroClass(float _posX = 0.0f, float _posY = 0.0f)
        {
            this._posX = _posX;
            this._posY = _posY;
        }

        

        public void load(int X, int Y)
        {
            _Size = new Vector2(X, Y);
            
            // Body
            var koroBody = new AssetKoro(new Vector3(0f, 0f, 0));
            koroBody.createBody(0f, -3.7f, 0f, 1.5f, 2.5f, 1.1f, 50, 50);
            KORO.Add(koroBody);


            /*AssetKoro koroHead = AssetKoro.LoadFromFile(path + "Kepala.obj");
            koroBody.child.Add(koroHead);*/

            /*model a = model.LoadFromFile(path + "Kepala.obj");
            a.TextureID = textures["Koro face.jpeg"];
            Model.Add(a);*/


            // Head and Utensils
            // KORO[0].child[0]
            var koroHead = new AssetKoro(new Vector3(1f, 1f, 0));
            koroHead.createEllipsoid(0f, 2f, 0f, 0.8f, 0.8f, 0.8f, 100, 100);
            koroBody.child.Add(koroHead);

            // KORO[0].child[0].child[0]
            var koroCap = new AssetKoro(new Vector3(0f, 0f, 0));
            koroCap.createCap(0f, 3f, 0f, 0.1f);
            koroHead.child.Add(koroCap);

            // KORO[0].child[0].child[1]
            var koroCapTop = new AssetKoro(new Vector3(0f, 0f, 0));
            koroCapTop.createCapTop(0f, 3.4f, 0f, 0.75f);
            koroCapTop.rotate(koroCapTop.objectCenter, koroCapTop._euler[1], 45);
            koroHead.child.Add(koroCapTop);

            // KORO[0].child[0].child[2]
            var koroEyeR = new AssetKoro(new Vector3(0f, 0f, 0));
            koroEyeR.createEllipsoid(0.3f, 2.15f, 0.75f, 0.04f, 0.04f, 0.03f, 100, 100);
            koroHead.child.Add(koroEyeR);

            // KORO[0].child[0].child[3]
            var koroEyeL = new AssetKoro(new Vector3(0f, 0f, 0));
            koroEyeL.createEllipsoid(-0.3f, 2.15f, 0.75f, 0.04f, 0.04f, 0.03f, 100, 100);
            koroHead.child.Add(koroEyeL);

            // KORO[0].child[0].child[4]
            var innerEyeR = new AssetKoro(new Vector3(1f, 1f, 1f));
            innerEyeR.createEllipsoid(0.3f, 2.15f, 0.77f, 0.02f, 0.02f, 0.015f, 100, 100);
            koroHead.child.Add(innerEyeR);

            // KORO[0].child[0].child[5]
            var innerEyeL = new AssetKoro(new Vector3(1f, 1f, 1f));
            innerEyeL.createEllipsoid(-0.3f, 2.15f, 0.77f, 0.02f, 0.02f, 0.015f, 100, 100);
            koroHead.child.Add(innerEyeL);

            // KORO[0].child[0].child[6]
            var koroMouth = new AssetKoro(new Vector3(1f, 1f, 1f));
            koroMouth.createEllipsoid2(0f, 1.66f, 0.3f, 0.55f, 0.24f, 0.45f, 400.0f, 400.0f);
            koroHead.child.Add(koroMouth);

            // KORO[0].child[0].child[7]
            var koroMouthOuter = new AssetKoro(new Vector3(0f, 0f, 0));
            koroMouthOuter.createEllipsoid2(0f, 1.67f, 0.291f, 0.569f, 0.253f, 0.45f, 400.0f, 400.0f);
            koroHead.child.Add(koroMouthOuter);


            // Body-Collar
            // KORO[0].child[1]
            var collarInner = new AssetKoro(new Vector3(0f, 0f, 0));
            collarInner.createBody(0f, 0.6f, 0f, 0.85f, 0.3f, 0.7f, 50, 50);
            koroBody.child.Add(collarInner);

            // KORO[0].child[2]
            var collarOuter = new AssetKoro(new Vector3(1f, 1f, 0));
            collarOuter.createBody(0f, 0.55f, 0f, 0.85f, 0.3f, 0.7f, 50, 50);
            koroBody.child.Add(collarOuter);

            // KORO[0].child[3]
            var collarTop = new AssetKoro(new Vector3(1f, 1f, 1f));
            collarTop.createCollarTop(0f, 0.85f, 0f, 0.45f, 0.5f, 0.65f, 100, 100);
            koroBody.child.Add(collarTop);


            // Body-Tie
            // KORO[0].child[4]
            var koroTieL = new AssetKoro(new Vector3(1f, 1f, 1f));
            koroTieL.createTieL(0.35f, 0.975f, 0.65f, 0.5f);
            koroBody.child.Add(koroTieL);

            // KORO[0].child[5]
            var koroTieR = new AssetKoro(new Vector3(1f, 1f, 1f));
            koroTieR.createTieR(-0.35f, 0.975f, 0.65f, 0.5f);
            koroBody.child.Add(koroTieR);

            // KORO[0].child[6]
            var koroTieM = new AssetKoro(new Vector3(0f, 0f, 0));
            koroTieM.createEllipsoid(0f, 0.9f, 0.55f, 0.15f, 0.15f, 0.1f, 100, 100);
            koroBody.child.Add(koroTieM);

            // KORO[0].child[7]
            var koroTieD = new AssetKoro(new Vector3(1f, 1f, 0));
            koroTieD.createTieD(0f, 0.675f, 0.6f, 0.5f);
            koroTieD.rotate(koroTieD.objectCenter, koroTieD._euler[0], 25);
            koroBody.child.Add(koroTieD);

            // KORO[0].child[8]
            var koroTieD2 = new AssetKoro(new Vector3(1f, 1f, 0));
            koroTieD2.createTieD2(0f, 1.15f, 0.12f, 0.5f);
            koroTieD2.rotate(koroTieD2.objectCenter, koroTieD2._euler[0], 25);
            koroBody.child.Add(koroTieD2);



            // MOVEMENT PART OF THE BODY -- HAND AND FOOT

            // Hand for Clothes
            // Right Hand

            // Joint Ball Right Hand
            // KORO[0].child[9]
            var jointR = new AssetKoro(new Vector3(1f, 0f, 0f));
            jointR.createEllipsoid(-1f, 0.15f, 0f, 0.1f, 0.1f, 0.1f, 100, 100);
            koroBody.child.Add(jointR);

            // KORO[0].child[9].child[0]
            var koroHandR = new AssetKoro(new Vector3(0.319f, 0.167f, 0.365f));
            koroHandR.createHand(-0.5f, -1.4f, 0f, 1.25f, 2.25f, 0.45f, 100, 100);
            jointR.child.Add(koroHandR);

            // KORO[0].child[9].child[0].child[0]
            var GradientR1 = new AssetKoro(new Vector3(0f, 0f, 0));
            GradientR1.createHandStrip(-0.95f, -0.55f, 0f, 0.115f, 0.5f, -0.75f);
            GradientR1.rotate(GradientR1.objectCenter, GradientR1._euler[2], -30);
            koroHandR.child.Add(GradientR1);

            // KORO[0].child[9].child[0].child[1]
            var GradientR2 = new AssetKoro(new Vector3(0f, 0f, 0));
            GradientR2.createHandStrip(-1.05f, -0.95f, 0f, 0.115f, 0.6f, -0.6f);
            GradientR2.rotate(GradientR2.objectCenter, GradientR2._euler[2], -20);
            koroHandR.child.Add(GradientR2);


            // Left Hand
            // Joint Ball Left Hand
            // KORO[0].child[10]
            var jointL = new AssetKoro(new Vector3(1f, 0f, 0f));
            jointL.createEllipsoid(1f, 0.15f, 0f, 0.1f, 0.1f, 0.1f, 100, 100);
            koroBody.child.Add(jointL);

            // KORO[0].child[10].child[0]
            var koroHandL = new AssetKoro(new Vector3(0.319f, 0.167f, 0.365f));
            koroHandL.createHand(0.5f, -1.4f, 0f, 1.25f, 2.25f, 0.45f, 100, 100);
            jointL.child.Add(koroHandL);

            // KORO[0].child[10].child[0].child[0]
            var GradientL1 = new AssetKoro(new Vector3(0f, 0f, 0));
            GradientL1.createHandStrip(0.95f, -0.55f, 0f, 0.115f, 0.5f, 0.75f);
            GradientL1.rotate(GradientL1.objectCenter, GradientL1._euler[2], 30);
            koroHandL.child.Add(GradientL1);

            // KORO[0].child[10].child[0].child[1]
            var GradientL2 = new AssetKoro(new Vector3(0f, 0f, 0));
            GradientL2.createHandStrip(1.05f, -0.95f, 0f, 0.115f, 0.6f, 0.6f);
            GradientL2.rotate(GradientL2.objectCenter, GradientL2._euler[2], 20);
            koroHandL.child.Add(GradientL2);


            // Hand for Touch
            // Right Hand
            // KORO[0].child[9].child[0].child[2]
            var koroLongHandR = new AssetKoro(new Vector3(1f, 1f, 0));
            koroLongHandR.createHandDown(-0.85f, 0f, 2.75f, 0.125f, 2.75f, 0.65f);
            koroLongHandR.rotate(koroBody.objectCenter, koroBody._euler[0], 90);
            koroLongHandR.rotate(koroBody.objectCenter, koroBody._euler[2], -30);
            koroHandR.child.Add(koroLongHandR);

            // KORO[0].child[9].child[0].child[2].child[0]
            var koroPalmR = new AssetKoro(new Vector3(1f, 1f, 0));
            koroPalmR.createEllipsoid(-2.75f, -3.075f, 0f, 0.225f, 0.225f, 0.225f, 100, 100);
            koroLongHandR.child.Add(koroPalmR);

            // KORO[0].child[9].child[0].child[2].child[0].child[0]
            var koroFingerR1 = new AssetKoro(new Vector3(1f, 1f, 0));
            koroFingerR1.createHandStrip(-2.4f, -2.675f, -1.4f, 0.05f, 1f, -0.35f);
            koroFingerR1.rotate(koroBody.objectCenter, koroBody._euler[0], -60);
            koroFingerR1.rotate(koroBody.objectCenter, koroBody._euler[1], -30);
            koroPalmR.child.Add(koroFingerR1);

            // KORO[0].child[9].child[0].child[2].child[0].child[1]
            var koroFingerR2 = new AssetKoro(new Vector3(1f, 1f, 0));
            koroFingerR2.createHandStrip(-2.4f, -3.35f, 0.15f, 0.05f, 1f, -0.35f);
            koroFingerR2.rotate(koroBody.objectCenter, koroBody._euler[0], -30);
            koroFingerR2.rotate(koroBody.objectCenter, koroBody._euler[1], -30);
            koroPalmR.child.Add(koroFingerR2);

            /*var GloveR = new AssetKoro(new Vector3(0.435f, 0.372f, 0.279f));
            GloveR.createEllipsoid(-2.75f, -3.075f, 0f, 0.775f, 0.7f, 0.325f, 100, 100);
            koroPalmR.child.Add(GloveR);*/


            // Left Hand
            // KORO[0].child[10].child[0].child[2]
            var koroLongHandL = new AssetKoro(new Vector3(1f, 1f, 0));
            koroLongHandL.createHandDown(-0.85f, 0f, 2.8f, 0.125f, 2.75f, 0.65f);
            koroLongHandL.rotate(koroBody.objectCenter, koroBody._euler[0], 90);
            koroLongHandL.rotate(koroBody.objectCenter, koroBody._euler[1], 180);
            koroLongHandL.rotate(koroBody.objectCenter, koroBody._euler[2], 30);
            koroHandL.child.Add(koroLongHandL);

            // KORO[0].child[10].child[0].child[2].child[0]
            var koroPalmL = new AssetKoro(new Vector3(1f, 1f, 0));
            koroPalmL.createEllipsoid(2.75f, -3.075f, 0f, 0.225f, 0.225f, 0.225f, 100, 100);
            koroLongHandL.child.Add(koroPalmL);

            // KORO[0].child[10].child[0].child[2].child[0].child[0]
            var koroFingerL1 = new AssetKoro(new Vector3(1f, 1f, 0));
            koroFingerL1.createHandStrip(2.25f, -2.75f, -1.4f, 0.05f, 1f, 0.35f);
            koroFingerL1.rotate(koroBody.objectCenter, koroBody._euler[0], -60);
            koroFingerL1.rotate(koroBody.objectCenter, koroBody._euler[1], 30);
            koroPalmL.child.Add(koroFingerL1);

            // KORO[0].child[10].child[0].child[2].child[0].child[1]
            var koroFingerL2 = new AssetKoro(new Vector3(1f, 1f, 0));
            koroFingerL2.createHandStrip(2.4f, -3.4f, 0.15f, 0.05f, 1f, 0.35f);
            koroFingerL2.rotate(koroBody.objectCenter, koroBody._euler[0], -30);
            koroFingerL2.rotate(koroBody.objectCenter, koroBody._euler[1], 30);
            koroPalmL.child.Add(koroFingerL2);


            // Foot
            // Right Side
            // KORO[0].child[11]
            var koroFoot1 = new AssetKoro(new Vector3(1f, 1f, 0));
            koroFoot1.createFoot(1.5f, 0f, 3.65f, 0.75f, 2.4f, 1.05f, 1.5f);
            koroFoot1.rotate(koroBody.objectCenter, koroBody._euler[0], 90);
            koroFoot1.rotate(koroBody.objectCenter, koroBody._euler[2], -45);
            koroBody.child.Add(koroFoot1);

            // Left Side
            // KORO[0].child[12]
            var koroFoot2 = new AssetKoro(new Vector3(0.865f, 0.842f, 0.162f));
            koroFoot2.createFoot(1.5f, -0f, 3.65f, 0.75f, 2.4f, 1.05f, 1.5f);
            koroFoot2.rotate(koroBody.objectCenter, koroBody._euler[0], -90);
            koroFoot2.rotate(koroBody.objectCenter, koroBody._euler[2], -135);
            koroBody.child.Add(koroFoot2);

            // Right - Front
            // KORO[0].child[13]
            var koroFoot3 = new AssetKoro(new Vector3(0.865f, 0.842f, 0.162f));
            koroFoot3.createFoot(1.5f, 0f, 3.65f, 0.75f, 2.4f, 1.05f, 1.5f);
            koroFoot3.rotate(koroBody.objectCenter, koroBody._euler[0], 60);
            koroFoot3.rotate(koroBody.objectCenter, koroBody._euler[1], 30);
            koroFoot3.rotate(koroBody.objectCenter, koroBody._euler[2], -45);
            koroBody.child.Add(koroFoot3);

            // Left - Front
            // KORO[0].child[14]
            var koroFoot4 = new AssetKoro(new Vector3(1f, 1f, 0));
            koroFoot4.createFoot(1.5f, 0f, 3.65f, 0.75f, 2.4f, 1.05f, 1.5f);
            koroFoot4.rotate(koroBody.objectCenter, koroBody._euler[0], -60);
            koroFoot4.rotate(koroBody.objectCenter, koroBody._euler[1], 30);
            koroFoot4.rotate(koroBody.objectCenter, koroBody._euler[2], -135);
            koroBody.child.Add(koroFoot4);

            // Right - Back
            // KORO[0].child[15]
            var koroFoot5 = new AssetKoro(new Vector3(0.865f, 0.842f, 0.162f));
            koroFoot5.createFoot(1.5f, 0f, -3.65f, 0.75f, 2.4f, 1.05f, 1.5f);
            koroFoot5.rotate(koroBody.objectCenter, koroBody._euler[0], -60);
            koroFoot5.rotate(koroBody.objectCenter, koroBody._euler[1], -30);
            koroFoot5.rotate(koroBody.objectCenter, koroBody._euler[2], -45);
            koroBody.child.Add(koroFoot5);

            /*var koroFoot5P = new AssetKoro(new Vector3(1f, 1f, 0));
            koroFoot5P.createEllipsoid(1.45f, -4.825f, -1.6f, 0.65f, 0.65f, 1f, 100, 100);
            koroFoot5P.rotate(KORO[0].objectCenter, Vector3.UnitY, -5);
            koroFoot5.child.Add(koroFoot5P);*/


            // Left - Back
            // KORO[0].child[16]
            var koroFoot6 = new AssetKoro(new Vector3(1f, 1f, 0));
            koroFoot6.createFoot(1.5f, 0f, -3.65f, 0.75f, 2.4f, 1.05f, 1.5f);
            koroFoot6.rotate(koroBody.objectCenter, koroBody._euler[0], 60);
            koroFoot6.rotate(koroBody.objectCenter, koroBody._euler[1], -30);
            koroFoot6.rotate(koroBody.objectCenter, koroBody._euler[2], -135);
            koroBody.child.Add(koroFoot6);


            KORO[0].translate(-5.5f, 5.3f, 0);
            KORO[0].scale(1.2f, 1.2f, 1.2f);



            foreach (AssetKoro i in KORO)
            {
                i.load(X, Y);
            }
        }

        
        public void render(Camera _camera)
        {
            KORO[0].render(_camera);
            //KORO[1].render(_camera);

            /*GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);

            //shaders[activeShader].EnableVertexAttribArrays();

            int indiceat = 0;

            // Draw all our objects
            foreach (volume v in Model)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, v.TextureID);
                //GL.UniformMatrix4(shaders[activeShader].GetUniform("modelview"), false, ref v.ModelViewProjectionMatrix);

                *//*if (shaders[activeShader].GetUniform("maintexture") != -1)
                {
                    GL.Uniform1(shaders[activeShader].GetUniform("maintexture"), 0);
                }*//*

                GL.DrawElements(BeginMode.Triangles, v.IndiceCount, DrawElementsType.UnsignedInt, indiceat * sizeof(uint));
                indiceat += v.IndiceCount;
            }

            //shaders[activeShader].DisableVertexAttribArrays();

            GL.Flush();
            SwapBuffers();*/


        }


        public void animate(FrameEventArgs args, float time)
        {
            
            //float turn = 30f;

            switch (counter)
            {
                case 0:
                    // Fly
                    if (KORO[0].objectCenter.Y < 5.5f)
                    {
                        flyAngle = 1;
                    }
                    if (KORO[0].objectCenter.Y > 20f)
                    {
                        flyAngle = -1;
                    }
                    KORO[0].translate(0f, 5f / 10f * flyAngle, 0f);
                    KORO[0].rotateY_all(MathHelper.DegreesToRadians(5f * 2));

                    inner_counter += 0.1f;
                    if (inner_counter > 18f)
                    {
                        counter++;
                        inner_counter = 0;
                    }
                    break;

                case 1:
                    KORO[0].translate(0, 0, 0.15f);

                    // Head
                    if (angleHead > 45f)
                    {
                        angleHPoint = -1f;
                    }
                    else if (angleHead < -45f)
                    {
                        angleHPoint = 1f;
                    }

                    angleHead += time * 30f * angleHPoint;
                    KORO[0].child[0].rotate(KORO[0].child[0].objectCenter, KORO[0].child[0]._euler[1], angleHPoint * 30f * time);

                    // Right Hand
                    if (angle > 0f)
                    {
                        angleSum = -1f;
                    }
                    else if (angle < -30f)
                    {
                        angleSum = 1f;
                    }

                    angle += time * 15f * angleSum;
                    KORO[0].child[9].rotate(KORO[0].child[9].objectCenter, KORO[0].child[9]._euler[2], angleSum * 15f * time);

                    KORO[0].child[9].child[0].child[2].child[0].rotate(KORO[0].child[9].child[0].child[2].child[0].objectCenter,
                       KORO[0].child[9].child[0].child[2].child[0]._euler[2],
                       angleSum * 15f * time);


                    // Left Hand
                    if (angle2 < 0f)
                    {
                        angleSum2 = -1f;
                    }
                    else if (angle2 > 30f)
                    {
                        angleSum2 = 1f;
                    }

                    angle2 += time * -15f * angleSum2;
                    KORO[0].child[10].rotate(KORO[0].child[10].objectCenter, KORO[0].child[10]._euler[2], angleSum2 * -15f * time);

                    KORO[0].child[10].child[0].child[2].child[0].rotate(KORO[0].child[10].child[0].child[2].child[0].objectCenter,
                       KORO[0].child[10].child[0].child[2].child[0]._euler[2],
                       angleSum2 * -15f * time);

                    inner_counter += 0.05f;
                    if (inner_counter > 4f)
                    {
                        counter++;
                        inner_counter = 0;
                    }
                    break;

                case 2:
                    KORO[0].rotate(KORO[0].objectCenter, KORO[0]._euler[1], 60 * 0.093f);
                    inner_counter += 0.01f;
                    if (inner_counter > 0.15f)
                    {
                        counter++;
                        inner_counter = 0;
                    }
                    break;

                case 3:
                    KORO[0].translate(0.15f, 0, 0);

                    // Head
                    if (angleHead > 45f)
                    {
                        angleHPoint = -1f;
                    }
                    else if (angleHead < -45f)
                    {
                        angleHPoint = 1f;
                    }

                    angleHead += time * 30f * angleHPoint;
                    KORO[0].child[0].rotate(KORO[0].child[0].objectCenter, KORO[0].child[0]._euler[1], angleHPoint * 30f * time);

                    // Right Hand
                    if (angle > 0f)
                    {
                        angleSum = -1f;
                    }
                    else if (angle < -30f)
                    {
                        angleSum = 1f;
                    }

                    angle += time * 15f * angleSum;
                    KORO[0].child[9].rotate(KORO[0].child[9].objectCenter, KORO[0].child[9]._euler[2], angleSum * 15f * time);

                    KORO[0].child[9].child[0].child[2].child[0].rotate(KORO[0].child[9].child[0].child[2].child[0].objectCenter,
                       KORO[0].child[9].child[0].child[2].child[0]._euler[2],
                       angleSum * 15f * time);


                    // Left Hand
                    if (angle2 < 0f)
                    {
                        angleSum2 = -1f;
                    }
                    else if (angle2 > 30f)
                    {
                        angleSum2 = 1f;
                    }

                    angle2 += time * -15f * angleSum2;
                    KORO[0].child[10].rotate(KORO[0].child[10].objectCenter, KORO[0].child[10]._euler[2], angleSum2 * -15f * time);

                    KORO[0].child[10].child[0].child[2].child[0].rotate(KORO[0].child[10].child[0].child[2].child[0].objectCenter,
                       KORO[0].child[10].child[0].child[2].child[0]._euler[2],
                       angleSum2 * -15f * time);

                    inner_counter += 0.05f;
                    if (inner_counter > 4f)
                    {
                        counter++;
                        inner_counter = 0;
                    }
                    break;

                case 4:
                    KORO[0].rotate(KORO[0].objectCenter, KORO[0]._euler[1], 60 * 0.093f);
                    inner_counter += 0.01f;
                    if (inner_counter > 0.15f)
                    {
                        counter++;
                        inner_counter = 0;
                    }
                    break;

                case 5:
                    KORO[0].translate(0, 0, -0.15f);

                    // Head
                    if (angleHead > 45f)
                    {
                        angleHPoint = -1f;
                    }
                    else if (angleHead < -45f)
                    {
                        angleHPoint = 1f;
                    }

                    angleHead += time * 30f * angleHPoint;
                    KORO[0].child[0].rotate(KORO[0].child[0].objectCenter, KORO[0].child[0]._euler[1], angleHPoint * 30f * time);

                    // Right Hand
                    if (angle > 0f)
                    {
                        angleSum = -1f;
                    }
                    else if (angle < -30f)
                    {
                        angleSum = 1f;
                    }

                    angle += time * 15f * angleSum;
                    KORO[0].child[9].rotate(KORO[0].child[9].objectCenter, KORO[0].child[9]._euler[2], angleSum * 15f * time);

                    KORO[0].child[9].child[0].child[2].child[0].rotate(KORO[0].child[9].child[0].child[2].child[0].objectCenter,
                       KORO[0].child[9].child[0].child[2].child[0]._euler[2],
                       angleSum * 15f * time);


                    // Left Hand
                    if (angle2 < 0f)
                    {
                        angleSum2 = -1f;
                    }
                    else if (angle2 > 30f)
                    {
                        angleSum2 = 1f;
                    }

                    angle2 += time * -15f * angleSum2;
                    KORO[0].child[10].rotate(KORO[0].child[10].objectCenter, KORO[0].child[10]._euler[2], angleSum2 * -15f * time);

                    KORO[0].child[10].child[0].child[2].child[0].rotate(KORO[0].child[10].child[0].child[2].child[0].objectCenter,
                       KORO[0].child[10].child[0].child[2].child[0]._euler[2],
                       angleSum2 * -15f * time);

                    inner_counter += 0.05f;
                    if (inner_counter > 8f)
                    {
                        counter++;
                        inner_counter = 0;
                    }
                    break;

                case 6:
                    KORO[0].rotate(KORO[0].objectCenter, KORO[0]._euler[1], 60 * 0.093f);
                    inner_counter += 0.01f;
                    if (inner_counter > 0.15f)
                    {
                        counter++;
                        inner_counter = 0;
                    }
                    break;

                case 7:
                    KORO[0].translate(-0.15f, 0, 0);

                    // Head
                    if (angleHead > 45f)
                    {
                        angleHPoint = -1f;
                    }
                    else if (angleHead < -45f)
                    {
                        angleHPoint = 1f;
                    }

                    angleHead += time * 30f * angleHPoint;
                    KORO[0].child[0].rotate(KORO[0].child[0].objectCenter, KORO[0].child[0]._euler[1], angleHPoint * 30f * time);

                    // Right Hand
                    if (angle > 0f)
                    {
                        angleSum = -1f;
                    }
                    else if (angle < -30f)
                    {
                        angleSum = 1f;
                    }

                    angle += time * 15f * angleSum;
                    KORO[0].child[9].rotate(KORO[0].child[9].objectCenter, KORO[0].child[9]._euler[2], angleSum * 15f * time);

                    KORO[0].child[9].child[0].child[2].child[0].rotate(KORO[0].child[9].child[0].child[2].child[0].objectCenter,
                       KORO[0].child[9].child[0].child[2].child[0]._euler[2],
                       angleSum * 15f * time);


                    // Left Hand
                    if (angle2 < 0f)
                    {
                        angleSum2 = -1f;
                    }
                    else if (angle2 > 30f)
                    {
                        angleSum2 = 1f;
                    }

                    angle2 += time * -15f * angleSum2;
                    KORO[0].child[10].rotate(KORO[0].child[10].objectCenter, KORO[0].child[10]._euler[2], angleSum2 * -15f * time);

                    KORO[0].child[10].child[0].child[2].child[0].rotate(KORO[0].child[10].child[0].child[2].child[0].objectCenter,
                       KORO[0].child[10].child[0].child[2].child[0]._euler[2],
                       angleSum2 * -15f * time);

                    inner_counter += 0.05f;
                    if (inner_counter > 8f)
                    {
                        counter++;
                        inner_counter = 0;
                    }
                    break;

                case 8:
                    KORO[0].rotate(KORO[0].objectCenter, KORO[0]._euler[1], 60 * 0.093f);
                    inner_counter += 0.01f;
                    if (inner_counter > 0.15f)
                    {
                        counter++;
                        inner_counter = 0;
                    }
                    break;

                case 9:
                    KORO[0].translate(0, 0, 0.15f);

                    // Head
                    if (angleHead > 45f)
                    {
                        angleHPoint = -1f;
                    }
                    else if (angleHead < -45f)
                    {
                        angleHPoint = 1f;
                    }

                    angleHead += time * 30f * angleHPoint;
                    KORO[0].child[0].rotate(KORO[0].child[0].objectCenter, KORO[0].child[0]._euler[1], angleHPoint * 30f * time);

                    // Right Hand
                    if (angle > 0f)
                    {
                        angleSum = -1f;
                    }
                    else if (angle < -30f)
                    {
                        angleSum = 1f;
                    }

                    angle += time * 15f * angleSum;
                    KORO[0].child[9].rotate(KORO[0].child[9].objectCenter, KORO[0].child[9]._euler[2], angleSum * 15f * time);

                    KORO[0].child[9].child[0].child[2].child[0].rotate(KORO[0].child[9].child[0].child[2].child[0].objectCenter,
                       KORO[0].child[9].child[0].child[2].child[0]._euler[2],
                       angleSum * 15f * time);


                    // Left Hand
                    if (angle2 < 0f)
                    {
                        angleSum2 = -1f;
                    }
                    else if (angle2 > 30f)
                    {
                        angleSum2 = 1f;
                    }

                    angle2 += time * -15f * angleSum2;
                    KORO[0].child[10].rotate(KORO[0].child[10].objectCenter, KORO[0].child[10]._euler[2], angleSum2 * -15f * time);

                    KORO[0].child[10].child[0].child[2].child[0].rotate(KORO[0].child[10].child[0].child[2].child[0].objectCenter,
                       KORO[0].child[10].child[0].child[2].child[0]._euler[2],
                       angleSum2 * -15f * time);

                    inner_counter += 0.05f;
                    if (inner_counter > 4f)
                    {
                        counter++;
                        inner_counter = 0;
                    }
                    break;

                case 10:
                    KORO[0].rotate(KORO[0].objectCenter, KORO[0]._euler[1], 60 * 0.093f);
                    inner_counter += 0.01f;
                    if (inner_counter > 0.15f)
                    {
                        counter++;
                        inner_counter = 0;
                    }
                    break;

                case 11:
                    KORO[0].translate(0.15f, 0, 0);

                    // Head
                    if (angleHead > 45f)
                    {
                        angleHPoint = -1f;
                    }
                    else if (angleHead < -45f)
                    {
                        angleHPoint = 1f;
                    }

                    angleHead += time * 30f * angleHPoint;
                    KORO[0].child[0].rotate(KORO[0].child[0].objectCenter, KORO[0].child[0]._euler[1], angleHPoint * 30f * time);

                    // Right Hand
                    if (angle > 0f)
                    {
                        angleSum = -1f;
                    }
                    else if (angle < -30f)
                    {
                        angleSum = 1f;
                    }

                    angle += time * 15f * angleSum;
                    KORO[0].child[9].rotate(KORO[0].child[9].objectCenter, KORO[0].child[9]._euler[2], angleSum * 15f * time);

                    KORO[0].child[9].child[0].child[2].child[0].rotate(KORO[0].child[9].child[0].child[2].child[0].objectCenter,
                       KORO[0].child[9].child[0].child[2].child[0]._euler[2],
                       angleSum * 15f * time);


                    // Left Hand
                    if (angle2 < 0f)
                    {
                        angleSum2 = -1f;
                    }
                    else if (angle2 > 30f)
                    {
                        angleSum2 = 1f;
                    }

                    angle2 += time * -15f * angleSum2;
                    KORO[0].child[10].rotate(KORO[0].child[10].objectCenter, KORO[0].child[10]._euler[2], angleSum2 * -15f * time);

                    KORO[0].child[10].child[0].child[2].child[0].rotate(KORO[0].child[10].child[0].child[2].child[0].objectCenter,
                       KORO[0].child[10].child[0].child[2].child[0]._euler[2],
                       angleSum2 * -15f * time);

                    inner_counter += 0.05f;
                    if (inner_counter > 4f)
                    {
                        counter++;
                        inner_counter = 0;
                    }
                    break;

                case 12:
                    KORO[0].rotate(KORO[0].objectCenter, KORO[0]._euler[1], -60 * 0.093f);
                    inner_counter += 0.01f;
                    if (inner_counter > 0.15f)
                    {
                        counter = 0;
                        inner_counter = 0;
                    }
                    break;
            }


            /*switch (counter)
            {
                
                case 0:
                    KORO[0].child[9].rotate(KORO[0].child[9].objectCenter, KORO[0].child[9]._euler[2],  -45f * 0.05f);
                    inner_counter += 0.1f;
                    if (inner_counter > 2.5f)
                    {
                        counter++;
                        inner_counter = 0;
                    }
                    break;


                case 1:
                    KORO[0].child[9].rotate(KORO[0].child[9].objectCenter, KORO[0].child[9]._euler[2], 0f * 0.05f);
                    inner_counter += 0.1f;
                    if (inner_counter > 5f)
                    {
                        counter++;
                        inner_counter = 0;
                    }
                    break;

                case 2:
                    KORO[0].child[9].rotate(KORO[0].child[9].objectCenter, KORO[0].child[9]._euler[2], 45f * 0.05f);
                    inner_counter += 0.1f;
                    if (inner_counter > 2.5f)
                    {
                        counter = 0;
                        inner_counter = 0;
                    }
                    break;


                case 3:
                    if (KORO[0].objectCenter.Y < 5.5f)
                    {
                        flyAngle = 1;
                    }
                    if (KORO[0].objectCenter.Y > 20f)
                    {
                        flyAngle = -1;
                    }
                    KORO[0].translate(0f, 5f / 10f * flyAngle, 0f);
                    KORO[0].rotateY_all(MathHelper.DegreesToRadians(5f * 2));

                    //inner_counter += 0.1f;
                    if (countTime > 24f)
                    {
                        counter++;
                        inner_counter = 0;
                        countTime = 0;
                        reset();
                    }
                    break;

            }*/


        }

        
        public void reset()
        {
            KORO.Clear();
            load((int)_Size.X, (int)_Size.Y);
        }


        public void rotateControl(KeyboardState input)
        {
            
            if (input.IsKeyDown(Keys.W))
            {
                KORO[0].rotate(KORO[0].objectCenter, KORO[0]._euler[0], -5);
            }
            if (input.IsKeyDown(Keys.S))
            {
                KORO[0].rotate(KORO[0].objectCenter, KORO[0]._euler[0], 5);
            }
            if (input.IsKeyDown(Keys.A))
            {
                KORO[0].rotate(KORO[0].objectCenter, KORO[0]._euler[1], -5);
            }
            if (input.IsKeyDown(Keys.D))
            {
                KORO[0].rotate(KORO[0].objectCenter, KORO[0]._euler[1], 5);
            }
            if (input.IsKeyDown(Keys.Q))
            {
                KORO[0].rotate(KORO[0].objectCenter, KORO[0]._euler[2], -5);
            }
            if (input.IsKeyDown(Keys.E))
            {
                KORO[0].rotate(KORO[0].objectCenter, KORO[0]._euler[2], 5);
            }
            if (input.IsKeyDown(Keys.P))
            {
                KORO[0].reset();
            }
        }

        public void scalingControl(KeyboardState input)
        {
            if (input.IsKeyDown(Keys.N))
            {
                KORO[0].translate(0, 0, 0.25f);
            }
            if (input.IsKeyDown(Keys.M))
            {
                KORO[0].translate(0, 0, -0.25f);
            }
            if (input.IsKeyDown(Keys.Up))
            {
                KORO[0].translate(0, 0.5f, 0);
            }
            if (input.IsKeyDown(Keys.Down))
            {
                KORO[0].translate(0, -0.5f, 0);
            }
            if (input.IsKeyDown(Keys.Right))
            {
                KORO[0].translate(0.5f, 0, 0);
            }
            if (input.IsKeyDown(Keys.Left))
            {
                KORO[0].translate(-0.5f, 0, 0);
            }

        }

    }
}
