using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.SiteProvider;
using CMS.WebAnalytics;

namespace DancingGoat.Generator.WebAnalytics
{
    public class WebAnalyticsDataGenerator
    {
        private const int TotalMonthsVisits = 20000;
        private const int VisitsVariance = 10;
        private const int VisitsGradient = 6;

        public static string[] StatisticCodeNames = {
            "aggviews",
            "avgtimeonpage",
            "browsertype",
            "countries",
            "crawler",
            "exitpage",
            "filedownloads",
            "flash",
            "java",
            "landingpage",
            "mobiledevice",
            "onsitesearchkeyword",
            "operatingsystem",
            "pagenotfound",
            "pageviews",
            "referringsite_direct",
            "referringsite_local",
            "referringsite_referring",
            "registereduser",
            "referringsite_search",
            "screencolor",
            "screenresolution",
            "searchkeyword",
            "silverlight",
            "urlreferrals",
            "visitfirst",
            "visitreturn"
        };

        private readonly Dictionary<string, int> _mBrowsersDataSource = new Dictionary<string, int>
        {
            {
                "IE10.0",
                7
            },
            {
                "Internet Explorer 11",
                35
            },
            {
                "Firefox36.0",
                7
            },
            {
                "Unknown",
                10
            },
            {
                "Firefox37.0",
                30
            },
            {
                "Chrome",
                50
            },
            {
                "Opera9.8",
                1
            },
            {
                "Android Browser",
                8
            },
            {
                "Safari8.0",
                20
            }
        };

        private readonly Dictionary<string, int> _mCountriesDataSource = new Dictionary<string, int>
        {
            {
                "USA",
                50
            },
            {
                "Mexico",
                3
            },
            {
                "Spain",
                3
            },
            {
                "Canada",
                12
            },
            {
                "Germany",
                3
            },
            {
                "UK",
                5
            },
            {
                "Japan",
                2
            },
            {
                "Czech Republic",
                1
            }
        };

        private readonly Dictionary<string, int> _mFileDownloadsDataSource = new Dictionary<string, int>
        {
            {
                "/Files/logo",
                10
            },
            {
                "/Files/Pertnership-program.pdf",
                6
            },
            {
                "/Files/Price-list.xlsx",
                7
            },
            {
                "/Ebooks/Everything-about-coffee.epub",
                10
            },
            {
                "/Ebooks/Everything-about-coffee.pdf",
                2
            },
            {
                "/Ebooks/How-to-be-better-barista.epub",
                9
            },
            {
                "/Ebooks/How-to-be-better-barista.pdf",
                3
            },
            {
                "/Manuals/Mazzer-Super-Jolly-DR.pdf",
                7
            },
            {
                "/Manuals/Macap-MD4.pdf",
                6
            }
        };

        private readonly Dictionary<string, int> _mMobileDevicesDataSource = new Dictionary<string, int>
        {
            {
                "Alcatel One Touch",
                4
            },
            {
                "Amazon Kindle Fire",
                6
            },
            {
                "Apple iPad 4",
                8
            },
            {
                "Apple iPhone 4",
                8
            },
            {
                "Apple iPhone 5S",
                10
            },
            {
                "Apple iPhone 6",
                9
            },
            {
                "Apple iPhone 6 Plus",
                7
            },
            {
                "BlackBerry 9900",
                3
            },
            {
                "BlackBerry Z30",
                5
            },
            {
                "Google Nexus 10",
                8
            },
            {
                "Google Nexus 9",
                7
            },
            {
                "HTC One S",
                5
            },
            {
                "HTC Sensation",
                4
            },
            {
                "HTC Thunderbolt",
                4
            },
            {
                "Huawei Ascend Y300",
                3
            },
            {
                "LG G3",
                6
            },
            {
                "LG Optimus L5",
                5
            },
            {
                "Motorola Moto E",
                4
            },
            {
                "Motorola Moto G",
                4
            },
            {
                "Nokia Lumia 920",
                4
            },
            {
                "Nokia Lumia 928",
                5
            },
            {
                "Samsung Galaxy Nexus",
                7
            },
            {
                "Samsung Galaxy Note",
                6
            },
            {
                "Samsung Galaxy S6",
                9
            },
            {
                "Sony Xperia S",
                6
            },
            {
                "Sony Xperia Z",
                7
            }
        };

