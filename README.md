# SimpleCommandLine
Simple command line formatting/parsing library because I'm stubborn and want to do my own thing.

# Example usage

## Define options object(s)
```
public class ProgramOptions
{
    [Option( "i", "input", "filepath", "Specifies the path to the file to use as input.", Required = true )]
    public string Input { get; set; }

    [Option( "if", "input-format", "auto|pb|mb|f1|obj|dae|fbx", "Specifies the input format of the specified input file." )]
    public InputFormat InputFormat { get; set; }

    [Option( "o", "output", "filepath", "Specifies the path to the file to save the output to.", Required = true )]
    public string Output { get; set; }

    [Option( "of", "output-format", "auto|pb|mb|f1|obj|dae|fbx", "Specifies the conversion output format." )]
    public OutputFormat OutputFormat { get; set; }

    [Group( "ai" )]
    public AssimpOptions Assimp { get; set; }

    [Group( "pb" )]
    public PackedModelOptions PackedModel { get; set; }

    [Option( "ts", "tmx-scale", "decimal scale factor", "Specifies the scaling used for texture conversions.", DefaultValue = 1.0f )]
    public float TmxScale { get; set; }

    [Group( "mb" )]
    public ModelOptions Model { get; set; }

    [Group( "f1" )]
    public FieldOptions Field { get; set; }

    public class AssimpOptions
    {
        [Option( "a", "input-anim", "When specified, the input is treated as an animation file, rather than a model file which affects the conversion process." )]
        public bool TreatInputAsAnimation { get; set; }

        [Option( "pbm", "output-pb-motion", "When specified, motions found within the given packed model file are exported when exporting a PB model obj/dae/fbx." )]
        public bool OutputPbMotion { get; set; }
    }
    public class PackedModelOptions
    {
        [Option( "i", "replace-input", "filepath", "Specifies the base PB file to use for the conversion." )]
        public string ReplaceInput { get; set; }

        [Option( "mi", "replace-motion-index", "0-based index", "Specifies the index of the motion in the PB file to replace." )]
        public int ReplaceMotionIndex { get; set; } = -1;

        [Option( "mpi", "replace-motion-pack-index", "0-based index", "Specifies the index of the motion pack in the PB file to replace.", DefaultValue = 0 )]
        public int ReplaceMotionPackIndex { get; set; }

        [Option( "mmi", "replace-motion-model-index", "0-based index", "Specifies the index of the model in the PB file to use when replacing motions.", DefaultValue = 0 )]
        public int ReplaceMotionModelIndex { get; set; }
    }

    public class ModelOptions
    {
        [Option( "mo", "material-overlays", "When specified, enables the usage of overlay materials." )]
        public bool EnableMaterialOverlays { get; set; }


        [Option( "umt", "unweighted-mesh-type", "1|8", "Specifies the mesh type to be used for unweighted meshes.", DefaultValue = 1 )]
        public MeshType UnweightedMeshType { get; set; }


        [Option( "wmt", "weighted-mesh-type", "2|7", "Specifies the mesh type to be used for weighted meshes.", DefaultValue = 7 )]
        public MeshType WeightedMeshType { get; set; }


        [Option( "wl", "mesh-weight-limit", "integer", "Specifies the max number of weights to be used per mesh. 3 might give better results.", DefaultValue = 4 )]
        public int MeshWeightLimit { get; set; }

        [Option( "vl", "batch-vertex-limit", "integer", "Specifies the max number of vertices to be used per batch.", DefaultValue = 24 )]
        public int BatchVertexLimit { get; set; }
    }

    public class FieldOptions
    {
        [Option( "lbi", "lb-replace-input", "filepath", "Specifies the base field LB file to use for the conversion." )]
        public string LbReplaceInput { get; set; }
    }
}
```

## Format about/usage to console
```
string about = SimpleCommandLineFormatter.Default.FormatAbout<ProgramOptions>( "TGE", "A model converter for DDS3 engine games." );
Console.WriteLine( about );
```

## Parse command line arguments
```
public static ProgramOptions Options { get; private set; }

static void Main( string[] args )
{
    try
    {
        Options = SimpleCommandLineParser.Default.Parse<ProgramOptions>( args );
    }
    catch (Exception e)
    {
        Console.WriteLine( e.Message );
        return;
    }
}
```
