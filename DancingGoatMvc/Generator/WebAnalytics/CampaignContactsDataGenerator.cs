using System;
using System.Collections.Generic;
using System.Linq;
using CMS.ContactManagement;

namespace DancingGoat.Generator.WebAnalytics
{
    public class CampaignContactsDataGenerator
    {
        private readonly string[] _firstNames = {
            "Deneen",
            "Antonio",
            "Marlon",
            "Nolan",
            "Johnetta",
            "Florence",
            "Modesto",
            "Alissa",
            "Calvin",
            "Diamond",
            "Mardell",
            "Dinorah",
            "Andrea",
            "Tyrell",
            "Yong",
            "Tom",
            "Kimbery",
            "Genaro",
            "Roselyn",
            "Nancee",
            "Jaime",
            "Fonda",
            "Muoi",
            "Pearlene",
            "Eustolia",
            "Marilynn",
            "Pamila",
            "Lieselotte",
            "Sharron",
            "Bennett",
            "Spring",
            "Lashay",
            "Veronika",
            "Mark",
            "Peggy",
            "Tyron",
            "Terese",
            "Bibi",
            "Bruno",
            "Cristen",
            "Daine",
            "Gerald",
            "Lela",
            "Sharda",
            "Omar",
            "Marlyn",
            "Shiela",
            "Marica",
            "Garland",
            "Mora",
            "Mariana",
            "Betty",
            "Analisa",
            "Evelyn",
            "April",
            "Zachariah",
            "Sheilah",
            "Araceli",
            "Lashawna",
            "Charline",
            "Melania",
            "Ema",
            "Lynelle",
            "Dorcas",
            "Gregg",
            "Karla",
            "Judson",
            "Alyson",
            "Winston",
            "Jarod",
            "Israel",
            "Miquel",
            "Brianne",
            "Tamara",
            "Elliot",
            "Gearldine",
            "Debi",
            "Leota",
            "Tyler",
            "Starr",
            "Loreen",
            "Raisa",
            "Grace",
            "Maryann",
            "Jason",
            "Gisele",
            "Juliane",
            "Willette",
            "Sherril",
            "Ashleigh",
            "Bret",
            "Brittney",
            "Lashell",
            "Barbie",
            "Ricki",
            "Bess",
            "Lisha",
            "Edgar",
            "Jettie",
            "Jefferson"
        };

        private readonly string[] _lastNames =  {
            "Fernald",
            "Buker",
            "Loos",
            "Steckler",
            "Tall",
            "Ramsdell",
            "Speaker",
            "Ferguson",
            "Hollier",
            "Paik",
            "Dohrmann",
            "Clower",
            "Humbert",
            "Galvan",
            "Inskeep",
            "Goldschmidt",
            "Rincon",
            "Kneeland",
            "Mulvey",
            "Jacobson",
            "Link",
            "Belnap",
            "Ishmael",
            "Minjarez",
            "Studstill",
            "Manos",
            "Turnbow",
            "Satcher",
            "Mellon",
            "Heatherington",
            "Hessel",
            "Blazier",
            "Lecuyer",
            "Spitz",
            "Olson",
            "Bednarczyk",
            "Betty",
            "Kling",
            "Spier",
            "Bussey",
            "Pridemore",
            "Turpen",
            "Briese",
            "Bonnie",
            "Martin",
            "Pettie",
            "Cleland",
            "Granada",
            "Reagan",
            "Gillmore",
            "Rossow",
            "Pollan",
            "Costilla",
            "Mendez",
            "Rubino",
            "Roberson",
            "Steinhauser",
            "Vallance",
            "Weise",
            "Durante",
            "Nightingale",
            "Stiltner",
            "Threet",
            "Cully",
            "Carranco",
            "Heiner",
            "Siegmund",
            "Oday",
            "Laxton",
            "Turrentine",
            "Shanklin",
            "Jorstad",
            "Darrow",
            "Rulison",
            "Rameriz",
            "Nova",
            "Fritts",
            "Cape",
            "Saleem",
            "Hyden",
            "Spigner",
            "Germain",
            "Vigue",
            "Munsch",
            "Chon",
            "Mcquillen",
            "Comeaux",
            "Dodrill",
            "Weymouth",
            "Dearman",
            "Bourne",
            "Cron",
            "Hampson",
            "Dinwiddie",
            "Wiener",
            "Pedretti",
            "Raley",
            "Schuetz",
            "Boots",
            "Hinkle"
        };

        private const int NumberOfGeneratedContacts = 531;
        private readonly Random _random;

        private readonly string _emailAddressPostfix;

        public CampaignContactsDataGenerator(string eMailAddressPostFix)
        {
            _random = new Random();
            _emailAddressPostfix = eMailAddressPostFix;
        }

        /// <summary>Performs campaign contacts sample data generating.</summary>
        public void Generate()
        {
            DeleteOldContacts();
            GenerateContacts();
        }

        private void DeleteOldContacts()
        {
            ContactInfoProvider.GetContacts().WhereEndsWith("ContactEmail", _emailAddressPostfix).ToList()
                .ForEach(ContactInfoProvider.DeleteContactInfo);
        }

        private void GenerateContacts()
        {
            for (var index = 0; index < NumberOfGeneratedContacts; ++index)
            {
                var firstname = GetRandomName(_firstNames);
                var lastName = GetRandomName(_lastNames);

                ContactInfoProvider.SetContactInfo(new ContactInfo
                {
                    ContactFirstName = firstname,
                    ContactLastName = lastName,
                    ContactEmail = $"{firstname}.{lastName}@localhost{index}.{_emailAddressPostfix}"
                });
            }
        }

        private string GetRandomName(IReadOnlyList<string> nameArray)
        {
            var index = _random.Next(0, nameArray.Count - 1);
            return nameArray[index];
        }
    }
}