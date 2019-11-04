using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using CMS.Base;
using CMS.DancingGoat.Samples;
using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.Globalization;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;
using DancingGoat.Generator;
using DancingGoat.Generator.WebAnalytics;
using DancingGoat.Models.Generator;

namespace DancingGoat.Controllers
{
    public class GeneratorController : Controller
    {
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private readonly IList<CountryInfo> countries;

        private readonly string[] customerNames = new string[100]
        {
            "Deneen Fernald",
            "Antonio Buker",
            "Marlon Loos",
            "Nolan Steckler",
            "Johnetta Tall",
            "Florence Ramsdell",
            "Modesto Speaker",
            "Alissa Ferguson",
            "Calvin Hollier",
            "Diamond Paik",
            "Mardell Dohrmann",
            "Dinorah Clower",
            "Andrea Humbert",
            "Tyrell Galvan",
            "Yong Inskeep",
            "Tom Goldschmidt",
            "Kimbery Rincon",
            "Genaro Kneeland",
            "Roselyn Mulvey",
            "Nancee Jacobson",
            "Jaime Link",
            "Fonda Belnap",
            "Muoi Ishmael",
            "Pearlene Minjarez",
            "Eustolia Studstill",
            "Marilynn Manos",
            "Pamila Turnbow",
            "Lieselotte Satcher",
            "Sharron Mellon",
            "Bennett Heatherington",
            "Spring Hessel",
            "Lashay Blazier",
            "Veronika Lecuyer",
            "Mark Spitz",
            "Peggy Olson",
            "Tyron Bednarczyk",
            "Terese Betty",
            "Bibi Kling",
            "Bruno Spier",
            "Cristen Bussey",
            "Daine Pridemore",
            "Gerald Turpen",
            "Lela Briese",
            "Sharda Bonnie",
            "Omar Martin",
            "Marlyn Pettie",
            "Shiela Cleland",
            "Marica Granada",
            "Garland Reagan",
            "Mora Gillmore",
            "Mariana Rossow",
            "Betty Pollan",
            "Analisa Costilla",
            "Evelyn Mendez",
            "April Rubino",
            "Zachariah Roberson",
            "Sheilah Steinhauser",
            "Araceli Vallance",
            "Lashawna Weise",
            "Charline Durante",
            "Melania Nightingale",
            "Ema Stiltner",
            "Lynelle Threet",
            "Dorcas Cully",
            "Gregg Carranco",
            "Karla Heiner",
            "Judson Siegmund",
            "Alyson Oday",
            "Winston Laxton",
            "Jarod Turrentine",
            "Israel Shanklin",
            "Miquel Jorstad",
            "Brianne Darrow",
            "Tamara Rulison",
            "Elliot Rameriz",
            "Gearldine Nova",
            "Debi Fritts",
            "Leota Cape",
            "Tyler Saleem",
            "Starr Hyden",
            "Loreen Spigner",
            "Raisa Germain",
            "Grace Vigue",
            "Maryann Munsch",
            "Jason Chon",
            "Gisele Mcquillen",
            "Juliane Comeaux",
            "Willette Dodrill",
            "Sherril Weymouth",
            "Ashleigh Dearman",
            "Bret Bourne",
            "Brittney Cron",
            "Dustin Evans",
            "Barbie Dinwiddie",
            "Ricki Wiener",
            "Bess Pedretti",
            "Monica King",
            "Edgar Schuetz",
            "Jettie Boots",
            "Jefferson Hinkle"
        };

        private readonly Random rand;

        public GeneratorController()
        {
            countries = CountryInfoProvider.GetCountries().ToList();
            rand = new Random(DateTime.Now.Millisecond);
        }

        public ActionResult Index()
        {
            var model = new IndexViewModel();
            if (!MembershipContext.AuthenticatedUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Admin))
            {
                model.DisplayAccessDeniedError = true;
                return View(model);
            }

            if (SystemContext.DevelopmentMode)
            {
                model.DisplayDevelopmentErrorMessage = true;
                return View("Index", model);
            }

            return View(model);
        }


        [HttpGet]
        [ActionName("Generate")]
        public ActionResult GenerateGet()
        {
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Generate()
        {
            var model = new IndexViewModel();
            if (!MembershipContext.AuthenticatedUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.Admin))
            {
                model.DisplayAccessDeniedError = true;
                return View("Index", model);
            }

            if (SystemContext.DevelopmentMode)
            {
                model.DisplayDevelopmentErrorMessage = true;
                return View("Index", model);
            }

            try
            {
                new PersonaGenerator().Generate();
                new ContactGroupGenerator().Generate();
                new EcommerceGenerator().Generate(1);
                new WebAnalyticsGenerator().Generate(1);

                model.DisplaySuccessMessage = true;
            }
            catch (Exception e)
            {
                model.DisplayErrorMessage = true;
                model.ErrorMessage = $"Exception occured. {e.Message} {Environment.NewLine} {e.StackTrace}";
            }

            return View("Index", model);
        }
    }
}