        private readonly string[] _mNameDataSource = {
            "Jannine Stevens",
            "Bart Christians",
            "Kacey Chambers",
            "Mervin Victor",
            "Deb Albertson",
            "Barclay Cory",
            "Leona Graves",
            "Jordana Attaway",
            "Eveleen Foss",
            "Tessa Myers",
            "Fannie\tAlvarez",
            "Debra Klein",
            "Lula Clarke",
            "Ed Hanson",
            "Lorraine Roberson",
            "Phillip Watson",
            "Patricia Webster",
            "Jackie Schwartz",
            "Jennie Gregory",
            "Darlene Henry",
            "Santos Olson",
            "Rhonda Diaz",
            "Cheryl Garrett",
            "Wanda Ballard",
            "Robert Silva",
            "Hannah Watkins",
            "Katrina Larson",
            "Kerry Horton",
            "Jonathon Schmidt",
            "Vickie Garza",
            "Ora Barton",
            "Suzanne Patton",
            "Jared Walsh",
            "June Ray",
            "Henrietta Rodgers"
        };

        private readonly Dictionary<string, int> _mOperatingSystemDataSource = new Dictionary<string, int>
        {
            {
                "Windows",
                60
            },
            {
                "Linux",
                3
            },
            {
                "OS X",
                6
            },
            {
                "UNIX",
                5
            },
            {
                "Unknown OS",
                12
            }
        };

        private readonly Dictionary<string, int> _mReferralsDataSource = new Dictionary<string, int>
        {
            {
                "http://www.facebook.com/",
                10
            },
            {
                "http://coffeegeeks.com/articles/where-to-get-best-coffee.html",
                5
            },
            {
                "http://www.hario.com/resellers/",
                3
            },
            {
                "http://coffeedictionary.org/ethiopia/yirgacheffe/stores",
                2
            },
            {
                "http://coffeedictionary.org/colombia/carlos-imbachi/stores",
                1
            }
        };

        private readonly Dictionary<string, int> _mScreenColorDataSource = new Dictionary<string, int>
        {
            {
                "24-bit",
                50
            },
            {
                "16-bit",
                2
            },
            {
                "32-bit",
                1
            }
        };

        private readonly Dictionary<string, int> _mScreenResolutionDataSource = new Dictionary<string, int>
        {
            {
                "1366×768",
                18
            },
            {
                "1920×1080",
                7
            },
            {
                "360×640",
                6
            },
            {
                "1024×768",
                5
            },
            {
                "1280×800",
                5
            },
            {
                "1440×900",
                4
            },
            {
                "1280×1024",
                4
            },
            {
                "1600×900",
                4
            },
            {
                "320×568",
                3
            },
            {
                "1680×1050",
                2
            }
        };

        private readonly Dictionary<string, int> _mSearchEnginesDataSource = new Dictionary<string, int>
        {
            {
                "Google",
                10
            },
            {
                "Bing",
                7
            },
            {
                "Yahoo",
                4
            }
        };

        private readonly Dictionary<string, int> _mSearchKeywordDataSource = new Dictionary<string, int>
        {
            {
                "coffee",
                10
            },
            {
                "arabica",
                9
            },
            {
                "colombia",
                9
            },
            {
                "techniques",
                3
            },
            {
                "partnership",
                4
            },
            {
                "wholesale",
                3
            },
            {
                "nicaragua dipilto",
                2
            },
            {
                "panama honey",
                2
            },
            {
                "grinders",
                8
            },
            {
                "macap m2d",
                1
            },
            {
                "porlex",
                2
            },
            {
                "espresso",
                4
            },
            {
                "hario",
                5
            },
            {
                "plunger",
                6
            },
            {
                "aeropress",
                5
            },
            {
                "filter",
                7
            },
            {
                "accessories",
                7
            }
        };

        private readonly SiteInfo _mSite;

        public WebAnalyticsDataGenerator(SiteInfo site)
        {
            _mSite = site;
        }

