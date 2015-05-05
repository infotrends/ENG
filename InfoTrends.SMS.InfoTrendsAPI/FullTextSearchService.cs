using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace InfoTrendsAPI
{
    public class FullTextSearchCriteria
    {
        public string SelectedField { get; set; }
        public string Group { get; set; }
        public string Logic { get; set; }
    }

    public enum MemberType { Employee, Member }

    public class FullTextSearchService
    {
        public static Dictionary<string, string> stateToAbbrev = new Dictionary<string, string>() { { "alabama", "AL" }, { "alaska", "AK" }, { "arizona", "AZ" }, { "arkansas", "AR" }, { "california", "CA" }, { "colorado", "CO" }, { "connecticut", "CT" }, { "delaware", "DE" }, { "district of columbia", "DC" }, { "florida", "FL" }, { "georgia", "GA" }, { "hawaii", "HI" }, { "idaho", "ID" }, { "illinois", "IL" }, { "indiana", "IN" }, { "iowa", "IA" }, { "kansas", "KS" }, { "kentucky", "KY" }, { "louisiana", "LA" }, { "maine", "ME" }, { "maryland", "MD" }, { "massachusetts", "MA" }, { "michigan", "MI" }, { "minnesota", "MN" }, { "mississippi", "MS" }, { "missouri", "MO" }, { "montana", "MT" }, { "nebraska", "NE" }, { "nevada", "NV" }, { "newhampshire", "NH" }, { "newjersey", "NJ" }, { "newmexico", "NM" }, { "newyork", "NY" }, { "northcarolina", "NC" }, { "northdakota", "ND" }, { "ohio", "OH" }, { "oklahoma", "OK" }, { "oregon", "OR" }, { "pennsylvania", "PA" }, { "rhodeisland", "RI" }, { "southcarolina", "SC" }, { "southdakota", "SD" }, { "tennessee", "TN" }, { "texas", "TX" }, { "utah", "UT" }, { "vermont", "VT" }, { "virginia", "VA" }, { "washington", "WA" }, { "westvirginia", "WV" }, { "wisconsin", "WI" }, { "wyoming", "WY" } };
        public static IEnumerable<RegionInfo> regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(x => new RegionInfo(x.LCID));

        //brand
        public List<string> Brand { get; set; }

        //Company Info
        public List<string> Channel { get; set; }
        public List<string> Employees { get; set; }
        public List<string> Revenue { get; set; }
        public List<string> Location { get; set; }
        public List<string> ProfessionalServices { get; set; }
        public List<string> OnlineServices { get; set; }
        public List<string> Markets { get; set; }
        public List<string> States { get; set; }


        //Product
        string Duplicator = "";
        string Fax = "";
        string WFP = "";
        string DocumentSoftware = "";
        string OfiicePrinterMFP = "";
        string ProductPrinterMFP = "";
        string Scanner = "";
        string ProductColor = "";
        string ProductFeedType = "";
        string OfficeColor = "";
        string OfficeEnvironement = "";
        string OfficePageSize = "";

        List<string> ListDocumentSoftware;
        List<string> ListScanner;
        List<string> ListProductColor;
        List<string> ListProductFeedType;
        List<string> ListOfficeColor;
        List<string> ListOfficeEnvironement;
        List<string> ListOfficePageSize;




        //Country
        public List<string> Country { get; set; }

        /// <summary>
        /// ParseJson string to lists
        /// </summary>
        /// <param name="js"></param>
        public void ParseJson(string js)
        {
            // Processing the string to be standard query

            // 1. remove the letter '/'
            js = js.Replace("/", string.Empty);
            // 2. replace space
            js = js.Replace(" ", string.Empty);
            // 3. replace letter '_'
            js = js.Replace("_", string.Empty);
            // 4. replace letter '('
            js = js.Replace("(", string.Empty);
            // 4. replace letter '('
            js = js.Replace(")", string.Empty);

            var l = JsonConvert.DeserializeObject<List<string>>(js);

            if (l.Any(c => c.Equals("Root|Products|Fax")))
                Fax = "Fax";
            if (l.Any(c => c.Contains("Root|Products|Duplicator")))
                Duplicator = "Duplicator";
            if (l.Any(c => c.Contains("Root|Products|WFP")))
                WFP = "WFP";

            if (l.Any(c => c.Contains("Root|Brands")))
            {
                var tempList = l.Where(c => c.Contains("Root|Brands")).Select(c => c.Substring(c.LastIndexOf("|") + 1)).ToList<string>();
                if (Brand == null)
                    Brand = tempList;
                else
                    Brand.AddRange(tempList);
                Brand = Brand.Distinct().ToList();
            }

            if (l.Any(c => c.Contains("Root|Products|OfficePrinterMFP")))
            {
                OfiicePrinterMFP = "OfficePrinterMFP";
            }

            if (l.Any(c => c.Contains("Root|Products|OfficePrinterMFP|ColorType")))
            {
                OfficeColor = "ColorType";
                var tempList = l.Where(c => c.Contains("Root|Products|OfficePrinterMFP|ColorType")).Select(c => c.Substring(c.LastIndexOf("|") + 1)).ToList<string>();
                if (ListOfficeColor == null)
                    ListOfficeColor = tempList;
                else
                    ListOfficeColor.AddRange(tempList);
                ListOfficeColor = ListOfficeColor.Distinct().ToList();
            }

            if (l.Any(c => c.Contains("Root|Products|OfficePrinterMFP|PageSize")))
            {
                OfficePageSize = "PageSize";
                var tempList = l.Where(c => c.Contains("Root|Products|OfficePrinterMFP|PageSize")).Select(c => c.Substring(c.LastIndexOf("|") + 1)).ToList<string>();
                if (ListOfficePageSize == null)
                    ListOfficePageSize = tempList;
                else
                    ListOfficePageSize.AddRange(tempList);
                ListOfficePageSize = ListOfficePageSize.Distinct().ToList();
            }

            if (l.Any(c => c.Contains("Root|Products|OfficePrinterMFP|Environment")))
            {
                OfficeEnvironement = "Environment";
                var tempList = l.Where(c => c.Contains("Root|Products|OfficePrinterMFP|Environment")).Select(c => c.Substring(c.LastIndexOf("|") + 1)).ToList<string>();
                if (ListOfficeEnvironement == null)
                    ListOfficeEnvironement = tempList;
                else
                    ListOfficeEnvironement.AddRange(tempList);
                ListOfficeEnvironement = ListOfficeEnvironement.Distinct().ToList();
            }

            if (l.Any(c => c.Contains("Root|Products|DocumentSoftware")))
            {
                DocumentSoftware = "DocumentSoftware";
                var tempList = l.Where(c => c.Contains("Root|Products|DocumentSoftware|")).Select(c => c.Substring(c.LastIndexOf("|") + 1)).ToList<string>();
                if (ListDocumentSoftware == null)
                    ListDocumentSoftware = tempList;
                else
                    ListDocumentSoftware.AddRange(tempList);
                ListDocumentSoftware = ListDocumentSoftware.Distinct().ToList();
            }

            if (l.Any(c => c.Contains("Root|Products|ProductionPrinterMFP")))
            {
                ProductPrinterMFP = "ProductionPrinterMFP";
            }

            if (l.Any(c => c.Contains("Root|Products|ProductionPrinterMFP|ColorType")))
            {
                ProductColor = "ColorType";
                var tempList = l.Where(c => c.Contains("Root|Products|ProductionPrinterMFP|ColorType")).Select(c => c.Substring(c.LastIndexOf("|") + 1)).ToList<string>();
                if (ListProductColor == null)
                    ListProductColor = tempList;
                else
                    ListProductColor.AddRange(tempList);
                ListProductColor = ListProductColor.Distinct().ToList();
            }

            if (l.Any(c => c.Contains("Root|Products|ProductionPrinterMFP|FeedType")))
            {
                ProductFeedType = "ColorType";
                var tempList = l.Where(c => c.Contains("Root|Products|ProductionPrinterMFP|FeedType")).Select(c => c.Substring(c.LastIndexOf("|") + 1)).ToList<string>();
                if (ListProductFeedType == null)
                    ListProductFeedType = tempList;
                else
                    ListProductFeedType.AddRange(tempList);
                ListProductFeedType = ListProductFeedType.Distinct().ToList();
            }

            if (l.Any(c => c.Contains("Root|Products|Scanner")))
            {
                Scanner = "Scanner";
                var tempList = l.Where(c => c.Contains("Root|Products|Scanner|")).Select(c => c.Substring(c.LastIndexOf("|") + 1)).ToList<string>();
                if (ListScanner == null)
                    ListScanner = tempList;
                else
                    ListScanner.AddRange(tempList);
                ListScanner = ListScanner.Distinct().ToList();
            }

            if (l.Any(c => c.Contains("Root|CompanyInfo|Channel")))
            {
                var tempList = l.Where(c => c.Contains("Root|CompanyInfo|Channel")).Select(c => c.Substring(c.LastIndexOf("|") + 1)).ToList<string>();
                if (Channel == null)
                    Channel = tempList;
                else
                    Channel.AddRange(tempList);
                Channel = Channel.Distinct().ToList();
            }

            if (l.Any(c => c.Contains("Root|CompanyInfo|Employees")))
            {
                var tempList = l.Where(c => c.Contains("Root|CompanyInfo|Employees")).Select(c => c.Substring(c.LastIndexOf("|") + 1)).ToList<string>();
                if (Employees == null)
                    Employees = tempList;
                else
                    Employees.AddRange(tempList);
                Employees = Employees.Distinct().ToList();
            }

            if (l.Any(c => c.Contains("Root|CompanyInfo|Revenue")))
            {
                var tempList = l.Where(c => c.Contains("Root|CompanyInfo|Revenue")).Select(c => c.Substring(c.LastIndexOf("|") + 1)).ToList<string>();
                if (Revenue == null)
                    Revenue = tempList;
                else
                    Revenue.AddRange(tempList);
                Revenue = Revenue.Distinct().ToList();
            }

            if (l.Any(c => c.Contains("Root|CompanyInfo|Location")))
            {
                var tempList = l.Where(c => c.Contains("Root|CompanyInfo|Location")).Select(c => c.Substring(c.LastIndexOf("|") + 1)).ToList<string>();
                if (Location == null)
                    Location = tempList;
                else
                    Location.AddRange(tempList);
                Location = Location.Distinct().ToList();
            }

            if (l.Any(c => c.Contains("Root|CompanyInfo|ProfessionalServices")))
            {
                var tempList = l.Where(c => c.Contains("Root|CompanyInfo|ProfessionalServices")).Select(c => c.Substring(c.LastIndexOf("|") + 1)).ToList<string>();
                if (ProfessionalServices == null)
                    ProfessionalServices = tempList;
                else
                    ProfessionalServices.AddRange(tempList);
                ProfessionalServices = ProfessionalServices.Distinct().ToList();
            }

            if (l.Any(c => c.Contains("Root|CompanyInfo|OnlineServices")))
            {
                var tempList = l.Where(c => c.Contains("Root|CompanyInfo|OnlineServices")).Select(c => c.Substring(c.LastIndexOf("|") + 1)).ToList<string>();
                if (OnlineServices == null)
                    OnlineServices = tempList;
                else
                    OnlineServices.AddRange(tempList);
                OnlineServices = OnlineServices.Distinct().ToList();
            }

            if (l.Any(c => c.Contains("Root|CompanyInfo|Markets")))
            {
                var tempList = l.Where(c => c.Contains("Root|CompanyInfo|Markets")).Select(c => c.Substring(c.LastIndexOf("|") + 1)).ToList<string>();
                if (Markets == null)
                    Markets = tempList;
                else
                    Markets.AddRange(tempList);
                Markets = Markets.Distinct().ToList();
            }

            if (l.Any(c => c.Contains("Root|CompanyInfo|States")))
            {
                var tempList = l.Where(c => c.Contains("Root|CompanyInfo|States")).Select(c => c.Substring(c.LastIndexOf("|") + 1)).ToList<string>();
                if (States == null)
                    States = tempList;
                else
                    States.AddRange(tempList);
                States = States.Distinct().Select(c => FullTextSearchService.GetTwoLetterStateCode(c)).ToList();
            }

            if (l.Any(c => c.Contains("Root|Country")))
            {
                var tempList = l.Where(c => c.Contains("Root|Country")).Select(c => c.Substring(c.LastIndexOf("|") + 1)).ToList<string>();
                if (Country == null)
                    Country = tempList;
                else
                    Country.AddRange(tempList);
                Country = Country.Distinct().Select(c => FullTextSearchService.GetThreeLetterCountryCode(c)).ToList();
            }

        }

        /// <summary>
        /// Generate query for each criterrial on the UI
        /// </summary>
        /// <returns></returns>
        public string GenFullTextSearchQuery()
        {
            var rValue = string.Empty;

            var productQ = GenerateProductQuery();

            var companyQ = GenerateCompanyInfoQuery();

            var countryQ = GenerateCountryQuery();

            var l = new List<string>();
            if (!string.IsNullOrWhiteSpace(productQ))
                l.Add(productQ);
            if (!string.IsNullOrWhiteSpace(companyQ))
                l.Add(companyQ);
            if (!string.IsNullOrWhiteSpace(countryQ))
                l.Add(countryQ);

            return string.Join(" AND ", l);
        }

        /// <summary>
        /// First version function, we dont use it anymore. Use GetBeautyListStringNew insted
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public List<string> GetBeautyListString(string s)
        {
            var rList = new List<string>();
            var l = ((Newtonsoft.Json.Linq.JContainer)(JsonConvert.DeserializeObject(s)));

            foreach (var i in l)
            {
                rList.Add(i[1].ToString());
            }
            return rList;
        }

        /// <summary>
        /// Convert raw data that user filter to list that application can consume. Move to utility static class later
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static List<FullTextSearchCriteria> GetBeautyListStringNew(string s)
        {
            var rList = new List<FullTextSearchCriteria>();
            if (string.IsNullOrWhiteSpace(s)) return rList;
            var l = ((Newtonsoft.Json.Linq.JContainer)(JsonConvert.DeserializeObject(s)));
            foreach (var i in l)
            {
                if (!string.IsNullOrWhiteSpace(i[1].ToString()))
                {
                    var c = new FullTextSearchCriteria { Logic = i[0].ToString(), SelectedField = i[1].ToString(), Group = i.Count() > 2 ? i[2].ToString() : string.Empty };
                    rList.Add(c);
                }
            }
            return rList;
        }

        /// <summary>
        /// Move to Utility static class later
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns></returns>
        public static string GetThreeLetterCountryCode(string countryName)
        {
            if (countryName.Length < 4)
                return countryName;

            var englishRegion = regions.FirstOrDefault(region => region.EnglishName.ToLower().Replace(" ", string.Empty).Contains(countryName.ToLower()));

            return englishRegion.ThreeLetterISORegionName;
        }

        public static string GetTwoLetterStateCode(string stateName)
        {
            if (stateName.Length < 3)
                return stateName;
            return stateToAbbrev[stateName.ToLower()];
        }


        public static string GenerateFilter(string criterias)
        {
            var l = FullTextSearchService.GetBeautyListStringNew(criterias);
            var preG = string.Empty;//prev group

            //Act , gernerate query for each criterria and them handle group.
            var filter = string.Empty;
            foreach (var i in l)
            {
                var svr = new FullTextSearchService();
                svr.ParseJson(i.SelectedField);
                if (!string.IsNullOrWhiteSpace(preG) && i.Group != preG)
                    filter = filter.TrimEnd() + ")";

                var mainQ = svr.GenFullTextSearchQuery();

                if (!string.IsNullOrWhiteSpace(mainQ))
                    mainQ = string.Format("({0})", mainQ);

                if (!string.IsNullOrWhiteSpace(i.Group) && i.Group != preG)
                    filter += string.Format(" {0} ({1}", i.Logic, mainQ);
                else
                    filter += string.Format(" {0} {1}", i.Logic, mainQ);
                preG = i.Group;
            }
            if (l.Count > 0 && !string.IsNullOrWhiteSpace(l.Last().Group))
                filter = filter.TrimEnd() + ")";
            filter = filter.Trim();

            //Handle old data
            if (!string.IsNullOrWhiteSpace(filter))
                filter = filter.Substring(filter.IndexOf("("));
            else
                filter = "(sp)";

            return filter;
        }

        public static string GenerateCountryFilter(string limitCountries, MemberType memberType, string alias)
        {
            if (memberType == MemberType.Employee || string.IsNullOrWhiteSpace(limitCountries))
                return string.Empty;//all country
            return string.Format("AND ({0})", limitCountries.ToLower().Replace("country", alias + "country"));
        }


        private void setupList()
        {
            if (ListProductColor == null) ListProductColor = new List<string>() { "?" };
            if (ListProductFeedType == null) ListProductFeedType = new List<string>() { "?" };
            if (ListOfficeColor == null) ListOfficeColor = new List<string>() { "?" };
            if (ListOfficeEnvironement == null) ListOfficeEnvironement = new List<string>() { "?" };
            if (ListOfficePageSize == null) ListOfficePageSize = new List<string>() { "?" };
        }
        private string GenerateProductQuery()
        {
            setupList();

            var documentSoftwareQuery = string.Empty;
            if (ListDocumentSoftware != null && ListDocumentSoftware.Count > 0 & ListDocumentSoftware.Count < 4)
            {
                documentSoftwareQuery = GenerateSimpleBrandProducts(ListDocumentSoftware, DocumentSoftware);
            }
            if (string.IsNullOrWhiteSpace(documentSoftwareQuery) && !string.IsNullOrWhiteSpace(DocumentSoftware))
            {
                documentSoftwareQuery = GenerateSimpleBrandProducts(DocumentSoftware);
            }

            var scannerQuery = string.Empty;
            if (ListScanner != null && ListScanner.Count > 0 & ListScanner.Count < 4)
            {
                scannerQuery = GenerateSimpleBrandProducts(ListScanner,Scanner);
                
            }
            if (string.IsNullOrWhiteSpace(scannerQuery) && !string.IsNullOrWhiteSpace(Scanner))
            {                
                    scannerQuery = GenerateSimpleBrandProducts(Scanner);
            }


            var officePrinterMFPQuery = string.Empty;

            if (!string.IsNullOrEmpty(OfiicePrinterMFP))
            {
                var lst = from l1 in ListOfficeColor
                          from l2 in ListOfficeEnvironement
                          from l3 in ListOfficePageSize
                          select new { l1, l2, l3 };
                var newlst = new List<string>();
                foreach (var item in lst)
                {
                    var tmpList = new List<string>();
                    if (item.l1 != "?")
                        tmpList.Add(item.l1);
                    if (item.l2 != "?")
                        tmpList.Add(item.l2);
                    if (item.l3 != "?")
                        tmpList.Add(item.l3);
                    if (tmpList.Count > 0)
                        newlst.Add(string.Join("~", tmpList));
                }

                // if select all child of node
                if (newlst.Count == 12)
                {
                    newlst = new List<string>();
                }
                officePrinterMFPQuery = GenerateSimpleBrandProducts(newlst, OfiicePrinterMFP);                  
            }


            var productPrinterMFPQuery = string.Empty;

            if (!string.IsNullOrWhiteSpace(ProductPrinterMFP))
            {
                var lst = from l1 in ListProductColor
                          from l2 in ListProductFeedType
                          select new { l1, l2 };
                var newlst = new List<string>();

                foreach (var item in lst)
                {
                    var tmpList = new List<string>();
                    if (item.l1 != "?")
                        tmpList.Add(item.l1);
                    if (item.l2 != "?")
                        tmpList.Add(item.l2);
                    if (tmpList.Count > 0)
                        newlst.Add(string.Join("~", tmpList));
                }

                // If all child of parent checked
                if (newlst.Count == 4)
                {
                    newlst = new List<string>();
                }
                productPrinterMFPQuery = GenerateSimpleBrandProducts(newlst, ProductPrinterMFP);                
            }


            var duplicatorQuery = GenerateSimpleBrandProducts(Duplicator);
            var faxQuery = GenerateSimpleBrandProducts(Fax);
            var wfpQuery = GenerateSimpleBrandProducts(WFP);

            var products = new List<string>();

            if (!string.IsNullOrWhiteSpace(documentSoftwareQuery))
                products.Add(documentSoftwareQuery);
            if (!string.IsNullOrWhiteSpace(duplicatorQuery))
                products.Add(duplicatorQuery);
            if (!string.IsNullOrWhiteSpace(faxQuery))
                products.Add(faxQuery);
            if (!string.IsNullOrWhiteSpace(officePrinterMFPQuery))
                products.Add(officePrinterMFPQuery);
            if (!string.IsNullOrWhiteSpace(productPrinterMFPQuery))
                products.Add(productPrinterMFPQuery);
            if (!string.IsNullOrWhiteSpace(scannerQuery))
                products.Add(scannerQuery);
            if (!string.IsNullOrWhiteSpace(wfpQuery))
                products.Add(wfpQuery);

            var query = string.Empty;

            if (products.Count > 0)
            {
                query = string.Join(" AND ", products);

                if (products.Count > 1)
                {
                    query = "(" + query + ")";
                }
            }


            if (string.IsNullOrWhiteSpace(query) && Brand!=null && Brand.Count > 0)
            {
                query = string.Join(" OR ", Brand);

                if (Brand.Count > 1)
                {
                    query = "(" + query + ")";
                }
            }

            return query;
        }

        private string GenerateCompanyInfoQuery()
        {
            var l = new List<string>();



            var ChanelQuery = GenerateSimpleQuery(Channel);
            var EmployeesQuery = GenerateSimpleQuery(Employees);
            var RevenueQuery = GenerateSimpleQuery(Revenue);
            var LocationQuery = GenerateSimpleQuery(Location);
            var ProfessionalServicesQuery = GenerateSimpleQuery(ProfessionalServices);
            var OnlineServicesQuery = GenerateSimpleQuery(OnlineServices);
            var MarketsQuery = GenerateSimpleQuery(Markets);
            var StatesQuery = GenerateSimpleQuery(States);

            if (!string.IsNullOrWhiteSpace(ChanelQuery))
                l.Add(ChanelQuery);
            if (!string.IsNullOrWhiteSpace(EmployeesQuery))
                l.Add(EmployeesQuery);
            if (!string.IsNullOrWhiteSpace(RevenueQuery))
                l.Add(RevenueQuery);
            if (!string.IsNullOrWhiteSpace(LocationQuery))
                l.Add(LocationQuery);
            if (!string.IsNullOrWhiteSpace(ProfessionalServicesQuery))
                l.Add(ProfessionalServicesQuery);
            if (!string.IsNullOrWhiteSpace(OnlineServicesQuery))
                l.Add(OnlineServicesQuery);
            if (!string.IsNullOrWhiteSpace(MarketsQuery))
                l.Add(MarketsQuery);
            if (!string.IsNullOrWhiteSpace(StatesQuery))
                l.Add(StatesQuery);

            return string.Join(" AND ", l);
        }

        private string GenerateCountryQuery()
        {
            return GenerateSimpleQuery(Country);
        }

        private string GenerateSimpleQuery(List<string> list)
        {
            if (list == null || list.Count == 0)
                return string.Empty;
            var rValue = string.Empty;

            rValue = string.Join(" OR ", list);
            if (list.Count > 1)
                rValue = string.Format("({0})", rValue);
            return rValue;
        }

        private string GenerateSimpleBrandProducts(string productProperty)
        {
            if (string.IsNullOrWhiteSpace(productProperty))
                return string.Empty;
            else
            {
                if (Brand == null)
                    return productProperty;
                else if (Brand.Count == 1)
                    return Brand[0] + "~" + productProperty;
                else
                    return string.Format("({0})",string.Join(" OR ", Brand.Select(c => c + "~" + productProperty)));
            }
        }

        private string GenerateSimpleBrandProducts(List<string> productProperties, string parentProperty)
        {
            if (productProperties.Count == 0)
                productProperties.Add(parentProperty);
            else
                productProperties = productProperties.Select(c => parentProperty + "~" + c).ToList();


            if (Brand == null || Brand.Count == 0)
            {
                return productProperties.Count == 1 ? productProperties[0] : string.Format("({0})",string.Join(" OR ", productProperties));
            }
            else
            {
                    var tmpList = from l0 in Brand
                                  from l1 in productProperties
                                  select new { data = l0 + "~" + l1 };
                return tmpList.Count() == 1? tmpList.First().data : string.Format("({0})",string.Join(" OR ", tmpList.Select(c => c.data)));
            }
        }
    }


}
