﻿using System.Diagnostics;

namespace Tic_Tac_again.Models.Services
{
    public class ClientService
    {
        public async Task<Client> AddClient(Client client)
        {
            //Debug.WriteLine($"CLIENT DATA: {client.Name}");
            DataSource.GetInstance()._clients.Add(client);
            //Debug.WriteLine($"CLIENT DATA FROM INSTANCE: {DataSource.GetInstance()._clients[DataSource.GetInstance()._clients.Count-1].Name}");
            return await Task.FromResult(client);
        }

        public async Task<Client?> GetClient(int id) {
            return await Task.FromResult(DataSource.GetInstance()._clients.Find(x => x.ConnId == id));
        }

        public async Task<List<Client>> GetClients()
        {
            return await Task.FromResult(DataSource.GetInstance()._clients);
        }

        public void UpdateClientState(Client client) { 
            DataSource.GetInstance()._clients.Remove(DataSource.GetInstance()._clients.First(x => x.ConnId==client.ConnId));
            DataSource.GetInstance()._clients.Add(client);
        }

    }
}