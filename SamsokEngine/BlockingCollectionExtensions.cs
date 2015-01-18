using System.Collections.Concurrent;
using System.Linq;

namespace SamsokEngine
{ 
    public static class BlockingCollectionExtensions
    {
        public static void AddIfNotExist(this BlockingCollection<SearchResultElement> collection, SearchResultElement elementToAdd)
        {
            var nodeAlreadyExist = collection.FirstOrDefault(x => x.Isbn == elementToAdd.Isbn
                                                                         && x.Title == elementToAdd.Title
                                                                         && x.Year == elementToAdd.Year
                                                                         && x.MediaType == elementToAdd.MediaType
                                                                         && !x.LibraryName.Contains(elementToAdd.LibraryName));

            if (nodeAlreadyExist != null)
            {
                nodeAlreadyExist.LibraryName += string.Format(", {0}", elementToAdd.LibraryName);
                nodeAlreadyExist.ATagToLibrary += string.Format("<br/>{0}", elementToAdd.ATagToLibrary);
            }
            else
                collection.Add(elementToAdd);
        }
    }
}