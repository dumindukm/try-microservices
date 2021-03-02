using ConfernceFaas.Models;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConfernceFaas.Services
{
    public static class ConferenceDbService
    {
        static ConferenceDbService()
        {
            cosmosClient = new CosmosClient(EndpointUrl, PrimaryKey);
            container = cosmosClient.GetContainer(databaseId, containerId);
        }
        /// The Azure Cosmos DB endpoint for running this GetStarted sample.
        private static string EndpointUrl = "https://localhost:8081";// Environment.GetEnvironmentVariable("EndpointUrl");

        /// The primary key for the Azure DocumentDB account.
        private static string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";// Environment.GetEnvironmentVariable("PrimaryKey");

        // The Cosmos client instance
        private static CosmosClient cosmosClient;

        // The database we will create
        private static Database database;

        // The container we will create.
        private static Container container;

        // The name of the database and container we will create
        private static string databaseId = "ConferenceDB";
        private static string containerId = "Conferences";

        public static async Task<List<Conference>> QueryConferencesAsync()
        {
            var sqlQueryText = "SELECT * FROM c ";

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<Conference> queryResultSetIterator = container.GetItemQueryIterator<Conference>(queryDefinition);

            List<Conference> conferences = new List<Conference>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Conference> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Conference conf in currentResultSet)
                {
                    conferences.Add(conf);

                }
            }

            return conferences;
        }


    }
}