        public void Generate()
        {
            var whereCondition = new WhereCondition().WhereEquals("StatisticsSiteID", _mSite.SiteID)
                .WhereIn("StatisticsCode", StatisticCodeNames);
            StatisticsInfoProvider.RemoveAnalyticsData(DateTimeHelper.ZERO_TIME, DateTimeHelper.ZERO_TIME,
                _mSite.SiteID, whereCondition.ToString(true));
            var action = RegisterGenerators();
            var dateTime1 = DateTime.Now.AddDays(1.0);
            var dateTime2 = dateTime1.AddDays(-35.0);
            var random = new Random();
            RegisteredUsers(random);
            var days1 = (dateTime1 - dateTime2).Days;
            for (var date = dateTime2; date < dateTime1; date = date.AddDays(1.0))
            {
                var days2 = (date - dateTime2).Days;
                var num1 = (int) (TotalMonthsVisits / days1 + Logit(random.NextDouble()) * VisitsVariance +
                                  (days2 - days1 / 2) * VisitsGradient);
                var num2 = (int) ((0.2 + random.NextDouble() * 0.1) * num1);
                var dictionary = new Dictionary<string, int>
                {
                    {
                        "en-US",
                        num1 - num2
                    },
                    {
                        "es-ES",
                        num2
                    }
                };
                action(date, dictionary, random);
                WebAnalyticsEvents.GenerateStatistics.StartEvent(date, dictionary);
            }
        }

        private Action<DateTime, Dictionary<string, int>, Random> RegisterGenerators() =>
            (Action<DateTime, Dictionary<string, int>, Random>) Delegate.Combine(
                Delegate.Combine(
                    Delegate.Combine(
                        Delegate.Combine(
                            Delegate.Combine(
                                Delegate.Combine(
                                    Delegate.Combine(
                                        Delegate.Combine(
                                            Delegate.Combine(
                                                Delegate.Combine(
                                                    Delegate.Combine(
                                                        Delegate.Combine(
                                                            Delegate.Combine(
                                                                Delegate.Combine(
                                                                    Delegate.Combine(
                                                                        Delegate.Combine(null,
                                                                            new Action<DateTime, Dictionary<string, int>
                                                                                , Random>(
                                                                                PageViewsAndAverageTimeOnPage)),
                                                                        new Action<DateTime, Dictionary<string, int>,
                                                                            Random>(Countries)),
                                                                    new Action<DateTime, Dictionary<string, int>, Random
                                                                    >(VisitorsAndTopLandingAndExitPages)),
                                                                new Action<DateTime, Dictionary<string, int>, Random>(
                                                                    OnSiteAndExternalKeywords)),
                                                            new Action<DateTime, Dictionary<string, int>, Random>(
                                                                FileDownloads)),
                                                        new Action<DateTime, Dictionary<string, int>, Random>(
                                                            AggregatedViews)),
                                                    new Action<DateTime, Dictionary<string, int>, Random>(
                                                        InvalidPages)),
                                                new Action<DateTime, Dictionary<string, int>, Random>(SearchEngines)),
                                            new Action<DateTime, Dictionary<string, int>, Random>(UrlReferrals)),
                                        new Action<DateTime, Dictionary<string, int>, Random>(MobileDevices)),
                                    new Action<DateTime, Dictionary<string, int>, Random>(BrowserTypes)),
                                new Action<DateTime, Dictionary<string, int>, Random>(FlashJavaAndSilverligthSupport)),
                            new Action<DateTime, Dictionary<string, int>, Random>(OperatingSystems)),
                        new Action<DateTime, Dictionary<string, int>, Random>(ScreenColors)),
                    new Action<DateTime, Dictionary<string, int>, Random>(ScreenResolution)),
                new Action<DateTime, Dictionary<string, int>, Random>(Conversions));

        private void Countries(DateTime date, Dictionary<string, int> visitors, Random random)
        {
            LogHit("countries", visitors.Sum(visitor => visitor.Value), 0, date,
                GetRandomDataSourceValue(_mCountriesDataSource, random), null, 0);
        }

        private void RegisteredUsers(Random random)
        {
            for (var index = 0; index < _mNameDataSource.Length; ++index)
            {
                var objectName = _mNameDataSource[index];
                LogHit("registereduser", 1, 0, DateTime.Now.AddDays(random.Next(-30, 0)), objectName, null, index + 1);
            }
        }

        private void MobileDevices(DateTime date, Dictionary<string, int> visitors, Random random)
        {
            var amountOfVisitors = GetRelativeAmountOfVisitors(0.6, 0.2, random, visitors);
            var dataSourceFrequency = GetDataSourceFrequency(_mMobileDevicesDataSource);
            var num = amountOfVisitors.Sum(visitor => visitor.Value);
            foreach (var keyValuePair in _mMobileDevicesDataSource)
            {
                LogHit("mobiledevice", (int) (dataSourceFrequency[keyValuePair.Key] * num), 0, date, keyValuePair.Key,
                    null, 0);
            }
        }

