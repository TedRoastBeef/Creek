using System.IO;
using System.Net;

namespace Creek.UI.EFML.Base.JS.ScriptingTypes
{
    public class XmlHttpRequest
    {
        private int _readyState;
        private HttpWebRequest _webrequest;
        public dynamic onreadystatechange;

        public int readystate
        {
            get { return _readyState; }
            set
            {
                if (onreadystatechange != null)
                    onreadystatechange();
                _readyState = value;
            }
        }

        public string mimetype
        {
            get { return _webrequest.MediaType; }
            set { _webrequest.MediaType = value; }
        }

        public string responsetext { get; set; }

        public void open(string method, string url)
        {
            _webrequest = (HttpWebRequest) WebRequest.Create(url);
            _webrequest.Method = method;
            readystate = 0;
        }

        public void send(object data)
        {
            if (data == null)
            {
                var resp = (HttpWebResponse) _webrequest.GetResponse();
                using (Stream s = resp.GetResponseStream())
                {
                    responsetext = new StreamReader(s).ReadToEnd();
                    readystate = 1;
                }
            }
            else
            {
                using (Stream s = _webrequest.GetRequestStream())
                {
                    var sw = new StreamWriter(s);
                    sw.Write(data);
                    sw.Flush();
                    readystate = 2;
                }
            }
        }
    }
}