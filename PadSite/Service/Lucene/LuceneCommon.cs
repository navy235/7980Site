using System.IO;
using System.Web;
using Lucene.Net.Util;

namespace PadSite.Service
{
    internal static class LuceneCommon
    {
        internal static readonly string IndexOutDoorDirectory = Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data", "Lucene", "OutDoor");
        internal static readonly string IndexTestDirectory = Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data", "LuceneTest", "OutDoor");
        internal static readonly string IndexMetadataPath = Path.Combine(IndexOutDoorDirectory, "index.metadata");
        internal static readonly Version LuceneVersion = Version.LUCENE_30;
    }
}