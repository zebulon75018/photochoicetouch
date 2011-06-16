using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;

namespace SurfaceApplicationMaurice.Utility
{
    public class gradientManager
    {        
        public gradientManager()
        {            
        }

        public void setgradient(Panel elm)
        {
            LinearGradientBrush myBrush = new LinearGradientBrush();

            Point startpoint = new Point();
            int angle = CommunConfig.getInstance().Angle;
            startpoint.X = System.Math.Cos(((double)angle / 180) * System.Math.PI)/2 + 0.5;
            startpoint.Y = System.Math.Sin(((double)angle / 180) * System.Math.PI)/2 + 0.5;

            angle -= 180;
            Point endpoint = new Point();
            endpoint.X = System.Math.Cos(((double)angle / 180) * System.Math.PI)/2 +0.5;
            endpoint.Y = System.Math.Sin(((double)angle / 180) * System.Math.PI)/2 +0.5;
 
            myBrush.StartPoint = startpoint;
            myBrush.EndPoint = endpoint;
            System.Windows.Media.Color c = new Color();
            System.Drawing.Color  drawingColor  = CommunConfig.getInstance().StartColor;            
            c = System.Windows.Media.Color.FromRgb(drawingColor.R, drawingColor.G, drawingColor.B);
            myBrush.GradientStops.Add(new GradientStop(c, 1));

            drawingColor = CommunConfig.getInstance().EndColor;
            c = System.Windows.Media.Color.FromRgb(drawingColor.R, drawingColor.G, drawingColor.B);

            myBrush.GradientStops.Add(new GradientStop(c, 0));                                   
            elm.Background = myBrush;
        }     
    }
}
