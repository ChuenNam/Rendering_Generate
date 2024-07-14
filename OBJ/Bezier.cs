using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

class Bezier
{
    public Vector3 start;
    public Vector3 b1;
    public Vector3 b2;
    public Vector3 end;

    public List<Vector3> curve = new List<Vector3>();

    public Bezier() { }
    public Bezier(Vector3 start, Vector3 b1, Vector3 b2, Vector3 end)
    {
        this.start = start;
        this.b1 = b1;
        this.b2 = b2;
        this.end = end;
    }

    public static List<Vector3> GenBesselPoints(Vector3 start, Vector3 b1, Vector3 b2, Vector3 end)
    {
        float t = 0;
        List<Vector3> curve = new List<Vector3>();

        while (t <= 1)
        {
            Vector3 p1 = TAMath.LerpV(start, b1, t);
            Vector3 p2 = TAMath.LerpV(b1, b2, t);
            Vector3 p3 = TAMath.LerpV(b2, end, t);

            Vector3 pA = TAMath.LerpV(p1, p2, t);
            Vector3 pB = TAMath.LerpV(p2, p3, t);

            Vector3 point = TAMath.LerpV(pA, pB, t);

            curve.Add(point);

            t += 0.02f;
        }

        return curve;
    }

    public static void GenBesselCurve(Vector3 start, Vector3 b1, Vector3 b2, Vector3 end)
    {
        Obj curve = new Obj();

        List<Vector3> points = GenBesselPoints(start, b1, b2, end);

        for (int i = 0; i < points.Count - 1; i++)
        {
            Obj.AddQuad(curve, points[i], points[i + 1] - points[i], new Vector3(0.03f, 0, 0));
        }

        curve.Save("BesselCurve.obj");
    }

    public static void GenBesselCurves(Vector3 start, Vector3 b1, Vector3 b2, Vector3 middle, Vector3 p1, Vector3 p2, Vector3 end)
    {
        Obj curve = new Obj();

        List<Vector3> pointsA = GenBesselPoints(start, b1, b2, middle);
        List<Vector3> pointsB = GenBesselPoints(middle, p1, p2, end);

        for (int i = 0; i < pointsA.Count - 1; i++)
        {
            Obj.AddQuad(curve, pointsA[i], pointsA[i + 1] - pointsA[i], new Vector3(0.03f, 0, 0));
        }
        for (int i = 0; i < pointsB.Count - 1; i++)
        {
            Obj.AddQuad(curve, pointsB[i], pointsB[i + 1] - pointsB[i], new Vector3(0.03f, 0, 0));
        }

        curve.Save("BesselCurves.obj");
    }

    public static void GenLove()
    {
        Obj love = new Obj();

        Vector3 p1 = new Vector3(0, 0, 0);
        Vector3 p2 = new Vector3(1f, 1f, 0);
        Vector3 p3 = new Vector3(0.5f, 1.5f, 0);
        Vector3 p4 = new Vector3(0, 1, 0);
        Vector3 p5 = new Vector3(-0.5f, 1.5f, 0);
        Vector3 p6 = new Vector3(-1f, 1f, 0);

        List<Vector3> pointsA = GenBesselPoints(p1, p2, p3, p4);
        List<Vector3> pointsB = GenBesselPoints(p4, p5, p6, p1);

        for (int i = 0; i < pointsA.Count - 1; i++)
        {
            Vector3 v1 = pointsA[i];
            Vector3 v2 = pointsA[i + 1];
            love.AddTriangle(new Triangle(p1, v1, v2));
        }
        for (int i = 0; i < pointsB.Count - 1; i++)
        {
            Vector3 v1 = pointsB[i];
            Vector3 v2 = pointsB[i + 1];
            love.AddTriangle(new Triangle(p1, v1, v2));
        }
        love.Save("Love.obj");
    }

    public static void SurfHelper(int count, Obj obj, List<Vector3> toInput, List<Vector3> axisX1, List<Vector3> axisX2, List<Vector3> axisX3, List<Vector3> axisX4)
    {
        if (count == axisX1.Count - 1) return;

        List<Vector3> axisY1 = toInput;
        List<Vector3> axisY2 = GenBesselPoints(axisX1[count + 1], axisX2[count + 1], axisX3[count + 1], axisX4[count + 1]);

        for (int i = 0; i < axisY1.Count - 1; i++)
        {
            Vector3 v1 = axisY1[i];
            Vector3 v2 = axisY1[i + 1];
            Vector3 v3 = axisY2[i];
            obj.AddTriangle(new Triangle(v1, v2, v3));
        }
        for (int i = 0; i < axisY2.Count - 1; i++)
        {
            Vector3 v1 = axisY1[i + 1];
            Vector3 v2 = axisY2[i + 1];
            Vector3 v3 = axisY2[i];
            obj.AddTriangle(new Triangle(v1, v2, v3));
        }
        axisY1 = axisY2;

        SurfHelper(count + 1, obj, axisY1, axisX1, axisX2, axisX3, axisX4);
    }

    public static void GenBesselSurface(List<Vector3> wave1, List<Vector3> wave2, List<Vector3> wave3, List<Vector3> wave4)
    {
        Obj surface = new Obj();

        List<Vector3> axisX1 = new List<Vector3>(wave1);
        List<Vector3> axisX2 = new List<Vector3>(wave2);
        List<Vector3> axisX3 = new List<Vector3>(wave3);
        List<Vector3> axisX4 = new List<Vector3>(wave4);

        for (int i = 0; i < axisX1.Count; i++)
        {
            axisX2[i] += new Vector3(0, 0, 1);
            axisX3[i] += new Vector3(0, 0, 2);
            axisX4[i] += new Vector3(0, 0, 3);
        }

        for (int i = 0; i < wave1.Count; i++)
        {
            List<Vector3> axisY1 = GenBesselPoints(axisX1[i], axisX2[i], axisX3[i], axisX4[i]);
            SurfHelper(i, surface, axisY1, axisX1, axisX2, axisX3, axisX4);
        }

        surface.Save("GenBesselSurface.obj");
    }

}
