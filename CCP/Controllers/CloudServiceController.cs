using CCP.API.Dtos;

using Microsoft.AspNetCore.Mvc;

namespace CCP.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CloudServiceController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<CloudServiceDto> GetCloudServices()
        {
            var services = new List<CloudServiceDto>
            {
                new CloudServiceDto { Id = 1, Name = "Microsoft Office 365", Description = "Office suite including Word, Excel, PowerPoint, and more." },
                new CloudServiceDto { Id = 2, Name = "Salesforce CRM", Description = "Customer Relationship Management software for businesses." },
                new CloudServiceDto { Id = 3, Name = "Amazon Web Services", Description = "Comprehensive cloud services platform offering compute power, database storage, and various other functionalities." },
                new CloudServiceDto { Id = 4, Name = "Google Workspace", Description = "Integrated suite of secure, cloud-native collaboration and productivity apps powered by Google AI." },
                new CloudServiceDto { Id = 5, Name = "Adobe Creative Cloud", Description = "Collection of 20+ desktop and mobile apps and services for photography, design, video, web, UX, and more." },
                new CloudServiceDto { Id = 6, Name = "Slack", Description = "Business communication platform offering many IRC-style features, including persistent chat rooms (channels) organized by topic." },
                new CloudServiceDto { Id = 7, Name = "Zoom Video Conferencing", Description = "Remote conferencing services using cloud computing, offering telecommunications software that combines video conferencing, online meetings, chat, and mobile collaboration." }
            };

            return services;
        }
    }
}
