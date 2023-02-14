using System.Drawing;

namespace ColorSpaceConverter;

public static class ColorCorrector
{
    public static void ColorCorrection(Bitmap imageFrom, Bitmap imageTo)
    {
        var imageFromEv = CountEv(imageFrom);
        var imageToEv = CountEv(imageTo);
        
        var imageFromCv = CountCv(imageFrom);
        var imageToCv = CountCv(imageTo);

        for (int x = 0; x < imageTo.Width; x++)
        {
            for (int y = 0; y < imageTo.Height; y++)
            {
                var pixel = imageTo.GetPixel(x, y);

                var rgb = ColorConvertor.RgbFromZeroToOne(new[] { pixel.R, pixel.G, pixel.B });
                var lab = ColorConvertor.LmsToLab(ColorConvertor.RgbToLms(rgb));
                lab[0, 0] = imageFromEv[0] + (lab[0, 0] - imageToEv[0]) * imageFromCv[0] / imageToCv[0];
                lab[1, 0] = imageFromEv[1] + (lab[1, 0] - imageToEv[1]) * imageFromCv[1] / imageToCv[1];
                lab[2, 0] = imageFromEv[2] + (lab[2, 0] - imageToEv[2]) * imageFromCv[2] / imageToCv[2];

                var finalRgb = ColorConvertor.RgbNormalizer(
                    ColorConvertor.LmsToRgb(ColorConvertor.LabToLms(lab))
                );
                
                imageTo.SetPixel(x, y, Color.FromArgb(finalRgb.R, finalRgb.G, finalRgb.B));
            }
        }
    }

    private static double[] CountEv(Bitmap image)
    {
        var totalPixels = image.Width * image.Height;
        var channelValues = new double[3];
        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                var pixel = image.GetPixel(x, y);

                var rgb = ColorConvertor.RgbFromZeroToOne(new[] { pixel.R, pixel.G, pixel.B });
                var lab = ColorConvertor.LmsToLab(ColorConvertor.RgbToLms(rgb));

                channelValues[0] += lab[0, 0];
                channelValues[1] += lab[1, 0];
                channelValues[2] += lab[2, 0];
            }
        }

        var ev = new[]
        {
            channelValues[0] / totalPixels,
            channelValues[1] / totalPixels,
            channelValues[2] / totalPixels
        };

        return ev;
    }

    private static double[] CountCv(Bitmap image)
    {
        var totalPixels = image.Width * image.Height;
        var channelValues = new double[3];
        var ev = CountEv(image);
        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                var pixel = image.GetPixel(x, y);

                var rgb = ColorConvertor.RgbFromZeroToOne(new[] { pixel.R, pixel.G, pixel.B });
                var lab = ColorConvertor.LmsToLab(ColorConvertor.RgbToLms(rgb));

                channelValues[0] += Math.Pow(lab[0, 0] - ev[0], 2);
                channelValues[1] += Math.Pow(lab[1, 0] - ev[1], 2);
                channelValues[2] += Math.Pow(lab[2, 0] - ev[2], 2);
            }
        }

        var res = new[]
        {
            Math.Sqrt(channelValues[0] / totalPixels),
            Math.Sqrt(channelValues[1] / totalPixels),
            Math.Sqrt(channelValues[2] / totalPixels)
        };

        return res;
    }
}