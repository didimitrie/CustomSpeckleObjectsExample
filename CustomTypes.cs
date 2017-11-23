using Rhino.Geometry;
using SpeckleCore;
using SpeckleRhinoConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomObjectsExample
{
    /// <summary>
    /// Fake base class
    /// </summary>
    [Serializable]
    public class BeamBase
    {
        public Brep MainVolume;
        public Brep DetailedVolume;

        public Curve[] CurveList; // max 4

        public Curve TopLeftCurve
        {
            get
            {
                return CurveList[0];
            }
            set
            {
                CurveList[0] = value;
            }
        }

        public Curve TopRightCurve
        {
            get
            {
                return CurveList[1];
            }
            set
            {
                CurveList[1] = value;
            }
        }

        public Curve BottomRightCurve
        {
            get
            {
                return CurveList[2];
            }
            set
            {
                CurveList[2] = value;
            }
        }

        public Curve BottomLeftCurve
        {
            get
            {
                return CurveList[3];
            }
            set
            {
                CurveList[3] = value;
            }
        }

        public BeamBase()
        {
            CurveList = new Curve[4];
        }

        public void SayHi()
        {
            System.Diagnostics.Debug.WriteLine("Hello.");
        }

        public SpeckleBrep ToSpeckle()
        {
            SpeckleBrep speckleBrep = MainVolume.ToSpeckle();

            speckleBrep.Properties[GlobalVar.discriminator] = "BeamBase";

            speckleBrep.Properties["TopLeft"] = TopLeftCurve.ToSpeckle();
            speckleBrep.Properties["TopRight"] = TopRightCurve.ToSpeckle();
            speckleBrep.Properties["BottomRight"] = BottomRightCurve.ToSpeckle();
            speckleBrep.Properties["BottomLeft"] = BottomLeftCurve.ToSpeckle();

            speckleBrep.Properties["DetailedVolume"] = DetailedVolume.ToSpeckle();

            return speckleBrep;
        }

        public static BeamBase FromSpeckle(SpeckleBrep BaseObj)
        {
            if ((string)BaseObj.Properties[GlobalVar.discriminator] != "BeamBase")
                throw new Exception("Can't convert to BeamBase, missing discriminator.");

            using (RhinoConverter rhConv = new RhinoConverter())
            {
                var myBeamBase = new BeamBase();

                myBeamBase.MainVolume = (Brep)rhConv.ToNative(BaseObj);

                myBeamBase.DetailedVolume = (Brep)rhConv.ToNative((SpeckleObject)BaseObj.Properties["DetailedVolume"]);

                myBeamBase.TopLeftCurve = (Curve)rhConv.ToNative((SpeckleObject)BaseObj.Properties["TopLeft"]);

                myBeamBase.TopRightCurve = (Curve)rhConv.ToNative((SpeckleObject)BaseObj.Properties["TopRight"]);

                myBeamBase.BottomRightCurve = (Curve)rhConv.ToNative((SpeckleObject)BaseObj.Properties["BottomRight"]);

                myBeamBase.BottomLeftCurve = (Curve)rhConv.ToNative((SpeckleObject)BaseObj.Properties["BottomLeft"]);

                return myBeamBase;
            }
        }
    }

    /// <summary>
    /// Fake inheritance of base class with extra random props
    /// </summary>
    [Serializable]
    public class DetailedBeam : BeamBase
    {
        // add some random properties
        public Curve OperationX, OperationY, OperationZ;
        public double Tolerance = 0.333;
        public double DensityMat = 12;

        public DetailedBeam() : base()
        {
            // Do something
        }

        public void sayBye()
        {
            System.Diagnostics.Debug.WriteLine("Bye bye!");
        }

        public SpeckleBrep ToSpeckle()
        {
            SpeckleBrep speckleBrep = ((BeamBase)this).ToSpeckle();

            speckleBrep.Properties[GlobalVar.discriminator] = "DetailedBeam";
            speckleBrep.Properties["tolerance"] = Tolerance;
            speckleBrep.Properties["densityMat"] = DensityMat;

            return speckleBrep;
        }

        public static DetailedBeam FromSpeckle(SpeckleBrep BaseObj)
        {
            return new DetailedBeam();
        }
    }

    [Serializable]
    public class BeamJoint
    {
        public BeamBase A, B;
        public Point3d IntersectionPoint;

        public SpecklePoint ToSpeckle()
        {
            SpecklePoint basePt = IntersectionPoint.ToSpeckle();
            basePt.Properties[GlobalVar.discriminator] = "BeamJoint";
            basePt.Properties["BeamA"] = A.ToSpeckle();
            basePt.Properties["BeamB"] = B.ToSpeckle();

            return basePt;
        }

        public static BeamJoint FromSpeckle(SpecklePoint BasePt)
        {
            var bj = new BeamJoint();
            bj.IntersectionPoint = BasePt.ToRhino();
            bj.A = BeamBase.FromSpeckle((SpeckleBrep)BasePt.Properties["BeamA"]);
            bj.B = BeamBase.FromSpeckle((SpeckleBrep)BasePt.Properties["BeamB"]);
            return bj;
        }

    }
}
