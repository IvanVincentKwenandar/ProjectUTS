using OpenTK.Windowing.Desktop;
using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;


namespace ConsoleApp4
{
    class Constants
    {
        public const string path = "../../../";
    }

    internal class Doraemon
    {
        List<DoraemonAsset3D> objectList = new List<DoraemonAsset3D>();

        public Vector2 _Size;

        double totalTime = 0;
        double _time;

        int State = 0;
        List<float> temp = new List<float>();

        int direction = 0;

        public void OnLoad(int sizeX, int sizeY)
        {
            _Size = new Vector2(sizeX, sizeY);

            //0
            var body = new DoraemonAsset3D(0.1647f, 0.6157f, 0.9569f);
            body.setTrack(true);
            body.createEllipsoid(0.0f, -0.3f, 0.0f, 0.5f, 0.52f, 0.5f, 100, 100);
            objectList.Add(body);

            //1
            var head = new DoraemonAsset3D(0.1647f, 0.6157f, 0.9569f);
            head.setTrack(true);
            head.createEllipsoid(0.0f, 0.5f, 0.0f, 0.5f, 0.5f, 0.5f, 100, 100);
            body.Child.Add(head);
            objectList.Add(head);

            //2
            var faceBase = new DoraemonAsset3D(0.95f, 0.95f, 0.95f);
            faceBase.createEllipsoid(0.0f, 0.47f, 0.07f, 0.46f, 0.46f, 0.45f, 100, 100);
            head.Child.Add(faceBase);
            objectList.Add(faceBase);

            //3
            var nose = new DoraemonAsset3D(1.0f, 0.0f, 0.0f);
            nose.createEllipsoid(0.0f, 0.55f, 0.55f, 0.05f, 0.05f, 0.05f, 100, 100);
            faceBase.Child.Add(nose);
            objectList.Add(nose);

            //4
            var eyeLeft = new DoraemonAsset3D(1.0f, 1.0f, 1.0f);
            eyeLeft.createEllipsoid(-0.1f, 0.6f, 0.35f, 0.15f, 0.2f, 0.15f, 100, 100);
            faceBase.Child.Add(eyeLeft);
            objectList.Add(eyeLeft);

            //5
            var pupilLeft = new DoraemonAsset3D(0, 0, 0);
            pupilLeft.createEllipsoid(-0.1f, 0.66f, 0.46f, 0.05f, 0.05f, 0.05f, 100, 100);
            eyeLeft.Child.Add(pupilLeft);
            objectList.Add(pupilLeft);

            //6
            var eyeRight = new DoraemonAsset3D(1.0f, 1.0f, 1.0f);
            eyeRight.createEllipsoid(0.1f, 0.6f, 0.35f, 0.15f, 0.2f, 0.15f, 100, 100);
            faceBase.Child.Add(eyeRight);
            objectList.Add(eyeRight);

            //7
            var pupilRight = new DoraemonAsset3D(0.0f, 0.0f, 0.0f);
            pupilRight.createEllipsoid(0.1f, 0.66f, 0.46f, 0.05f, 0.05f, 0.05f, 100, 100);
            eyeRight.Child.Add(pupilRight);
            objectList.Add(pupilRight);

            //8
            var legLeft = new DoraemonAsset3D(0.1647f, 0.6157f, 0.9569f);
            legLeft.createCylinderUp(-0.25f, -0.75f, 0.0f, 0.17f, 0.5f, 20);
            legLeft.rotationCenter = new Vector3(-0.25f, -0.5f, 0.0f);
            body.Child.Add(legLeft);
            objectList.Add(legLeft);

            //9
            var legRight = new DoraemonAsset3D(0.1647f, 0.6157f, 0.9569f);
            legRight.createCylinderUp(0.25f, -0.75f, 0.0f, 0.17f, 0.5f, 20);
            body.Child.Add(legRight);
            objectList.Add(legRight);

            //10
            var footLeft = new DoraemonAsset3D(1, 1, 1);
            footLeft.createEllipsoid(-0.25f, -0.92f, 0.0f, 0.2f, 0.15f, 0.2f, 100, 100);
            legLeft.Child.Add(footLeft);
            objectList.Add(footLeft);

            //11
            var footRight = new DoraemonAsset3D(1, 1, 1);
            footRight.createEllipsoid(0.25f, -0.92f, 0.0f, 0.2f, 0.15f, 0.2f, 100, 100);
            legRight.Child.Add(footRight);
            objectList.Add(footRight);

            //12
            var collar = new DoraemonAsset3D(1.0f, 0.0f, 0.0f);
            collar.createCylinderUp(0.0f, 0.1f, 0.0f, 0.37f, 0.07f, 100);
            body.Child.Add(collar);
            objectList.Add(collar);

            //13
            var bell = new DoraemonAsset3D(1.0f, 0.9f, 0.0f);
            bell.createEllipsoid(collar._centerPosition[0], collar._centerPosition[1], 0.405f, 0.07f, 0.07f, 0.07f, 100, 100);
            collar.Child.Add(bell);
            objectList.Add(bell);

            //14
            var belly = new DoraemonAsset3D(1.0f, 1.0f, 1.0f);
            belly.createEllipsoid(0.0f, -0.35f, 0.25f, 0.37f, 0.4f, 0.3f, 100, 100);
            body.Child.Add(belly);
            objectList.Add(belly);

            //15
            var pouchUp = new DoraemonAsset3D();
            List<Vector3> guide1 = new List<Vector3>();
            guide1.Add(new Vector3(-0.25f, -0.35f, 0.6f));
            guide1.Add(new Vector3(0f, -0.35f, 0.7f));
            guide1.Add(new Vector3(0.25f, -0.35f, 0.6f));
            pouchUp.createBezierCurve(guide1);
            belly.Child.Add(pouchUp);
            objectList.Add(pouchUp);

            //16
            var pouchBottom = new DoraemonAsset3D();
            List<Vector3> guide2 = new List<Vector3>();
            guide2.Add(new Vector3(-0.25f, -0.35f, 0.6f));
            guide2.Add(new Vector3(0f, -0.7f, 0.65f));
            guide2.Add(new Vector3(0.25f, -0.35f, 0.6f));
            pouchBottom.createBezierCurve(guide2);
            belly.Child.Add(pouchBottom);
            objectList.Add(pouchBottom);

            //17
            var pouch = new DoraemonAsset3D(0.9f, 0.9f, 1.0f);
            pouch.createMouth(pouchUp, pouchBottom);
            objectList.Add(pouch);
            belly.Child.Add(pouch);
            pouch.multModel(Matrix4.CreateTranslation(0f, 0f, -0.079f));

            //18
            var armLeft = new DoraemonAsset3D(0.1647f, 0.6157f, 0.9569f);
            armLeft.createCylinderSide(-0.5f, -0.1f, 0.0f, 0.129f, 0.5f, 100);
            body.Child.Add(armLeft);
            objectList.Add(armLeft);

            //19
            var armRight = new DoraemonAsset3D(0.1647f, 0.6157f, 0.9569f);
            armRight.createCylinderSide(0.5f, -0.1f, 0.0f, 0.129f, 0.5f, 100);
            body.Child.Add(armRight);
            objectList.Add(armRight);

            //20
            var handRight = new DoraemonAsset3D(1, 1, 1);
            handRight.createEllipsoid(0.75f, -0.1f, 0.0f, 0.17f, 0.17f, 0.17f, 100, 100);
            armRight.Child.Add(handRight);
            objectList.Add(handRight);

            //21
            var handLeft = new DoraemonAsset3D(1, 1, 1);
            handLeft.createEllipsoid(-0.75f, -0.1f, 0.0f, 0.17f, 0.17f, 0.17f, 100, 100);
            armLeft.Child.Add(handLeft);
            objectList.Add(handLeft);

            //22
            var tail = new DoraemonAsset3D(1, 0, 0);
            tail.createEllipsoid(0.0f, -0.55f, -0.5f, 0.07f, 0.07f, 0.07f, 100, 100);
            body.Child.Add(tail);
            objectList.Add(tail);

            //23
            var lipUp = new DoraemonAsset3D();
            guide1.Clear();
            guide1.Add(new Vector3(-0.25f, 0.42f, 0.55f));
            guide1.Add(new Vector3(0f, 0.42f, 0.69f));
            guide1.Add(new Vector3(0.25f, 0.42f, 0.55f));
            lipUp.createBezierCurve(guide1);
            faceBase.Child.Add(lipUp);
            objectList.Add(lipUp);

            //24
            var lipBottom = new DoraemonAsset3D();
            guide2.Clear();
            guide2.Add(new Vector3(-0.25f, 0.4f, 0.55f));
            guide2.Add(new Vector3(0f, 0f, 0.5f));
            guide2.Add(new Vector3(0.25f, 0.4f, 0.55f));
            lipBottom.createBezierCurve(guide2);
            faceBase.Child.Add(lipBottom);
            objectList.Add(lipBottom);

            //25
            var smile = new DoraemonAsset3D(1.0f, 0.2f, 0f);
            smile.createMouth(lipUp, lipBottom);
            objectList.Add(smile);
            faceBase.Child.Add(smile);
            smile.multModel(Matrix4.CreateTranslation(0f, 0f, -0.078f));

            //26
            var armLeftPivot = new DoraemonAsset3D();
            armLeftPivot.createDot(-0.25f, -0.1f, 0.0f);
            body.Child.Add(armLeftPivot);
            objectList.Add(armLeftPivot);

            //27
            var armRightPivot = new DoraemonAsset3D();
            armRightPivot.createDot(0.25f, -0.1f, 0.0f);
            body.Child.Add(armRightPivot);
            objectList.Add(armRightPivot);

            //28
            var legLeftPivot = new DoraemonAsset3D();
            legLeftPivot.createDot(-0.25f, -0.5f, 0f);
            body.Child.Add(legLeftPivot);
            objectList.Add(legLeftPivot);

            //29
            var legRightPivot = new DoraemonAsset3D();
            legRightPivot.createDot(0.25f, -0.5f, 0f);
            body.Child.Add(legRightPivot);
            objectList.Add(legRightPivot);

            //30
            var sirene = new DoraemonAsset3D(0f, 0f, 0.5f);
            sirene.createHalfHyperOneSheet(-0.8f, 0.2f, 0f, 0.2f, 0.5f, 5, 100);
            sirene.scaleStay(0.5f, 0.5f, 0.5f);
            handLeft.Child.Add(sirene);
            objectList.Add(sirene);

            //31
            var sirenehandle = new DoraemonAsset3D(1f, 0, 0);
            sirenehandle.createCylinderUp(-0.85f, 0.09f, 0f, 0.05f, 0.3f, 100);
            sirene.Child.Add(sirenehandle);
            objectList.Add(sirenehandle);

            //32
            var sireneback = new DoraemonAsset3D(0f, 0f, 0.5f);
            sireneback.createBoxVertices(-0.82f, 0.2f, 0f, 0.2f);
            sirene.Child.Add(sireneback);
            objectList.Add(sireneback);

            //33
            var balingBase = new DoraemonAsset3D(1.0f, 1.0f, 0.0f);
            balingBase.createHalfHyperTwoSheet(0, 1, 0, 0.05f, 0.1f, 40, 40);
            balingBase.multModel(Matrix4.CreateTranslation(0, 0.07f, 0));
            head.addChild(balingBase);
            objectList.Add(balingBase);

            //34
            var balingStick = new DoraemonAsset3D(1.0f, 1.0f, 0.0f);
            balingStick.createCylinderUp(0, 1.1f, 0, 0.01f, 0.2f, 30);
            balingBase.addChild(balingStick);
            objectList.Add(balingStick);

            //35
            var balingBlade = new DoraemonAsset3D(1, 1, 1);
            balingBlade.createCircleUp(0, 1.2f, 0, 0.2f, 50);
            balingBase.addChild(balingBlade);
            objectList.Add(balingBlade);

            //Pre Condition
            armLeft.rotate(armLeftPivot._centerPosition, armLeft._euler[2], 45f);
            armRight.rotate(armRightPivot._centerPosition, armRight._euler[2], -45f);
            objectList[18].resetEuler();
            objectList[19].resetEuler();
            body.scale(2f, 2f, 2f);
            body.translate(-10f, 2.25f, -5.5f, true);


            foreach (DoraemonAsset3D obj in objectList)
            {
                obj.load(Constants.path + "shader.vert", Constants.path + "shader.frag", sizeX, sizeY);
            }

            //animateStep
            temp.Add(0);    // 0 - angle
            temp.Add(1);    // 1 - plus or minus
            //turning
            temp.Add(0);    // 2 - angle
            //jumping & falling
            temp.Add(0);    // 3 - TIDAK DIPAKAI
            //Shaking
            temp.Add(0);    // 4 - angle
            temp.Add(1);    // 5 - shake head right or left
            //BoardingBody
            temp.Add(0);    // 6 - angle
            //fly
            temp.Add(1);    // 7 - up or down
            //BoardingHead
            temp.Add(0);    // 8 - angle
            //LandingHead
            temp.Add(0);    // 9 - angle
            //landingBody
            temp.Add(0);    // 10 - angle
        }

