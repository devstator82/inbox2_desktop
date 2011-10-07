using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace Inbox2.Framework.UI
{
    /// <summary>
    /// Animates a grid length value just like the DoubleAnimation animates a double value
    /// </summary>
    public class GridLengthAnimation : AnimationTimeline
    {
        private bool isCompleted;

        /// <summary>
        /// Marks the animation as completed
        /// </summary>
        public bool IsCompleted
        {
            get { return isCompleted; }
            set { isCompleted = value; }
        }

        /// <summary>
        /// Sets the reverse value for the second animation
        /// </summary>
        public double ReverseValue
        {
            get { return (double)GetValue(ReverseValueProperty); }
            set { SetValue(ReverseValueProperty, value); }
        }


        /// <summary>
        /// Dependency property. Sets the reverse value for the second animation
        /// </summary>
        public static readonly DependencyProperty ReverseValueProperty =
            DependencyProperty.Register("ReverseValue", typeof(double), typeof(GridLengthAnimation), new UIPropertyMetadata(0.0));


        /// <summary>
        /// Returns the type of object to animate
        /// </summary>
        public override Type TargetPropertyType
        {
            get
            {
                return typeof(GridLength);
            }
        }

        /// <summary>
        /// Creates an instance of the animation object
        /// </summary>
        /// <returns>Returns the instance of the GridLengthAnimation</returns>
        protected override System.Windows.Freezable CreateInstanceCore()
        {
            return new GridLengthAnimation();
        }

        /// <summary>
        /// Dependency property for the From property
        /// </summary>
        public static readonly DependencyProperty FromProperty = DependencyProperty.Register("From", typeof(GridLength),
                typeof(GridLengthAnimation));

        /// <summary>
        /// CLR Wrapper for the From depenendency property
        /// </summary>
        public GridLength From
        {
            get
            {
                return (GridLength)GetValue(GridLengthAnimation.FromProperty);
            }
            set
            {
                SetValue(GridLengthAnimation.FromProperty, value);
            }
        }

        /// <summary>
        /// Dependency property for the To property
        /// </summary>
        public static readonly DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(GridLength),
                typeof(GridLengthAnimation));

        /// <summary>
        /// CLR Wrapper for the To property
        /// </summary>
        public GridLength To
        {
            get
            {
                return (GridLength)GetValue(GridLengthAnimation.ToProperty);
            }
            set
            {
                SetValue(GridLengthAnimation.ToProperty, value);
            }
        }

        AnimationClock clock;

        /// <summary>
        /// registers to the completed event of the animation clock
        /// </summary>
        /// <param name="clock">the animation clock to notify completion status</param>
        void VerifyAnimationCompletedStatus(AnimationClock clock)
        {
            if (this.clock == null)
            {
                this.clock = clock;
                this.clock.Completed += new EventHandler(delegate(object sender, EventArgs e) { isCompleted = true; });
            }
        }

        /// <summary>
        /// Animates the grid let set
        /// </summary>
        /// <param name="defaultOriginValue">The original value to animate</param>
        /// <param name="defaultDestinationValue">The final value</param>
        /// <param name="animationClock">The animation clock (timer)</param>
        /// <returns>Returns the new grid length to set</returns>
        public override object GetCurrentValue(object defaultOriginValue,
            object defaultDestinationValue, AnimationClock animationClock)
        {
            //check the animation clock event
            VerifyAnimationCompletedStatus(animationClock);

            //check if the animation was completed
            if (isCompleted)
                return (GridLength)defaultDestinationValue;

            //if not then create the value to animate
            double fromVal = this.From.Value;
            double toVal = this.To.Value;

            //check if the value is already collapsed
            if (((GridLength)defaultOriginValue).Value == toVal)
            {
                fromVal = toVal;
                toVal = this.ReverseValue;
            }
            else
                //check to see if this is the last tick of the animation clock.
                if (animationClock.CurrentProgress.Value == 1.0)
                    return To;

            if (fromVal > toVal)
                return new GridLength((1 - animationClock.CurrentProgress.Value) *
                    (fromVal - toVal) + toVal, this.From.IsStar ? GridUnitType.Star : GridUnitType.Pixel);
            else
                return new GridLength(animationClock.CurrentProgress.Value *
                    (toVal - fromVal) + fromVal, this.From.IsStar ? GridUnitType.Star : GridUnitType.Pixel);
        }
    }

    /// <summary>
    /// Animates a double value 
    /// </summary>
    public class ExpanderDoubleAnimation : DoubleAnimationBase
    {
        /// <summary>
        /// Dependency property for the From property
        /// </summary>
        public static readonly DependencyProperty FromProperty = DependencyProperty.Register("From", typeof(double?),
                typeof(ExpanderDoubleAnimation));

        /// <summary>
        /// CLR Wrapper for the From depenendency property
        /// </summary>
        public double? From
        {
            get
            {
                return (double?)GetValue(ExpanderDoubleAnimation.FromProperty);
            }
            set
            {
                SetValue(ExpanderDoubleAnimation.FromProperty, value);
            }
        }

        /// <summary>
        /// Dependency property for the To property
        /// </summary>
        public static readonly DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(double?),
                typeof(ExpanderDoubleAnimation));

        /// <summary>
        /// CLR Wrapper for the To property
        /// </summary>
        public double? To
        {
            get
            {
                return (double?)GetValue(ExpanderDoubleAnimation.ToProperty);
            }
            set
            {
                SetValue(ExpanderDoubleAnimation.ToProperty, value);
            }
        }

        /// <summary>
        /// Sets the reverse value for the second animation
        /// </summary>
        public double? ReverseValue
        {
            get { return (double)GetValue(ReverseValueProperty); }
            set { SetValue(ReverseValueProperty, value); }
        }

        /// <summary>
        /// Sets the reverse value for the second animation
        /// </summary>
        public static readonly DependencyProperty ReverseValueProperty =
            DependencyProperty.Register("ReverseValue", typeof(double?), typeof(ExpanderDoubleAnimation), new UIPropertyMetadata(0.0));


        /// <summary>
        /// Creates an instance of the animation
        /// </summary>
        /// <returns></returns>
        protected override Freezable CreateInstanceCore()
        {
            return new ExpanderDoubleAnimation();
        }

        /// <summary>
        /// Animates the double value
        /// </summary>
        /// <param name="defaultOriginValue">The original value to animate</param>
        /// <param name="defaultDestinationValue">The final value</param>
        /// <param name="animationClock">The animation clock (timer)</param>
        /// <returns>Returns the new double to set</returns>
        protected override double GetCurrentValueCore(double defaultOriginValue, double defaultDestinationValue, AnimationClock animationClock)
        {
            double fromVal = this.From.Value;
            double toVal = this.To.Value;

            if (defaultOriginValue == toVal)
            {
                fromVal = toVal;
                toVal = this.ReverseValue.Value;
            }

            if (fromVal > toVal)
                return (1 - animationClock.CurrentProgress.Value) *
                    (fromVal - toVal) + toVal;
            else
                return (animationClock.CurrentProgress.Value *
                    (toVal - fromVal) + fromVal);
        }
    }
}
