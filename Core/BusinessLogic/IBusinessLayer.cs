using Core.Model;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.BusinessLogic
{
    public interface IBusinessLayer
    {
        //User Login(string username, string password);

        // VideoGame
        List<VideoGame> FetchVideoGameB(Func<VideoGame, bool> filter = null);
        VideoGame CercaVideoGameB(string videogame);
        List<VideoGame> GetVideoGames();
        VideoGame GetVideoGameId(int id);
        VideoGame InsertVideoGameId(VideoGame v);
        VideoGame UpdateVideoGame(VideoGame v);
        void DeleteVideoGame(int id);

        // Acquisto
        void VisualizzaStoricoAcquisti();

        List<Acquisto> GetAcquisto();
        Acquisto GetAcquistoId(int id);
        Acquisto InsertAcquistoId(Acquisto a);
        Acquisto UpdateAcquisto(Acquisto a);
        void DeleteAcquisto(int id);

        // Account
        List<Account> FetchAccountB(Func<Account, bool> filter = null);
        List<Account> GetAccount();
        Account GetAccountId(int id);
        Account InsertAccountId(Acquisto a);
        Account UpdateAccount(Acquisto a);
        void DeleteAccount(int id);


        // OrdineAcquisto
        List<OrdineAcquisto> FetchOrdineAcquistoB(Func<OrdineAcquisto, bool> filter = null);
        List<OrdineAcquisto> GetOrdineAcquisto();
        OrdineAcquisto GetOrdineAcquistoId(int id);
        OrdineAcquisto InsertOrdineAcquistoId(Acquisto a);
        OrdineAcquisto UpdateOrdineAcquisto(Acquisto a);
        void DeleteOrdineAcquisto(int id);

        // KeyGame
        List<KeyGame> FetchKeyGameB(Func<KeyGame, bool> filter = null);
        List<KeyGame> GetKeyGame();
        KeyGame GetKeyGameId(int id);
        KeyGame InsertKeyGameId(Acquisto k);
        KeyGame UpdateKeyGame(KeyGame k);
        void DeleteKeyGame(int id);

    }
}