        public void UrlReferrals(DateTime date, Dictionary<string, int> visitors, Random random)
        {
            var amountOfVisitors = GetRelativeAmountOfVisitors(0.15, 0.1, random, visitors);
            var dataSourceFrequency = GetDataSourceFrequency(_mReferralsDataSource);
            var num = amountOfVisitors.Sum(visitor => visitor.Value);
            foreach (var keyValuePair in _mReferralsDataSource)
            {
                var visits = (int) (dataSourceFrequency[keyValuePair.Key] * num);
                LogHit("urlreferrals", visits, 0, date, keyValuePair.Key, null, 0);
                LogHit("referringsite_referring", visits, 0, date, URLHelper.GetDomain(keyValuePair.Key), null, 0);
            }
        }

        public void SearchEngines(DateTime date, Dictionary<string, int> visitors, Random random)
        {
            var amountOfVisitors = GetRelativeAmountOfVisitors(0.3, 0.1, random, visitors);
            var dataSourceFrequency = GetDataSourceFrequency(_mSearchEnginesDataSource);
            var num = amountOfVisitors.Sum(visitor => visitor.Value);
            foreach (var keyValuePair in _mSearchEnginesDataSource)
            {
                var visits = (int) (dataSourceFrequency[keyValuePair.Key] * num);
                LogHit("crawler", visits, 0, date, keyValuePair.Key, null, 0);
                LogHit("referringsite_search", visits, 0, date, keyValuePair.Key, null, 0);
            }
        }

        private void AggregatedViews(DateTime date, Dictionary<string, int> visitors, Random random)
        {
            foreach (var relativeAmountOfVisitor in GetRelativeAmountOfVisitors(0.05, 0.001, random, visitors))
            {
                var pair = relativeAmountOfVisitor;
                foreach (var treeNode in GetRandomDocuments(5, 3, "DancingGoatMvc.Article")
                    .Where(document => document.DocumentCulture == pair.Key))
                {
                    LogHit("aggviews", relativeAmountOfVisitor.Value, 0, date, treeNode.NodeAliasPath,
                        treeNode.DocumentCulture, treeNode.NodeID);
                }
            }
        }

        public void FileDownloads(DateTime date, Dictionary<string, int> visitors, Random random)
        {
            foreach (var relativeAmountOfVisitor in GetRelativeAmountOfVisitors(0.05, 0.001, random, visitors))
            {
                LogHit("filedownloads", relativeAmountOfVisitor.Value, 0, date,
                    GetRandomDataSourceValue(_mFileDownloadsDataSource, random), relativeAmountOfVisitor.Key, 0);
            }
        }

        private void VisitorsAndTopLandingAndExitPages(
            DateTime date,
            Dictionary<string, int> visitors,
            Random random)
        {
            Dictionary<string, int> belowThreshold;
            Dictionary<string, int> overThreshold;
            SplitVisitors(0.7, 0.1, random, visitors, out belowThreshold, out overThreshold);
            LogHit("visitfirst", belowThreshold.Sum(visitor => visitor.Value), 0, date, _mSite.SiteName, null, 0);
            LogHit("visitreturn", overThreshold.Sum(visitor => visitor.Value), 0, date, _mSite.SiteName, null, 0);
            var list1 = GetRandomDocuments(5, 3).ToList();
            foreach (var keyValuePair in belowThreshold)
            {
                var visitor = keyValuePair;
                var list2 = list1.Where(document => document.DocumentCulture == visitor.Key).ToList();
                foreach (var treeNode in list2)
                {
                    LogHit("exitpage", visitor.Value / list2.Count(), 0, date, treeNode.NodeAliasPath,
                        treeNode.DocumentCulture, treeNode.NodeID);
                    LogHit("landingpage", visitor.Value / list2.Count(), 0, date, treeNode.NodeAliasPath,
                        treeNode.DocumentCulture, treeNode.NodeID);
                    LogHit("referringsite_direct", visitor.Value / list2.Count(), 0, date, treeNode.NodeAliasPath,
                        treeNode.DocumentCulture, treeNode.NodeID);
                }
            }
        }