        public void OnRenderFrame(FrameEventArgs args, Camera _camera)
        {
            _time = args.Time;
            totalTime += _time;

            objectList[0].render(_camera);
            shaking(40 * _time);

            if (State == 0)
            {
                timerTurn();
                move(7 * _time);
            }
            else if (State == 1)
            {
                turning(1f);
                totalTime = 0;
            }
            else if (State == 2)
            {
                boardingBody(40 * _time);
            }
            else if (State == 3)
            {
                boardingHead(40 * _time);
            }
            else if (State == 4)
            {
                timerFly();
                fly(30 * _time);
            }
            else if (State == 5)
            {
                landingHead(40 * _time);
            }
            else if (State == 6)
            {
                landingBody(40 * _time);
            }
        }

        public void OnKeyDown(KeyboardKeyEventArgs e) { }

        public void OnKeyUp(KeyboardKeyEventArgs e)
        {
            if (e.Key.Equals(Keys.R))
            {
                reset();
            }

        }

        public void reset()
        {
            objectList.Clear();
            temp.Clear();
            OnLoad((int)_Size.X, (int)_Size.Y);
            direction = 0;
            State = 0;
            totalTime = 0;

        }

        public void timerTurn()
        {
            if (totalTime > 3.5)
            {
                totalTime = 0;
                State = 1;
                direction += 1;
            }
        }

