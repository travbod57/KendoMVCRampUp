using Kendo.Mvc;
using KendoMVCRampUp.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace KendoMVCRampUp.CustomKendo
{
    public class KendoFilterBuilder
    {
        private List<object> paramList = new List<object>();
        public object[] paramArray;
        private string _dynamicLinqString;
        private int _filterCount = 0;

        public KendoFilterBuilder AssessFilters(IEnumerable<IFilterDescriptor> filters, string logicalOperator = "")
        {
            StringBuilder builder = new StringBuilder();

            if (filters != null && filters.Any())
            {
                foreach (var filter in filters)
                {
                    var descriptor = filter as FilterDescriptor;
                    if (descriptor != null && !string.IsNullOrEmpty(descriptor.Value.ToString()))
                    {
                        builder.Append(string.Format(ConvertDescriptorOperator(descriptor.Operator, logicalOperator), descriptor.Member, _filterCount++));

                        if (descriptor.Member.ToLower().Contains("date"))
                        {
                            paramList.Add(ParseDateTimeValue(descriptor.Value));
                        }
                        else
                        {
                            paramList.Add(descriptor.Value);
                        }
                        _dynamicLinqString = builder.ToString();


                    }
                    else if (filter is CompositeFilterDescriptor)
                    {
                        AssessFilters(((CompositeFilterDescriptor)filter).FilterDescriptors, ((CompositeFilterDescriptor)filter).LogicalOperator.ToString());
                    }
                }
            }

            return this;
        }

        public FilterDto BuildDynamicLinqQuery()
        {
            paramArray = paramList.ToArray<object>();

            if (!String.IsNullOrEmpty(_dynamicLinqString) && _dynamicLinqString.EndsWith(" And "))
                _dynamicLinqString = _dynamicLinqString.Substring(0, _dynamicLinqString.Length - (" and ".Length));

            return new FilterDto() { IsFilterApplying = !String.IsNullOrEmpty(_dynamicLinqString), DynamicLinqString = _dynamicLinqString, Parameters = paramArray };
        }

        /// <summary>
        /// safe for Contains, StartsWith, Is Equal To, Is Not Equal To
        /// </summary>
        /// <param name="filterOperator"></param>
        /// <param name="logicalOperator"></param>
        /// <returns></returns>
        private string ConvertDescriptorOperator(FilterOperator filterOperator, string logicalOperator)
        {
            string result;

            switch (filterOperator)
            {
                case FilterOperator.IsEqualTo: result = string.Format("{{0}} == @{{1}} {0} ", logicalOperator); break;
                case FilterOperator.IsNotEqualTo: result = string.Format("{{0}} != @{{1}} {0} ", logicalOperator); break;
                case FilterOperator.IsGreaterThan: result = string.Format("{{0}} > @{{1}} {0} ", logicalOperator); break;
                case FilterOperator.IsGreaterThanOrEqualTo: result = string.Format("{{0}} >= @{{1}} {0} ", logicalOperator); break;
                case FilterOperator.IsLessThan: result = string.Format("{{0}} < @{{1}} {0} ", logicalOperator); break;
                case FilterOperator.IsLessThanOrEqualTo: result = string.Format("{{0}} <= @{{1}} {0} ", logicalOperator); break;
                default: result = string.Format("{{0}}.{0}(@{{1}}) {1} ", filterOperator.ToString(), logicalOperator); break;
            }

            return String.IsNullOrEmpty(logicalOperator) ? result.Trim() : result;
        }


        private object ParseDateTimeValue(object value)
        {
            string valAsString = value.ToString();
            //string pattern = "ddd MMM d HH:mm:ss UTC yyyy";
            //string pattern = "ddd MMM d HH:mm:ss UTCzzzzz yyyy";
            string pattern = "ddd MMM d yyyy";

            string yearPat = "[1-3][0-9]{3}";
            string dmPat = @"\D{3}\s\D{3}\s\d{1,2}";

            var regexYear = new Regex(yearPat);
            var year = regexYear.Match(valAsString).Groups[0];

            var regexdm = new Regex(dmPat);
            var dm = regexdm.Match(valAsString).Groups[0];

            string complete = string.Format("{0} {1}", dm, year);



            DateTime dt;

            if (DateTime.TryParseExact(complete, pattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                return dt;
            }
            else
                return value;
        }
    }
}