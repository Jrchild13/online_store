﻿namespace online_store.ViewModels
{
    public class ClientRowListView
    {
        public List<ClientRowView> ClientsRecords { get; set; }

        public ClientRowListView()
        {
            ClientsRecords = new List<ClientRowView>();
        }


    }
}
