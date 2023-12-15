using Emgu.CV.CvEnum;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Emgu.CV.Structure;
using System.Diagnostics;

namespace AutoFocus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string selectedImagePath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseImageButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image Files | *.jpg; *.png; *.jpeg; *.gif; *.bmp";
            if (openFileDialog.ShowDialog() == true)
            {
                selectedImagePath = openFileDialog.FileName;

                // Display the original image
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedImagePath);
                bitmap.EndInit();
                OriginalImage.Source = bitmap;
            }
        }

        private void ApplyFilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedImagePath))
            {
                
                calculateVariance(selectedImagePath);
            }
            else
            {
                MessageBox.Show("Please select an image first.");
            }
        }

        private void calculateVariance(string imagePath)
        {
            Mat image = CvInvoke.Imread(imagePath, ImreadModes.Grayscale);
            // Load your image using EmguCV

            // Calculate gradients in X and Y directions
            Mat gradX = new Mat();
            Mat gradY = new Mat();
            CvInvoke.Sobel(image, gradX, DepthType.Cv32F, 1, 0,3);
            CvInvoke.Sobel(image, gradY, DepthType.Cv32F, 0, 1,3);

            double normGx = CvInvoke.Norm(gradX);
            double normGy = CvInvoke.Norm(gradY);

            double sumSq = normGx * normGx + normGy * normGy;
            double sharpness = (1.0 / (sumSq / image.Size.Width * image.Size.Height + 1e-6) );

            Debug.WriteLine($"{selectedImagePath} Calculated sharpness {sharpness}");
        }

        // Example method to convert Bitmap to BitmapImage
        private BitmapImage ConvertToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        // Example filter method (replace this with your filter implementation)
        // private Bitmap ApplyFilter(string imagePath)
        // {
        //     // Implement your filtering logic here
        //     // For example, you can use libraries like OpenCV or System.Drawing for image processing
        //     // Return the filtered Bitmap object
        // }
    }
}
