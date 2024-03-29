﻿using System.Diagnostics;

namespace Tic_Tac_again.Models.Services
{
    public class ClientService
    {
        public Client AddClient(Client client)
        {
            DataSource.GetInstance()._clients.Add(client);
            return client;
        }

        public Client? GetClient(int id) {
            return DataSource.GetInstance()._clients.Find(x => x.ConnId == id);
        }

        public List<Client> GetClients()
        {
            return DataSource.GetInstance()._clients;
        }

        public void UpdateClientState(Client client) { 
            DataSource.GetInstance()._clients.Remove(DataSource.GetInstance()._clients.First(x => x.ConnId==client.ConnId));
            DataSource.GetInstance()._clients.Add(client);
        }

        public void RemoveClient(Client client)
        {
            DataSource.GetInstance()._clients.Remove(client);
        }

    }
}
