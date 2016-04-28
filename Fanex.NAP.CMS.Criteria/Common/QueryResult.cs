namespace Fanex.NAP.CMS.Criteria.Common
{
    using System.Collections.Generic;

    public struct PagingQueryResult<T>
    {
        private readonly int _itemsSize;

        private readonly IEnumerable<T> _items;

        public PagingQueryResult(PagingCriteria criteria, IEnumerable<T> result)
        {
            _itemsSize = criteria.ItemsCount;
            _items = result;
        }

        public int ItemsCount
        {
            get { return _itemsSize; }
        }

        public IEnumerable<T> Items
        {
            get { return _items; }
        }
    }
}