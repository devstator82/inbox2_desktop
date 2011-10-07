using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace Inbox2.Framework.UI.Animation
{
    public class Animator
    {
        #region PennerDoubleAnimation Helpers

        /// <summary>
        /// Starts a PennerDoubleAnimation.
        /// </summary>
        /// <param name="element">The DependencyObject containing the property to be animated.</param>
        /// <param name="prop">The DependencyProperty to animate.</param>
        /// <param name="type">The PennerDoubleAnimation.Equations easing equation to use.</param>
        /// <param name="to">The target value.</param>
        /// <param name="durationMS">Duration of the animation, in milliseconds.</param>
        /// <param name="callbackFunc">Optional method to be called when animation completes.</param>
        /// <returns>The AnimationClock used to control the animation.</returns>
        public static AnimationClock AnimatePenner(
            DependencyObject element,
            DependencyProperty prop,
            PennerDoubleAnimation.Equations type,
            double to,
            int durationMS,
            EventHandler callbackFunc)
        {
            return AnimatePenner(element, prop, type, null, to, durationMS, callbackFunc);
        }

        /// <summary>
        /// Starts a PennerDoubleAnimation.
        /// </summary>
        /// <param name="element">The DependencyObject containing the property to be animated.</param>
        /// <param name="prop">The DependencyProperty to animate.</param>
        /// <param name="type">The PennerDoubleAnimation.Equations easing equation to use.</param>
        /// <param name="from">Optional start value.</param>
        /// <param name="to">The target value.</param>
        /// <param name="durationMS">Duration of the animation, in milliseconds.</param>
        /// <param name="callbackFunc">Optional method to be called when animation completes.</param>
        /// <returns>The AnimationClock used to control the animation.</returns>
        public static AnimationClock AnimatePenner(
            DependencyObject element,
            DependencyProperty prop,
            PennerDoubleAnimation.Equations type,
            double? from,
            double to,
            int durationMS,
            EventHandler callbackFunc)
        {
            double defaultFrom = double.IsNaN((double)element.GetValue(prop)) ?
                                 0 :
                                 (double)element.GetValue(prop);

            PennerDoubleAnimation anim = new PennerDoubleAnimation(type, from.GetValueOrDefault(defaultFrom), to);
            return Animate(element, prop, anim, durationMS, null, null, callbackFunc);
        }

        #endregion

        #region DoubleAnimation Helpers

        /// <summary>
        /// Starts a DoubleAnimation.
        /// </summary>
        /// <param name="element">The DependencyObject containing the property to be animated.</param>
        /// <param name="prop">The DependencyProperty to animate.</param>
        /// <param name="from">Optional start value.</param>
        /// <param name="to">The target value.</param>
        /// <param name="durationMS">Duration of the animation, in milliseconds.</param>
        /// <param name="accel">Optional acceleration value.</param>
        /// <param name="decel">Optional deceleration value.</param>
        /// <param name="callbackFunc">Optional method to be called when animation completes.</param>
        /// <returns>The AnimationClock used to control the animation.</returns>
        public static AnimationClock AnimateDouble(
            DependencyObject element,
            DependencyProperty prop,
            double? from,
            double to,
            int durationMS,
            double? accel,
            double? decel,
            EventHandler callbackFunc)
        {
            double defaultFrom = double.IsNaN((double)element.GetValue(prop)) ?
                                 0 :
                                 (double)element.GetValue(prop);

            DoubleAnimation anim = new DoubleAnimation();
            anim.From = from.GetValueOrDefault(defaultFrom);
            anim.To = to;

            return Animate(element, prop, anim, durationMS, null, null, callbackFunc);
        }

        #endregion

        /// <summary>
        /// Method to configure and start an animation.
        /// </summary>
        private static AnimationClock Animate(
            DependencyObject animatable,
            DependencyProperty prop,
            AnimationTimeline anim,
            int duration,
            double? accel,
            double? decel,
            EventHandler func
            )
        {
            anim.AccelerationRatio = accel.GetValueOrDefault(0);
            anim.DecelerationRatio = decel.GetValueOrDefault(0);
            anim.Duration = TimeSpan.FromMilliseconds(duration);
            anim.Freeze();

            AnimationClock animClock = anim.CreateClock();

            // When animation is complete, remove animation and set the animation's "To" 
            // value as the new value of the property.
            EventHandler eh = null;
            eh = delegate(object sender, EventArgs e)
            {
                animatable.SetValue(prop, animatable.GetValue(prop));

                ((IAnimatable)animatable).ApplyAnimationClock(prop, null);

                animClock.Completed -= eh;
            };

            animClock.Completed += eh;

            // assign completed eventHandler, if defined
            if (func != null)
                animClock.Completed += func;

            animClock.Controller.Begin();

            // goferit
            ((IAnimatable)animatable).ApplyAnimationClock(prop, animClock);

            return animClock;
        }
    }
}
