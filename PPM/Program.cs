using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Obj obj = new Obj();
        obj.Read("MOD/SamRotate.obj");
        PPM texture = new PPM();
        texture.Read("IMG/SamTex.ppm");

        //PPM ppm = new PPM(4096, 4096, Color.black);
        //PPM UV = new PPM(2048, 2048, Color.black);
        //PPM zbuffer = new PPM(1080, 1080, Color.black);
        PPM ppmAA = new PPM(4096, 4096, Color.black);

        //PPM.ChangeUV(UV);
        //PPM.DrawLine(ppm, 0, 0, 511, 50, Color.white);

        //PPM.DrawModelLine(ppm, obj, Color.white);
        //PPM.DrawTriangleLine(ppm, obj, Color.white);
        //PPM.DrawModelColor(ppm, obj);

        //PPM.EasyModel(ppm, obj);
        //PPM.ZBuffer(zbuffer, obj);
        //PPM.CommonModel(ppm, obj);
        //PPM.MSAA(ppmAA, obj);
        PPM.Texture_MSAA(ppmAA, obj, texture);

        //PPM.TextureMap(ppm, obj, texture);
        //PPM.TextureLight(ppm, obj, texture);

        //PPM.ZBuffer_PRE(zbuffer, obj);
        //PPM.CommonModel_PRE(ppm, obj);


        //ppm.Save("IMG/Test.ppm");
        //zbuffer.Save("IMG/Test_Z.ppm");
        ppmAA.Save("IMG/MSAA.ppm");

        //PPM ppmRead = new PPM();
        //ppmRead.Read("IMG/SamTex.ppm");
        //ppmRead.Save("IMG/COPY.ppm");
    }
}