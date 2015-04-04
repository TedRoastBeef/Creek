using System;

namespace Creek.UI.EFML.Base
{
    public struct Tag : IEquatable<Tag>
    {
        private readonly string Content;

        internal Tag(string v)
        {
            Content = v;
        }

        public static Tag Audio
        {
            get { return new Tag("audio"); }
        }

        public static Tag Video
        {
            get { return new Tag("video"); }
        }

        public static Tag Label
        {
            get { return new Tag("label"); }
        }

        public static Tag Button
        {
            get { return new Tag("button"); }
        }

        public static Tag Passwordbox
        {
            get { return new Tag("passwordbox"); }
        }

        public static Tag Textbox
        {
            get { return new Tag("textbox"); }
        }

        public static Tag Link
        {
            get { return new Tag("link"); }
        }

        public static Tag Div
        {
            get { return new Tag("div"); }
        }

        public static Tag Group
        {
            get { return new Tag("group"); }
        }

        public static Tag Image
        {
            get { return new Tag("image"); }
        }

        public static Tag Dropdown
        {
            get { return new Tag("dropdown"); }
        }

        public static Tag MailTextbox
        {
            get { return new Tag("mail"); }
        }

        public static Tag Checkbox
        {
            get { return new Tag("checkbox"); }
        }

        public static Tag Flash
        {
            get { return new Tag("flash"); }
        }

        public static Tag Embbed
        {
            get { return new Tag("embbed"); }
        }

        public static Tag Radiobutton
        {
            get { return new Tag("radiobutton"); }
        }

        public static Tag Navigator
        {
            get { return new Tag("navigator"); }
        }

        public static Tag Line
        {
            get { return new Tag("line"); }
        }

        #region IEquatable<Tag> Members

        public bool Equals(Tag other)
        {
            return Content.Equals(other.Content);
        }

        #endregion

        public static Tag FromString(string s)
        {
            return new Tag(s);
        }

        public override string ToString()
        {
            return Content;
        }

        public override int GetHashCode()
        {
            return Content.GetHashCode();
        }
    }
}