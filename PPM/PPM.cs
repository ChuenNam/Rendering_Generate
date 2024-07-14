using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public class TF
{
    public static List<Vector3> GetAllPoints(Obj obj)
    {
        List<Vector3> W_point = new List<Vector3>();
        for (int i = 0; i < obj.positionIndex.Count; i += 3)
        {
            for (int j = 0; j < 3; j++)
            {
                Vector3 p = obj.positions[obj.positionIndex[i + j] - 1];
                W_point.Add(p);
            }
        }
        return W_point;
    }

    public static List<Vector3> Transform(List<Vector3> AllPoints, float ratio)
    {
        float PI = float.PositiveInfinity;
        float NI = float.NegativeInfinity;
        Vector3 Max_XYZ = new Vector3(NI,NI,NI);
        Vector3 Min_XYZ = new Vector3(PI,PI,PI);

        foreach (var item in AllPoints)
        {
            if (item.x > Max_XYZ.x)
                Max_XYZ.x = item.x;
            if (item.y > Max_XYZ.y)
                Max_XYZ.y = item.y;
            if (item.z > Max_XYZ.z)
                Max_XYZ.z = item.z;

            if (item.x < Min_XYZ.x)
                Min_XYZ.x = item.x;
            if (item.y < Min_XYZ.y)
                Min_XYZ.y = item.y;
            if (item.z < Min_XYZ.z)
                Min_XYZ.z = item.z;
        }

        //if ((Max_XYZ.x <= 1 && Max_XYZ.y <= 1 && Max_XYZ.z <= 1) && (Min_XYZ.x > -1 && Min_XYZ.y > -1 && Min_XYZ.z > -1))
        //    return AllPoints;

        float scale = Math.Max((Max_XYZ - Min_XYZ).z, Math.Max((Max_XYZ - Min_XYZ).x, (Max_XYZ - Min_XYZ).y)) * 0.5f * (1 / ratio);
        for (int i = 0; i < AllPoints.Count; i++)
        {
            AllPoints[i] = new Vector3(AllPoints[i].x / scale, AllPoints[i].y / scale, AllPoints[i].z / scale);
        }

        return AllPoints;
    }


    public static Vector3 PreTF(Vector3 point)
    {
        float l, r, t, b, n, f;
        l = -2f; r = 2f;
        t = 1f;  b = 1f;
        n = 0.5f;  f = 2f;

        Vector4 p = new Vector4(point,1);

        Vector4 c1 = new Vector4((2*n)/(r-l),0,0,0);
        Vector4 c2 = new Vector4(0,(2*n)/(t-b),0,0);
        Vector4 c3 = new Vector4((l+r)/(l-r),(b+t)/(b-t),(f+n)/(f-n),1);
        Vector4 c4 = new Vector4(0,0,(2*n*f)/(n-f),0);

        Vector4 v = new Vector4(p*c1,p*c2,p*c3,p*c4);
        Vector3 newPoint = new Vector3(v.x / v.w, v.y / v.w, v.z / v.w);
        return newPoint;
    }

    public static Vector2 ViewTF(PPM ppm, Vector3 vec)
    {
        int x = (int)((vec.x + 1.0f) * ppm.Width / 2.0f);
        int y = (int)((vec.y + 1.0f) * ppm.Height / 2.0f);
        Vector2 vector2 = new Vector2(x, y);
        return vector2;
    }
}

public class Box
{
    public Vector2 max;
    public Vector2 min;
}


public class Color
{
    public int r, g, b;
    public Color(int r, int g, int b)
    {
        this.r = r;
        this.g = g;
        this.b = b;
    }
    public override string ToString()
    {
        return r + " " + g + " " + b + " ";
    }

    public static Color white => new Color(255, 255, 255);
    public static Color black => new Color(0, 0, 0);
    public static Color red => new Color(255, 0, 0);
    public static Color green => new Color(0, 255, 0);
    public static Color blue => new Color(0, 0, 255);
}


public class PPM
{
    #region 图像头部信息
    public static readonly string h1 = "P3";
    public static readonly int maxColor = 255;
    int width = 2;
    int height = 2;
    #endregion

    public int Width { get { return width; } }
    public int Height { get { return height; } }

    public List<List<Color>> ppm;

    public PPM() { }
    public PPM(int width, int height , Color color)
    {
        this.width = width;
        this.height = height;
        ppm = new List<List<Color>>();

        for (int y = 0; y < height; y++)
        {
            List<Color> line = new List<Color>();
            for (int x = 0; x < width; x++)
            {
                line.Add(color);
            }
            ppm.Add(line);
        }
    }
    public List<Color> this[int index]
    {
        get
        {
            return ppm[index];
        }
    }

    public void Save(string path)
    {
        List<string> ppmdata = new List<string>();

        ppmdata.Add(h1);
        ppmdata.Add(width + " " + height);
        ppmdata.Add(maxColor.ToString());

        for (int y = 0; y < ppm.Count; y++)
        {
            string line = "";
            for (int x = 0; x < ppm[y].Count; x++)
            {
                line += ppm[y][x];
            }
            ppmdata.Add(line);
        }
        File.WriteAllLines(path, ppmdata);
    }

