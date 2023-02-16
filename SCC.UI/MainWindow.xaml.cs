using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using SCC.Application;
using SCC.Application.ColorSpaces;

namespace SCC
{
    public partial class MainWindow : Window
    {
        private ImageContainer ImageContainer { get; }

        private const string FileFilter =
            "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.bmp) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.bmp";

        public MainWindow()
        {
            ImageContainer = new ImageContainer();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = FileFilter
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            using var bitmap = new Bitmap(openFileDialog.FileName);
            ImageContainer.SourceImage = new Bitmap(openFileDialog.FileName);
            Image1.Source = GetBitMapSource(bitmap);
            AllowToMakeMagic();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = FileFilter
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            using var bitmap = new Bitmap(openFileDialog.FileName);
            ImageContainer.TargetImage = new Bitmap(openFileDialog.FileName);
            Image2.Source = GetBitMapSource(bitmap);
            AllowToMakeMagic();
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            if (LabCb.IsChecked != null && LabCb.IsChecked.Value)
                ImageContainer.ColorSpaces.Add(new LabColorSpace());

            if (HslCb.IsChecked != null && HslCb.IsChecked.Value)
                ImageContainer.ColorSpaces.Add(new HslColorSpace());

            if (!ImageContainer.ColorSpaces.Any())
                return;

            var images = ColorCorrectionService.ColorCorrection(ImageContainer);

            ImageContainer.ColorSpaces = new List<IColorSpace>();
            Image3.Source = GetBitMapSource(images[0]);
            Image4.Source = GetBitMapSource(images[1]);
        }

        private static BitmapSource? GetBitMapSource(Bitmap? bitmap)
        {
            if (bitmap != null)
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bitmap.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

            return null;
        }

        private void AllowToMakeMagic()
        {
            if (Image1.Source != null && Image2.Source != null)
                ButtonMagic.IsEnabled = true;
        }
    }
}