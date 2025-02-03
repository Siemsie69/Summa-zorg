using System;
using System.Windows;
using System.Windows.Media.Animation;

public class GridLengthAnimation : AnimationTimeline
{
    public override Type TargetPropertyType => typeof(GridLength);

    public static readonly DependencyProperty FromProperty = DependencyProperty.Register(
        nameof(From), typeof(GridLength), typeof(GridLengthAnimation));

    public static readonly DependencyProperty ToProperty = DependencyProperty.Register(
        nameof(To), typeof(GridLength), typeof(GridLengthAnimation));

    public static readonly DependencyProperty EasingFunctionProperty = DependencyProperty.Register(
        nameof(EasingFunction), typeof(EasingFunctionBase), typeof(GridLengthAnimation));

    public GridLength From
    {
        get => (GridLength)GetValue(FromProperty);
        set => SetValue(FromProperty, value);
    }

    public GridLength To
    {
        get => (GridLength)GetValue(ToProperty);
        set => SetValue(ToProperty, value);
    }

    public EasingFunctionBase EasingFunction
    {
        get => (EasingFunctionBase)GetValue(EasingFunctionProperty);
        set => SetValue(EasingFunctionProperty, value);
    }

    public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
    {
        if (animationClock.CurrentProgress == null)
            return defaultOriginValue;

        double fromValue = From.Value;
        double toValue = To.Value;

        double progress = animationClock.CurrentProgress.Value;

        // Apply the easing function to the progress
        if (EasingFunction != null)
        {
            progress = EasingFunction.Ease(progress);
        }

        double currentValue = fromValue + (toValue - fromValue) * progress;

        return new GridLength(currentValue, From.IsStar ? GridUnitType.Star : GridUnitType.Pixel);
    }

    protected override Freezable CreateInstanceCore()
    {
        return new GridLengthAnimation();
    }
}