        public void turning(double time)
        {
            if (temp[2] < 90f)
            {
                if (temp[2] + (float)time <= 90f)
                {
                    objectList[0].rotate(objectList[0]._centerPosition, objectList[0]._euler[1], (float)time);
                    temp[2] += (float)time;
                }
                else
                {
                    objectList[0].rotate(objectList[0]._centerPosition, objectList[0]._euler[1], 90 - temp[2]);
                    temp[2] = 90f;
                }
            }
            else
            {
                temp[2] = 0;
                State = 0;
                if (direction == 4)
                {
                    reset();
                    State = 2;
                }
            }
        }

        public void move(double time)
        {
            if (direction % 4 == 0)
            {
                objectList[0].translate(0f, 0f, (float)(time), true);
            }
            else if (direction % 4 == 1)
            {
                objectList[0].translate((float)(time), 0f, 0f, true);
            }
            else if (direction % 4 == 2)
            {
                objectList[0].translate(0f, 0f, (float)(time * -1), true);
            }
            else
            {
                objectList[0].translate((float)(time * -1), 0f, 0f, true);
            }
            animateStep(20 * time);
        }

        public void animateStep(double time)
        {
            if (temp[0] > 30f)
            {
                temp[1] = -1;
            }
            if (temp[0] < -30f)
            {
                temp[1] = 1;
            }
            float speed = (float)(time * temp[1]);
            temp[0] += speed;
            objectList[8].rotate(objectList[28]._centerPosition, objectList[8]._euler[0], speed); //left leg
            objectList[9].rotate(objectList[29]._centerPosition, objectList[8]._euler[0], speed * -1f); //right leg
            objectList[18].rotate(objectList[26]._centerPosition, objectList[18]._euler[0], speed * -1f); //left arm
            objectList[19].rotate(objectList[27]._centerPosition, objectList[19]._euler[0], speed); //right arm
        }

