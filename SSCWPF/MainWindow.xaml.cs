using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Drawing;
using ColorSpaceConverter;

namespace SSCWPF
{
    public partial class MainWindow : Window
    {
        private Bitmap _firstImage;
        private Bitmap _secondImage;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            using var bitmap = new Bitmap(openFileDialog.FileName);
            _firstImage = new Bitmap(openFileDialog.FileName);
            var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            Image1.Source = bitmapSource;
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            using var bitmap = new Bitmap(openFileDialog.FileName);
            _secondImage = new Bitmap(openFileDialog.FileName);
            var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            Image2.Source = bitmapSource;
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            ColorConvertor.RgbToLab(_firstImage);
            var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                _firstImage.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            Image3.Source = bitmapSource;
        }
    }
}