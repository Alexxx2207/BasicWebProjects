using BattleCards.Data;
using BattleCards.Models;
using BattleCards.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCards.Services
{
    public class CardService : ICardService
    {
        private ApplicationDbContext db;

        public CardService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task AddCard(string name, string imageUrl, string keyword, int attack, int health, string description)
        {
            var card = new Card()
            {
                Name = name,
                ImageUrl = imageUrl,
                Keyword = keyword,
                Attack = attack,
                Health = health,
                Description = description
            };

            db.Cards.Add(card);
            await db.SaveChangesAsync();
        }

        public async Task AddCardToCollection(string userId, int cardId)
        {
            if(this.db.Cards.Any(uc => uc.Id == cardId))
                this.db.UsersCards.Add(new UserCard
                { 
                    UserId = userId,
                    CardId = cardId
                });

            await db.SaveChangesAsync();
        }

        public IEnumerable<CardViewModel> AllCards()
        {
            var cards = this.db.Cards
                .Select(c => new CardViewModel
                { 
                    Id = c.Id,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl,
                    Attack = c.Attack,
                    Health = c.Health,
                    Keyword = c.Keyword,
                    Description = c.Description
                }
                )
                .ToList();

            return cards;

        }

        public IEnumerable<CardViewModel> MyCollection(string userId)
        {
            var cards = this.db.UsersCards
                .Where(uc => uc.UserId == userId)
                .Select(uc => uc.Card)
                .Select(c => new CardViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl,
                    Attack = c.Attack,
                    Health = c.Health,
                    Keyword = c.Keyword,
                    Description = c.Description
                }
                )
                .ToList();

            return cards;
        }

        public async Task RemoveCardFromCollection(string userId, int cardId)
        {
            if (this.db.Cards.Any(uc => uc.Id == cardId))
            {
                var userCard = this.db.UsersCards
                    .FirstOrDefault(uc => uc.CardId == cardId && uc.UserId == userId);

                this.db.UsersCards.Remove(userCard);
            }

            await db.SaveChangesAsync();
        }
    }
}
