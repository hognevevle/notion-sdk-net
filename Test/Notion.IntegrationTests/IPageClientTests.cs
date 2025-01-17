﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Notion.Client;
using Xunit;

namespace Notion.IntegrationTests
{
    public class IPageClientTests
    {
        [Fact]
        public async Task CreateAsync_CreatesANewPage()
        {
            var options = new ClientOptions
            {
                AuthToken = Environment.GetEnvironmentVariable("NOTION_AUTH_TOKEN")
            };

            IPagesClient _client = new PagesClient(new RestClient(options));

            PagesCreateParameters pagesCreateParameters = PagesCreateParametersBuilder.Create(new DatabaseParentInput
            {
                DatabaseId = "f86f2262-0751-40f2-8f63-e3f7a3c39fcb"
            })
            .AddProperty("Name", new TitlePropertyValue
            {
                Title = new List<RichTextBase>
                {
                    new RichTextText
                    {
                        Text = new Text
                        {
                            Content = "Test Page Title"
                        }
                    }
                }
            })
            .Build();

            var page = await _client.CreateAsync(pagesCreateParameters);

            page.Should().NotBeNull();
            page.Parent.Should().BeOfType<DatabaseParent>().Which
                .DatabaseId.Should().Be("f86f2262-0751-40f2-8f63-e3f7a3c39fcb");

            page.Properties.Should().ContainKey("Name");
            page.Properties["Name"].Should().BeOfType<TitlePropertyValue>().Which
                .Title.First().PlainText.Should().Be("Test Page Title");

            await _client.UpdateAsync(page.Id, new PagesUpdateParameters
            {
                Archived = true
            });
        }
    }
}
