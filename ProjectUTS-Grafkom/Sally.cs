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
    internal class Sally
    {
        #region component
        List<Sally3DAssets> SallyObjectList = new List<Sally3DAssets>();
        Sally3DAssets body;
        Sally3DAssets sallyHead;
        Sally3DAssets eyeRight;
        Sally3DAssets eyeLeft;
        Sally3DAssets mouthTop;
        Sally3DAssets mouthBottom;
        Sally3DAssets wingRight;
        Sally3DAssets wingLeft;
        Sally3DAssets shinLeft;
        Sally3DAssets feetLeft;
        Sally3DAssets shinRight;
        Sally3DAssets feetRight;
        Sally3DAssets HatBase;
        Sally3DAssets HatFront;
        Sally3DAssets polaTopi1;
        Sally3DAssets polaTopi2;
        Sally3DAssets polaTopi3;
        Sally3DAssets polaTopi4;
        Sally3DAssets polaTopi5;

        #endregion
        public void LoadSally(int sizeX, int sizeY, Vector3 HatColor, Vector3 InitPos)
        {
            //Body
            body = new Sally3DAssets(new Vector3(1f, 1f, 0));
            body.CreateEllipsoid(0f, -1.5f, 0f, 0.875f, 0.975f, 0.875f, 100, 100);
            SallyObjectList.Add(body);

            //head
            sallyHead = new Sally3DAssets(new Vector3(1f, 1f, 0));
            sallyHead.CreateEllipsoid(0f, -.35f, 0f, 1f, 1f, 1f, 100, 100);
            body.child.Add(sallyHead);

            //mata
            eyeRight = new Sally3DAssets(new Vector3(0f, 0f, 0));
            eyeRight.CreateEllipsoid(0.135f, 0f, 0.9f, 0.1f, 0.1f, 0.1f, 100, 100);
            sallyHead.child.Add(eyeRight);
            eyeLeft = new Sally3DAssets(new Vector3(0f, 0f, 0));
            eyeLeft.CreateEllipsoid(-0.135f, 0f, 0.9f, 0.1f, 0.1f, 0.1f, 100, 100);
            sallyHead.child.Add(eyeLeft);


            //mulut
            mouthTop = new Sally3DAssets(new Vector3(1f, 0.5f, 0f));
            mouthTop.CreateEllipsoid(0, -0.275f, 1f, 0.275f, 0.175f, 0.1f, 100, 100);
            sallyHead.child.Add(mouthTop);
            mouthBottom = new Sally3DAssets(new Vector3(1f, .5f, 0f));
            mouthBottom.CreateEllipsoid(0, -0.45f, 1f, 0.275f, 0.175f, 0.1f, 100, 100);
            sallyHead.child.Add(mouthBottom);


            //Sayap
            wingRight = new Sally3DAssets(new Vector3(1f, 0.8f, 0));
            wingRight.CreateHalfEllipsoid(0.5f, -1.3f, 0f, 0.4f, 1f, 0.15f);
            wingRight.rotate(wingRight.objectCenter, Vector3.UnitZ, 50f);
            body.child.Add(wingRight);

            wingLeft = new Sally3DAssets(new Vector3(1f, 0.8f, 0));
            wingLeft.CreateHalfEllipsoid(-0.5f, -1.3f, 0f, 0.4f, 1f, 0.15f);
            wingLeft.rotate(wingLeft.objectCenter, Vector3.UnitZ, -50f);
            body.child.Add(wingLeft);

            //kaki
            shinLeft = new Sally3DAssets(new Vector3(1f, 0.5f, 0f));
            shinLeft.createCylinder(-0.3f, -2.4f, 0f, 0.2f, 0.65f);
            shinLeft.SetPivot(new Vector3(0f, 0.3f, 0f));
            body.child.Add(shinLeft);

            feetLeft = new Sally3DAssets(new Vector3(1f, 0.5f, 0f));
            feetLeft.CreateOneThreeEllipsoid(-0.3f, -2.7f, 0.02f, 0.22f, 0.2f, 0.33f);
            feetLeft.rotate(feetLeft.objectCenter, Vector3.UnitX, 20f);
            shinLeft.child.Add(feetLeft);

            shinRight = new Sally3DAssets(new Vector3(1f, 0.5f, 0f));
            shinRight.createCylinder(0.3f, -2.4f, 0f, 0.2f, 0.65f);
            shinRight.SetPivot(new Vector3(0f, 0.3f, 0f));
            body.child.Add(shinRight);

            feetRight = new Sally3DAssets(new Vector3(1f, 0.5f, 0f));
            feetRight.CreateOneThreeEllipsoid(0.3f, -2.7f, 0.02f, 0.22f, 0.2f, 0.33f);
            feetRight.rotate(feetLeft.objectCenter, Vector3.UnitX, 20f);
            shinRight.child.Add(feetRight);

            //Accesory
            //Topi
            HatBase = new Sally3DAssets(HatColor);
            HatBase.CreateHalfEllipsoid(0f, 0.4f, 0f, .7f, .7f, .7f);
            HatBase.rotate(HatBase.objectCenter, Vector3.UnitX, 180f);
            sallyHead.child.Add(HatBase);

            HatFront = new Sally3DAssets(HatColor);
            HatFront.CreateHalfEllipsoid(0f, 0.5f, 0.5f, .6f, .8f, .12f);
            HatFront.rotate(HatFront.objectCenter, Vector3.UnitX, -90f);
            HatBase.child.Add(HatFront);

            polaTopi1 = new Sally3DAssets(new Vector3(1f, 1f, 1f));
            polaTopi1.createBezierCylinder(0f, 0.5f, 0.27f, 0.05f, 1f, 1f);
            polaTopi1.rotate(polaTopi1.objectCenter, Vector3.UnitZ, 90f);
            HatBase.child.Add(polaTopi1);

            polaTopi2 = new Sally3DAssets(new Vector3(1f, 1f, 1f));
            polaTopi2.createBezierCylinder(0.23f, 0.5f, 0f, 0.05f, 1f, 1f);
            polaTopi2.rotate(polaTopi2.objectCenter, Vector3.UnitZ, 90f);
            polaTopi2.rotate(polaTopi2.objectCenter, Vector3.UnitY, 90f);
            HatBase.child.Add(polaTopi2);

            polaTopi3 = new Sally3DAssets(new Vector3(1f, 1f, 1f));
            polaTopi3.createBezierCylinder(-0.23f, 0.5f, 0f, 0.05f, 1f, 1f);
            polaTopi3.rotate(polaTopi3.objectCenter, Vector3.UnitZ, 90f);
            polaTopi3.rotate(polaTopi3.objectCenter, Vector3.UnitY, -90f);
            HatBase.child.Add(polaTopi3);

            polaTopi4 = new Sally3DAssets(new Vector3(1f, 1f, 1f));
            polaTopi4.createBezierCylinder(0f, 0.5f, -0.24f, 0.05f, 1f, 1f);
            polaTopi4.rotate(polaTopi4.objectCenter, Vector3.UnitZ, 90f);
            polaTopi4.rotate(polaTopi4.objectCenter, Vector3.UnitY, 180f);
            HatBase.child.Add(polaTopi4);

            polaTopi5 = new Sally3DAssets(new Vector3(1f, 1f, 1f));
            polaTopi5.CreateParaboloid(0f, 1.1f, 0f, 0.1f, 0.1f, 0.1f);
            polaTopi5.rotate(polaTopi5.objectCenter, Vector3.UnitX, -90f);
            HatBase.child.Add(polaTopi5);


            SallyObjectList[0].translate(InitPos.X, InitPos.Y, InitPos.Z);

            foreach (Sally3DAssets i in SallyObjectList)
            {
                i.load(sizeX, sizeY);
            }
        }

        

        public void RenderSally(Camera _camera)
        {
            foreach (Sally3DAssets i in SallyObjectList)
            {
                i.render(_camera);
                
                //i.scale(.5f, .5f, .5f);
                /*foreach (Asset3d j in i.child)
                {
                    j.rotate(Vector3.Zero, Vector3.UnitY, 720 * time);
                }*/
            }
        }


        //-------------------Animation--------------------------------
        float incrementTime;
        float armRotateAngle;
        float wingRotateScale = 1f;
        private float ArmRotateSpeed = 600f;
        public bool canFly = true;
        public void SallyFly(float time)
        {
            incrementTime += time;

            if (canFly)
            {
                body.translate(0f, MathF.Sin(incrementTime * 2) * 0.6f, 0f);

                body.rotate(new Vector3(8f, 0f, 8f), Vector3.UnitY, time * 90f);
            }
                


            if (armRotateAngle > 0f) 
                wingRotateScale = -1f;
            else if(armRotateAngle < -45f) 
                wingRotateScale = 1f;


            armRotateAngle += time * ArmRotateSpeed * wingRotateScale;
            Console.WriteLine(armRotateAngle);

            //body.rotate(body.objectCenter, Vector3.UnitZ, time * 10f * rotateScale);
            wingLeft.rotate(wingLeft.objectCenter, wingLeft._euler[0], time * ArmRotateSpeed * wingRotateScale);
            wingRight.rotate(wingRight.objectCenter, wingRight._euler[0], time * ArmRotateSpeed * wingRotateScale);

        }



        float shinRotateAngle;
        float shinRotateScale = 1f;
        float shinRotateSpeed = 50f;
        public void SallySitFeetSwing(float time)
        {
            incrementTime += time;

            if (shinRotateAngle > 35f)
                shinRotateScale = -1f;
            else if (shinRotateAngle < 0f)
                shinRotateScale = 1f;

            shinRotateAngle += time * shinRotateSpeed * shinRotateScale;
            shinLeft.rotate(body.objectCenter, shinLeft._euler[0], time * shinRotateSpeed * shinRotateScale);
            shinRight.rotate(body.objectCenter, shinRight._euler[0], time * shinRotateSpeed * (-shinRotateScale));
        }

        

        //Pose
      
        public void SitPosition()
        {
            body.rotate(body.objectCenter, body._euler[0], -25f); 
            shinLeft.rotate(body.objectCenter, shinLeft._euler[0], -40f);
            shinRight.rotate(body.objectCenter, shinRight._euler[0], -40f);
        }

        public void FlyPosition()
        {
            body.rotate(body.objectCenter, body._euler[0], 50f);
            sallyHead.rotate(sallyHead.objectCenter, sallyHead._euler[0], -35f);
        }
        



    }
}
