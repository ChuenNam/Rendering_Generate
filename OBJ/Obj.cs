using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class TAMath
{
    public static readonly float d2r = 0.01745329f;
    public static float Degree2Radius(float angle)
    {
        return angle * d2r;
    }
    public static float Lerp(float start, float end, float t)
    {
        return start + (end - start) * t;
    }
    public static Vector3 LerpV(Vector3 start, Vector3 end, float t)
    {
        Vector3 vec = new Vector3();
        vec.x = Lerp(start.x, end.x, t);
        vec.y = Lerp(start.y, end.y, t);
        vec.z = Lerp(start.z, end.z, t);
        return vec;
    }
    public static void Swap<T>(ref T x,ref T y)
    {
        T t = x;
        x = y;
        y = t;
    }
    public static Vector3 CheckInside(Vector2 A, Vector2 B, Vector2 C, Vector2 p)
    {
        Vector2 AB = B - A;
        Vector2 BC = C - B;
        Vector2 CA = A - C;

        Vector2 AP = p - A;
        Vector2 BP = p - B;
        Vector2 CP = p - C;

        Vector3 vector3 = new Vector3((AB ^ AP).z, (BC ^ BP).z, (CA ^ CP).z);
        return vector3;
    }
    public static float Distance(Vector2 v1, Vector2 v2)
    {
        float x = v1.x - v2.x;
        float y = v1.y - v2.y;
        float distance = (float)Math.Sqrt(x * x + y * y);
        return distance;
    }
}

public struct Vector4
{
    public float x, y, z, w;
    public Vector4() { }
    public Vector4(float x,float y, float z, float w)
    {
        this.x = x; this.y = y;
        this.z = z; this.w = w;
    }
    public Vector4(Vector3 p, float w)
    {
        x = p.x; y = p.y; z = p.z;
        this.w = w;
    }

    public static float operator *(Vector4 v1,Vector4 v2)
    {
        float t;
        t = v1.x * v2.x + v1.y * v2.y+ v1.z * v2.z+ v1.w * v2.w;
        return t;
    }
    public static Vector4 operator +(Vector4 v1,Vector4 v2)
    {
        Vector4 v;
        v.x = v1.x + v2.x;
        v.y = v1.y + v2.y;
        v.z = v1.z + v2.z;
        v.w = v1.w + v2.w;
        return v;
    }

    public override string ToString()
    {
        return x + " " + y + " " + z + " " + w + " ";
    }
}
 
public struct Vector3
{
    public float x, y, z;
    public Vector3() { }
    public Vector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public override string ToString()
    {
        return x + " " + y + " " + z + " ";
    }

    #region 运算符重载
    public static Vector3 operator +(Vector3 v1, Vector3 v2)
    {
        Vector3 v;
        v.x = v1.x + v2.x;
        v.y = v1.y + v2.y;
        v.z = v1.z + v2.z;
        return v;
    }
    public static Vector3 operator -(Vector3 v1, Vector3 v2)
    {
        Vector3 v;
        v.x = v1.x - v2.x;
        v.y = v1.y - v2.y;
        v.z = v1.z - v2.z;
        return v;
    }
    public static Vector3 operator *(Vector3 v1, Vector3 v2)
    {
        Vector3 v;
        v.x = v1.x * v2.x;
        v.y = v1.y * v2.y;
        v.z = v1.z * v2.z;
        return v;
    }
    public static Vector3 operator -(Vector3 v1)
    {
        Vector3 v;
        v.x = -v1.x;
        v.y = -v1.y;
        v.z = -v1.z;
        return v;
    }
    public static Vector3 operator *(Vector3 v1, float scale)
    {
        Vector3 v;
        v.x = v1.x * scale;
        v.y = v1.y * scale;
        v.z = v1.z * scale;
        return v;
    }
    public static Vector3 operator ^(Vector3 v1, Vector3 v2)
    {
        float h = v1.y * v2.z - v1.z * v2.y;
        float m = v1.z * v2.x - v1.x * v2.z;
        float n = v1.x * v2.y - v1.y * v2.x;
        Vector3 V = new Vector3(h, m, n);
        return V;
    }

