using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Pet;
using Xunit;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ManualResetEvent mre = new ManualResetEvent(false);
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("你好吗");
            var handler = new HttpClientHandler();
            var cookie = new CookieContainer();
            handler.CookieContainer = cookie;
            string index = "/index.php";
            string urlbase = "http://dz.d7w.biz";
            HttpClient client = new HttpClient(handler);
            string dge = "/html/body/div[@id='main']/div[3]/table/tbody[@id='cate_6']/tr[3]/td[1]/a";
            string dgeList = "/html/body/div[@id='main']/div[3]/table/tbody[1]/tr[@class='tr3 t_one']/td[1]/a[1]";
            string picList = "//input[@type='image']";
            var t1 = Task.Run(async () =>
            {
                var resp = await GetSingleNode(client, urlbase + index, dge, p => p.Attributes["href"].Value);
                var resp2 = await GetManyNodes(client, urlbase + "/" + resp, dgeList, p => p.Attributes["href"].Value);
                foreach (var item in resp2)
                { 
                    var resp4 = await GetManyNodes(client, urlbase + "/" + item, picList, p => p.Attributes["src"].Value); 
                    foreach (var item2 in resp4)
                    {
                        Console.WriteLine(item2);
                    }
                }
                  Console.WriteLine("over");
            });
            t1.GetAwaiter().OnCompleted(() => mre.Set());
            mre.WaitOne();
        }

        public static async Task<string> GetSingleNode(HttpClient client, string url, string searchPattern, Func<HtmlNode, string> fun1)
        {
            return await Task.Run(async () =>
                       {
                           var doc = await GetDocument(client, url);
                           var val = doc.DocumentNode.SelectSingleNode(searchPattern);
                           return fun1.Invoke(val);
                       });
        }
        public static async Task<HtmlDocument> GetDocument(HttpClient client, string url)
        {

            HttpRequestMessage msg = new HttpRequestMessage();
            msg.Headers.ExpectContinue = false;
            msg.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36");
            msg.Headers.Referrer = new Uri("http://dz.d7w.biz/index.php");
            msg.Method = HttpMethod.Get;
            msg.RequestUri = new Uri(url);
            var content = await client.SendAsync(msg);
            var html = await content.Content.ReadAsStringAsync();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html); 
            return doc;
        }
        public static async Task<List<string>> GetManyNodes(HttpClient client, string url, string searchPattern, Func<HtmlNode, string> fun1)
        {
            return await Task.Run(async () =>
                       {
                           var doc = await GetDocument(client, url);
                           List<string> lis = new List<string>();
                           var nodes = doc.DocumentNode.SelectNodes(searchPattern);
                           if (nodes == null || nodes.Count == 0)
                               return lis; 
                           foreach (var item in nodes)
                           {
                               lis.Add(fun1.Invoke(item));
                           }
                           return lis;
                       });
        }
        [Fact]
        public void TestName()
        {
            //Given
            IPet pet = new Dog();
            //When
            pet.Name = "fuck";
            //Then
            Assert.True(pet.Talk() == "fuck:wang wang");
            pet.Name = "fuck1";
            //error
            Assert.True(pet.Talk() == "fuck:wang wang");
        }

        [Fact]
        public void testhelloworld()
        {
            List<IPet> lis = new List<IPet>();
            lis.Add(new Dog() { Name = "dog1" });
            lis.Add(new Cat() { Name = "cat1" });
            foreach (var item in lis)
            {
                Console.WriteLine(item.Talk());
            }
            Console.WriteLine("Hello World!");
        }

    }
}
