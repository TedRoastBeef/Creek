using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using Creek.UI.EFML.Base;
using Creek.UI.EFML.Base.EFML.Elements;
using Creek.UI.EFML.Base.EFML.Processors;
using Creek.UI.EFML.Base.Exceptions;

namespace Creek.UI.EFML
{
    public class Builder
    {
        private readonly ProcessorCollection Processors = new ProcessorCollection();
        public Document document = new Document();

        public Builder()
        {
            AddProcessor<ImageProcessor>();
            AddProcessor<LinkProcessor>();
            AddProcessor<InputProcessor>();
            AddProcessor<LabelProcessor>();
            AddProcessor<ObjectProcessor>();
            AddProcessor<FlashProcessor>();
            AddProcessor<DivProcessor>();
            AddProcessor<GroupProcessor>();
            AddProcessor<DropDownProcessor>();
            AddProcessor<AudioProcessor>();
            AddProcessor<VideoProcessor>();
            AddProcessor<TableProcessor>();
            AddProcessor<TabControlProcessor>();
            AddProcessor<LineProcessor>();
            AddProcessor<NavigatorProcessor>();
        }

        private void AddProcessor(ElementProcessor p)
        {
            Processors.Add(new KeyValuePair<string, ElementProcessor>(p.Tagname, p));
        }

        private void AddProcessor<T>() where T : ElementProcessor, new()
        {
            AddProcessor(new T());
        }

        [DebuggerStepThrough]
        public void Load(byte[] buffer)
        {
            Load(Encoding.ASCII.GetString(buffer));
        }

        [DebuggerStepThrough]
        public void Load(string efml)
        {
            EFMLDocument doc = EFMLDocument.Load(efml);

            //new SourceElement().Process(document.Header, doc);
            new MetaElement().Process(document.Header.Meta, doc);
            new ScriptElement().Process(document.Header.Scripts, doc);
            new StyleElement().Process(document.Header.Styles, doc);
            new ValidatorElement().Process(document.Header.Validators, doc);

            UiBaseElement(doc.Body.ChildNodes, document.Body);
        }

        [DebuggerStepThrough]
        internal void UiBaseElement(XmlNodeList t, List<ElementBase> parent)
        {
            foreach (XmlNode markupTag in t)
            {
                if (markupTag is XmlComment)
                    continue;
                var e = new UiElement();

                new DefaultEventProvider().Resolve(e, markupTag);

                if (Processors[markupTag.Name] != null)
                    Processors[markupTag.Name].Process(out e, markupTag, this);
                else
                    throw new EfmlException("'" + markupTag.Name + "'-Tag is not found");

                if (markupTag.HasAttribute("id"))
                    e.ID = markupTag.GetAttributeByName("id");
                else
                    throw new EfmlException("'id' attribute is required on '" + markupTag.Name + "'");

                if (markupTag.HasAttribute("Content"))
                {
                    string l = markupTag.GetAttributeByName("Content");

                    e.content = l;
                }
                else
                {
                    e.content = markupTag.InnerText;
                }

                if (e.GetType().Name != typeof (UiElement).Name)
                    parent.Add(e);
            }
        }
    }
}