    #endregion

    public Vector3 Normalize()
    {
        float length = (float)Math.Sqrt(x * x + y * y + z * z);
        return this * (1.0f / length);
    }
    public float normalize()
    {
        float length = (float)Math.Sqrt(x * x + y * y + z * z);
        return length;
    }

    public static Vector3 Add(Vector3 v1, Vector3 v2)
    {
        Vector3 vector = new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        return vector;
    }

    public static float Dot(Vector3 v1, Vector3 v2)
    {
        float f = v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        return f;
    }

    public Vector3 RotateXY(float angle)
    {
        //当前坐标
        float x1 = this.x;
        float y1 = this.y;

        //旋转后坐标
        float x2, y2;
        float a = angle * TAMath.d2r;

        x2 = (float)(x1 * Math.Cos(a) - y1 * Math.Sin(a));
        y2 = (float)(x1 * Math.Sin(a) + y1 * Math.Cos(a));

        return new Vector3(x2, y2, this.z);
    }

    public float Length
    {
        get
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }
    }
    public Vector3 Identity
    {
        get
        {
            float length = (float)Math.Sqrt(x * x + y * y + z * z);
            return this * (1.0f / length);
        }
    }
}

public struct Vector2
{
    public float x, y;
    public Vector2() { }
    public Vector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
    public override string ToString()
    {
        return x + " " + y + " ";
    }
    public int Xint => (int)x;
    public int Yint => (int)y;
    public float Length
    {
        get
        {
            return (float)Math.Sqrt(x * x + y * y);
        }
    }
    public Vector2 Normalize()
    {
        float length = (float)Math.Sqrt(x * x + y * y);
        return this * (1.0f / length);
    }

    #region 运算符重载
    public static Vector2 operator +(Vector2 v1, Vector2 v2)
    {
        Vector2 v;
        v.x = v1.x + v2.x;
        v.y = v1.y + v2.y;
        return v;
    }
    public static Vector2 operator -(Vector2 v1, Vector2 v2)
    {
        Vector2 v;
        v.x = v1.x - v2.x;
        v.y = v1.y - v2.y;
        return v;
    }
    public static Vector2 operator *(Vector2 v1, Vector2 v2)
    {
        Vector2 v;
        v.x = v1.x * v2.x;
        v.y = v1.y * v2.y;
        return v;
    }
    public static Vector2 operator -(Vector2 v1)
    {
        Vector2 v;
        v.x = -v1.x;
        v.y = -v1.y;
        return v;
    }
    public static Vector2 operator *(Vector2 v1, float scale)
    {
        Vector2 v;
        v.x = v1.x * scale;
        v.y = v1.y * scale;
        return v;
    }
    public static Vector3 operator ^(Vector2 v1, Vector2 v2)
    {
        Vector3 A = new Vector3(v1.x, v1.y, 0);
        Vector3 B = new Vector3(v2.x, v2.y, 0);
        float h = A.y * B.z - A.z * B.y;
        float m = A.z * B.x - A.x * B.z;
        float n = A.x * B.y - A.y * B.x;
        Vector3 V = new Vector3(h, m, n);
        return V;
    }
    #endregion
}


public class Triangle
{
    public List<Vector3> positions = new List<Vector3>();
    public List<Vector3> normals = new List<Vector3>();
    public List<Vector2> uvs = new List<Vector2>();

    public List<int> positionIndex = new List<int>();
    public List<int> uvIndex = new List<int>();
    public List<int> normalIndex = new List<int>();

    public Triangle() { }

    public Triangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        positions.Add(p1);
        positions.Add(p2);
        positions.Add(p3);

        positionIndex.Add(1);
        positionIndex.Add(2);
        positionIndex.Add(3);
    }

    public Triangle GetCopy()
    {
        Triangle copy = new Triangle();

        foreach (var item in positions)
        {
            copy.positions.Add(item);
        }
        foreach (var item in uvs)
        {
            copy.uvs.Add(item);
        }
        foreach (var item in normals)
        {
            copy.normals.Add(item);
        }
        foreach (var item in positionIndex)
        {
            copy.positionIndex.Add(item);
        }
        foreach (var item in uvIndex)
        {
            copy.uvIndex.Add(item);
        }
        foreach (var item in normalIndex)
        {
            copy.normalIndex.Add(item);
        }

        return copy;
    }
}


