using System;
using System.Collections;
using System.Globalization;
using System.Net;
using System.Text;
using System.Xml;

namespace XmlRpcCore
{
	public class GbxCall
	{
        public string MethodName { get; private set; }
        public MessageTypes MessageType { get; private set; }
        public string Xml { get; }
        public ArrayList Params { get; }
        public int Handle { get; internal set; }
        public bool HasError { get; private set; }
        public string ErrorString { get; private set; }
        public int ErrorCode { get; private set; }

        public GbxCall(int handle, byte[] data)
		{
			this.MessageType = MessageTypes.None;
			this.Handle = handle;
			this.Xml = Encoding.UTF8.GetString(data);
			this.ErrorCode = 0;
			this.ErrorString = "";
            this.Params = new ArrayList();

			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(this.Xml);
			XmlElement xmlElement = null;

			if (xmlDocument["methodCall"] != null)
			{
				this.MessageType = handle > 0 ? MessageTypes.Callback : MessageTypes.Request;

				if (xmlDocument["methodCall"]["methodName"] != null)
				{
					this.MethodName = xmlDocument["methodCall"]["methodName"].InnerText;
				}
				else
				{
					this.HasError = true;
				}
				if (xmlDocument["methodCall"]["params"] != null)
				{
					this.HasError = false;
					xmlElement = xmlDocument["methodCall"]["params"];
				}
				else
				{
					this.HasError = true;
				}
			}
			else if (xmlDocument["methodResponse"] != null)
			{
				this.MessageType = MessageTypes.Response;
				if (xmlDocument["methodResponse"]["fault"] != null)
				{
					Hashtable hashtable = (Hashtable)this.ParseXml(xmlDocument["methodResponse"]["fault"]);
					this.ErrorCode = (int)hashtable["faultCode"];
					this.ErrorString = (string)hashtable["faultString"];
					this.HasError = true;
				}
				else if (xmlDocument["methodResponse"]["params"] != null)
				{
					this.HasError = false;
					xmlElement = xmlDocument["methodResponse"]["params"];
				}
				else
				{
					this.HasError = true;
				}
			}
			else
			{
				this.HasError = true;
			}
			if (xmlElement != null)
			{
				foreach (XmlElement inParam in xmlElement)
				{
					this.Params.Add(this.ParseXml(inParam));
				}
			}
		}

		public GbxCall(object[] @params)
		{
			this.Xml = "<?xml version=\"1.0\" ?>\n";
			this.Xml += "<methodResponse>\n";
			this.Xml += "<params>\n";
			foreach (object inParam in @params)
			{
			    this.Xml = this.Xml + "<param>" + this.ParseObject(inParam) + "</param>\n";
			}
			this.Xml += "</params>";
			this.Xml += "</methodResponse>";
		}

		public GbxCall(string methodName, object[] @params)
		{
			this.Xml = "<?xml version=\"1.0\" ?>\n";
			this.Xml += "<methodCall>\n";
			this.Xml = this.Xml + "<methodName>" + methodName + "</methodName>\n";
			this.Xml   += "<params>\n";
			foreach (object inParam in @params)
			{
			    this.Xml = this.Xml + "<param>" + this.ParseObject(inParam) + "</param>\n";
			}
			this.Xml += "</params>";
			this.Xml += "</methodCall>";
		}

		private string ParseObject(object param)
		{
			string text = "<value>";
			if (param is string)
			{
				text = text + "<string>" + WebUtility.HtmlEncode((string)param) + "</string>";
			}
			else if (param is int)
			{
				object obj = text;
				text = string.Concat(obj, "<int>", (int)param, "</int>");
			}
			else if (param is double)
			{
				object obj2 = text;
				text = string.Concat(obj2, "<double>", (double)param, "</double>");
				if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ",")
				{
					text = text.Replace(",", ".");
				}
			}
			else if (param is bool)
			{
				if ((bool)param)
				{
					text += "<boolean>1</boolean>";
				}
				else
				{
					text += "<boolean>0</boolean>";
				}
			}
			else if (param.GetType() == typeof(ArrayList))
			{
				text += "<array><data>";
				foreach (object current in ((ArrayList)param))
				{
					text += this.ParseObject(current);
				}
				text += "</data></array>";
			}
			else if (param.GetType() == typeof(Hashtable))
			{
				text += "<struct>";
				foreach (object current2 in ((Hashtable)param).Keys)
				{
					text += "<member>";
					text = text + "<name>" + current2 + "</name>";
					text += this.ParseObject(((Hashtable)param)[current2]);
					text += "</member>";
				}
				text += "</struct>";
			}
			else if (param.GetType() == typeof(byte[]))
			{
				text += "<base64>";
				text += Convert.ToBase64String((byte[])param);
				text += "</base64>";
			}
			return text + "</value>\n";
		}

		private object ParseXml(XmlElement inParam)
		{
		    var xmlElement = inParam["value"] ?? inParam;

			if (xmlElement["string"] != null)
			{
				return xmlElement["string"].InnerText;
			}

			if (xmlElement["int"] != null)
			{
				return int.Parse(xmlElement["int"].InnerText);
			}

			if (xmlElement["i4"] != null)
			{
				return int.Parse(xmlElement["i4"].InnerText);
			}

			if (xmlElement["double"] != null)
			{
				if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ",")
				{
					xmlElement["double"].InnerText = xmlElement["double"].InnerText.Replace(".", ",");
				}

				return double.Parse(xmlElement["double"].InnerText);
			}
			if (xmlElement["boolean"] != null)
			{
				if (xmlElement["boolean"].InnerText == "1")
				{
					return true;
				}

				return false;
			}

            if (xmlElement["struct"] != null)
		    {
		        Hashtable hashtable = new Hashtable();
		        foreach (XmlElement xmlElement2 in xmlElement["struct"])
		        {
		            hashtable.Add(xmlElement2["name"].InnerText, this.ParseXml(xmlElement2));
		        }
		        return hashtable;
		    }

		    if (xmlElement["array"] != null)
		    {
		        ArrayList arrayList = new ArrayList();
		        foreach (XmlElement inParam2 in xmlElement["array"]["data"])
		        {
		            arrayList.Add(this.ParseXml(inParam2));
		        }
		        return arrayList;
		    }

		    if (xmlElement["base64"] != null)
		    {
		        return Convert.FromBase64String(xmlElement["base64"].InnerText);
		    }

		    return null;
		}
	}
}