        private void OnSiteAndExternalKeywords(
            DateTime date,
            Dictionary<string, int> visitors,
            Random random)
        {
            var amountOfVisitors = GetRelativeAmountOfVisitors(0.1, 0.02, random, visitors);
            var input = new Dictionary<string, int>(_mSearchKeywordDataSource);
            input.Add("(not provided)", 40);
            LogHit("onsitesearchkeyword", amountOfVisitors.Sum(visitor => visitor.Value), 0, date,
                GetRandomDataSourceValue(_mSearchKeywordDataSource, random), null, 0);
            LogHit("searchkeyword", amountOfVisitors.Sum(visitor => visitor.Value), 0, date,
                GetRandomDataSourceValue(input, random), null, 0);
        }

        private void InvalidPages(DateTime date, Dictionary<string, int> visitors, Random random)
        {
            var amountOfVisitors = GetRelativeAmountOfVisitors(0.05, 0.001, random, visitors);
            var list = GetRandomDocuments(3, 1).ToList();
            for (var index = 0; index < list.Count; ++index)
            {
                var treeNode = list[index];
                var nodeAliasPath = treeNode.NodeAliasPath;
                if (nodeAliasPath.Length > 2)
                {
                    string objectName;
                    if (index < 2)
                    {
                        objectName = nodeAliasPath.Insert(random.Next(0, nodeAliasPath.Length - 1), " ");
                    }
                    else
                    {
                        var length = treeNode.NodeAliasPath.Length;
                        objectName = treeNode.NodeAliasPath.Substring(0, random.Next(2, length));
                    }

                    LogHit("pagenotfound", amountOfVisitors.Sum(visitor => visitor.Value), 0, date, objectName,
                        treeNode.DocumentCulture, 0);
                }
            }
        }

        private void PageViewsAndAverageTimeOnPage(
            DateTime date,
            Dictionary<string, int> visitors,
            Random random)
        {
            var randomDocuments = GetRandomDocuments(5, 3).ToList();
            var getVisitorsRatio = (Func<string, double>) (culture =>
                (double) visitors[culture] / randomDocuments.Where(document => document.DocumentCulture == culture)
                    .Sum(document => 1.0 / (double) document.NodeAliasPath.Length));
            var dictionary = visitors.ToDictionary(visitor => visitor.Key, visitor => getVisitorsRatio(visitor.Key));
            foreach (var treeNode in randomDocuments)
            {
                LogHit("pageviews", (int) (1.0 / treeNode.NodeAliasPath.Length * dictionary[treeNode.DocumentCulture]),
                    0, date, treeNode.NodeAliasPath, treeNode.DocumentCulture, treeNode.NodeID);
                LogHit("avgtimeonpage", 1, GetRandomValue(random, 10, 40), date, treeNode.NodeAliasPath,
                    treeNode.DocumentCulture, treeNode.NodeID);
            }
        }

        private void BrowserTypes(DateTime date, Dictionary<string, int> visitors, Random random)
        {
            LogHit("browsertype", visitors.Sum(visitor => visitor.Value), 0, date,
                GetRandomDataSourceValue(_mBrowsersDataSource, random), null, 0);
        }

        private void FlashJavaAndSilverligthSupport(
            DateTime date,
            Dictionary<string, int> visitors,
            Random random)
        {
            ThirdPartySupport(date, visitors, random, 0.8, "flash", "hf", "nf");
            ThirdPartySupport(date, visitors, random, 0.85, "silverlight", "hs", "ns");
            ThirdPartySupport(date, visitors, random, 0.95, "java", "hj", "nj");
        }

        private void ThirdPartySupport(
            DateTime date,
            Dictionary<string, int> visitors,
            Random random,
            double threshold,
            string hitType,
            string supportedValue,
            string notsupportedValue)
        {
            Dictionary<string, int> belowThreshold;
            Dictionary<string, int> overThreshold;
            SplitVisitors(threshold, 0.2, random, visitors, out belowThreshold, out overThreshold);
            LogHit(hitType, belowThreshold.Sum(visitor => visitor.Value), 0, date, notsupportedValue, null, 0);
            LogHit(hitType, overThreshold.Sum(visitor => visitor.Value), 0, date, supportedValue, null, 0);
        }

        private void OperatingSystems(DateTime date, Dictionary<string, int> visitors, Random random)
        {
            LogHit("operatingsystem", visitors.Sum(visitor => visitor.Value), 0, date,
                GetRandomDataSourceValue(_mOperatingSystemDataSource, random), null, 0);
        }