public class Obj
{
    public List<Vector3> positions = new List<Vector3>();
    public List<Vector3> normals = new List<Vector3>();
    public List<Vector2> uvs = new List<Vector2>();

    public List<int> positionIndex = new List<int>();
    public List<int> uvIndex = new List<int>();
    public List<int> normalIndex = new List<int>();

    private void Init()
    {
        positions = new List<Vector3>();
        normals = new List<Vector3>();
        uvs = new List<Vector2>();

        positionIndex = new List<int>();
        uvIndex = new List<int>();
        normalIndex = new List<int>();
    }

    public Obj()
    {
        Init();
    }

    //Read
    public void Read(string path)
    {
        Init();

        //读取文件信息
        string[] lines = File.ReadAllLines(path);

        //解析信息
        foreach (string line in lines)
        {
            //解析坐标
            if (line.StartsWith("v") && !line.StartsWith("vt") && !line.StartsWith("vn"))
            {
                //替换V为空，剔除空格，存入数组
                string newLine = line.Replace("v", "");
                string[] newLines = newLine.Split(' ');

                //检测空字符
                int count = 0;
                float[] xyz = new float[3];
                for (int i = 0; i < newLines.Length; i++)
                {
                    if (newLines[i].Length > 0)
                    {
                        //如果不是空字符
                        xyz[count] = float.Parse(newLines[i]);
                        count++;
                    }
                }

                //位置信息解析正确后
                Vector3 pos = new Vector3(xyz[0], xyz[1], xyz[2]);
                //存到 positions链表
                positions.Add(pos);
            }
            //解析UV
            else if (line.StartsWith("vt"))
            {
                string newLine = line.Replace("vt", "");
                string[] newLines = newLine.Split(' ');

                int count = 0;
                float[] xy = new float[2];
                for (int i = 0; i < newLines.Length; i++)
                {
                    if (newLines[i].Length > 0)
                    {
                        xy[count] = float.Parse(newLines[i]);
                        count++;
                    }
                }

                Vector2 uv = new Vector2(xy[0], xy[1]);
                uvs.Add(uv);
            }
            //解析法线
            else if (line.StartsWith("vn"))
            {
                string newLine = line.Replace("vn", "");
                string[] newLines = newLine.Split(' ');

                int count = 0;
                float[] xyz = new float[3];
                for (int i = 0; i < newLines.Length; i++)
                {
                    if (newLines[i].Length > 0)
                    {
                        xyz[count] = float.Parse(newLines[i]);
                        count++;
                    }
                }

                Vector3 normal = new Vector3(xyz[0], xyz[1], xyz[2]);
                normals.Add(normal);
            }
            //解析Index Face
            else if (line.StartsWith("f"))
            {

                //位置 UV 法线 都有
                if (uvs.Count > 0 && normals.Count > 0)
                {
                    string newLine = line.Replace("f", "");
                    string[] newLines = newLine.Split(' ');

                    foreach (var item in newLines)
                    {
                        if (item.Length == 0) continue;

                        string[] posUVNormal = item.Split('/');
                        positionIndex.Add(int.Parse(posUVNormal[0]));
                        uvIndex.Add(int.Parse(posUVNormal[1]));
                        normalIndex.Add(int.Parse(posUVNormal[2]));
                    }
                }

                //pos
                else if (positions.Count > 0 && uvs.Count == 0 && normals.Count == 0)
                {
                    string newLine = line.Replace("f", "");
                    string[] newLines = newLine.Split(' ');

                    foreach (var item in newLines)
                    {
                        if (item.Length == 0) continue;
                        positionIndex.Add(int.Parse(item));
                    }

                }

                //pos uv
                else if (positions.Count > 0 && uvs.Count > 0 && normals.Count == 0)
                {
                    string newLine = line.Replace("f", "");
                    string[] newLines = newLine.Split(' ');

                    foreach (var item in newLines)
                    {
                        if (item.Length == 0) continue;

                        string[] posUVNormal = item.Split('/');
                        positionIndex.Add(int.Parse(posUVNormal[0]));
                        uvIndex.Add(int.Parse(posUVNormal[1]));
                    }
                }

                //pos normal
                else if (positions.Count > 0 && uvs.Count == 0 && normals.Count > 0)
                {
                    string newLine = line.Replace("f", "");
                    string[] newLines = newLine.Split(' ');

                    foreach (var item in newLines)
                    {
                        if (item.Length == 0) continue;

                        string newItem = item.Replace("//", "/");

                        string[] posUVNormal = newItem.Split('/');
                        positionIndex.Add(int.Parse(posUVNormal[0]));
                        normalIndex.Add(int.Parse(posUVNormal[1]));
                    }
                }
            }

        }

        #region 输出测试
        //foreach (var item in positionIndex)
        //{
        //    Console.WriteLine(item + " ");
        //}
        //return;


        //foreach (var item in positions)
        //{
        //    Console.WriteLine(item);
        //}
        //foreach (var item in uvs)
        //{
        //    Console.WriteLine(item);
        //}
        //foreach (var item in normals)
        //{
        //    Console.WriteLine(item);
        //}


        //foreach (var item in positionIndex)
        //{
        //    Console.Write(item);
        //}
        //Console.WriteLine();

        //foreach (var item in uvIndex)
        //{
        //    Console.Write(item);
        //}
        //Console.WriteLine();

        //foreach (var item in normalIndex)
        //{
        //    Console.Write(item);
        //}
        //Console.WriteLine();

        #endregion
    }

