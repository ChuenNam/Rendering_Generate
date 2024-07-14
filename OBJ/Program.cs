using System;
using System.Collections.Generic;
using System.IO;

namespace OBJ
{
    class Program
    {
        static void Main(string[] args)
        {
            //Obj obj = new Obj();
            //obj.Read("Triangle.obj");
            //obj.Save("Tri_save.obj");

            //Obj.ProceduralGeneration01();
            //Obj.ProceduralGeneration02();
            //Obj.ProceduralGeneration03();
            //Obj.ProceduralGeneration04();
            //Obj.ModifyModel();
            //Obj.GenCircle(4, 2, "GenCircle01.obj");
            //Obj.GenCone(64, 2, 2, "Cone01.obj");
            //Obj.GenCylinder(3, 3, 3, "Cylinder01.obj");
            //Obj.L_System();
            //Obj.L_System_Random();
            //Obj.GenL_System_Paper(1, 0.05f, 300, 0.05f);

            //Vector3 v1 = new Vector3(1,8,4);
            //Console.WriteLine(-v1);

            Vector3 p1 = new Vector3(0, 0, 0);
            Vector3 p2 = new Vector3(1f, 2f, 0);
            Vector3 p3 = new Vector3(2f, -2f, 0);
            Vector3 p4 = new Vector3(3, 0, 0);
            
            Vector3 b1 = new Vector3(0, 0, 0);
            Vector3 b2 = new Vector3(1f, -2f, 0);
            Vector3 b3 = new Vector3(2f, 2f, 0);
            Vector3 b4 = new Vector3(3, 0, 0);

            Vector3 a1 = new Vector3(0, 0, 0);
            Vector3 a2 = new Vector3(1f, -3f, 0);
            Vector3 a3 = new Vector3(2f, 1f, 0);
            Vector3 a4 = new Vector3(3, 0, 0);
            
            Vector3 c1 = new Vector3(0, 0, 0);
            Vector3 c2 = new Vector3(1f, -1f, 0);
            Vector3 c3 = new Vector3(2f, -1f, 0);
            Vector3 c4 = new Vector3(3, 0, 0);

            List<Vector3> wave1 = Bezier.GenBesselPoints(p1, p2, p3, p4);
            List<Vector3> wave2 = Bezier.GenBesselPoints(a1, a2, a3, a4);
            List<Vector3> wave3 = Bezier.GenBesselPoints(b1, b2, b3, b4);
            List<Vector3> wave4 = Bezier.GenBesselPoints(c1, c2, c3, c4);

            //Vector3 p5 = new Vector3(-0.5f, 1.5f, 0);
            //Vector3 p6 = new Vector3(-1f, 1f, 0);
            //Vector3 p7 = new Vector3(0, 1, 0);


            //Bezier.GenBesselPoints(p1, p2, p3, p4);
            //Bezier.GenBesselCurve(p1, p2, p3, p4);
            //Bezier.GenBesselCurves(p1, p2, p3, p4, p5, p6, p1);
            //Bezier.GenLove();
            Bezier.GenBesselSurface(wave1, wave2, wave3, wave4);

            //Console.WriteLine();
        }
    }
}