using System;
using System.Collections;
using System.Globalization;
using System.Net;
using System.Text;
using System.Xml;

namespace TMXmlRpcLib
{
	// Token: 0x0200000F RID: 15
	public class GbxCall
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00002AFC File Offset: 0x00001AFC
		public GbxCall(int in_handle, byte[] in_data)
		{
			this.m_type = MessageTypes.None;
			this.m_handle = in_handle;
			this.m_xml = Encoding.UTF8.GetString(in_data);
			this.m_error_code = 0;
			this.m_error_string = "";
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(this.m_xml);
			XmlElement xmlElement = null;
			if (xmlDocument["methodCall"] != null)
			{
				if (in_handle > 0)
				{
					this.m_type = MessageTypes.Callback;
				}
				else
				{
					this.m_type = MessageTypes.Request;
				}
				if (xmlDocument["methodCall"]["methodName"] != null)
				{
					this.m_method_name = xmlDocument["methodCall"]["methodName"].InnerText;
				}
				else
				{
					this.m_error = true;
				}
				if (xmlDocument["methodCall"]["params"] != null)
				{
					this.m_error = false;
					xmlElement = xmlDocument["methodCall"]["params"];
				}
				else
				{
					this.m_error = true;
				}
			}
			else if (xmlDocument["methodResponse"] != null)
			{
				this.m_type = MessageTypes.Response;
				if (xmlDocument["methodResponse"]["fault"] != null)
				{
					Hashtable hashtable = (Hashtable)this.ParseXml(xmlDocument["methodResponse"]["fault"]);
					this.m_error_code = (int)hashtable["faultCode"];
					this.m_error_string = (string)hashtable["faultString"];
					this.m_error = true;
				}
				else if (xmlDocument["methodResponse"]["params"] != null)
				{
					this.m_error = false;
					xmlElement = xmlDocument["methodResponse"]["params"];
				}
				else
				{
					this.m_error = true;
				}
			}
			else
			{
				this.m_error = true;
			}
			if (xmlElement != null)
			{
				foreach (XmlElement inParam in xmlElement)
				{
					this.m_params.Add(this.ParseXml(inParam));
				}
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002D30 File Offset: 0x00001D30
		public GbxCall(object[] in_params)
		{
			this.m_xml = "<?xml version=\"1.0\" ?>\n";
			this.m_xml += "<methodResponse>\n";
			this.m_xml += "<params>\n";
			for (int i = 0; i < in_params.Length; i++)
			{
				object inParam = in_params[i];
				this.m_xml = this.m_xml + "<param>" + this.ParseObject(inParam) + "</param>\n";
			}
			this.m_xml += "</params>";
			this.m_xml += "</methodResponse>";
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002DE8 File Offset: 0x00001DE8
		public GbxCall(string in_method_name, object[] in_params)
		{
			this.m_xml = "<?xml version=\"1.0\" ?>\n";
			this.m_xml += "<methodCall>\n";
			this.m_xml = this.m_xml + "<methodName>" + in_method_name + "</methodName>\n";
			this.m_xml += "<params>\n";
			for (int i = 0; i < in_params.Length; i++)
			{
				object inParam = in_params[i];
				this.m_xml = this.m_xml + "<param>" + this.ParseObject(inParam) + "</param>\n";
			}
			this.m_xml += "</params>";
			this.m_xml += "</methodCall>";
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002EBC File Offset: 0x00001EBC
		private string ParseObject(object inParam)
		{
			string text = "<value>";
			if (inParam.GetType() == typeof(string))
			{
				text = text + "<string>" + WebUtility.HtmlEncode((string)inParam) + "</string>";
			}
			else if (inParam.GetType() == typeof(int))
			{
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					"<int>",
					(int)inParam,
					"</int>"
				});
			}
			else if (inParam.GetType() == typeof(double))
			{
				object obj2 = text;
				text = string.Concat(new object[]
				{
					obj2,
					"<double>",
					(double)inParam,
					"</double>"
				});
				if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ",")
				{
					text = text.Replace(",", ".");
				}
			}
			else if (inParam.GetType() == typeof(bool))
			{
				if ((bool)inParam)
				{
					text += "<boolean>1</boolean>";
				}
				else
				{
					text += "<boolean>0</boolean>";
				}
			}
			else if (inParam.GetType() == typeof(ArrayList))
			{
				text += "<array><data>";
				foreach (object current in ((ArrayList)inParam))
				{
					text += this.ParseObject(current);
				}
				text += "</data></array>";
			}
			else if (inParam.GetType() == typeof(Hashtable))
			{
				text += "<struct>";
				foreach (object current2 in ((Hashtable)inParam).Keys)
				{
					text += "<member>";
					text = text + "<name>" + current2.ToString() + "</name>";
					text += this.ParseObject(((Hashtable)inParam)[current2]);
					text += "</member>";
				}
				text += "</struct>";
			}
			else if (inParam.GetType() == typeof(byte[]))
			{
				text += "<base64>";
				text += Convert.ToBase64String((byte[])inParam);
				text += "</base64>";
			}
			return text + "</value>\n";
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003198 File Offset: 0x00002198
		private object ParseXml(XmlElement inParam)
		{
			XmlElement xmlElement;
			if (inParam["value"] == null)
			{
				xmlElement = inParam;
			}
			else
			{
				xmlElement = inParam["value"];
			}
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
			else
			{
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

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00003400 File Offset: 0x00002400
		public string MethodName
		{
			get
			{
				return this.m_method_name;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00003408 File Offset: 0x00002408
		public MessageTypes Type
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00003410 File Offset: 0x00002410
		public string Xml
		{
			get
			{
				return this.m_xml;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00003418 File Offset: 0x00002418
		public ArrayList Params
		{
			get
			{
				return this.m_params;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00003420 File Offset: 0x00002420
		public int Size
		{
			get
			{
				return this.m_xml.Length;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600004A RID: 74 RVA: 0x0000342D File Offset: 0x0000242D
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00003435 File Offset: 0x00002435
		public int Handle
		{
			get
			{
				return this.m_handle;
			}
			set
			{
				this.m_handle = value;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600004C RID: 76 RVA: 0x0000343E File Offset: 0x0000243E
		public bool Error
		{
			get
			{
				return this.m_error;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00003446 File Offset: 0x00002446
		public string ErrorString
		{
			get
			{
				return this.m_error_string;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600004E RID: 78 RVA: 0x0000344E File Offset: 0x0000244E
		public int ErrorCode
		{
			get
			{
				return this.m_error_code;
			}
		}

		// Token: 0x0400001F RID: 31
		private int m_handle;

		// Token: 0x04000020 RID: 32
		private string m_xml;

		// Token: 0x04000021 RID: 33
		private ArrayList m_params = new ArrayList();

		// Token: 0x04000022 RID: 34
		private bool m_error;

		// Token: 0x04000023 RID: 35
		private string m_error_string;

		// Token: 0x04000024 RID: 36
		private int m_error_code;

		// Token: 0x04000025 RID: 37
		private string m_method_name;

		// Token: 0x04000026 RID: 38
		private MessageTypes m_type;
	}
}