    //Save
    public void Save(string path)
    {
        List<string> lines = new List<string>();

        if (positions.Count > 0)
        {
            foreach (var item in positions)
            {
                string line = "v ";
                line += item.x + " " + item.y + " " + item.z;
                lines.Add(line);
            }
        }

        if (uvs.Count > 0)
        {
            foreach (var item in uvs)
            {
                string line = "vt ";
                line += item.x + " " + item.y;
                lines.Add(line);
            }
        }

        if (normals.Count > 0)
        {
            foreach (var item in normals)
            {
                string line = "vn ";
                line += item.x + " " + item.y + " " + item.z;
                lines.Add(line);
            }
        }

        //pos uv normal
        if (positions.Count > 0 && uvs.Count > 0 && normals.Count > 0)
        {
            int count = 0;
            string[] vertex = new string[3];
            for (int i = 0; i < positionIndex.Count; i++)
            {
                int pos = positionIndex[i];
                int uv = uvIndex[i];
                int normal = normalIndex[i];

                string part = pos + "/" + uv + "/" + normal;
                vertex[count++] = part;

                if (count == 3)
                {
                    string line = "f " + vertex[0] + " " + vertex[1] + " " + vertex[2];
                    lines.Add(line);
                    count = 0;
                }
            }
        }

        //pos
        else if (positions.Count > 0 && uvs.Count == 0 && normals.Count == 0)
        {
            int count = 0;
            string[] vertex = new string[3];
            for (int i = 0; i < positionIndex.Count; i++)
            {
                int pos = positionIndex[i];

                string part = pos.ToString();
                vertex[count++] = part;

                if (count == 3)
                {
                    string line = "f " + vertex[0] + " " + vertex[1] + " " + vertex[2];
                    lines.Add(line);
                    count = 0;
                }
            }
        }

        //pos uv 
        else if (positions.Count > 0 && uvs.Count > 0 && normals.Count == 0)
        {
            int count = 0;
            string[] vertex = new string[3];
            for (int i = 0; i < positionIndex.Count; i++)
            {
                int pos = positionIndex[i];
                int uv = uvIndex[i];

                string part = pos + "/" + uv;
                vertex[count++] = part;

                if (count == 3)
                {
                    string line = "f " + vertex[0] + " " + vertex[1] + " " + vertex[2];
                    lines.Add(line);
                    count = 0;
                }
            }
        }

        //pos normal 
        else if (positions.Count > 0 && uvs.Count == 0 && normals.Count > 0)
        {
            int count = 0;
            string[] vertex = new string[3];
            for (int i = 0; i < positionIndex.Count; i++)
            {
                int pos = positionIndex[i];
                int normal = normalIndex[i];

                string part = pos + "//" + normal;
                vertex[count++] = part;

                if (count == 3)
                {
                    string line = "f " + vertex[0] + " " + vertex[1] + " " + vertex[2];
                    lines.Add(line);
                    count = 0;
                }
            }
        }

        //写入文件信息
        File.WriteAllLines(path, lines);
    }

