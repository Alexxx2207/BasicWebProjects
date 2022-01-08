using BattleCards.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BattleCards.Services
{
    public interface ICardService
    {
        Task AddCard(string name, string imageUrl, string keyword, int attack, int health, string description);

        IEnumerable<CardViewModel> AllCards();

        IEnumerable<CardViewModel> MyCollection(string userId);

        Task AddCardToCollection(string userId, int cardId);

        Task RemoveCardFromCollection(string userId, int cardId);
    }
}
