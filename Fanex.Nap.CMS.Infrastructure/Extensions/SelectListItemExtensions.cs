namespace Fanex.Nap.CMS.Infrastructure.Extensions
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure.Resources;

    public static class SelectListItemExtensions
    {
        public static IEnumerable<SelectListItem> InsertAllItem(this IEnumerable<SelectListItem> collection)
            => collection.InsertItem(Labels.All);

        public static IEnumerable<SelectListItem> InsertPleaseSelectItem(this IEnumerable<SelectListItem> collection)
            => collection.InsertItem(Labels.PleaseSelect);

        public static IEnumerable<SelectListItem> InsertNoneItem(this IEnumerable<SelectListItem> collection)
            => collection.InsertItem(Labels.None);
        
        public static IEnumerable<SelectListItem> SetDefaultValue(this IEnumerable<SelectListItem> collection, string selectedValue)
        {
            foreach (var item in collection)
            {
                item.Selected = item.Value == selectedValue;

                yield return item;
            }
        }

        private static IEnumerable<SelectListItem> InsertItem(
            this IEnumerable<SelectListItem> collection,
            string firstItem)
        {
            yield return ToSelectListItem(firstItem);

            foreach (var item in collection)
            {
                yield return item;
            }
        }

        private static SelectListItem ToSelectListItem(string firstItem)
            => new SelectListItem { Text = firstItem, Value = "0" };
    }
}
