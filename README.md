# CustomSpeckleObjectsExample
Shows how one can take some custom data types (here implemented within Rhino, convert them to "base" speckle objects and then reconvert them back to their original types. 

For example: 
```c#
BeamBase myBeam = new BeamBase();

myBeam.TopLeftCurve = (new Line(0, 0, 0, 0, 0, 2)).ToNurbsCurve();
myBeam.MainVolume = (new Box(Plane.WorldXY, new Interval(0, 1), new Interval(0, 10), new Interval(0, 1))).ToBrep();
//...

// Convert a BeamBase to a Speckle object that can be stored in the database
SpeckleBrep convertedObject = myBeam.ToSpeckle();

// Then cast it back to a BeamBase type :)
BeamBase recastObject = BeamBase.FromSpeckle(convertedObject);
    
```

Where `BeamBase` is a dummy class, defined [here](https://github.com/didimitrie/CustomSpeckleObjectsExample/blob/master/CustomTypes.cs#L16). It converts to a `SpeckleBrep` with a set of custom properties, the most important one being the `custom_type_discriminator`, which essentially can be manually set or gotten using `object.GetType().ToString();`.

Like this you can easily define serialisation logic to-and-from Custom SpeckleObjects. There's some basic automation in `BeamConverter` class for taking a bunch of objects and casting them back and forth without thinking too much, with a graceful fail to basic speckle objects.


