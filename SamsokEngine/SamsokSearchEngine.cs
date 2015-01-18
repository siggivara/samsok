using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SamsokEngine  
{
    public static class SamsokSearchEngine
    {
        private static readonly string[] SearchUrlList =
        {
            "http://agder.samsok.no/cgi-bin/samsok-agder?mode=visResultat&v_meny1=&v_tekst1={0}&v_avgrens_aarfra=&v_fraforslag=0&v_avgrens_aartil=&v_para1=og&v_para2=alle&v_para3=alle&b_alle1=on&b_alle2=on&sok=S%C3%B8k",
            "http://akershus.samsok.no/cgi-bin/samsok-akershus?mode=visResultat&v_meny1=&v_tekst1={0}&v_avgrens_aarfra=&v_fraforslag=0&v_avgrens_aartil=&v_para1=og&v_para2=alle&v_para3=alle&b_alle1=on&b_alle2=on&sok=S%C3%B8k",
            "http://finnmark.samsok.no/cgi-bin/samsok-finnmark?mode=visResultat&v_meny1=&v_tekst1={0}&v_avgrens_aarfra=&v_fraforslag=0&v_avgrens_aartil=&v_para1=og&v_para2=alle&v_para3=alle&b_alle1=on&b_alle2=on&sok=S%C3%B8k",
            "http://hedmark.samsok.no/cgi-bin/samsok-hedmark?mode=visResultat&v_meny1=&v_tekst1={0}&v_fraforslag=0&v_avgrens_aarfra=&v_avgrens_aartil=&v_para1=og&v_para2=alle&v_para3=alle&b_alle1=on&b_alle2=on&sok=S%C3%B8k",
            "http://hordaland.samsok.no/cgi-bin/samsok-hordaland?mode=visResultat&v_meny1=&v_tekst1={0}&v_fraforslag=0&v_avgrens_aarfra=&v_avgrens_aartil=&v_para1=og&v_para2=alle&v_para3=alle&b_alle1=on&b_alle2=on&sok=S%C3%B8k",
            "http://mr.samsok.no/cgi-bin/samsok-mr?mode=visResultat&v_meny1=&v_tekst1={0}&v_fraforslag=0&v_avgrens_aarfra=&v_avgrens_aartil=&v_para1=og&v_para2=alle&v_para3=alle&b_alle1=on&b_alle2=on&sok=S%C3%B8k",
            "http://nordland.samsok.no/cgi-bin/samsok-nordland?mode=visResultat&v_meny1=&v_tekst1={0}&v_fraforslag=0&v_avgrens_aarfra=&v_avgrens_aartil=&v_para1=og&v_para2=alle&v_para3=alle&b_alle1=on&b_alle2=on&sok=S%C3%B8k",
            "http://oppland.samsok.no/cgi-bin/samsok-oppland?mode=visResultat&v_meny1=&v_tekst1={0}&v_fraforslag=0&v_avgrens_aarfra=&v_avgrens_aartil=&v_para1=og&v_para2=alle&v_para3=alle&b_alle1=on&b_alle2=on&sok=S%C3%B8k",
            "http://sf.samsok.no/cgi-bin/samsok-sf?mode=visResultat&v_meny1=&v_tekst1={0}&v_fraforslag=0&v_avgrens_aarfra=&v_avgrens_aartil=&v_para1=og&v_para2=alle&v_para3=alle&b_alle1=on&b_alle2=on&sok=S%C3%B8k",
            "http://telemark.samsok.no/cgi-bin/samsok-telemark?mode=visResultat&v_meny1=&v_tekst1={0}&v_fraforslag=0&v_avgrens_aarfra=&v_avgrens_aartil=&v_para1=og&v_para2=alle&v_para3=alle&b_alle1=on&b_alle2=on&sok=S%C3%B8k",
            "http://troms.samsok.no/cgi-bin/samsok-troms?mode=visResultat&v_meny1=&v_tekst1={0}&v_avgrens_aarfra=&v_avgrens_aartil=&v_fraforslag=0&v_para1=og&v_para2=alle&v_para3=alle&b_alle1=on&b_alle2=on&sok=S%C3%B8k",
            "http://trondelag.samsok.no/cgi-bin/samsok-trondelag?mode=visResultat&v_meny1=&v_tekst1={0}&v_avgrens_aarfra=&v_avgrens_aartil=&v_fraforslag=0&v_para1=og&v_para2=alle&v_para3=alle&b_alle1=on&b_alle2=on&sok=S%C3%B8k",
            "http://vestfold.samsok.no/cgi-bin/samsok-vestfold?mode=visResultat&v_meny1=&v_tekst1={0}&v_avgrens_aarfra=&v_avgrens_aartil=&v_fraforslag=0&v_para1=og&v_para2=alle&v_para3=alle&b_alle1=on&b_alle2=on&sok=S%C3%B8k",
            "http://ostfold.samsok.no/cgi-bin/samsok-ostfold?mode=visResultat&v_meny1=&v_tekst1={0}&v_fraforslag=0&v_avgrens_aarfra=&v_avgrens_aartil=&v_para1=og&v_para2=alle&v_para3=alle&b_alle1=on&sok=S%C3%B8k"
        };

        private static List<SearchResultElement> DoLibrarySearch(string searchUrl)
        {
            var webSite = new HtmlWeb();
            var document = webSite.Load(searchUrl);
            var result = document.DocumentNode.SelectNodes("//table[@id='treffliste']//tr[@isbn]");
            var resultSet = new List<SearchResultElement>();

            if (result == null) 
                return resultSet;

            foreach (HtmlNode node in result)
            {
                var url = node.SelectSingleNode(".//a[@name='tittel']").GetAttributeValue("href", string.Empty);
                var libraryName = node.ParentNode.GetAttributeValue("name", string.Empty); 
                resultSet.Add(new SearchResultElement
                {
                    Isbn = node.GetAttributeValue("isbn", string.Empty),
                    Title = node.SelectSingleNode(".//a[@name='tittel']").InnerText,
                    Url = url,
                    Author =
                        node.SelectSingleNode(".//td[@name='person']") == null
                            ? String.Empty
                            : node.SelectSingleNode(".//td[@name='person']").InnerText,
                    LibraryName = libraryName,
                    MediaType =
                        node.SelectSingleNode(".//td[@mediekode]") == null
                            ? String.Empty
                            : node.SelectSingleNode(".//td[@mediekode]").InnerText,
                    Year =
                        node.SelectSingleNode(".//td[@name='aar']") == null
                            ? String.Empty
                            : node.SelectSingleNode(".//td[@name='aar']").InnerText,
                    ATagToLibrary = string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a>", url, libraryName) 
                });
            }

            return resultSet;
        }


        public static IEnumerable<SearchResultElement> SearchAll(string searchTerm)
        {
            var resultList = new BlockingCollection<SearchResultElement>();
            Parallel.ForEach(SearchUrlList, url =>
            {
                var result = DoLibrarySearch(string.Format(url, searchTerm.Replace(" ", "+")));
                foreach (var searchResultElement in result)
                {
                    resultList.AddIfNotExist(searchResultElement);
                }
            });

            return resultList;
        }
    }
}
