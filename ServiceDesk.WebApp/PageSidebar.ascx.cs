using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using System;
using System.Linq;
using System.Web.UI;

namespace ServiceDesk.WebApp
{
    public partial class PageSidebar : UserControl
    {
        private readonly IMenuRepository _menuRepository;
        public PageSidebar(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        //private MenuService _mnuService = new MenuService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Claim.Session[Config.UserId] != null && Claim.Session[Config.RoleId] != null)
            {
                var lang = Claim.Session[Config.LanguageId] != null ? Claim.Session[Config.LanguageId].ToString() : "vi-VN";
                var userId = Claim.Session[Config.UserId].ToString();
                var roleId = Helper.ConvertToInt(Claim.Session[Config.RoleId].ToString());
                var currentUrl = Helper.CurrentPage();
                InitPageSidebar(lang, userId, roleId, currentUrl);
            }
            else
                Helper.PageRedirecting("~/Account/Login");
        }

        public void InitPageSidebar(string lang, string userId, int roleId, string url)
        {
            //var items = _mnuService.SidebarList(lang, userId, roleId).ToList();
            var items = _menuRepository.SidebarList(lang, userId, roleId).ToList();
            var parentItem = items.Where(p => p.ParentId == null);
            PlaceHolder1.Controls.Add(new LiteralControl("<ul class='nav sidebar-menu'>"));
            foreach (var m in parentItem)
            {
                var countItem = items.Count(p => p.ParentId != null && p.ParentId == m.MenuId);
                if (countItem > 0)
                {
                    //var hasParent = _security.GetMenuParentId();
                    var parentId = items.Where(y => y.Url
                            .Replace(".aspx", "") == url)
                            .Select(p => p.ParentId).FirstOrDefault();

                    if (Equals(m.MenuId, parentId))
                        PlaceHolder1.Controls.Add(new LiteralControl("<li class='active open' id='mymenu-" +
                        m.Url + "'><a href='" + ResolveUrl("~/" + m.Url) + "' " +
                        "class='menu-dropdown'><i class='menu-icon " + m.IconName + "'></i><span class='menu-text fist-text'>" +
                        m.MenuName + "</span>" + "<i class='menu-expand'></i></a>"));
                    else
                        PlaceHolder1.Controls.Add(new LiteralControl("<li id='mymenu-" + m.MenuId + "'><a href='" +
                        ResolveUrl("~/" + m.Url) + "' class='menu-dropdown'><i class='menu-icon " +
                        m.IconName + "'></i><span class='menu-text fist-text'>" + m.MenuName + "</span><i class='menu-expand'></i></a>"));

                    var subItem = items.Where(p => p.ParentId != null && p.ParentId == m.MenuId);
                    PlaceHolder1.Controls.Add(new LiteralControl("<ul class='submenu'>"));
                    foreach (var sm in subItem)
                        PlaceHolder1.Controls.Add(new LiteralControl("<li><a  runat='server' href='" +
                           ResolveUrl("~/" + sm.Url) + "'><span class='menu-text'>" + sm.MenuName + "</span></a></li>"));

                    PlaceHolder1.Controls.Add(new LiteralControl("</ul></li>"));
                }
                else
                {
                    PlaceHolder1.Controls.Add(new LiteralControl("<li id='mymenu-" + m.MenuId + "'><a href = '" +
                         ResolveUrl("~/" + m.Url) + "'>" + "<i class='menu-icon " + m.IconName +
                         "'></i><span class='menu-text fist-text'>" + m.MenuName + "</span></a></li>"));
                }
            }
            PlaceHolder1.Controls.Add(new LiteralControl("</ul>"));
        }
    }
}