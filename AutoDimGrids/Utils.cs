using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AutoDimGrids
{
    internal static class Utils
    {
        internal static void SimpleDialog(string header, string content)
        {
            TaskDialog mainDialog = new TaskDialog("Gas Tools");
            mainDialog.TitleAutoPrefix = false;
            mainDialog.MainInstruction = header;
            mainDialog.MainContent = content;
            mainDialog.Show();
        }
        internal static void SimpleDialog(string header)
        {
            TaskDialog mainDialog = new TaskDialog("Gas Tools");
            mainDialog.TitleAutoPrefix = false;
            mainDialog.MainInstruction = header;
            mainDialog.Show();
        }

        internal static void CatchDialog(Exception ex)
        {
            string head = ex.Source + " - " + ex.GetType().ToString();
            string moreText = ex.Message + "\n\n" + ex.StackTrace + "\n\n" + ex.Data;

            Utils.SimpleDialog(head, moreText);
        }


        internal static ImageSource RetriveImage(string imagePath)
        {
            Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(imagePath);
            string str = imagePath.Substring(imagePath.Length - 3);

            if (str == "jpg")
                return (ImageSource)new JpegBitmapDecoder(manifestResourceStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default).Frames[0];
            else if (str == "bmp")
                return (ImageSource)new BmpBitmapDecoder(manifestResourceStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default).Frames[0];
            else if (str == "png")
                return (ImageSource)new PngBitmapDecoder(manifestResourceStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default).Frames[0];
            else if (str == "ico")
                return (ImageSource)new IconBitmapDecoder(manifestResourceStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default).Frames[0];
            else
                return (ImageSource)null;
        }
    }
}
