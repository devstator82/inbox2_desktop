using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using Microsoft.Expression.Interactivity;

namespace Inbox2.Framework.UI
{
    /// <summary>
    /// Custom behavior to add a glass like effect to the background of a Visual
    /// </summary>
    public class GlassBehavior : Behavior<FrameworkElement>
    {
        /// <summary>
        /// This is the object the behavior is currently attached to.
        /// This will be null if no object is attached.
        /// </summary>
        private FrameworkElement m_attachedObject;

        /// <summary>
        /// The VisualBrush that is used directly on the background/fill of 
        /// the attached object.
        /// </summary>
        private readonly VisualBrush m_directVisualBrush = new VisualBrush();

        /// <summary>
        /// This is the visual that is used to apply the effect on and has the
        /// VisualBrush set as the background.
        /// </summary>
        private readonly Rectangle m_surrogateVisual = new Rectangle();

        /// <summary>
        /// The VisualBrush that is used as the background of hte surrogate visual
        /// </summary>
        private readonly VisualBrush m_surrogateVisualBrush = new VisualBrush();

        /// <summary>
        /// Creates a new instance of the GlassBehavior
        /// </summary>
        public GlassBehavior()
        {
            /* Lets setup some possible optimizations */
            RenderOptions.SetEdgeMode(m_directVisualBrush, EdgeMode.Aliased);
            RenderOptions.SetCachingHint(m_directVisualBrush, CachingHint.Cache);

            RenderOptions.SetEdgeMode(m_surrogateVisualBrush, EdgeMode.Aliased);
            RenderOptions.SetCachingHint(m_surrogateVisualBrush, CachingHint.Cache);

            /* This makes sure our brush is not stretched.  This would make the glass
             * not look correct relative to the visual it is displaying */
            m_directVisualBrush.Stretch = Stretch.None;

            /* The ViewboxUnits are absolute because our transformation values
             * will be in absolute, so this makes things easier */
            m_surrogateVisualBrush.ViewboxUnits = BrushMappingMode.Absolute;
            m_surrogateVisualBrush.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
            m_surrogateVisualBrush.Viewport = new Rect(0, 0, 1, 1);
        }

        #region Visual

        public static readonly DependencyProperty VisualProperty =
            DependencyProperty.Register("Visual",
                                        typeof(Visual),
                                        typeof(GlassBehavior),
                                        new FrameworkPropertyMetadata(null,
                                                                      new PropertyChangedCallback(OnVisualChanged)));

        /// <summary>
        /// The target Visual to use for the glass effect
        /// </summary>
        public Visual Visual
        {
            get { return (Visual)GetValue(VisualProperty); }
            set { SetValue(VisualProperty, value); }
        }

        private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GlassBehavior)d).OnVisualChanged(e);
        }

        protected virtual void OnVisualChanged(DependencyPropertyChangedEventArgs e)
        {
            SetupVisual();
        }

        #endregion

        #region Effect

        public static readonly DependencyProperty EffectProperty =
            DependencyProperty.Register("Effect", typeof(Effect), typeof(GlassBehavior),
                                        new FrameworkPropertyMetadata(null,
                                                                      new PropertyChangedCallback(OnEffectChanged)));

        /// <summary>
        /// The pixel shader Effect to apply to the glass
        /// </summary>
        public Effect Effect
        {
            get { return (Effect)GetValue(EffectProperty); }
            set { SetValue(EffectProperty, value); }
        }

        private static void OnEffectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GlassBehavior)d).OnEffectChanged(e);
        }

        protected virtual void OnEffectChanged(DependencyPropertyChangedEventArgs e)
        {
            SetEffect();
        }

        #endregion

        /// <summary>
        /// Sets the pixel shader Effect to the value of the Effect dependancy property.
        /// </summary>
        private void SetEffect()
        {
            if (m_surrogateVisual == null)
                return;

            m_surrogateVisual.Effect = Effect;
        }

        /// <summary>
        /// Initializes the Visual for use in our glass effect
        /// </summary>
        private void SetupVisual()
        {
            var element = Visual as FrameworkElement;

            if (element == null || m_attachedObject == null)
                return;

            /* Set our pixel shader, if any */
            SetEffect();

            m_surrogateVisualBrush.Visual = element;
            m_surrogateVisual.Fill = m_surrogateVisualBrush;

            /* Set the direct visual brush to the surrogate visual */
            m_directVisualBrush.Visual = m_surrogateVisual;

            EnsureBrushSyncWithVisual();
        }

        /// <summary>
        /// Keeps the VisualBrush in visual sync with the Visual
        /// </summary>
        private void EnsureBrushSyncWithVisual()
        {
            if (m_attachedObject == null || Visual == null)
                return;

            /* Make the surrogate visual the same size of
             * our attached FrameworkElement */
            m_surrogateVisual.Width = m_attachedObject.ActualWidth;
            m_surrogateVisual.Height = m_attachedObject.ActualHeight;

            /* Get the transform of our attached FrameworkElement to the
             * Visual we want to use as our glass effect */
            GeneralTransform trans = m_attachedObject.TransformToVisual(Visual);

            /* Calculate the difference between 0,0 coord of our attached FrameworkElement
             * and 0,0 coord of our target Visual for the glass effect */
            Point pos = trans.Transform(new Point(0, 0));
            
            /* Create a new Viewbox for the VisualBrush.  This shows a specific
             * area of the Visual of the VisualBrush. */
            var viewbox = new Rect
                              {
                                  X = pos.X,
                                  Y = pos.Y,
                                  Width = m_attachedObject.ActualWidth,
                                  Height = m_attachedObject.ActualHeight
                              };

            m_surrogateVisualBrush.Viewbox = viewbox;
        }

        /// <summary>
        /// Called when the behavior is attached to a DependencyObject
        /// </summary>
        protected override void OnAttached()
        {
            if (m_attachedObject != null)
            {
                /* Unhook our old event and avoid any hidden refs */
                m_attachedObject.LayoutUpdated -= AssociatedObject_LayoutUpdated;
            }

            m_attachedObject = AssociatedObject;

            /* Search for a property we can set our VisualBrush to.  Right
             * now we search for a BackGround or Fill property.  */
            PropertyInfo info = FindFillProperty(m_attachedObject);

            if (info != null)
            {
                info.SetValue(m_attachedObject, m_directVisualBrush, null);
            }

            /* Hook into the LayoutUpdated so we can keep everything in sync
             * when the layout changes */
            m_attachedObject.LayoutUpdated += AssociatedObject_LayoutUpdated;

            /* Make sure our Visual is setup */
            SetupVisual();

            base.OnAttached();
        }

        /// <summary>
        /// Called when the behavior is removed from the DependencyObject
        /// </summary>
        protected override void OnDetaching()
        {
            if (m_attachedObject != null)
            {
                /* Remove our handler to avoid any leaks */
                m_attachedObject.LayoutUpdated -= AssociatedObject_LayoutUpdated;
            }

            base.OnDetaching();
        }

        /// <summary>
        /// Searches for a property on DependencyObject to set a Brush to
        /// </summary>
        /// <param name="obj">The DependencyObject to search</param>
        /// <returns></returns>
        private static PropertyInfo FindFillProperty(DependencyObject obj)
        {
            Type t = obj.GetType();

            PropertyInfo info = t.GetProperty("Background") ?? t.GetProperty("Fill");

            return info;
        }

        private void AssociatedObject_LayoutUpdated(object sender, EventArgs e)
        {
            EnsureBrushSyncWithVisual();
        }
    }
}


