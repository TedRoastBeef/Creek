// Benötigt die Klasse DelayedCall, ebenfalls verfügbar auf beta.unclassified.de.
//
// Anwendung:
//
// Ausblenden und anschließendes Schließen des Fensters:
// (Close ist Methode des Fensters this)
//
//   new Animation(AnimationTypes.FadeOut, this, 0, Close);
//
// Sanftes Ändern der Höhe des Fensters auf 500 Pixel:
// (SomeTaskWhenResized ist die aufzurufende Funktion nach Abschluss der Größenänderung)
//
//   new Animation(AnimationTypes.ResizeVert, this, 500 - Height, SomeTaskWhenResized);
//
// .NET-Kompatibilität: 2.0, 3.5
//
// Hinweise für .NET 1.1: Als EventHandler muss ein 'new delegate...' übergeben werden,
// der Funktionsname allein ist eine Abkürzung, die seit .NET 2.0 möglich ist.

namespace Creek.UI.Effects
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    using Creek.Tools;

    public enum AnimationTypes
    {
        None,
        ResizeHoriz,
        ResizeVert,
        FadeIn,
        FadeOut,
        Callback
    }

    public delegate void AnimationFinishedHandler(object target);

    public delegate void AnimationCallback(object target, int value);

    /// <seealso cref="http://beta.unclassified.de/code/dotnet/animation/"/>
    public class Animation
    {
        private readonly AnimationCallback callback;
        private readonly int duration;
        private readonly int end;
        private readonly AnimationFinishedHandler handler;
        private readonly int interval;
        private readonly int start;
        private readonly object target;
        private readonly AnimationTypes type;
        private bool cancellationPending;
        private int offset;
        private int timePassed;

        public Animation(AnimationTypes type, object target, int offset, AnimationFinishedHandler handler)
            : this(type, target, offset, handler, 0, null, 0)
        {
        }

        public Animation(AnimationTypes type, object target, int offset, AnimationFinishedHandler handler, int duration)
            : this(type, target, offset, handler, 0, null, 0)
        {
        }

        public Animation(AnimationTypes type, object target, int offset, AnimationFinishedHandler handler, int duration,
                         AnimationCallback callback, int startValue)
        {
            this.type = type;
            this.target = target;
            this.offset = offset;
            this.handler = handler;
            this.duration = duration;

            // timings in ms
            this.interval = 10;
            this.timePassed = 0;

            Control c;
            Form f;
            switch (type)
            {
                case AnimationTypes.ResizeHoriz:
                    c = target as Control;
                    if (c == null) return;
                    this.start = c.Width;
                    this.end = this.start + offset;
                    if (this.duration == 0) this.duration = 150;
                    break;
                case AnimationTypes.ResizeVert:
                    c = target as Control;
                    if (c == null) return;
                    this.start = c.Height;
                    this.end = this.start + offset;
                    if (this.duration == 0) this.duration = 150;
                    break;
                case AnimationTypes.FadeIn:
                    f = target as Form;
                    if (f == null) return;
                    this.start = (int) (f.Opacity*100);
                    this.end = this.start + offset;
                    if (this.duration == 0) this.duration = 250;
                    break;
                case AnimationTypes.FadeOut:
                    f = target as Form;
                    if (f == null) return;
                    this.start = (int) (f.Opacity*100);
                    this.end = this.start + offset;
                    if (this.duration == 0) this.duration = 2000;
                    break;
                case AnimationTypes.Callback:
                    if (callback == null) return;
                    this.start = startValue;
                    this.end = this.start + offset;
                    if (this.duration == 0) this.duration = 1000;
                    this.callback = callback;
                    break;
                default:
                    return;
            }

            this.Next();
        }

        public void Cancel()
        {
            this.cancellationPending = true;
        }

        private double MakeCurve()
        {
            double timePercent = this.timePassed/(double) this.duration;

            // we use the sine function from 3pi/2 to 5pi/2
            // scale down linear time percentage from 0...1 to 3pi/2 to 5pi/2
            double curve = Math.Sin(1.5*Math.PI + timePercent*Math.PI);
            // translate sine output from -1...1 to 0...1
            curve = (curve + 1)/2;
            // DEBUG: don't use curve but linear progress instead
            //curve = timePercent;

            return curve;
        }

        public void Next()
        {
            if (this.cancellationPending) return; // and don't come back

            this.timePassed += this.interval;
            if (this.timePassed > this.duration)
            {
                if (this.handler != null) this.handler(this.target);
                return;
            }

            try
            {
                Control c;
                Form f;
                Rectangle wa;
                switch (this.type)
                {
                    case AnimationTypes.ResizeVert:
                        c = this.target as Control;
                        c.Height = this.start + (int) ((this.end - this.start)*this.MakeCurve());
                        wa = Screen.FromControl(c).WorkingArea;
                        if (c is Form && c.Bottom > wa.Bottom)
                        {
                            c.Top -= c.Bottom - wa.Bottom;
                            if (c.Top < wa.Top)
                            {
                                c.Top = wa.Top;
                            }
                        }
                        break;
                    case AnimationTypes.ResizeHoriz:
                        c = this.target as Control;
                        c.Width = this.start + (int) ((this.end - this.start)*this.MakeCurve());
                        wa = Screen.FromControl(c).WorkingArea;
                        if (c is Form && c.Right > wa.Right)
                        {
                            c.Left -= c.Right - wa.Right;
                            if (c.Left < wa.Left)
                            {
                                c.Left = wa.Left;
                            }
                        }
                        break;
                    case AnimationTypes.FadeIn:
                        f = this.target as Form;
                        f.Opacity = (this.start + ((this.end - this.start)*this.MakeCurve()))/100;
                        break;
                    case AnimationTypes.FadeOut:
                        f = this.target as Form;
                        f.Opacity = (this.start + ((this.end - this.start)*this.MakeCurve()))/100;
                        break;
                    case AnimationTypes.Callback:
                        this.callback(this.target, this.start + (int) ((this.end - this.start)*this.MakeCurve()));
                        break;
                }

                DelayedCall.Start(Next, this.interval);
            }
            catch (ObjectDisposedException)
            {
                // Control is gone, stop here
            }
        }
    }
}