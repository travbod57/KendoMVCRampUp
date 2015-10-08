using Kendo.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KendoMVCRampUp.CustomKendo
{
    public class KendoSortBuilder
    {
        private string _SortString { get; set; }

        StringBuilder builder = new StringBuilder();

        public KendoSortBuilder CreateString(IEnumerable<SortDescriptor> sorts, string defaultSort)
        {
            StringBuilder builder = new StringBuilder();

            // if no sorts are applied then use the default supplied sort
            if (sorts == null || sorts.Count() == 0)
            {
                _SortString = defaultSort;
                return this;
            }

            // if there are sorts then start to build a string which concatenates them
            if (sorts.Any())
            {
                foreach (var sort in sorts)
                {
                    builder.AppendFormat("{0} {1}, ", sort.Member, sort.SortDirection.ToString() == "Ascending" ? "Asc" : "Desc");
                }
            }

            _SortString = builder.ToString();

            return this;
        }

        /// <summary>
        /// Removes any trailing commas from the sort string
        /// </summary>
        /// <returns>The sort string minus trailing commas</returns>
        public string CleanString()
        {
            if (_SortString.EndsWith(", "))
                _SortString = _SortString.Substring(0, _SortString.Length - (",".Length + 1));

            return _SortString;
        }
    }
}