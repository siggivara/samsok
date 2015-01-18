<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Samsok.Default" %>
<%@ Import Namespace="Samsok" %>
<%@ Import Namespace="SamsokEngine" %>


<%--<asp:Literal runat="server" ID="ltrHtml" />--%>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    
<head runat="server">
    <title></title>
    
    <!-- jQuery -->
	<script src="frontend/jquery-latest.min.js"></script>

	<!-- Demo stuff -->
	<link rel="stylesheet" href="frontend/jq.css">
	<link href="frontend/prettify.css" rel="stylesheet">
	<script src="frontend/prettify.js"></script>
	<script src="frontend/docs.js"></script>

	<!-- jQuery UI theme switcher: https://github.com/pontikis/jui_theme_switch/ -->
	<style>
	.switcher_container { padding: 5px; }
	.switcher_list { padding: 2px; }
	.switcher_label { margin-right: 5px; }
	</style>
	<script src="frontend/jquery.jui_theme_switch.min.js"></script>

	<!-- Tablesorter: required; also include any of the jQuery UI themes -->
	<link  rel="stylesheet" href="frontend/jquery-ui.min.css">
	<link href="frontend/theme.jui.css" rel="stylesheet">

	<script src="frontend/jquery.tablesorter.js"></script>
	<script src="frontend/jquery.tablesorter.widgets.js"></script>
    
</head>
<body>
    <form id="form1" runat="server">
        <asp:TextBox ID="txtbSearchTerm" runat="server" />
        <asp:Button ID="btnSearch" runat="server" Text="Søk" OnClick="SearchButtonClick" />
        
        <br/>
        <asp:Literal runat="server" ID="ltrTimer" />
        <br/>

        <asp:Repeater runat="server" ID="rptResult">
            <HeaderTemplate>
                <table id="tblResult" class="tablesorter" cellspacing="1" border="1" style="border-style:Solid;" rules="all" >
                    <thead>
                      <tr>
		                    <th>Title</th><th>Forfatter</th><th>Type</th><th>År</th><th>ISBN</th><th>Bibliotek</th><%--<th>Url</th>--%>
	                    </tr>
                    </thead>
            </HeaderTemplate> 
            <ItemTemplate>
                <tr>
                    <td><%# ((SearchResultElement)Container.DataItem).Title %></td>
                    <td><%# ((SearchResultElement)Container.DataItem).Author %></td>
                    <td><%# ((SearchResultElement)Container.DataItem).MediaType %></td>
                    <td><%# ((SearchResultElement)Container.DataItem).Year %></td>
                    <td><%# ((SearchResultElement)Container.DataItem).Isbn %></td>
                    <td><%# ((SearchResultElement)Container.DataItem).ATagToLibrary %></td>
                    <%--<td><a target="_blank" href="<%# ((SearchResultElement)Container.DataItem).Url %>"><%# ((SearchResultElement)Container.DataItem).Url %></a></td>--%>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
                <script>
                    $(function () {

                        // Extend the themes to change any of the default class names ** NEW **
                        $.extend($.tablesorter.themes.jui, {
                            // change default jQuery uitheme icons - find the full list of icons here: http://jqueryui.com/themeroller/ (hover over them for their name)
                            table: 'ui-widget ui-widget-content ui-corner-all', // table classes
                            caption: 'ui-widget-content ui-corner-all',
                            header: 'ui-widget-header ui-corner-all ui-state-default', // header classes
                            footerRow: '',
                            footerCells: '',
                            icons: 'ui-icon', // icon class added to the <i> in the header
                            sortNone: 'ui-icon-carat-2-n-s',
                            sortAsc: 'ui-icon-carat-1-n',
                            sortDesc: 'ui-icon-carat-1-s',
                            active: 'ui-state-active', // applied when column is sorted
                            hover: 'ui-state-hover',  // hover class
                            filterRow: '',
                            even: 'ui-widget-content', // odd row zebra striping
                            odd: 'ui-state-default'   // even row zebra striping
                        });

                        // call the tablesorter plugin and apply the ui theme widget
                        $("table").tablesorter({

                            theme: 'jui', // theme "jui" and "bootstrap" override the uitheme widget option in v2.7+

                            headerTemplate: '{content} {icon}', // needed to add icon for jui theme

                            // widget code now contained in the jquery.tablesorter.widgets.js file
                            widgets: ['uitheme', 'zebra'],

                            widgetOptions: {
                                // zebra striping class names - the uitheme widget adds the class names defined in
                                // $.tablesorter.themes to the zebra widget class names
                                zebra: ["even", "odd"],

                                // set the uitheme widget to use the jQuery UI theme class names
                                // ** this is now optional, and will be overridden if the theme name exists in $.tablesorter.themes **
                                // uitheme : 'jui'
                            }

                        });

                    });
                </script>
            </FooterTemplate>
        </asp:Repeater>
    </form>
</body>
</html>