    //三角形
    public void AddTriangle(Triangle triCopy)
    {
        Triangle tri = triCopy.GetCopy();

        if (tri.positions.Count > 0)
            this.positions.AddRange(tri.positions);
        if (tri.uvs.Count > 0)
            this.uvs.AddRange(tri.uvs);
        if (tri.normals.Count > 0)
            this.normals.AddRange(tri.normals);

        if (tri.positionIndex.Count > 0)
        {
            for (int i = 0; i < tri.positionIndex.Count; i++)
            {
                tri.positionIndex[i] += this.positionIndex.Count;
            }

            this.positionIndex.AddRange(tri.positionIndex);
        }
        if (tri.uvIndex.Count > 0)
        {
            for (int i = 0; i < tri.uvIndex.Count; i++)
            {
                tri.uvIndex[i] += this.uvIndex.Count;
            }

            this.uvIndex.AddRange(tri.uvIndex);
        }
        if (tri.normalIndex.Count > 0)
        {
            for (int i = 0; i < tri.normalIndex.Count; i++)
            {
                tri.normalIndex[i] += this.normalIndex.Count;
            }

            this.normalIndex.AddRange(tri.normalIndex);
        }
    }

    //生成三角形
    public static void ProceduralGeneration01()
    {
        Obj obj = new Obj();

        Triangle tri = new Triangle();
        tri.positions.Add(new Vector3(0, 0, 0));
        tri.positions.Add(new Vector3(1, 0, 0));
        tri.positions.Add(new Vector3(0, 1, 0));
        tri.positionIndex = new List<int> { 1, 2, 3 };

        obj.AddTriangle(tri);

        obj.Save("ProceduralGeneration01.obj");
    }

    //生成正方形
    public static void ProceduralGeneration02()
    {
        Obj obj = new Obj();

        Triangle tri = new Triangle();
        tri.positions.Add(new Vector3(0, 0, 0));
        tri.positions.Add(new Vector3(1, 0, 0));
        tri.positions.Add(new Vector3(0, 1, 0));
        tri.positionIndex = new List<int> { 1, 2, 3 };

        Triangle tri2 = new Triangle();
        tri2.positions.Add(new Vector3(1, 0, 0));
        tri2.positions.Add(new Vector3(0, 1, 0));
        tri2.positions.Add(new Vector3(1, 1, 0));
        tri2.positionIndex = new List<int> { 1, 2, 3 };

        obj.AddTriangle(tri);
        obj.AddTriangle(tri2);

        obj.Save("ProceduralGeneration02.obj");
    }

    //生成表面细分
    public static void ProceduralGeneration03()
    {
        Obj obj = new Obj();

        Random rand = new Random();

        //宽高
        float w = 10;
        float h = 10;
        //细分数量
        float count = 100;

        float dx = w / count;
        float dy = h / count;

        for (int y = 0; y < count; y++)
        {
            for (int x = 0; x < count; x++)
            {
                Vector3 p1 = new Vector3(0 + x * dx, (float)rand.NextDouble() * 1.1f, 0 + y * dy);
                Vector3 p2 = new Vector3(dx + x * dx, (float)rand.NextDouble() * 1.1f, 0 + y * dy);
                Vector3 p3 = new Vector3(0 + x * dx, (float)rand.NextDouble() * 1.1f, dy + y * dy);
                Vector3 p4 = new Vector3(dx + x * dx, (float)rand.NextDouble() * 1.1f, dy + y * dy);

                Triangle tri = new Triangle();
                tri.positions.Add(p1);
                tri.positions.Add(p2);
                tri.positions.Add(p3);
                tri.positionIndex = new List<int> { 1, 2, 3 };
                obj.AddTriangle(tri);

                Triangle tri2 = new Triangle();
                tri2.positions.Add(p2);
                tri2.positions.Add(p3);
                tri2.positions.Add(p4);
                tri2.positionIndex = new List<int> { 1, 2, 3 };
                obj.AddTriangle(tri2);
            }
        }

        obj.Save("ProceduralGeneration03.obj");
    }

