using System;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RustItemImageExporter
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string path = string.Empty;
            while (!Directory.Exists(path))
            {
                FolderBrowserDialog browsewrDialog = new FolderBrowserDialog();
                browsewrDialog.ShowDialog();
                path = browsewrDialog.SelectedPath;
            }

            string exportPath = Path.Combine(Environment.CurrentDirectory, "exports");
            if (!Directory.Exists(exportPath))
            {
                Directory.CreateDirectory(exportPath);
            }

            DirectoryInfo dirInfo = new DirectoryInfo(path);
            Image image, resizedImage;
            string destinationPath;

            foreach (FileInfo file in dirInfo.GetFiles().Where(x => x.Name.Contains(".png")))
            {
                // Get original image
                image = Image.FromFile(file.FullName);
                // Resize image and reassign local variable
                resizedImage = ResizeImage(image, new Size(64, 64));
                image.Dispose();

                destinationPath = Path.Combine(exportPath, file.Name);

                // Safe delete
                try
                {
                    File.Delete(destinationPath);
                } catch { }

                // Save the resized image to the destination
                resizedImage.Save(destinationPath);
                resizedImage.Dispose();

                Console.WriteLine($"{file.Name} resized to 64x64");
            }
           
            Console.WriteLine("Completed.");
            Console.ReadLine();
        }

        public static Image ResizeImage(Image currentImg, Size size)
        {
            return new Bitmap(currentImg, size);
        }
    }
}
