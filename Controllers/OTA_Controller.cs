using Admin_Console.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Admin_Console.Controllers
{
    [Route("api/OTA")]
    [ApiController]
    public class OTA_Controller : ControllerBase
    {
        private readonly TCUContext _context;
        
        public OTA_Controller(TCUContext context)
        {
            _context = context;
        }

        [HttpGet("getModels")]
        public IActionResult GetModelsData()
        {
            var modelsInfo = (from _model in _context.Models
                              select new JObject
                              {
                                  new JProperty("id", _model.Id),
                                  new JProperty("name", _model.Name)
                              }).ToList();

            return Ok(new JObject
            {
                new JProperty("models", modelsInfo)
            });
        }

        [HttpGet("getApps")]
        public IActionResult GetAppsData()
        {
            var appsInfo = (from _app in _context.Apps
                            where _app.FeatureId == null
                              select new JObject
                              {
                                  new JProperty("id", _app.AppId),
                                  new JProperty("name", _app.Repo + ":" + _app.Tag)
                              }).ToList();

            return Ok(new JObject
            {
                new JProperty("apps", appsInfo)
            });
        }

        [HttpPost("publishFeature")]
        public IActionResult PublishFeature([FromBody] JObject featureInfo)
        {
            string appName;
            List<long> modelsSelected;
            long releaseId;
            DateTime releaseTime;
            string description;
            try
            {
                appName = featureInfo["appName"]?.Value<string>() ?? throw new ArgumentNullException();
                modelsSelected = featureInfo["modelId"]?.Values<long>().ToList() ?? throw new ArgumentNullException();
                releaseId = featureInfo["releaseId"]?.Value<long>() ?? throw new ArgumentNullException();
                releaseTime = featureInfo["releaseTime"]?.Value<DateTime>() ?? throw new ArgumentNullException();
                description = featureInfo["appName"]?.Value<string>() ?? throw new ArgumentNullException();
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }

            bool appFound = (from _app in _context.Apps
                       where _app.AppId == releaseId
                       && _app.FeatureId == null
                       select _app).Any();

            if (appFound == false)
                return NotFound();

            // create new feature
            Feature newFeature = new()
            {
                FeatureName = appName,
                AppId = releaseId,
                ReleaseDate = releaseTime,
                Description = description
            };

            _context.Features.Add(newFeature);
            _context.SaveChanges();

            // link feature to TCUs
            var models = (from _model in _context.Models
                         where modelsSelected.Contains(_model.Id)
                          select _model.Id).ToList();

#pragma warning disable CS8629 // Nullable value type may be null.
            var TcusToUpdate = (from _tcu in _context.Tcus
                                where models.Contains((long)_tcu.ModelId)
                                select _tcu).ToList();

            foreach(Tcu tcu in TcusToUpdate)
            {
                _context.TcuFeatures.Add(new TcuFeature
                {
                    TcuId = tcu.TcuId,
                    FeatureId = newFeature.FeatureId
                });

            }

            _context.SaveChanges();

#pragma warning restore CS8629 // Nullable value type may be null.

            return Ok();
        }
    }
}
