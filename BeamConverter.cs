using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeckleCore;
using SpeckleRhinoConverter;
using Rhino.Geometry;

namespace CustomObjectsExample
{

    public static class GlobalVar
    {
        public const string discriminator = "d2p_type";
    }

    class BeamConverter : SpeckleCore.Converter
    {
        public override IEnumerable<object> ToNative(IEnumerable<SpeckleObject> _objects)
        {
            return _objects.Select(o => ToNative(o));
        }

        public override IEnumerable<SpeckleObject> ToSpeckle(IEnumerable<object> _objects)
        {
            return _objects.Select(o => ToSpeckle(o));
        }

        public override object ToNative(SpeckleObject _object)
        {
            if ((string)_object.Properties[GlobalVar.discriminator] == "BeamBase")
                return BeamBase.FromSpeckle(((SpeckleBrep)_object));
            if ((string)_object.Properties[GlobalVar.discriminator] == "DetailedBeam")
                return DetailedBeam.FromSpeckle(((SpeckleBrep)_object));
            if ((string)_object.Properties[GlobalVar.discriminator] == "BeamJoint")
                return BeamJoint.FromSpeckle(((SpecklePoint)_object));
            
            // Graceful fail
            using (RhinoConverter rhConv = new RhinoConverter())
                return rhConv.ToNative(_object);
        }

        public override SpeckleObject ToSpeckle(object _object)
        {
            if (_object is BeamBase)
                return ((BeamBase)_object).ToSpeckle();
            if (_object is DetailedBeam)
                return ((DetailedBeam)_object).ToSpeckle();
            if (_object is BeamJoint)
                return ((BeamJoint)_object).ToSpeckle();

            // Graceful fail
            using (RhinoConverter rhConv = new RhinoConverter())
                return rhConv.ToSpeckle(_object);
        }
    }
}
