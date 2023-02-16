using MathNet.Numerics.LinearAlgebra;

namespace ColorSpaceConverter;

public static class ColorConvertor
{
    public static Matrix<double> RgbToLms(Matrix<double> rgb) => ColorSpaceConstants.RgbToLms.Multiply(rgb);
    
    public static Matrix<double> LmsToRgb(Matrix<double> lms) => ColorSpaceConstants.LmsToRgb.Multiply(lms);

    public static Matrix<double> LmsToLab(Matrix<double> lms)
    {
        for (int i = 0; i < 3; i++)
            if (lms[i, 0] == 0)
                lms[i, 0] = lms[i, 0] < 0.01176 ? 0.01176 : lms[i, 0];

        return ColorSpaceConstants.LmsToLab.Multiply(Matrix<double>.Log10(lms));
    }

    public static Matrix<double> LabToLms(Matrix<double> lab) =>
        ColorSpaceConstants.LabToLms.Multiply(lab).Map(x => Math.Pow(10, x));

    public static Matrix<double> RgbToHsl(Matrix<double> fullRgb)
    {
        double h, s;

        var rgb = fullRgb.AsColumnMajorArray();


        var cMin = rgb.Min();
        var cMax = rgb.Max();
        var d = cMax - cMin;

        var l = (cMax + cMin) / 2.0;

        const double tolerance = 1e-10;
        if (Math.Abs(d) < tolerance)
        {
            s = 0;
            h = 0;
        }
        else
        {
            s = d / (1 - Math.Abs(2 * l - 1));
            if (Math.Abs(cMax - rgb[0]) < tolerance)
                h = 60 * ((rgb[1] - rgb[2]) / d % 6);

            else if (Math.Abs(cMax - rgb[1]) < tolerance)
                h = 60 * (2 + (rgb[2] - rgb[0]) / d);

            else
                h = 60 * (4 + (rgb[0] - rgb[1]) / d);

            if (h < 0)
                h += 360;
        }

        return Matrix<double>.Build.DenseOfArray(new[,]
        {
            { h },
            { s },
            { l }
        });
    }

    public static Matrix<double> HslToRgb(Matrix<double> hsl)
    {
        var hslArr = hsl.AsColumnMajorArray();
        hslArr[0] /= 360;
        double r, g, b;
        if (hslArr[1] == 0.000000)
        {
            r = g = b = hslArr[2];
        }
        else
        {
            var q = hslArr[2] < 0.5
                ? hslArr[2] * (1 + hslArr[1])
                : hslArr[2] + hslArr[1] - hslArr[2] * hslArr[1];

            var p = 2 * hslArr[2] - q;

            r = HueToRgb(p, q, hslArr[0] + 1.0 / 3);
            g = HueToRgb(p, q, hslArr[0]);
            b = HueToRgb(p, q, hslArr[0] - 1.0 / 3);
        }

        return Matrix<double>.Build.DenseOfArray(new[,]
        {
            { r },
            { g },
            { b }
        });
    }

    private static double HueToRgb(double p, double q, double t)
    {
        if (t < 0) t += 1;
        if (t > 1) t -= 1;
        return t switch
        {
            < 1.0 / 6 => p + (q - p) * 6 * t,
            < 1.0 / 2 => q,
            < 2.0 / 3 => p + (q - p) * (2.0 / 3 - t) * 6,
            _ => p
        };
    }

    public static Matrix<double> RgbFromZeroToOne(byte[] rgb) => Matrix<double>.Build.DenseOfArray(new[,]
    {
        { rgb[0] / 255.0 },
        { rgb[1] / 255.0 },
        { rgb[2] / 255.0 },
    });

    public static (byte R, byte G, byte B) RgbNormalizer(Matrix<double> rgb)
    {
        var arrRgb = rgb.ToArray();
        for (int i = 0; i < arrRgb.GetLength(0); i++)
        {
            if (arrRgb[i, 0] < 0.01176)
                arrRgb[i, 0] = 0.01176;

            if (arrRgb[i, 0] > 0.92157)
                arrRgb[i, 0] = 0.92157;

            arrRgb[i, 0] *= 255;
        }

        return (
            (byte)arrRgb[0, 0],
            (byte)arrRgb[1, 0],
            (byte)arrRgb[2, 0]
        );
    }
}