using System.Drawing;
using SCC.Application.ColorSpaces;

namespace SCC.Application;

public class ImageContainer
{
    public Bitmap? SourceImage { get; set; }

    public Bitmap? TargetImage { get; set; }

    public List<IColorSpace> ColorSpaces { get; set; } = new();
}