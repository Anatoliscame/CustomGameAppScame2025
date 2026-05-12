using Core.Model;
using Core.RepositoryInterface;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;

namespace Core.BusinessLogic
{
    public class MainBusinessLayer : IBusinessLayer
    {
        private readonly IRepositoryAccount _accountRepo;
        private readonly IRepositoryAcquisto _acquistoRepo;
        private readonly IRepositoryVideoGame _videogameRepo;

        public MainBusinessLayer(IRepositoryAccount accounts, IRepositoryAcquisto acquisti, IRepositoryVideoGame videogames)
        {
            _accountRepo = accounts;
            _acquistoRepo = acquisti;
            _videogameRepo = videogames;

        }

        public MainBusinessLayer(IRepositoryVideoGame videogames)
        {
            _videogameRepo = videogames;
        }

        #region VideoGame
        public List<VideoGame> FetchVideoGameB(Func<VideoGame, bool> filter = null)
        {
            throw new NotImplementedException();
        }
        public VideoGame CercaVideoGameB(string videogame)
        {
            throw new NotImplementedException();
        }
        public void DeleteVideoGame(int id)
        {
            throw new NotImplementedException();
        }

        public VideoGame GetVideoGameId(int id)
        {
            throw new NotImplementedException();
        }

        public List<VideoGame> GetVideoGames()
        {
           return _videogameRepo.GetAllVideoGame();
        }

        public VideoGame InsertVideoGameId(VideoGame v)
        {
            throw new NotImplementedException();
        }
        public VideoGame UpdateVideoGame(VideoGame v)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Acquisto

        public void VisualizzaStoricoAcquisti()
        {
            throw new NotImplementedException();
        }

        public List<Acquisto> GetAcquisto()
        {
            throw new NotImplementedException();
        }

        public Acquisto GetAcquistoId(int id)
        {
            throw new NotImplementedException();
        }

        public Acquisto InsertAcquistoId(Acquisto a)
        {
            throw new NotImplementedException();
        }

        public Acquisto UpdateAcquisto(Acquisto a)
        {
            throw new NotImplementedException();
        }

        public void DeleteAcquisto(int id)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Account

        public List<Account> FetchAccountB(Func<Account, bool> filter = null)
        {
            throw new NotImplementedException();
        }
        public List<Account> GetAccount()
        {
            throw new NotImplementedException();
        }

        public Account GetAccountId(int id)
        {
            throw new NotImplementedException();
        }

        public Account InsertAccountId(Acquisto a)
        {
            throw new NotImplementedException();
        }

        public Account UpdateAccount(Acquisto a)
        {
            throw new NotImplementedException();
        }

        public void DeleteAccount(int id)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region OrdineAcquisto

        public List<OrdineAcquisto> FetchOrdineAcquistoB(Func<OrdineAcquisto, bool> filter = null)
        {
            throw new NotImplementedException();
        }

        public List<OrdineAcquisto> GetOrdineAcquisto()
        {
            throw new NotImplementedException();
        }

        public OrdineAcquisto GetOrdineAcquistoId(int id)
        {
            throw new NotImplementedException();
        }

        public OrdineAcquisto InsertOrdineAcquistoId(Acquisto a)
        {
            throw new NotImplementedException();
        }

        public OrdineAcquisto UpdateOrdineAcquisto(Acquisto a)
        {
            throw new NotImplementedException();
        }

        public void DeleteOrdineAcquisto(int id)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region KeyGame

        public List<KeyGame> FetchKeyGameB(Func<KeyGame, bool> filter = null)
        {
            throw new NotImplementedException();
        }

        public List<KeyGame> GetKeyGame()
        {
            throw new NotImplementedException();
        }

        public KeyGame GetKeyGameId(int id)
        {
            throw new NotImplementedException();
        }

        public KeyGame InsertKeyGameId(Acquisto k)
        {
            throw new NotImplementedException();
        }

        public KeyGame UpdateKeyGame(KeyGame k)
        {
            throw new NotImplementedException();
        }

        public void DeleteKeyGame(int id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