    //随机立方体
    public static void ProceduralGeneration04()
    {
        Obj obj = new Obj();

        Random rand = new Random();

        //宽高
        float w = 5;
        float h = 5;
        //细分数量
        float count = 100;

        float dx = w / count;
        float dy = h / count;

        for (int y = 0; y < count; y++)
        {
            for (int x = 0; x < count; x++)
            {
                Vector3 p1 = new Vector3(0 + (float)rand.NextDouble(), (float)rand.NextDouble() * 1.1f, 0 + (float)rand.NextDouble());
                Vector3 p2 = new Vector3(dx + (float)rand.NextDouble(), (float)rand.NextDouble() * 1.1f, 0 + (float)rand.NextDouble());
                Vector3 p3 = new Vector3(0 + (float)rand.NextDouble(), (float)rand.NextDouble() * 1.1f, dy + (float)rand.NextDouble());
                Vector3 p4 = new Vector3(dx + (float)rand.NextDouble(), (float)rand.NextDouble() * 1.1f, dy + (float)rand.NextDouble());

                Triangle tri = new Triangle();
                tri.positions.Add(p1);
                tri.positions.Add(p2);
                tri.positions.Add(p3);
                tri.positionIndex = new List<int> { 1, 2, 3 };
                obj.AddTriangle(tri);

                Triangle tri2 = new Triangle();
                tri2.positions.Add(p2);
                tri2.positions.Add(p3);
                tri2.positions.Add(p4);
                tri2.positionIndex = new List<int> { 1, 2, 3 };
                obj.AddTriangle(tri2);
            }
        }

        obj.Save("ProceduralGeneration04.obj");
    }

    //修改模型
    public static void ModifyModel()
    {
        Obj obj = new Obj();
        obj.Read("Monkey.obj");

        Random rand = new Random();

        //沿着法线方向放缩物体
        if (obj.normalIndex.Count == 0)
        {
            Console.WriteLine("模型没有法线");
            return;
        }
        for (int i = 0; i < obj.normalIndex.Count; i++)
        {
            int normalIdx = obj.normalIndex[i] - 1;
            int positionIdx = obj.positionIndex[i] - 1;

            Vector3 normal = obj.normals[normalIdx];
            Vector3 position = obj.positions[positionIdx];

            float scale = 0.2f;
            Vector3 scaleNormal = new Vector3(normal.x * scale, normal.y * scale, normal.z * scale);

            float posScale = 1.5f;
            Vector3 posX2 = new Vector3(position.x * posScale, position.y * posScale, position.z * posScale);

            float randValue = (float)rand.NextDouble();
            randValue = randValue - 0.5f;
            randValue *= 0.05f;

            //if (rand.NextDouble() < 0.25f)
            //{
            //    randValue = 0;
            //}

            Vector3 positionRandom = new Vector3(normal.x * randValue, normal.y * randValue, normal.z * randValue);

            //obj.positions[positionIdx] = Vector3.Add(position, scaleNormal);
            obj.positions[positionIdx] = Vector3.Add(position, positionRandom);
        }

        obj.Save("MonkeyScale.obj");
    }