        public void shaking(double time)
        {
            if (temp[4] > 20)
            {
                temp[5] = -1;
            }
            if (temp[4] < -20)
            {
                temp[5] = 1;
            }
            temp[4] += (float)(time * temp[5]);
            objectList[1].rotate(objectList[1]._centerPosition, objectList[1]._euler[1], (float)(time * temp[5]));

        }

        public void boardingBody(double time)
        {
            if (temp[6] < 60f)
            {
                if (temp[6] + (float)time <= 60f)
                {
                    objectList[0].rotate(objectList[0]._centerPosition, objectList[0]._euler[0], (float)time);
                    temp[6] += (float)time;
                }
                else
                {
                    objectList[0].rotate(objectList[0]._centerPosition, objectList[0]._euler[0], 60 - temp[6]);
                    temp[6] = 60f;
                }
            }
            else
            {
                temp[6] = 0;
                State = 3;
                totalTime = 0;
            }
        }

        public void boardingHead(double time)
        {
            if (temp[8] < 60f)
            {
                if (temp[8] + (float)time <= 60f)
                {
                    objectList[1].rotate(objectList[1]._centerPosition, objectList[1]._euler[0], (float)-time);
                    objectList[1].translate(0, (float)-time / 150, (float)time / 150, true);
                    temp[8] += (float)time;
                }
                else
                {
                    objectList[1].rotate(objectList[1]._centerPosition, objectList[1]._euler[0], (float)(60f - temp[8]));
                    temp[8] = 60f;
                }
            }
            else
            {
                temp[8] = 0;
                State = 4;
                totalTime = 0;
            }
        }

