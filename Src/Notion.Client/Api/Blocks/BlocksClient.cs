﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Notion.Client.ApiEndpoints;

namespace Notion.Client
{
    public class BlocksClient : IBlocksClient
    {
        private readonly IRestClient _client;

        public BlocksClient(IRestClient client)
        {
            _client = client;
        }

        public async Task<PaginatedList<Block>> RetrieveChildrenAsync(string blockId, BlocksRetrieveChildrenParameters parameters = null)
        {
            if (string.IsNullOrWhiteSpace(blockId))
            {
                throw new ArgumentNullException(nameof(blockId));
            }

            var url = BlocksApiUrls.RetrieveChildren(blockId);

            var queryParameters = (IBlocksRetrieveChildrenQueryParameters)parameters;

            var queryParams = new Dictionary<string, string>()
            {
                { "start_cursor", queryParameters?.StartCursor?.ToString() },
                { "page_size", queryParameters?.PageSize?.ToString() }
            };

            return await _client.GetAsync<PaginatedList<Block>>(url, queryParams);
        }

        public async Task<PaginatedList<Block>> AppendChildrenAsync(string blockId, BlocksAppendChildrenParameters parameters = null)
        {
            if (string.IsNullOrWhiteSpace(blockId))
            {
                throw new ArgumentNullException(nameof(blockId));
            }

            var url = BlocksApiUrls.AppendChildren(blockId);

            var body = (IBlocksAppendChildrenBodyParameters)parameters;

            return await _client.PatchAsync<PaginatedList<Block>>(url, body);
        }

        public async Task<Block> RetrieveAsync(string blockId)
        {
            if (string.IsNullOrWhiteSpace(blockId))
            {
                throw new ArgumentNullException(nameof(blockId));
            }

            var url = BlocksApiUrls.Retrieve(blockId);

            return await _client.GetAsync<Block>(url);
        }

        public async Task<Block> UpdateAsync(string blockId, IUpdateBlock updateBlock)
        {
            if (string.IsNullOrWhiteSpace(blockId))
            {
                throw new ArgumentNullException(nameof(blockId));
            }

            var url = BlocksApiUrls.Update(blockId);

            return await _client.PatchAsync<Block>(url, updateBlock);
        }

        public async Task DeleteAsync(string blockId)
        {
            if (string.IsNullOrWhiteSpace(blockId))
            {
                throw new ArgumentNullException(nameof(blockId));
            }

            var url = BlocksApiUrls.Delete(blockId);

            await _client.DeleteAsync(url);
        }
    }
}