    //程序化多边形（圆）
    public static void GenCircle(float step, float radius, string path, float maxAngle = 360f)
    {
        Obj obj = new Obj();

        float delta = maxAngle / step;

        Vector3 p1 = new Vector3(0, 0, 0);
        Vector3 p2 = new Vector3(radius, 0, 0);
        Vector3 p3;

        for (int i = 1; i <= step; i++)
        {
            float rad = i * delta * TAMath.d2r;
            float x = (float)Math.Cos(rad);
            float y = (float)Math.Sin(rad);

            p3 = new Vector3(x * radius, 0, y * radius);
            obj.AddTriangle(new Triangle(p1, p2, p3));

            p2 = p3;
        }

        obj.Save(path);
    }

    //程序化生成锥体
    public static void GenCone(float step, float high, float radius, string path, float maxAngle = 360f)
    {
        Obj obj = new Obj();

        float delta = maxAngle / step;

        Vector3 p0 = new Vector3(0, high, 0);
        Vector3 p1 = new Vector3(0, 0, 0);
        Vector3 p2 = new Vector3(radius, 0, 0);
        Vector3 p3;

        for (int i = 1; i <= step; i++)
        {
            float rad = i * delta * TAMath.d2r;
            float x = (float)Math.Cos(rad);
            float y = (float)Math.Sin(rad);

            p3 = new Vector3(x * radius, 0, y * radius);
            //生成底
            obj.AddTriangle(new Triangle(p1, p2, p3));
            //生成边
            obj.AddTriangle(new Triangle(p0, p2, p3));

            p2 = p3;
        }

        obj.Save(path);
    }

    //程序化生成柱体
    public static void GenCylinder(float step, float high, float radius, string path, float maxAngle = 360f)
    {
        Obj obj = new Obj();

        float delta = maxAngle / step;

        Vector3 p1 = new Vector3(0, 0, 0);
        Vector3 p2 = new Vector3(radius, 0, 0);
        Vector3 p3;

        Vector3 p1_t = new Vector3(0, high, 0);
        Vector3 p2_t = new Vector3(radius, high, 0);
        Vector3 p3_t;

        for (int i = 1; i <= step; i++)
        {
            float rad = i * delta * TAMath.d2r;
            float x = (float)Math.Cos(rad);
            float y = (float)Math.Sin(rad);

            p3 = new Vector3(x * radius, 0, y * radius);
            p3_t = new Vector3(x * radius, high, y * radius);

            //生成下底面
            obj.AddTriangle(new Triangle(p1, p2, p3));
            //生成边
            obj.AddTriangle(new Triangle(p2_t, p2, p3));
            obj.AddTriangle(new Triangle(p2_t, p3_t, p3));
            //生成上顶面
            obj.AddTriangle(new Triangle(p1_t, p2_t, p3_t));

            p2 = p3;
            p2_t = p3_t;
        }

        obj.Save(path);
    }

    #region L_System
    public static void AddQuad(Obj obj, Vector3 start, Vector3 growDir, Vector3 dir)
    {
        Vector3 end = start + growDir;

        Vector3 p1 = start - dir;
        Vector3 p2 = start + dir;
        Vector3 p3 = end + dir;
        Vector3 p4 = end - dir;

        Triangle t1 = new Triangle(p1, p2, p3);
        Triangle t2 = new Triangle(p1, p3, p4);

        obj.AddTriangle(t1);
        obj.AddTriangle(t2);
    }
    private static void Helper(int level, Obj obj, Vector3 start, Vector3 growDir, Vector3 dir, float H_scale, float W_scale, float rotateAngle)
    {
        if (level == 0) return;

        AddQuad(obj, start, growDir, dir);

        //右边生成
        {
            var startR = start + growDir;
            var dirR = dir.RotateXY(rotateAngle) * W_scale;
            var growDirR = growDir.RotateXY(rotateAngle) * H_scale;
            Helper(level - 1, obj, startR, growDirR, dirR, H_scale, W_scale, rotateAngle);
        }
        //左边生成
        {
            var startL = start + growDir;
            var dirL = dir.RotateXY(-rotateAngle) * W_scale;
            var growDirL = growDir.RotateXY(-rotateAngle) * H_scale;
            Helper(level - 1, obj, startL, growDirL, dirL, H_scale, W_scale, rotateAngle);
        }
    }
    private static void HelperRandom(int level, Obj obj, Vector3 start, Vector3 growDir, Vector3 dir, float rotateAngle, Random random)
    {
        if (level == 0) return;

        AddQuad(obj, start, growDir, dir);

        //右边生成
        {
            float H_random = (float)(random.NextDouble() * 0.5f + 0.5f);
            float W_random = (float)(random.NextDouble() * 0.5f + 0.5f);

            var startR = start + growDir;
            var dirR = dir.RotateXY(rotateAngle) * W_random;
            var growDirR = growDir.RotateXY(rotateAngle) * H_random;
            HelperRandom(level - 1, obj, startR, growDirR, dirR, rotateAngle, random);
        }
        //左边生成
        {
            float H_random = (float)(random.NextDouble() * 0.5f + 0.5f);
            float W_random = (float)(random.NextDouble() * 0.5f + 0.5f);

            var startR = start + growDir;
            var dirR = dir.RotateXY(-rotateAngle) * W_random;
            var growDirR = growDir.RotateXY(-rotateAngle) * H_random;
            HelperRandom(level - 1, obj, startR, growDirR, dirR, rotateAngle, random);
        }
    }

