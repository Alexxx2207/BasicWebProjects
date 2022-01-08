using BattleCards.Data;
using BattleCards.Services;
using BattleCards.ViewModels;
using SIS.HTTP;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCards.Controllers
{
    public class CardsController : Controller
    {
        private ICardService cardService;


        public CardsController(ICardService cardService)
        {
            this.cardService = cardService;
        }

        public HttpResponse All()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View(cardService.AllCards());
        }

        public HttpResponse Add()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(CardInputViewModel cardInputViewModel)
        {
            if (string.IsNullOrWhiteSpace(cardInputViewModel.Name) || cardInputViewModel.Name.Length < 5 || cardInputViewModel.Name.Length > 15)
            {
                return this.Error("Name must be between 5 and 15 characters");
            }

            if(string.IsNullOrWhiteSpace(cardInputViewModel.Image))
                return this.Error("Invalid image");

            if(string.IsNullOrWhiteSpace(cardInputViewModel.Keyword))
                return this.Error("Invalid keyword");

            if (string.IsNullOrWhiteSpace(cardInputViewModel.Attack) || !int.TryParse(cardInputViewModel.Attack, out int att) || att < 0)
            {
                return this.Error("Invalid attack");
            }

            if (string.IsNullOrWhiteSpace(cardInputViewModel.Health) || !int.TryParse(cardInputViewModel.Health, out int heal) || heal < 0)
            {
                return this.Error("Invalid health");
            }

            if (string.IsNullOrWhiteSpace(cardInputViewModel.Description) || cardInputViewModel.Description.Length > 200)
            {
                return this.Error("Invalid description. It's length must be between 0 and 200 characters");
            }

            cardService.AddCard(cardInputViewModel.Name, cardInputViewModel.Image, cardInputViewModel.Keyword, att, heal, cardInputViewModel.Description);

            return this.Redirect("/Cards/All");
        }

        public HttpResponse Collection()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View(cardService.MyCollection(this.Request.SessionData["UserId"]));
        }

        public HttpResponse AddToCollection(int cardId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (cardId == 0)
            {
                return this.Error("Missing Card");
            }

            cardService.AddCardToCollection(this.Request.SessionData["UserId"], cardId);

            return this.Redirect("/Cards/All");
        }

        public HttpResponse RemoveFromCollection(int cardId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (cardId == 0)
            {
                return this.Error("Missing Card");
            }

            cardService.RemoveCardFromCollection(this.Request.SessionData["UserId"], cardId);

            return this.Redirect("/Cards/Collection");
        }
    }
}