        public void landingBody(double time)
        {
            if (temp[10] < 60f)
            {
                if (temp[10] + (float)time <= 60f)
                {
                    objectList[0].rotate(objectList[0]._centerPosition, objectList[0]._euler[0], (float)-time);
                    temp[10] += (float)time;
                }
                else
                {
                    objectList[0].rotate(objectList[0]._centerPosition, objectList[0]._euler[0], -60 + temp[10]);
                    temp[10] = 60f;
                }
            }
            else
            {
                temp[10] = 0;
                reset();
                State = 0;
                totalTime = 0;
            }
        }

        public void landingHead(double time)
        {
            if (temp[9] < 60f)
            {
                if (temp[9] + (float)time <= 60f)
                {
                    objectList[1].rotate(objectList[1]._centerPosition, objectList[1]._euler[0], (float)time);
                    objectList[1].translate(0, (float)time / 150, (float)time / -150, true);
                    temp[9] += (float)time;
                }
                else
                {
                    objectList[1].rotate(objectList[1]._centerPosition, objectList[1]._euler[0], (float)(-60f + temp[9]));
                    temp[9] = 60f;
                }
            }
            else
            {
                temp[9] = 0;
                State = 6;
                totalTime = 0;
            }
        }

        public void timerFly()
        {
            if (totalTime > 24)
            {
                totalTime = 0;
                State = 5;
            }
        }

        public void fly(double time)
        {
            if (objectList[0]._centerPosition.Y > 20f)
            {
                temp[7] = -1;
            }
            if (objectList[0]._centerPosition.Y < 2f)
            {
                temp[7] = 1;
            }
            objectList[0].translate(0f, (float)time / 10 * temp[7], 0f, true);
            objectList[0].rotateY_all(MathHelper.DegreesToRadians((float)(time * 2)));
        }

    }

}
