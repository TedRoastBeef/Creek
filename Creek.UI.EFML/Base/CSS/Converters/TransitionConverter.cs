using Creek.UI.Effects;

namespace Creek.UI.EFML.Base.CSS.Converters
{
    // property duration type other
    public class TransitionConverter : IConverter<Transition>
    {
        #region Overrides of IConverter<Transition>

        public Transition Convert(string s, object target)
        {
            var spl = s.Split(' ');
            var prop = spl[0];
            var dur = new TimeConverter().Convert(spl[1]);
            var typ = spl[2];
            var des = spl[3];

            ITransitionType t = null;

            switch (typ)
            {
                case "linear":
                    t = new TransitionType_Linear(dur);
                    break;
                case "ease-inout":
                    t = new TransitionType_EaseInEaseOut(dur);
                    break;
                case "bounce":
                    t = new TransitionType_Bounce(dur);
                    break;
                case "throwcatch":
                    t = new TransitionType_ThrowAndCatch(dur);
                    break;
                case "damping":
                    t = new TransitionType_CriticalDamping(dur);
                    break;
                default:
                    t = new TransitionType_Linear(dur);
                    break;
            }

            var r = new Transition(t);
            r.add(target, prop, des);
            return r;
        }

        public override Transition Convert(string s)
        {
            return null;
        }

        public override string Convert(Transition s)
        {
            return "";
        }

        #endregion
    }
}
