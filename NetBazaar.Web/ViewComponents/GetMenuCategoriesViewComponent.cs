using Microsoft.AspNetCore.Mvc;
using NetBazaar.Application.Interfaces.Catalog;

namespace NetBazaar.Web.EndPoint.ViewComponents
{
    public class GetMenuCategoriesViewComponent : ViewComponent
    {
        private readonly IGetMenuItemService _menuItemService;

        public GetMenuCategoriesViewComponent(IGetMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await Task.Run(() => _menuItemService.GetMenuItems());
            return View("CatalogType", data);
        }
    }
}
