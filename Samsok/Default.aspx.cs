using System;
using System.Diagnostics;
using SamsokEngine;

namespace Samsok
{
    public partial class Default : System.Web.UI.Page
    { 
        protected void SearchButtonClick(object sender, EventArgs e) 
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            rptResult.DataSource = SamsokSearchEngine.SearchAll(txtbSearchTerm.Text); ;
            rptResult.DataBind();

            stopwatch.Stop();
            ltrTimer.Text = string.Format("Search took {0} ms.", stopwatch.ElapsedMilliseconds);
        }
    }
}