    public void Read(string path)
    {
        if (!path.EndsWith(".ppm"))
        {
            Console.WriteLine("NOT PPM");
            return;
        }
        if (!File.Exists(path))
        {
            Console.WriteLine("NOT FOUND");
            return;
        }

        string[] lines = File.ReadAllLines(path);

        bool isP3 = false;
        bool isWidthHight = false;
        bool isMaxColor = false;

        ppm = new List<List<Color>>();

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (line.Length == 0) continue;

            #region 处理头部信息
            //注释
            if (line.StartsWith("#"))
            {
                continue;
            }
            //P3
            if (line.StartsWith("P3"))
            {
                isP3 = true;
                continue;
            }
            //长宽
            if (!isWidthHight)
            {
                string[] wh = line.Split();
                if (wh.Length == 2)
                {
                    int w, h;
                    if (int.TryParse(wh[0], out w) && int.TryParse(wh[1], out h))
                    {
                        isWidthHight = true;
                        this.width = w;
                        this.height = h;
                        continue;
                    }
                }
            }
            //颜色
            if(!isMaxColor) 
            {
                int maxcolor;
                if(int.TryParse(line,out maxcolor))
                {
                    if( maxColor == 255)
                    {
                        isMaxColor = true;
                        continue;
                    }
                }
            }
            //判断
            if (!isP3 || !isMaxColor || !isWidthHight)
            {
                Console.WriteLine("格式错误");
                return;
            }
            #endregion

            string[] pixels = line.Split();

            List<Color> colorLine = new List<Color>();

            int rgbCount = 0;
            string[] rgb = new string[3];
            for (int j = 0; j < pixels.Length; j++)
            {
                if(pixels[j].Length > 0)
                {
                    rgb[rgbCount] = pixels[j];
                    rgbCount++;

                    if(rgbCount == 3)
                    {
                        rgbCount = 0;
                        Color color = new Color(int.Parse(rgb[0]), int.Parse(rgb[1]), int.Parse(rgb[2]));
                        colorLine.Add(color);
                    }
                }
            }
            ppm.Add(colorLine);
        }
    }

    public static void ChangeUV(PPM ppm)
    {
        for (int y = 0; y < ppm.Height; y++)
        {
            float v = (float)y / ppm.Height;
            v *= 255;
            for (int x = 0; x < ppm.Width; x++)
            {
                float u = (float)x / ppm.Width;
                u *= 255;
                ppm[y][x] = new Color((int)u, (int)v, 0);
            }
        }
    }

    public static int GetIndex(int x, int y, PPM ppm)
    {
        return (ppm.height * 2 - 1 - y) * ppm.width * 2 + x;
    }


    /// <summary>
    /// Bresenham’s 直线算法
    /// </summary>
    /// <param name="ppm">绘制的图像</param>
    /// <param name="x1">点A的x值</param>
    /// <param name="y1">点A的y值</param>
    /// <param name="x2">点B的x值</param>
    /// <param name="y2">点B的y值</param>
    /// <param name="color">线的颜色</param>
    public static void DrawLine(PPM ppm, int x1, int y1, int x2, int y2, Color color)
    {
        bool steep = false;
        //导数绝对值大于1
        if (Math.Abs(x1 - x2) < Math.Abs(y1 - y2))
        {
            TAMath.Swap(ref x1, ref y1);
            TAMath.Swap(ref x2, ref y2);
            steep = true;
        }
        //交换起点终点坐标
        if (x1 > x2)
        {
            TAMath.Swap(ref x1, ref y1);
            TAMath.Swap(ref x2, ref y2);
        }
        int y = y1;
        int eps = 0;
        int dx = x2 - x1;
        int dy = y2 - y1;
        int yi = 1;
        //处理[-1, 0]范围内的斜率
        if (dy < 0)
        {
            yi = -1;
            dy = -dy;
        }
        for (int x = x1; x < x2; x++)
        {
            if (steep)
                ppm[x][y] = color;
            else
                ppm[y][x] = color;

            eps += dy;
            // 这里用位运算 <<1 代替 *2
            if ((eps << 1) > dx)
            {
                y += yi;
                eps -= dx;
            }
        }
    }

    /// <summary>
    /// 绘制模型三角形线框
    /// </summary>
    /// <param name="ppm">绘制的图像</param>
    /// <param name="obj">模型</param>
    /// <param name="color">颜色</param>
    public static void DrawModelLine(PPM ppm, Obj obj, Color color)
    {
        // 循环模型里的所有三角形
        for (int i = 0; i < obj.positionIndex.Count; i += 3)
        {
            // 循环三角形三个顶点，每两个顶点连一条线
            for (int j = 0; j < 3; j++)
            {
                //根据索引差找坐标
                Vector3 v0 = obj.positions[obj.positionIndex[i + j] - 1];
                Vector3 v1 = obj.positions[obj.positionIndex[i + ((j + 1) % 3)] - 1];

                // 因为模型空间取值范围是 [-1, 1]^3，我们要把模型坐标平移到屏幕坐标中
                // 下面 (point + 1) * width(height) / 2 的操作学名为视口变换（Viewport Transformation）
                int x0 = (int)((v0.x + 1.0f) * ppm.width / 2.0f);
                int y0 = (int)((v0.y + 1.0f) * ppm.height / 2.0f);
                int x1 = (int)((v1.x + 1.0f) * ppm.width / 2.0f);
                int y1 = (int)((v1.y + 1.0f) * ppm.height / 2.0f);

                //画线
                DrawLine(ppm, x0, y0, x1, y1, color);
            }
        }
    }

    /// <summary>
    /// 用三角形组绘制模型线框
    /// </summary>
    /// <param name="ppm">绘制图像</param>
    /// <param name="obj">绘制模型</param>
    /// <param name="color">绘制颜色</param>
    public static void DrawTriangleLine(PPM ppm, Obj obj, Color color)
    {
        List<Triangle> tris = new List<Triangle>();

        for (int i = 0; i < obj.positionIndex.Count; i += 3)
        {
            Vector3 p1 = obj.positions[obj.positionIndex[i] - 1];
            Vector3 p2 = obj.positions[obj.positionIndex[i + 1] - 1];
            Vector3 p3 = obj.positions[obj.positionIndex[i + 2] - 1];
            Triangle tri = new Triangle(p1, p2, p3);
            tris.Add(tri);
        }
        foreach (var item in tris)
        {
            Vector3 v0 = item.positions[0];
            Vector3 v1 = item.positions[1];
            Vector3 v2 = item.positions[2];

            Vector2 vA = TF.ViewTF(ppm, v0);
            Vector2 vB = TF.ViewTF(ppm, v1);
            Vector2 vC = TF.ViewTF(ppm, v2);

            DrawLine(ppm, vA.Xint, vA.Yint, vB.Xint, vB.Yint, color);
            DrawLine(ppm, vB.Xint, vB.Yint, vC.Xint, vC.Yint, color);
            DrawLine(ppm, vC.Xint, vC.Yint, vA.Xint, vA.Yint, color);
        }
    }

    /// <summary>
    /// 绘制彩色模型
    /// </summary>
    /// <param name="ppm">绘制的图像</param>
    /// <param name="obj">模型</param>
    public static void DrawModelColor(PPM ppm, Obj obj)
    {
        for (int i = 0; i < obj.positionIndex.Count; i += 3)
        {
            Box box = new Box();
            List<Vector2> points = new List<Vector2>();

            Random rand = new Random();
            Color col = new Color((int)(rand.NextDouble() * 255), (int)(rand.NextDouble() * 255), (int)(rand.NextDouble() * 255));

            for (int j = 0; j < 3; j++)
            {
                Vector3 v0 = obj.positions[obj.positionIndex[i + j] - 1];
                int x0 = (int)((v0.x + 1.0f) * ppm.width / 2.0f);
                int y0 = (int)((v0.y + 1.0f) * ppm.height / 2.0f);
                points.Add(new Vector2(x0, y0));
            }

            box.min.x = Math.Min(points[0].x, Math.Min(points[1].x, points[2].x));
            box.min.y = Math.Min(points[0].y, Math.Min(points[1].y, points[2].y));
            box.max.x = Math.Max(points[0].x, Math.Max(points[1].x, points[2].x));
            box.max.y = Math.Max(points[0].y, Math.Max(points[1].y, points[2].y));

            for (int y = 0; y <= box.max.y - box.min.y; y++)
            {
                for (int x = 0; x <= box.max.x - box.min.x; x++)
                {
                    Vector3 N = TAMath.CheckInside(points[0], points[1], points[2], new Vector2((int)box.min.x + x, (int)box.min.y + y));
                    if (N.x > 0 || N.y > 0 || N.z > 0)
                    {
                        continue;
                    }
                    ppm[(int)box.min.y + y][(int)box.min.x + x] = col;
                }
            }
        }
    }

    /// <summary>
    /// 生成深度图
    /// </summary>
    /// <param name="ppm">图像</param>
    /// <param name="obj">物体</param>
    public static void ZBuffer(PPM ppm, Obj obj)
    {
        float[] zbuffer = new float[ppm.width * ppm.height];
        for (int i = 0; i < zbuffer.Length; i++)
        {
            zbuffer[i] = float.PositiveInfinity;
        }

        for (int i = 0; i < obj.positionIndex.Count; i += 3)
        {
            Box box = new Box();
            List<Vector3> points = new List<Vector3>();

            for (int j = 0; j < 3; j++)
            {
                Vector3 v0 = obj.positions[obj.positionIndex[i + j] - 1];
                int x0 = (int)((v0.x + 1.0f) * ppm.width / 2.0f);
                int y0 = (int)((v0.y + 1.0f) * ppm.height / 2.0f);
                points.Add(new Vector3(x0, y0,v0.z));
            }

            box.min.x = Math.Min(points[0].x, Math.Min(points[1].x, points[2].x));
            box.min.y = Math.Min(points[0].y, Math.Min(points[1].y, points[2].y));
            box.max.x = Math.Max(points[0].x, Math.Max(points[1].x, points[2].x));
            box.max.y = Math.Max(points[0].y, Math.Max(points[1].y, points[2].y));

            Vector2 p0 = new Vector2(points[0].x, points[0].y);
            Vector2 p1 = new Vector2(points[1].x, points[1].y);
            Vector2 p2 = new Vector2(points[2].x, points[2].y);

            for (int y = (int)box.min.y; y <= box.max.y; y++)
            {
                for (int x = (int)box.min.x; x <= box.max.x; x++)
                {
                    Vector2 pixel = new Vector2(x, y);

                    Vector3 N = TAMath.CheckInside(p0, p1, p2, new Vector2(x, y));
                    if (N.x > 0 || N.y > 0 || N.z > 0)
                        continue;

                    float a = TAMath.Distance(p0, pixel);
                    float b = TAMath.Distance(p1, pixel);
                    float c = TAMath.Distance(p2, pixel);
                    float A = a / (a + b + c);
                    float B = b / (a + b + c);
                    float C = c / (a + b + c);
                    float deep = A * points[0].z + B * points[1].z + C * points[2].z;

                    if (deep <= zbuffer[ppm.width * y + x])
                    {
                        zbuffer[ppm.width * y + x] = deep;
                        int col = (int)Math.Abs((zbuffer[ppm.width * y + x] * 255));
                        ppm[y][x] = new Color(col, col, col);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 生成深度图(投影变换)
    /// </summary>
    /// <param name="ppm">图像</param>
    /// <param name="obj">物体</param>
    /// <param name="ratio">模型屏占比</param>
    public static void ZBuffer_PRE(PPM ppm, Obj obj, float ratio = 1)
    {
        float[] zbuffer = new float[ppm.width * ppm.height];
        for (int i = 0; i < zbuffer.Length; i++)
        {
            zbuffer[i] = float.PositiveInfinity;
        }

        List<Vector3> newPoints = TF.Transform(TF.GetAllPoints(obj), ratio);

        for (int i = 0; i < newPoints.Count; i += 3)
        {
            Box box = new Box();
            List<Vector3> points = new List<Vector3>();

            for (int j = 0; j < 3; j++)
            {
                Vector3 v0 = newPoints[i + j];

                //
                //if (v0.x < -1 || v0.y < -1 || v0.z < -1)
                //{
                //    Console.WriteLine(i+j);
                //    Console.WriteLine(v0);
                //    Console.ReadLine();
                //}

                int x0 = (int)((v0.x + 1.0f) * ppm.width / 2.0f);
                int y0 = (int)((v0.y + 1.0f) * ppm.height / 2.0f);
                points.Add(new Vector3(x0, y0, v0.z));
            }

            box.min.x = Math.Max(0, Math.Min(points[0].x, Math.Min(points[1].x, points[2].x)));
            box.min.y = Math.Max(0, Math.Min(points[0].y, Math.Min(points[1].y, points[2].y)));
            box.max.x = Math.Min(ppm.width, Math.Max(points[0].x, Math.Max(points[1].x, points[2].x)));
            box.max.y = Math.Min(ppm.height, Math.Max(points[0].y, Math.Max(points[1].y, points[2].y)));

            Vector2 p0 = new Vector2(points[0].x, points[0].y);
            Vector2 p1 = new Vector2(points[1].x, points[1].y);
            Vector2 p2 = new Vector2(points[2].x, points[2].y);

            for (int y = (int)box.min.y; y < box.max.y; y++)
            {
                for (int x = (int)box.min.x; x < box.max.x; x++)
                {
                    Vector2 pixel = new Vector2(x, y);

                    Vector3 N = TAMath.CheckInside(p0, p1, p2, new Vector2(x, y));
                    if (N.x > 0 || N.y > 0 || N.z > 0)
                        continue;

                    float a = TAMath.Distance(p0, pixel);
                    float b = TAMath.Distance(p1, pixel);
                    float c = TAMath.Distance(p2, pixel);
                    float A = a / (a + b + c);
                    float B = b / (a + b + c);
                    float C = c / (a + b + c);
                    float deep = A * points[0].z + B * points[1].z + C * points[2].z;

                    if (deep <= zbuffer[ppm.width * y + x])
                    {
                        zbuffer[ppm.width * y + x] = deep;
                        int col = (int)Math.Abs((zbuffer[ppm.width * y + x] * 255));
                        ppm[y][x] = new Color(col, col, col);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 绘制模型(统一法线)
    /// </summary>
    /// <param name="ppm">图片</param>
    /// <param name="obj">模型</param>
    public static void EasyModel(PPM ppm, Obj obj)
    {
        //光线方向
        Vector3 L_dir = new Vector3(0, 0, -1);

        //定义深度图
        float[] zbuffer = new float[ppm.width * ppm.height];
        for (int i = 0; i < zbuffer.Length; i++)
        {
            zbuffer[i] = float.PositiveInfinity;
        }

        //对三个顶点（一组三角形）做处理
        for (int i = 0; i < obj.positionIndex.Count; i += 3)
        {
            Box box = new Box();    //包围盒
            List<Vector3> points = new List<Vector3>();     //顶点数据组（图像x，图像y，深度z）

            //根据索引查找顶点并做视口变换，得到图像坐标存入顶点数据组
            for (int j = 0; j < 3; j++)
            {
                Vector3 v0 = obj.positions[obj.positionIndex[i + j] - 1];
                int x0 = (int)((v0.x + 1.0f) * ppm.width / 2.0f);
                int y0 = (int)((v0.y + 1.0f) * ppm.height / 2.0f);
                points.Add(new Vector3(x0, y0, v0.z));
            }

            //根据顶点坐标范围缩小包围盒
            box.min.x = Math.Max(0, Math.Min(points[0].x, Math.Min(points[1].x, points[2].x)));
            box.min.y = Math.Max(0, Math.Min(points[0].y, Math.Min(points[1].y, points[2].y)));
            box.max.x = Math.Min(ppm.width, Math.Max(points[0].x, Math.Max(points[1].x, points[2].x)));
            box.max.y = Math.Min(ppm.height, Math.Max(points[0].y, Math.Max(points[1].y, points[2].y)));

            //提出三角形顶点的图像坐标
            Vector2 p0 = new Vector2(points[0].x, points[0].y);
            Vector2 p1 = new Vector2(points[1].x, points[1].y);
            Vector2 p2 = new Vector2(points[2].x, points[2].y);

            //对标准模型>>三个顶点法线相同时可用
            Vector3 n = obj.normals[obj.normalIndex[i] - 1];
            float itensity = Vector3.Dot(n.Normalize(), L_dir);
            if (itensity < 0)
                continue;
            int col = (int)(itensity * 255);

            //对包围盒内的每一个像素做处理
            for (int y = (int)box.min.y; y <= box.max.y; y++)
            {
                for (int x = (int)box.min.x; x <= box.max.x; x++)
                {
                    //这个像素的图像坐标
                    Vector2 pixel = new Vector2(x, y);

                    //检查这个像素是否在三角形内部
                    Vector3 N = TAMath.CheckInside(p0, p1, p2, new Vector2(x, y));
                    if (N.x > 0 || N.y > 0 || N.z > 0)
                        continue;

                    //计算像素的重心坐标的深度值
                    float a = TAMath.Distance(p0, pixel);
                    float b = TAMath.Distance(p1, pixel);
                    float c = TAMath.Distance(p2, pixel);
                    float A = a / (a + b + c);
                    float B = b / (a + b + c);
                    float C = c / (a + b + c);
                    float deep = A * points[0].z + B * points[1].z + C * points[2].z;

                    //深度更新并绘制
                    if (deep <= zbuffer[ppm.width * y + x])
                    {
                        zbuffer[ppm.width * y + x] = deep;
                        ppm[y][x] = new Color(col, col, col);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 绘制模型通用版(非统一法线)
    /// </summary>
    /// <param name="ppm">图片</param>
    /// <param name="obj">模型</param>
    public static void CommonModel(PPM ppm, Obj obj)
    {
        //光线方向
        Vector3 L_dir = new Vector3(0, 0, -1);

        //定义深度图
        float[] zbuffer = new float[ppm.width * ppm.height];
        for (int i = 0; i < zbuffer.Length; i++)
        {
            zbuffer[i] = float.PositiveInfinity;
        }

        //对三个顶点（一组三角形）做处理
        for (int i = 0; i < obj.positionIndex.Count; i += 3)
        {
            Box box = new Box();    //包围盒
            List<Vector2> S_point = new List<Vector2>();     //顶点数据组（屏幕坐标）
            List<Vector3> W_point = new List<Vector3>();     //顶点数据组（空间坐标）

            //根据索引查找顶点并做视口变换，得到图像坐标存入顶点数据组
            for (int j = 0; j < 3; j++)
            {
                Vector3 v0 = obj.positions[obj.positionIndex[i + j] - 1];
                int x0 = (int)((v0.x + 1.0f) * ppm.width / 2.0f);
                int y0 = (int)((v0.y + 1.0f) * ppm.height / 2.0f);
                S_point.Add(new Vector2(x0, y0));
                W_point.Add(v0);
            }

            //根据顶点坐标范围缩小包围盒
            box.min.x = Math.Max(0, Math.Min(S_point[0].x, Math.Min(S_point[1].x, S_point[2].x)));
            box.min.y = Math.Max(0, Math.Min(S_point[0].y, Math.Min(S_point[1].y, S_point[2].y)));
            box.max.x = Math.Min(ppm.width, Math.Max(S_point[0].x, Math.Max(S_point[1].x, S_point[2].x)));
            box.max.y = Math.Min(ppm.height, Math.Max(S_point[0].y, Math.Max(S_point[1].y, S_point[2].y)));

            //提出三角形顶点的图像坐标
            Vector2 p0 = new Vector2(S_point[0].x, S_point[0].y);
            Vector2 p1 = new Vector2(S_point[1].x, S_point[1].y);
            Vector2 p2 = new Vector2(S_point[2].x, S_point[2].y);

            //根据三角形两边叉乘得到三角面的法线
            Vector3 OA = W_point[1] - W_point[0];
            Vector3 AB = W_point[2] - W_point[1];
            Vector3 n = OA ^ AB;
            float itensity = Vector3.Dot(n.Normalize(), L_dir);
            if (itensity < 0)
                continue;
            int col = (int)(itensity * 255);

            //对包围盒内的每一个像素做处理
            for (int y = (int)box.min.y; y <= box.max.y; y++)
            {
                for (int x = (int)box.min.x; x <= box.max.x; x++)
                {
                    //这个像素的图像坐标
                    Vector2 pixel = new Vector2(x, y);

                    //检查这个像素是否在三角形内部
                    Vector3 N = TAMath.CheckInside(p0, p1, p2, new Vector2(x, y));
                    if (N.x > 0 || N.y > 0 || N.z > 0)
                        continue;

                    //计算像素的重心坐标的深度值
                    float a = TAMath.Distance(p0, pixel);
                    float b = TAMath.Distance(p1, pixel);
                    float c = TAMath.Distance(p2, pixel);
                    float A = a / (a + b + c);
                    float B = b / (a + b + c);
                    float C = c / (a + b + c);
                    float deep = A * W_point[0].z + B * W_point[1].z + C * W_point[2].z;

                    //深度更新并绘制
                    if (deep <= zbuffer[ppm.width * y + x])
                    {
                        zbuffer[ppm.width * y + x] = deep;
                        ppm[y][x] = new Color(col, col, col);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 绘制模型通用版(投影变换版)
    /// </summary>
    /// <param name="ppm">图片</param>
    /// <param name="obj">模型</param>
    /// <param name="ratio">模型屏占比</param>
    public static void CommonModel_PRE(PPM ppm, Obj obj, float ratio = 1)
    {
        //光线方向
        Vector3 L_dir = new Vector3(0, 0, -1);

        //定义深度图
        float[] zbuffer = new float[ppm.width * ppm.height];
        for (int i = 0; i < zbuffer.Length; i++)
        {
            zbuffer[i] = float.PositiveInfinity;
        }

        //缩放后顶点组
        List<Vector3> newPoints = TF.Transform(TF.GetAllPoints(obj), ratio);

        //对三个顶点（一组三角形）做处理
        for (int i = 0; i < newPoints.Count; i += 3)
        {
            Box box = new Box();    //包围盒
            List<Vector2> S_point = new List<Vector2>();     //顶点数据组（屏幕坐标）
            List<Vector3> W_point = new List<Vector3>();     //顶点数据组（空间坐标）

            //根据索引查找顶点并做视口变换，得到图像坐标存入顶点数据组
            for (int j = 0; j < 3; j++)
            {
                Vector3 v0 = newPoints[i + j];
                int x0 = (int)((v0.x + 1.0f) * ppm.width / 2.0f);
                int y0 = (int)((v0.y + 1.0f) * ppm.height / 2.0f);
                S_point.Add(new Vector2(x0, y0));
                W_point.Add(v0);
            }

            //根据顶点坐标范围缩小包围盒
            box.min.x = Math.Max(0, Math.Min(S_point[0].x, Math.Min(S_point[1].x, S_point[2].x)));
            box.min.y = Math.Max(0, Math.Min(S_point[0].y, Math.Min(S_point[1].y, S_point[2].y)));
            box.max.x = Math.Min(ppm.width, Math.Max(S_point[0].x, Math.Max(S_point[1].x, S_point[2].x)));
            box.max.y = Math.Min(ppm.height, Math.Max(S_point[0].y, Math.Max(S_point[1].y, S_point[2].y)));

            //提出三角形顶点的图像坐标
            Vector2 p0 = new Vector2(S_point[0].x, S_point[0].y);
            Vector2 p1 = new Vector2(S_point[1].x, S_point[1].y);
            Vector2 p2 = new Vector2(S_point[2].x, S_point[2].y);

            //根据三角形两边叉乘得到三角面的法线
            Vector3 OA = W_point[1] - W_point[0];
            Vector3 AB = W_point[2] - W_point[1];
            Vector3 n = OA ^ AB;
            float itensity = Vector3.Dot(n.Normalize(), L_dir);
            if (itensity < 0)
                continue;
            int col = (int)(itensity * 255);

            //对包围盒内的每一个像素做处理
            for (int y = (int)box.min.y; y < box.max.y; y++)
            {
                for (int x = (int)box.min.x; x < box.max.x; x++)
                {
                    //这个像素的图像坐标
                    Vector2 pixel = new Vector2(x, y);

                    //检查这个像素是否在三角形内部
                    Vector3 N = TAMath.CheckInside(p0, p1, p2, new Vector2(x, y));
                    if (N.x > 0 || N.y > 0 || N.z > 0)
                        continue;

                    //计算像素的重心坐标的深度值
                    float a = TAMath.Distance(p0, pixel);
                    float b = TAMath.Distance(p1, pixel);
                    float c = TAMath.Distance(p2, pixel);
                    float A = a / (a + b + c);
                    float B = b / (a + b + c);
                    float C = c / (a + b + c);
                    float deep = A * W_point[0].z + B * W_point[1].z + C * W_point[2].z;

                    //深度更新并绘制
                    if (deep <= zbuffer[ppm.width * y + x])
                    {
                        zbuffer[ppm.width * y + x] = deep;
                        ppm[y][x] = new Color(col, col, col);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 绘制贴图模型
    /// </summary>
    /// <param name="ppm">最终图像</param>
    /// <param name="obj">绘制模型</param>
    /// <param name="texture">模型贴图</param>
    public static void TextureMap(PPM ppm, Obj obj, PPM texture)
    {
        //定义深度图
        float[] zbuffer = new float[ppm.width * ppm.height];
        for (int i = 0; i < zbuffer.Length; i++)
        {
            zbuffer[i] = float.PositiveInfinity;
        }

        //对三个顶点（一组三角形）做处理
        for (int i = 0; i < obj.positionIndex.Count; i += 3)
        {
            Box box = new Box();    //包围盒
            List<Vector2> S_point = new List<Vector2>();     //顶点数据组（屏幕坐标）
            List<Vector3> W_point = new List<Vector3>();     //顶点数据组（空间坐标）
            List<Vector2> UVs = new List<Vector2>();         //顶点处UV坐标

            //根据索引查找顶点并做视口变换，得到图像坐标存入顶点数据组
            for (int j = 0; j < 3; j++)
            {
                Vector3 v0 = obj.positions[obj.positionIndex[i + j] - 1];
                Vector2 uv = obj.uvs[obj.uvIndex[i + j] - 1];
                int x0 = (int)((v0.x + 1.0f) * ppm.width / 2.0f);
                int y0 = (int)((v0.y + 1.0f) * ppm.height / 2.0f);
                S_point.Add(new Vector2(x0, y0));
                W_point.Add(v0);
                UVs.Add(uv);
            }

            //根据顶点坐标范围缩小包围盒
            box.min.x = Math.Max(0, Math.Min(S_point[0].x, Math.Min(S_point[1].x, S_point[2].x)));
            box.min.y = Math.Max(0, Math.Min(S_point[0].y, Math.Min(S_point[1].y, S_point[2].y)));
            box.max.x = Math.Min(ppm.width, Math.Max(S_point[0].x, Math.Max(S_point[1].x, S_point[2].x)));
            box.max.y = Math.Min(ppm.height, Math.Max(S_point[0].y, Math.Max(S_point[1].y, S_point[2].y)));

            //提出三角形顶点的图像坐标
            Vector2 p0 = new Vector2(S_point[0].x, S_point[0].y);
            Vector2 p1 = new Vector2(S_point[1].x, S_point[1].y);
            Vector2 p2 = new Vector2(S_point[2].x, S_point[2].y);
            //提出顶点的uv坐标
            Vector2 uv0 = new Vector2(UVs[0].x, UVs[0].y);
            Vector2 uv1 = new Vector2(UVs[1].x, UVs[1].y);
            Vector2 uv2 = new Vector2(UVs[2].x, UVs[2].y);

            //对包围盒内的每一个像素做处理
            for (int y = (int)box.min.y; y <= box.max.y; y++)
            {
                for (int x = (int)box.min.x; x <= box.max.x; x++)
                {
                    //这个像素的图像坐标
                    Vector2 pixel = new Vector2(x, y);

                    //检查这个像素是否在三角形内部
                    Vector3 N = TAMath.CheckInside(p0, p1, p2, pixel);
                    if (N.x > 0 || N.y > 0 || N.z > 0)
                        continue;

                    //计算像素的重心坐标的插值系数
                    float a = TAMath.Distance(p0, pixel);
                    float b = TAMath.Distance(p1, pixel);
                    float c = TAMath.Distance(p2, pixel);
                    float A = a / (a + b + c);
                    float B = b / (a + b + c);
                    float C = c / (a + b + c);
                    //通过插值计算像素的重心坐标的深度值
                    float deep = A * W_point[0].z + B * W_point[1].z + C * W_point[2].z;
                    //通过插值计算像素的uv
                    Vector2 uv;
                    uv.x = A * uv0.x + B * uv1.x + C * uv2.x;
                    uv.y = A * uv0.y + B * uv1.y + C * uv2.y;
                    uv.y = Math.Abs(uv.y - 1);      //因为贴图原点在左上角，读取顺序为从上到下，而实际UV原点在左下角

                    //深度更新并绘制
                    if (deep <= zbuffer[ppm.width * y + x])
                    {
                        zbuffer[ppm.width * y + x] = deep;
                        //通过插值计算内部贴图颜色信息
                        ppm[y][x] = texture[(int)(uv.y * texture.height)][(int)(uv.x * texture.width)];
                    }
                }
            }
        }

    }

    /// <summary>
    /// 绘制贴图模型(加光照)
    /// </summary>
    /// <param name="ppm">最终图像</param>
    /// <param name="obj">绘制模型</param>
    /// <param name="texture">模型贴图</param>
    public static void TextureLight(PPM ppm, Obj obj, PPM texture)
    {
        //光线方向
        Vector3 L_dir = new Vector3(0, 0, -1);

        //定义深度图
        float[] zbuffer = new float[ppm.width * ppm.height];
        for (int i = 0; i < zbuffer.Length; i++)
        {
            zbuffer[i] = float.PositiveInfinity;
        }

        //对三个顶点（一组三角形）做处理
        for (int i = 0; i < obj.positionIndex.Count; i += 3)
        {
            Box box = new Box();    //包围盒
            List<Vector2> S_point = new List<Vector2>();     //顶点数据组（屏幕坐标）
            List<Vector3> W_point = new List<Vector3>();     //顶点数据组（空间坐标）
            List<Vector2> UVs = new List<Vector2>();         //顶点处UV坐标

            //根据索引查找顶点并做视口变换，得到图像坐标存入顶点数据组
            for (int j = 0; j < 3; j++)
            {
                Vector3 v0 = obj.positions[obj.positionIndex[i + j] - 1];
                Vector2 uv = obj.uvs[obj.uvIndex[i + j] - 1];
                int x0 = (int)((v0.x + 1.0f) * ppm.width / 2.0f);
                int y0 = (int)((v0.y + 1.0f) * ppm.height / 2.0f);
                S_point.Add(new Vector2(x0, y0));
                W_point.Add(v0);
                UVs.Add(uv);
            }

            //根据顶点坐标范围缩小包围盒
            box.min.x = Math.Max(0, Math.Min(S_point[0].x, Math.Min(S_point[1].x, S_point[2].x)));
            box.min.y = Math.Max(0, Math.Min(S_point[0].y, Math.Min(S_point[1].y, S_point[2].y)));
            box.max.x = Math.Min(ppm.width, Math.Max(S_point[0].x, Math.Max(S_point[1].x, S_point[2].x)));
            box.max.y = Math.Min(ppm.height, Math.Max(S_point[0].y, Math.Max(S_point[1].y, S_point[2].y)));

            //提出三角形顶点的图像坐标
            Vector2 p0 = new Vector2(S_point[0].x, S_point[0].y);
            Vector2 p1 = new Vector2(S_point[1].x, S_point[1].y);
            Vector2 p2 = new Vector2(S_point[2].x, S_point[2].y);
            //提出顶点的uv坐标
            Vector2 uv0 = new Vector2(UVs[0].x, UVs[0].y);
            Vector2 uv1 = new Vector2(UVs[1].x, UVs[1].y);
            Vector2 uv2 = new Vector2(UVs[2].x, UVs[2].y);

            //根据三角形两边叉乘得到三角面的法线
            Vector3 OA = W_point[1] - W_point[0];
            Vector3 AB = W_point[2] - W_point[1];
            Vector3 n = OA ^ AB;
            float itensity = Vector3.Dot(n.Normalize(), L_dir);
            if (itensity < 0)
                continue;

            //对包围盒内的每一个像素做处理
            for (int y = (int)box.min.y; y <= box.max.y; y++)
            {
                for (int x = (int)box.min.x; x <= box.max.x; x++)
                {
                    //这个像素的图像坐标
                    Vector2 pixel = new Vector2(x, y);

                    //检查这个像素是否在三角形内部
                    Vector3 N = TAMath.CheckInside(p0, p1, p2, pixel);
                    if (N.x > 0 || N.y > 0 || N.z > 0)
                        continue;

                    //计算像素的重心坐标的插值系数
                    float a = TAMath.Distance(p0, pixel);
                    float b = TAMath.Distance(p1, pixel);
                    float c = TAMath.Distance(p2, pixel);
                    float A = a / (a + b + c);
                    float B = b / (a + b + c);
                    float C = c / (a + b + c);
                    //通过插值计算像素的重心坐标的深度值
                    float deep = A * W_point[0].z + B * W_point[1].z + C * W_point[2].z;
                    //通过插值计算像素的uv
                    Vector2 uv;
                    uv.x = A * uv0.x + B * uv1.x + C * uv2.x;
                    uv.y = A * uv0.y + B * uv1.y + C * uv2.y;
                    uv.y = Math.Abs(uv.y - 1);      //因为贴图原点在左上角，读取顺序为从上到下，而实际UV原点在左下角

                    //深度更新并绘制
                    if (deep <= zbuffer[ppm.width * y + x])
                    {
                        zbuffer[ppm.width * y + x] = deep;
                        //通过插值计算内部贴图颜色信息
                        int Cr = (int)(texture[(int)(uv.y * texture.height)][(int)(uv.x * texture.width)].r * itensity);
                        int Cg = (int)(texture[(int)(uv.y * texture.height)][(int)(uv.x * texture.width)].g * itensity);
                        int Cb = (int)(texture[(int)(uv.y * texture.height)][(int)(uv.x * texture.width)].b * itensity);
                        ppm[y][x] = new Color(Cr, Cg, Cb);
                    }
                }
            }
        }
    }

    /// <summary>
    /// MSAA优化绘制模型
    /// </summary>
    /// <param name="ppm">图像</param>
    /// <param name="obj">模型</param>
    public static void MSAA(PPM ppm, Obj obj)
    {
        //光线方向
        Vector3 L_dir = new Vector3(0, 0, -1);

        //定义次像素(左下，右下，左上，右上)
        List<Vector2> pos = [new Vector2(0.25f, 0.25f),
                             new Vector2(0.75f, 0.25f),
                             new Vector2(0.25f, 0.75f),
                             new Vector2(0.75f, 0.75f)];

        //定义深度图X4
        float[] zbuffer = new float[ppm.width * ppm.height * 4];
        for (int i = 0; i < zbuffer.Length; i++)
        {
            zbuffer[i] = float.PositiveInfinity;
        }
        //定义颜色图X4
        List<Color> colorBuf = new List<Color>();
        for (int i = 0; i < zbuffer.Length; i++)
        {
            colorBuf.Add(Color.black);
        }

        //对三个顶点（一组三角形）做处理
        for (int i = 0; i < obj.positionIndex.Count; i += 3)
        {
            Box box = new Box();    //包围盒
            List<Vector2> S_point = new List<Vector2>();     //顶点数据组（屏幕坐标）
            List<Vector3> W_point = new List<Vector3>();     //顶点数据组（空间坐标）

            //根据索引查找顶点并做视口变换，得到图像坐标存入顶点数据组
            for (int j = 0; j < 3; j++)
            {
                Vector3 v0 = obj.positions[obj.positionIndex[i + j] - 1];
                int x0 = (int)((v0.x + 1.0f) * ppm.width / 2.0f);
                int y0 = (int)((v0.y + 1.0f) * ppm.height / 2.0f);
                S_point.Add(new Vector2(x0, y0));
                W_point.Add(v0);
            }

            //根据顶点坐标范围缩小包围盒
            box.min.x = Math.Max(0, Math.Min(S_point[0].x, Math.Min(S_point[1].x, S_point[2].x)));
            box.min.y = Math.Max(0, Math.Min(S_point[0].y, Math.Min(S_point[1].y, S_point[2].y)));
            box.max.x = Math.Min(ppm.width, Math.Max(S_point[0].x, Math.Max(S_point[1].x, S_point[2].x)));
            box.max.y = Math.Min(ppm.height, Math.Max(S_point[0].y, Math.Max(S_point[1].y, S_point[2].y)));

            //提出三角形顶点的图像坐标
            Vector2 p0 = new Vector2(S_point[0].x, S_point[0].y);
            Vector2 p1 = new Vector2(S_point[1].x, S_point[1].y);
            Vector2 p2 = new Vector2(S_point[2].x, S_point[2].y);

            //根据三角形两边叉乘得到三角面的法线
            Vector3 OA = W_point[1] - W_point[0];
            Vector3 AB = W_point[2] - W_point[1];
            Vector3 n = OA ^ AB;
            float itensity = Vector3.Dot(n.Normalize(), L_dir);
            if (itensity < 0)
                continue;

            //对包围盒内的每一个像素做处理
            for (int y = (int)box.min.y; y <= box.max.y; y++)
            {
                for (int x = (int)box.min.x; x <= box.max.x; x++)
                {
                    //这个像素的图像坐标
                    Vector2 pixel = new Vector2(x, y);
                    //int count = 0;

                    //否则根据次像素的覆盖率计算该像素的颜色值
                    float col = itensity * 255;
                    int color = (int)col;

                    //这个像素的次像素的坐标
                    for (int t = 0; t < 4; t++)
                    {
                        Vector2 pp = pixel + pos[t];
                        //检查这个次像素是否在三角形内部
                        Vector3 N = TAMath.CheckInside(p0, p1, p2, pp);
                        if (N.x > 0 || N.y > 0 || N.z > 0)
                            continue;
                        else
                        {
                            float a_ = TAMath.Distance(p0, pp);
                            float b_ = TAMath.Distance(p1, pp);
                            float c_ = TAMath.Distance(p2, pp);
                            float A = a_ / (a_ + b_ + c_);
                            float B = b_ / (a_ + b_ + c_);
                            float C = c_ / (a_ + b_ + c_);
                            float deep = A * W_point[0].z + B * W_point[1].z + C * W_point[2].z;

                            if (deep <= zbuffer[GetIndex(x * 2 + t % 2, y * 2 + t / 2, ppm)])
                            {
                                zbuffer[GetIndex(x * 2 + t % 2, y * 2 + t / 2, ppm)] = deep;
                                colorBuf[GetIndex(x * 2 + t % 2, y * 2 + t / 2, ppm)] = new Color(color,color,color);
                            }

                            int r, g, b;
                            r = (colorBuf[GetIndex(x * 2, y * 2, ppm)].r + colorBuf[GetIndex(x * 2 + 1, y * 2, ppm)].r + colorBuf[GetIndex(x * 2, y * 2 + 1, ppm)].r + colorBuf[GetIndex(x * 2 + 1, y * 2 + 1, ppm)].r) / 4;
                            g = (colorBuf[GetIndex(x * 2, y * 2, ppm)].g + colorBuf[GetIndex(x * 2 + 1, y * 2, ppm)].g + colorBuf[GetIndex(x * 2, y * 2 + 1, ppm)].g + colorBuf[GetIndex(x * 2 + 1, y * 2 + 1, ppm)].g) / 4;
                            b = (colorBuf[GetIndex(x * 2, y * 2, ppm)].b + colorBuf[GetIndex(x * 2 + 1, y * 2, ppm)].b + colorBuf[GetIndex(x * 2, y * 2 + 1, ppm)].b + colorBuf[GetIndex(x * 2 + 1, y * 2 + 1, ppm)].b) / 4;
                            ppm[y][x] = new Color(r, g, b);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Texture_MSAA优化绘制模型
    /// </summary>
    /// <param name="ppm">图像</param>
    /// <param name="obj">模型</param>
    /// <param name="texture">贴图</param>
    public static void Texture_MSAA(PPM ppm, Obj obj, PPM texture)
    {
        //光线方向
        Vector3 L_dir = new Vector3(0, 0, -1);

        //定义次像素(左下，右下，左上，右上)
        List<Vector2> pos = [new Vector2(0.25f, 0.25f),
            new Vector2(0.75f, 0.25f),
            new Vector2(0.25f, 0.75f),
            new Vector2(0.75f, 0.75f)];

        //定义深度图X4
        float[] zbuffer = new float[ppm.width * ppm.height * 4];
        for (int i = 0; i < zbuffer.Length; i++)
        {
            zbuffer[i] = float.PositiveInfinity;
        }
        //定义颜色图X4
        List<Color> colorBuf = new List<Color>();
        for (int i = 0; i < zbuffer.Length; i++)
        {
            colorBuf.Add(Color.black);
        }

        //对三个顶点（一组三角形）做处理
        for (int i = 0; i < obj.positionIndex.Count; i += 3)
        {
            Box box = new Box();    //包围盒
            List<Vector2> S_point = new List<Vector2>();     //顶点数据组（屏幕坐标）
            List<Vector3> W_point = new List<Vector3>();     //顶点数据组（空间坐标）
            List<Vector2> UVs = new List<Vector2>();         //顶点处UV坐标

            //根据索引查找顶点并做视口变换，得到图像坐标存入顶点数据组
            for (int j = 0; j < 3; j++)
            {
                Vector3 v0 = obj.positions[obj.positionIndex[i + j] - 1];
                Vector2 uv = obj.uvs[obj.uvIndex[i + j] - 1];
                int x0 = (int)((v0.x + 1.0f) * ppm.width / 2.0f);
                int y0 = (int)((v0.y + 1.0f) * ppm.height / 2.0f);
                S_point.Add(new Vector2(x0, y0));
                W_point.Add(v0);
                UVs.Add(uv);
            }

            //根据顶点坐标范围缩小包围盒
            box.min.x = Math.Max(0, Math.Min(S_point[0].x, Math.Min(S_point[1].x, S_point[2].x)));
            box.min.y = Math.Max(0, Math.Min(S_point[0].y, Math.Min(S_point[1].y, S_point[2].y)));
            box.max.x = Math.Min(ppm.width, Math.Max(S_point[0].x, Math.Max(S_point[1].x, S_point[2].x)));
            box.max.y = Math.Min(ppm.height, Math.Max(S_point[0].y, Math.Max(S_point[1].y, S_point[2].y)));

            //提出三角形顶点的图像坐标
            Vector2 p0 = new Vector2(S_point[0].x, S_point[0].y);
            Vector2 p1 = new Vector2(S_point[1].x, S_point[1].y);
            Vector2 p2 = new Vector2(S_point[2].x, S_point[2].y);
            //提出顶点的uv坐标
            Vector2 uv0 = new Vector2(UVs[0].x, UVs[0].y);
            Vector2 uv1 = new Vector2(UVs[1].x, UVs[1].y);
            Vector2 uv2 = new Vector2(UVs[2].x, UVs[2].y);

            //根据三角形两边叉乘得到三角面的法线
            Vector3 OA = W_point[1] - W_point[0];
            Vector3 AB = W_point[2] - W_point[1];
            Vector3 n = OA ^ AB;
            float itensity = Vector3.Dot(n.Normalize(), L_dir);
            if (itensity < 0)
                continue;

            //对包围盒内的每一个像素做处理
            for (int y = (int)box.min.y; y <= box.max.y; y++)
            {
                for (int x = (int)box.min.x; x <= box.max.x; x++)
                {
                    //这个像素的图像坐标
                    Vector2 pixel = new Vector2(x, y);

                    //这个像素的次像素的坐标
                    for (int t = 0; t < 4; t++)
                    {
                        Vector2 pp = pixel + pos[t];
                        //检查这个次像素是否在三角形内部
                        Vector3 N = TAMath.CheckInside(p0, p1, p2, pp);
                        if (N.x > 0 || N.y > 0 || N.z > 0)
                            continue;
                        else
                        {
                            float a_ = TAMath.Distance(p0, pp);
                            float b_ = TAMath.Distance(p1, pp);
                            float c_ = TAMath.Distance(p2, pp);
                            float A = a_ / (a_ + b_ + c_);
                            float B = b_ / (a_ + b_ + c_);
                            float C = c_ / (a_ + b_ + c_);
                            float deep = A * W_point[0].z + B * W_point[1].z + C * W_point[2].z;

                            //通过插值计算像素的uv
                            Vector2 uv;
                            uv.x = A * uv0.x + B * uv1.x + C * uv2.x;
                            uv.y = A * uv0.y + B * uv1.y + C * uv2.y;
                            uv.y = Math.Abs(uv.y - 1);      //因为贴图原点在左上角，读取顺序为从上到下，而实际UV原点在左下角

                            //通过插值计算内部贴图颜色信息
                            int Cr = (int)(texture[(int)(uv.y * texture.height)][(int)(uv.x * texture.width)].r * itensity);
                            int Cg = (int)(texture[(int)(uv.y * texture.height)][(int)(uv.x * texture.width)].g * itensity);
                            int Cb = (int)(texture[(int)(uv.y * texture.height)][(int)(uv.x * texture.width)].b * itensity);

                            if (deep <= zbuffer[GetIndex(x * 2 + t % 2, y * 2 + t / 2, ppm)])
                            {
                                zbuffer[GetIndex(x * 2 + t % 2, y * 2 + t / 2, ppm)] = deep;
                                colorBuf[GetIndex(x * 2 + t % 2, y * 2 + t / 2, ppm)] = new Color(Cr, Cg, Cb);
                            }

                            int r, g, b;
                            r = (colorBuf[GetIndex(x * 2, y * 2, ppm)].r + colorBuf[GetIndex(x * 2 + 1, y * 2, ppm)].r + colorBuf[GetIndex(x * 2, y * 2 + 1, ppm)].r + colorBuf[GetIndex(x * 2 + 1, y * 2 + 1, ppm)].r) / 4;
                            g = (colorBuf[GetIndex(x * 2, y * 2, ppm)].g + colorBuf[GetIndex(x * 2 + 1, y * 2, ppm)].g + colorBuf[GetIndex(x * 2, y * 2 + 1, ppm)].g + colorBuf[GetIndex(x * 2 + 1, y * 2 + 1, ppm)].g) / 4;
                            b = (colorBuf[GetIndex(x * 2, y * 2, ppm)].b + colorBuf[GetIndex(x * 2 + 1, y * 2, ppm)].b + colorBuf[GetIndex(x * 2, y * 2 + 1, ppm)].b + colorBuf[GetIndex(x * 2 + 1, y * 2 + 1, ppm)].b) / 4;
                            ppm[y][x] = new Color(r, g, b);
                        }
                    }
                }
            }
        }
    }

}