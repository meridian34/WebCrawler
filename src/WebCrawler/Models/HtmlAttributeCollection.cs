using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Models
{
	public class HtmlAttributeCollection : Dictionary<string, string>
	{
		internal void Add(KeyValuePair<string, string> kv)
		{
			var key = kv.Key;

			if (!this.ContainsKey(key))
			{
				this.Add(key, kv.Value);
			}
		}

		public bool Contains(string key)
			=> this.ContainsKey(key);

	}
}
