using System;
using System.Collections.Generic;
using System.Drawing;
using ColorSpaceConverter;
using SCC.Application;
using SCC.Application.ColorSpaces;

namespace SSC;

public static class ColorCorrectionService
{
    public static Bitmap?[] ColorCorrection(ImageContainer container)
    {
        var images = new Bitmap[2];
        
        if (container.SourceImage == null || container.TargetImage == null)
            return images;

        for (var i = 0; i < container.ColorSpaces.Count; i++)
        {
            var space = container.ColorSpaces[i];
            var newImage = (Bitmap)container.TargetImage.Clone();

            var imageFromEv = CountEv(container.SourceImage, space);
            var imageToEv = CountEv(newImage, space);

            var imageFromCv = CountCv(container.SourceImage, space, imageFromEv);
            var imageToCv = CountCv(newImage, space, imageToEv);

            for (int x = 0; x < newImage.Width; x++)
            {
                for (int y = 0; y < newImage.Height; y++)
                {
                    var pixel = newImage.GetPixel(x, y);
                    var rgb = ColorConvertor.RgbFromZeroToOne(new[] { pixel.R, pixel.G, pixel.B });

                    var colorSpace = space.FromRgb(rgb);
                    colorSpace[0, 0] =
                        imageFromEv[0] + (colorSpace[0, 0] - imageToEv[0]) * imageFromCv[0] / imageToCv[0];
                    colorSpace[1, 0] =
                        imageFromEv[1] + (colorSpace[1, 0] - imageToEv[1]) * imageFromCv[1] / imageToCv[1];
                    colorSpace[2, 0] =
                        imageFromEv[2] + (colorSpace[2, 0] - imageToEv[2]) * imageFromCv[2] / imageToCv[2];

                    var finalRgb = ColorConvertor.RgbNormalizer(
                        space.ToRgb(colorSpace)
                    );

                    newImage.SetPixel(x, y, Color.FromArgb(finalRgb.R, finalRgb.G, finalRgb.B));
                }
            }

            images[i] = newImage;
        }

        return images;
    }

    private static double[] CountEv(Bitmap image, IColorSpace colorSpace)
    {
        var totalPixels = image.Width * image.Height;
        var channelValues = new double[3];
        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                var pixel = image.GetPixel(x, y);

                var rgb = ColorConvertor.RgbFromZeroToOne(new[] { pixel.R, pixel.G, pixel.B });
                var space = colorSpace.FromRgb(rgb);

                channelValues[0] += space[0, 0];
                channelValues[1] += space[1, 0];
                channelValues[2] += space[2, 0];
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

    private static double[] CountCv(Bitmap image, IColorSpace colorSpace, IReadOnlyList<double> ev)
    {
        var totalPixels = image.Width * image.Height;
        var channelValues = new double[3];
        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                var pixel = image.GetPixel(x, y);

                var rgb = ColorConvertor.RgbFromZeroToOne(new[] { pixel.R, pixel.G, pixel.B });
                var space = colorSpace.FromRgb(rgb);

                channelValues[0] += Math.Pow(space[0, 0] - ev[0], 2);
                channelValues[1] += Math.Pow(space[1, 0] - ev[1], 2);
                channelValues[2] += Math.Pow(space[2, 0] - ev[2], 2);
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