using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Duende.IdentityModel.Client;

using Storm.TechTask.Api.Endpoints.Project;
using Storm.TechTask.Api.IntegrationTests.Utilities;
using Storm.TechTask.SharedKernel.Authorization;

using Xunit;
using Xunit.Abstractions;

namespace Storm.TechTask.Api.IntegrationTests.Endpoints.Projects
{
    [Collection("Sequential")]
    public class GetByIdEndpointTests : BaseEndpointFixture
    {
        public GetByIdEndpointTests(ITestOutputHelper output, CustomWebApplicationFactory factory) : base(output, factory)
        {
        }

        [Theory]
        [AllRolesExcept(AppRole.SysAdmin)]
        public async Task ReturnsProject(AppRole role)
        {
            // Arrange
            var project = await NewProject().BuildAndPersist();
            this.HttpClient.SetBearerToken(await this.TokenIssuer.GetNewToken(role));

            // Act
            var response = await this.HttpClient.GetAsync($"/Projects/{project.Id}");

            // Assert
            await response.ShouldBeSuccess().WithObjectPayload(new ProjectDto(project.Id, project.Name));
        }

        [Fact]
        public async Task ReturnsNotFound()
        {
            // Arrange
            this.HttpClient.SetBearerToken(await this.TokenIssuer.GetNewToken(AppRole.ProjectReader));

            // Act
            var response = await this.HttpClient.GetAsync("/Projects/9999999");

            // Assert
            response.ShouldBeNotFound();
        }

        [Fact]
        public async Task ShouldBeForbidden()
        {
            // Arrange
            var project = await NewProject().BuildAndPersist();
            this.HttpClient.SetBearerToken(await this.TokenIssuer.GetNewToken(AppRole.SysAdmin));

            // Act
            var response = await this.HttpClient.GetAsync($"/Projects/{project.Id}");

            // Assert
            response.ShouldBeNotAuthorised();
        }

        [Fact]
        public async Task ShouldBeUnauthorized()
        {
            // Act
            var response = await this.HttpClient.GetAsync("/Projects/1");

            // Assert
            response.ShouldBeNotAuthenticated();
        }
    }
}
