using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Inbox2.Framework.UI
{
	/// <Remarks>
    ///     As a side effect ClippingBorder will surpress any databinding or animation of
    ///         its childs UIElement.Clip property until the child is removed from ClippingBorder
    /// 	From: http://social.msdn.microsoft.com/forums/en-US/wpf/thread/3364bdd1-0e74-41cb-9cb9-d91f02443ceb/
    /// </Remarks>
    public class ClippingBorder : Border {
		protected Geometry _clipRect;
		protected object _oldClip;

        protected override void OnRender(DrawingContext dc) {
            OnApplyChildClip();           
            base.OnRender(dc);
        }
       
        public override UIElement Child
        {
            get
            {
                return base.Child;
            }
            set
            {
                if (this.Child != value)
                {
                    if(this.Child != null)
                    {
                        // Restore original clipping
                        this.Child.SetValue(UIElement.ClipProperty, _oldClip);
                    }
                   
                    if(value != null)
                    {
                        _oldClip = value.ReadLocalValue(UIElement.ClipProperty);
                    }
                    else
                    {
                        // If we dont set it to null we could leak a Geometry object
                        _oldClip = null;
                    }
                   
                    base.Child = value;
                }
            }
        }
       
        protected virtual void OnApplyChildClip()
        {
            UIElement child = this.Child;

			if (_clipRect == null)
				_clipRect = new RectangleGeometry();

            if(child != null)
            {
            	var geometry = (RectangleGeometry) _clipRect;

				geometry.RadiusX = geometry.RadiusY = Math.Max(0.0, this.CornerRadius.TopLeft - (this.BorderThickness.Left * 0.5));
				geometry.Rect = new Rect(Child.RenderSize);
                child.Clip = _clipRect;
            }
        }      
    }
}