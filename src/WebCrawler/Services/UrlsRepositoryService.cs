using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebCrawler.Models;
using WebCrawler.Services.Abstractions;

namespace WebCrawler.Services
{
    public class UrlsRepositoryService : IUrlsRepositoryService
    {
        private readonly SemaphoreSlim _semaphore;
        private string _root;

        public UrlsRepositoryService()
        {
            LinkItems = new List<LinkItem>();
            _semaphore = new SemaphoreSlim(1, 1);
            _root = null;
            Count = 0;
        }

        public int Count { get; private set; }

        private List<LinkItem> LinkItems { get; set; }

        public async Task InitRootAsync(string url)
        {
            await SafeInvoke(() =>
            {
                if (IsUrl(url) && IsAbsoluteUrl(url))
                {
                    _root = GetUrlAuthority(url);
                    AddLink(url);
                }
                else
                {
                    throw new ArgumentException();
                }
            });
        }

        public async Task AddLinkAsync(string url)
        {
            await _semaphore.WaitAsync();
            AddLink(url);
            _semaphore.Release();
        }

        public async Task AddLinkAsync(IEnumerable<string> urls)
        {
            await SafeInvoke(() =>
            {
                foreach (var url in urls)
                {
                    AddLink(url);
                }
            });
        }

        public async Task<string> GetUniqueUrlAsync()
        {
            return await SafeInvoke(() =>
            {
                var url = LinkItems.Where(x => x.IsCrawled == false).FirstOrDefault();
                if (url == null)
                {
                    return null;
                }

                url.IsCrawled = true;
                if (Count > 0)
                {
                    Count--;
                }

                return url.Url;
            });
        }

        public async Task<IReadOnlyCollection<string>> GetListUniqueUrlsAsync()
        {
            return await SafeInvoke(() =>
            {
                var urls = LinkItems.Where(x => x.IsCrawled == false).ToList();
                urls.ForEach(x => x.IsCrawled = true);
                Count = 0;
                return urls.Select(x => x.Url).ToList();
            });
        }

        private bool AddLink(string url)
        {
            try
            {
                if (IsUrl(url) && !Contains(url) && url.Contains(_root))
                {
                    if (IsAbsoluteUrl(url))
                    {
                        Uri uriResult;
                        Uri.TryCreate(url, UriKind.Absolute, out uriResult);
                        if (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)
                        {
                            LinkItems.Add(new LinkItem { IsCrawled = false, Url = url });
                            Count++;
                            return true;
                        }

                        return false;
                    }
                    else
                    {
                        var root = new Uri(_root);
                        var newUri = new Uri(root, url);
                        AddLink(newUri.ToString());
                    }
                }

                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task SafeInvoke(Action action)
        {
            await _semaphore.WaitAsync();
            action.Invoke();
            _semaphore.Release();
        }

        private async Task<string> SafeInvoke(Func<string> func)
        {
            await _semaphore.WaitAsync();
            var result = func.Invoke();
            _semaphore.Release();
            return result;
        }

        private async Task<IReadOnlyCollection<string>> SafeInvoke(Func<IReadOnlyCollection<string>> func)
        {
            await _semaphore.WaitAsync();
            var result = func.Invoke();
            _semaphore.Release();
            return result;
        }

        private bool Contains(string url)
        {
            return LinkItems.Where(x => x.Url == url).FirstOrDefault() == null ? false : true;
        }

        private string GetUrlAuthority(string url)
        {
            Uri uriResult = new Uri(url);
            return uriResult.GetLeftPart(UriPartial.Authority);
        }

        private bool IsUrl(string url)
        {
            Uri uriResult;
            bool isLink = Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uriResult);

            if (isLink)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsAbsoluteUrl(string url)
        {
            Uri uriResult;
            return Uri.TryCreate(url, UriKind.Absolute, out uriResult);
        }
    }
}
