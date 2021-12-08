using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Models;

namespace WebCrawler.Services
{
    public class HtmlParserService
	{
		int index = 0;
		Queue<char> q;

		// These tags can be parsed as self closing tags without the '/' example <hr> instead of <hr />.
		private readonly IReadOnlyCollection<string> SelfClosingTags = new[] { "area", "base", "br", "col", "embed", "hr", "img", "input", "keygen", "link", "menuitem", "meta", "param", "source", "track", "wbr" };

		private readonly bool IgnoreWhitespace = true;

		public string Source { get; private set; }

		public HtmlParserService()//ParserOptions options = null)
		{
			//this.options = options ?? ParserOptions.Default;
		}


		public IEnumerable<HtmlNode> Parse(string source)
		{
			this.Source = source;

			this.index = 0;
			this.q = new Queue<char>(source);

			return InternalParse();
		}

		private IEnumerable<HtmlNode> InternalParse(HtmlNode parent = null)
		{
			var list = new List<HtmlNode>();
			HtmlNode node = null;
			while (q.Any())
			{
				
				var c = q.Peek();
				
				//var c2 = ;
				var data = new String(q.ToArray());
				var c4 = q.Where((value, index) => index < 4).ToString();
				if (q.Where((value, index) => index < 2).ToString() == "</")
				{
					Dequeue(2);
					if (parent != null)
						parent.EndTag = GetUpTo('>');
					else if (node != null)
						node.EndTag = GetUpTo('>');
					else
						GetUpTo('>');
					Dequeue();
					return list;
				}
				else if (q.Where((value, index) => index < 4).ToString() == "<!--")
				{
					var comment = GetComment();
					node = HtmlNode.CreateComment();
					node.Content = comment;
					list.Add(node);

				}
				else if (q.Where((value, index) => index < 2).ToString() == "<!")
				{
					node = CreateDocType();
					list.Add(node);
				}
				else if (c == '<')
				{
					node = HtmlNode.CreateElement(index);

					Dequeue();
					node.Tag = GetTagName();

					if (q.Peek() == ' ' || q.Peek() == '\r')

						node.Attributes = GetAttributes();

					SkipSpace();

					if (!q.Any())
					{
						node.SetEndIndex(index, Source);
						list.Add(node);
						break;
					}

					//var c3 = ;
					// instantly closed
					if (q.Peek() == '>' && (q.Where((value, index) => index < 3).ToString() == "></"))
					{
						Dequeue(3);
						node.EndTag = GetUpTo('>');
						Dequeue();

						node.SetEndIndex(index, Source);
						list.Add(node);
					}
					// self closing element
					else if (q.Where((value, index) => index < 2).ToString() == "/>")
					{
						Dequeue(2);
						node.SelfClosing = true;
						node.SetEndIndex(index, Source);
						list.Add(node);
					}
					// self closing tags that don't have '/>' ie: <br>
					else if (q.Peek() == '>' && SelfClosingTags.Contains(node.Tag))
					{
						Dequeue();
						node.SelfClosing = true;
						list.Add(node);

					}
					else if (node.Tag.Equals("script", StringComparison.OrdinalIgnoreCase))
					{
						Dequeue(); // >
						node.Content = GetUpTo("</script");
						node.EndTag = "script";
						Dequeue(9); // </script>
						node.SetEndIndex(index, Source);
						list.Add(node);
					}
				}
				else if (c == '>')
				{
					Dequeue();
					node.AddChildren(InternalParse(node));
					node.SetEndIndex(index, Source);
					list.Add(node);
				}
				else
				{
					var text = string.Empty;

					while (q.Any())
					{
						text += Dequeue();
						if (q.Any() && q.Peek() == '<') break;

					}
					if (text.Trim().Length == 0)
					{
						if (IgnoreWhitespace == false)
							list.Add(HtmlNode.CreateWhiteSpace(text));

					}
					else
						list.Add(HtmlNode.CreateText(text));
				}

			}

			return list;
		}


		HtmlNode CreateDocType()
		{
			Dequeue(2);
			var text = GetUpTo('>');
			var type = HtmlNodeType.DocType;
			Dequeue();

			return new HtmlNode(type)
			{
				Content = text
			};
		}


		HtmlAttributeCollection GetAttributes()
		{
			var attrs = new HtmlAttributeCollection();

			if (!q.Any()) return attrs;

			SkipSpace();

			if (q.Peek() == '/') return attrs;


			do
			{
				attrs.Add(GetAttribute());
				SkipSpace();

			} while (q.Any() && q.Peek() != '>' && q.Where((value, index) => index < 2).ToString() != "/>");

			return attrs;

			KeyValuePair<string, string> GetAttribute()
			{
				var name = GetUpTo('=', ' ', '>');

				if (q.Peek() == ' ') return new KeyValuePair<string, string>(name, null);

				Dequeue();

				if (q.Peek() == '>') return new KeyValuePair<string, string>(name, null);

				var c2 = q.Where((value, index) => index < 2).ToString();
				if (q.Where((value, index) => index < 2).ToString() == "''" || q.Where((value, index) => index < 2).ToString() == "\"\"" || q.Where((value, index) => index < 2).ToString() == "``")
				{
					Dequeue(2);
					return new KeyValuePair<string, string>(name, string.Empty);
				}

				if (q.Any())
				{
					// attr=value is valid so check for the scenerio
					if (q.Peek() == '\'' || q.Peek() == '"' || q.Peek() == '`')
					{
						var del = Dequeue();
						var value = GetUpTo(del);
						Dequeue();
						return new KeyValuePair<string, string>(name, value);
					}
					else
					{
						var value = GetUpTo(' ', '>', '<', '\'', '"', '=', '`');
						return new KeyValuePair<string, string>(name, value);
					}
				}

				return new KeyValuePair<string, string>(name, null);
			}
		}

		
		string GetTagName() => GetUpTo(' ', '>', '/', '\r', '\n');

		string GetUpTo(params char[] chars)
			 => GetUpTo(() => chars.Contains(q.Peek()));

		string GetUpTo(string text)
			=> GetUpTo(() => q.Where((value, index) => index < text.Length).ToString() == text);

		string GetComment()
		{
			this.Dequeue(4);
			var text = GetUpTo("-->");
			this.Dequeue(3);
			return text;
		}

		string GetUpTo(Func<bool> fn)
		{
			var text = string.Empty;
			while (q.Any())
			{
				text += Dequeue();
				if (!q.Any()) return text;
				if (fn())
					break;
			}
			return text;
		}

		void SkipSpace()
		{
			while (q.Any() && (q.Peek() == ' ' || q.Peek() == '\r' || q.Peek() == '\n'))
				Dequeue();
		}

		char Dequeue()
		{
			index++;
			return q.Dequeue();
		}

		string Dequeue(int length)
		{
			string text = string.Empty;
			while (length-- > 0)
			{
				index++;
                if (q.Count > 0)
                {
					text += q.Dequeue();
				}
				
			}
			return text;
		}
	}
}
