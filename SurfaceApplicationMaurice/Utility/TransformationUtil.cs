using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Artefact.Animation;
using System.Windows.Controls;
using System.Windows;

namespace SurfaceApplicationMaurice.Utility
{
    static public class TransformationUtil
    {
        static public void AppearControl(Control c)
        {
            if (c.Visibility == System.Windows.Visibility.Visible) return;
            c.Opacity = 0;
            c.Visibility = System.Windows.Visibility.Visible;
            var eog = new EaseObjectGroup();
            //eog.Complete += g => Debug.WriteLine("COMPLETE");
            var eo = ArtefactAnimator.AddEase(c, new[] { AnimationTypes.Alpha }, new object[] { 1 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);             
        }
        static public void DisAppearControl(Control c)
        {
            if (c.Visibility == System.Windows.Visibility.Hidden) return;
            var eog = new EaseObjectGroup();
            //eog.Complete += g => Debug.WriteLine("COMPLETE");
            var eo = ArtefactAnimator.AddEase(c, new[] { AnimationTypes.Alpha }, new object[] { 0 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
            eog.Complete += delegate(EaseObjectGroup easeObjectGroup) { c.Visibility = System.Windows.Visibility.Hidden; };
        }

        static public void AppearControl(StackPanel c)
        {
            if (c.Visibility == System.Windows.Visibility.Visible) return;
            c.Opacity = 0;
            c.Visibility = System.Windows.Visibility.Visible;
            var eog = new EaseObjectGroup();
            //eog.Complete += g => Debug.WriteLine("COMPLETE");
            var eo = ArtefactAnimator.AddEase(c, new[] { AnimationTypes.Alpha }, new object[] { 1 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
        }
        static public void DisAppearControl(StackPanel c)
        {
            if (c.Visibility == System.Windows.Visibility.Hidden) return;
            var eog = new EaseObjectGroup();
            //eog.Complete += g => Debug.WriteLine("COMPLETE");
            var eo = ArtefactAnimator.AddEase(c, new[] { AnimationTypes.Alpha }, new object[] { 0 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
            eog.Complete += delegate(EaseObjectGroup easeObjectGroup) { c.Visibility = System.Windows.Visibility.Hidden; };
        }

        static public void AppearControl(TextBlock c)
        {
            if (c.Visibility == System.Windows.Visibility.Visible) return;
            c.Opacity = 0;
            c.Visibility = System.Windows.Visibility.Visible;
            var eog = new EaseObjectGroup();
            //eog.Complete += g => Debug.WriteLine("COMPLETE");
            var eo = ArtefactAnimator.AddEase(c, new[] { AnimationTypes.Alpha }, new object[] { 1 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
        }

        static public void ShowAndDisappearText(TextBlock textBlockMessage)
        {            
            textBlockMessage.Opacity = 0;
            textBlockMessage.Visibility = Visibility.Visible;
            var eog = new EaseObjectGroup();
            //eog.Complete += g => Debug.WriteLine("COMPLETE");
            var eo = ArtefactAnimator.AddEase(textBlockMessage, new[] { AnimationTypes.Alpha }, new object[] { 1 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
            eog.Complete += delegate(EaseObjectGroup easeObjectGroup)
            {
                var eog2 = new EaseObjectGroup();
                //eog.Complete += g => Debug.WriteLine("COMPLETE");
                var eo2 = ArtefactAnimator.AddEase(textBlockMessage, new[] { AnimationTypes.Alpha }, new object[] { 0 }, 1.5, AnimationTransitions.CubicEaseOut, 1);
                eog2.AddEaseObject(eo2);
                eog2.Complete += delegate(EaseObjectGroup easeObjectGroup2) { textBlockMessage.Visibility = System.Windows.Visibility.Hidden; };
            };
        }

        static public void DisAppearControl(TextBlock c)
        {
            if (c.Visibility == System.Windows.Visibility.Hidden) return;
            var eog = new EaseObjectGroup();
            //eog.Complete += g => Debug.WriteLine("COMPLETE");
            var eo = ArtefactAnimator.AddEase(c, new[] { AnimationTypes.Alpha }, new object[] { 0 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
            eog.Complete += delegate(EaseObjectGroup easeObjectGroup) { c.Visibility = System.Windows.Visibility.Hidden; };
        }
        
        static public void AppearControl(System.Windows.Controls.Image c)
        {
            if (c.Visibility == System.Windows.Visibility.Visible) return;
            c.Opacity = 0;
            c.Visibility = System.Windows.Visibility.Visible;
            var eog = new EaseObjectGroup();
            //eog.Complete += g => Debug.WriteLine("COMPLETE");
            var eo = ArtefactAnimator.AddEase(c, new[] { AnimationTypes.Alpha }, new object[] { 1 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
        }
        static public void DisAppearControl(System.Windows.Controls.Image c)
        {
            if (c.Visibility == System.Windows.Visibility.Hidden) return;
            var eog = new EaseObjectGroup();
            //eog.Complete += g => Debug.WriteLine("COMPLETE");
            var eo = ArtefactAnimator.AddEase(c, new[] { AnimationTypes.Alpha }, new object[] { 0 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
            eog.Complete += delegate(EaseObjectGroup easeObjectGroup) { c.Visibility = System.Windows.Visibility.Hidden; };
        }

        static public void AppearGrid(Grid c)
        {
            if (c.Visibility == System.Windows.Visibility.Visible) return;
            c.Opacity = 0;
            c.Visibility = System.Windows.Visibility.Visible;
            var eog = new EaseObjectGroup();
            //eog.Complete += g => Debug.WriteLine("COMPLETE");
            var eo = ArtefactAnimator.AddEase(c, new[] { AnimationTypes.Alpha }, new object[] { 1 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
        }
        static public void DisAppearGrid(Grid c)
        {
            if (c.Visibility == System.Windows.Visibility.Hidden) return;
            var eog = new EaseObjectGroup();
            //eog.Complete += g => Debug.WriteLine("COMPLETE");
            var eo = ArtefactAnimator.AddEase(c, new[] { AnimationTypes.Alpha }, new object[] { 0 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);
            eog.Complete += delegate(EaseObjectGroup easeObjectGroup) { c.Visibility = System.Windows.Visibility.Hidden; };
        }
         
        
        


        static public void ShowDialog(Grid grid,int delta)
        {
            var eog = new EaseObjectGroup();
            var eo = ArtefactAnimator.AddEase(grid, new[] { AnimationTypes.MarginTop }, new object[] { System.Windows.SystemParameters.PrimaryScreenHeight/2- delta}, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);          
        }
        static public void HideDialog(Grid grid)
        {
            var eog = new EaseObjectGroup();
            var eo = ArtefactAnimator.AddEase(grid, new[] { AnimationTypes.MarginTop }, new object[] { System.Windows.SystemParameters.PrimaryScreenHeight + 100 }, 1.5, AnimationTransitions.CubicEaseOut, 0);
            eog.AddEaseObject(eo);          
        }


    }

    
}
