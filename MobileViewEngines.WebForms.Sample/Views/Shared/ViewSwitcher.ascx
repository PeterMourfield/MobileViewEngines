<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%  if (Request.Browser.IsMobileDevice && Request.HttpMethod == "GET")
    {%>
<div>
    <% if (ViewContext.HttpContext.GetOverriddenBrowser().IsMobileDevice)
       {%>
    Mobile View |
    <% =Html.ActionLink("Desktop View", "SwitchView", "ViewSwitcher", new { Area = "", mobile = false, returnUrl = Request.Url.PathAndQuery }, new { rel = "external" }) %>
    <%}
       else
       {%>
    <% =Html.ActionLink("Mobile View", "SwitchView", "ViewSwitcher", new { Area = "", mobile = true, returnUrl = Request.Url.PathAndQuery }, new { rel = "external" }) %>
    | Desktop View
    <% } %>
</div>
<% }%>