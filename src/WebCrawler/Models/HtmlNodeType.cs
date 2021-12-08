using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Models
{
	public enum HtmlNodeType
	{
		DocType,
		Whitespace,
		Comment,
		Element,
		Text
	}
}