        private void ScreenColors(DateTime date, Dictionary<string, int> visitors, Random random)
        {
            LogHit("screencolor", visitors.Sum(visitor => visitor.Value), 0, date,
                GetRandomDataSourceValue(_mScreenColorDataSource, random), null, 0);
        }

        private void ScreenResolution(DateTime date, Dictionary<string, int> visitors, Random random)
        {
            LogHit("screenresolution", visitors.Sum(visitor => visitor.Value), 0, date,
                GetRandomDataSourceValue(_mScreenResolutionDataSource, random), null, 0);
        }

        private void Conversions(DateTime date, Dictionary<string, int> visitors, Random random)
        {
            var list = ConversionInfoProvider.GetConversions().OnSite(_mSite.SiteID).ToList();
            var num = visitors.Sum(visitor => visitor.Value) / list.Count;
            foreach (var conversionInfo in list)
            {
                LogHit("conversion", (int) Math.Round(0.75 * (random.NextDouble() / 2.0) * num), 0, date,
                    conversionInfo.ConversionName, null, 0);
            }
        }

        private Dictionary<string, double> GetDataSourceFrequency(
            Dictionary<string, int> input)
        {
            var whole = input.Sum(pair => pair.Value);
            return input.ToDictionary(pair => pair.Key, pair => 1.0 / (double) whole * (double) pair.Value);
        }

        private string GetRandomDataSourceValue(Dictionary<string, int> input, Random random)
        {
            var num1 = input.Sum(pair => pair.Value);
            var list = input.OrderByDescending(pair => pair.Value).ToList();
            var num2 = random.Next(1, num1 + 1);
            var num3 = 0;
            foreach (var keyValuePair in list)
            {
                num3 += keyValuePair.Value;
                if (num3 >= num2)
                {
                    return keyValuePair.Key;
                }
            }

            return null;
        }

        private void SplitVisitors(
            double threshold,
            double variance,
            Random random,
            Dictionary<string, int> visitors,
            out Dictionary<string, int> belowThreshold,
            out Dictionary<string, int> overThreshold)
        {
            belowThreshold = new Dictionary<string, int>();
            overThreshold = new Dictionary<string, int>();
            foreach (var visitor in visitors)
            {
                var num1 = threshold + (1.0 - threshold) * variance * random.NextDouble();
                var num2 = (int) (visitor.Value * num1);
                belowThreshold.Add(visitor.Key, num2);
                overThreshold.Add(visitor.Key, visitor.Value - num2);
            }
        }

        private Dictionary<string, int> GetRelativeAmountOfVisitors(
            double relativeAmount,
            double variance,
            Random random,
            Dictionary<string, int> visitors)
        {
            var ratio = relativeAmount + (1.0 - relativeAmount) * variance * random.NextDouble();
            return visitors.ToDictionary(visitor => visitor.Key, visitor => (int) ((double) visitor.Value * ratio));
        }

        private int GetRandomValue(Random random, int baseValue = 5, int variance = 10) =>
            Math.Max(1, baseValue + (int) Logit(random.NextDouble()) * variance);

        public IEnumerable<TreeNode> GetRandomDocuments(
            int numberOfEnglishDocuments,
            int numberOfSpanishDocuments,
            string documentType = null)
        {
            var func = (Func<string, int, IEnumerable<TreeNode>>) ((culture, numberOfDocuments) =>
            {
                var source = DocumentHelper.GetDocuments().Culture(culture);
                if (documentType != null)
                {
                    source = source.Type(documentType);
                }

                return source.ToList().OrderBy(n => Guid.NewGuid()).Take(numberOfDocuments);
            });
            return func("en-US", numberOfEnglishDocuments).Union(func("es-ES", numberOfSpanishDocuments));
        }

        public double Logit(double x)
        {
            x = Math.Max(1E-12, x);
            x = Math.Min(1.0 / 1.0, x);
            return Math.Log(x / (1.0 - x));
        }

        private void LogHit(
            string codeName,
            int visits,
            int value,
            DateTime logTime,
            string objectName,
            string culture,
            int objectId)
        {
            HitLogProcessor.SaveLogToDatabase(new LogRecord
            {
                CodeName = codeName,
                Hits = visits,
                Value = value,
                LogTime = logTime,
                ObjectName = objectName,
                ObjectId = objectId,
                SiteName = _mSite.SiteName,
                Culture = culture
            });
        }
    }
}