    /// <summary>
    /// 无随机_L_System
    /// </summary>
    /// <param name="level">延伸次数</param>
    /// <param name="high">枝干高度</param>
    /// <param name="width">枝干宽度</param>
    /// <param name="H_scale">高度衰减</param>
    /// <param name="W_scale">宽度衰减</param>
    /// <param name="rotateAngle">分叉角度</param>
    public static void L_System(int level = 10, float high = 1, float width = 0.025f, float H_scale = 0.618f, float W_scale = 0.618f, float rotateAngle = 45)
    {
        Obj obj = new Obj();

        //方向
        Vector3 growDir = new Vector3(0, high, 0);
        Vector3 dir = new Vector3(width, 0, 0);
        //起始位置
        Vector3 start = new Vector3(0, 0, 0);
        //递归延申
        Helper(level, obj, start, growDir, dir, H_scale, W_scale, rotateAngle);

        obj.Save("L_System.obj");
    }

    /// <summary>
    /// 有随机_L_System
    /// </summary>
    /// <param name="level">延伸次数</param>
    /// <param name="high">枝干高度</param>
    /// <param name="width">枝干宽度</param>
    /// <param name="rotateAngle">分叉角度</param>
    public static void L_System_Random(int level = 10, float high = 1, float width = 0.025f, float rotateAngle = 45)
    {
        Obj obj = new Obj();

        //方向
        Vector3 growDir = new Vector3(0, high, 0);
        Vector3 dir = new Vector3(width, 0, 0);
        //起始位置
        Vector3 start = new Vector3(0, 0, 0);

        Random random = new Random();
        HelperRandom(level, obj, start, growDir, dir, rotateAngle, random);

        obj.Save("L_System_Random.obj");
    }

    /// <summary>
    /// 卡牌螺旋
    /// </summary>
    /// <param name="high">卡牌高</param>
    /// <param name="width">卡牌宽</param>
    /// <param name="deep">漩涡深度</param>
    /// <param name="shifting">向内深度</param>
    /// <param name="scale">卡牌缩放</param>
    /// <param name="angle">漩涡角度</param>
    public static void GenL_System_Paper(float high, float width, int deep, float shifting = 0.05f, float scale = 0.99f, float angle = 15)
    {
        Obj obj = new Obj();

        //方向
        Vector3 growDir = new Vector3(0, high, 0);
        Vector3 dir = new Vector3(width, 0, 0);
        //起始位置
        Vector3 start = new Vector3(0, 0, 0);

        for (int i = 0; i < deep; i++)
        {
            start.z = i * shifting;
            AddQuad(obj, start, growDir, dir);

            start = start + growDir;
            growDir = growDir * scale;
            dir = dir.RotateXY(angle);
            growDir = growDir.RotateXY(angle);
        }
        obj.Save("L_System_Paper.obj");
    }
    #endregion
}