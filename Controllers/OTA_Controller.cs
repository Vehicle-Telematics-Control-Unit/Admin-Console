using Admin_Console.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

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

        [HttpGet("getFeatures")]
        public IActionResult GetFeaturesData()
        {
            var appsInfo = (from _feature in _context.Features
                            select new JObject
                              {
                                  new JProperty("id", _feature.FeatureId),
                                  new JProperty("name", _feature.FeatureName)
                              }).ToList();

            return Ok(new JObject
            {
                new JProperty("features", appsInfo)
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

            App? app = (from _app in _context.Apps
                       where _app.AppId == releaseId
                       && _app.FeatureId == null
                       select _app).FirstOrDefault();

            if (app == null)
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
            app.FeatureId = newFeature.FeatureId;
            // link feature to TCUs
            var models = (from _model in _context.Models
                         where modelsSelected.Contains(_model.Id)
                          select _model.Id).ToList();
            foreach(long model in models)
            {
                _context.ModelsFeatures.Add(new ModelsFeature
                {
                    FeatureId = (long)app.FeatureId,
                    ModelId = model
                });
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet("getFeatures/{featureId}")]
        public IActionResult GetFeatureInfo(long featureId)
        {
            // get feature object
            Feature? feature = (from _feature in _context.Features
                               where _feature.FeatureId == featureId
                               select _feature).FirstOrDefault();

            if (feature == null)
                return NotFound();

            //get app info
            App app = (from _app in _context.Apps
                        where _app.AppId == feature.AppId
                        select _app).First();

            // get distribution models
            List<long> modelsId = (from _modelFeature in _context.ModelsFeatures
                                   where _modelFeature.FeatureId == feature.FeatureId
                                   select _modelFeature.ModelId).ToList();

            string[] env_VARIABLES = app.EnvVariables ?? new string[] { };
            string[] volumesBinding = app.Volumes ?? new string[] { };
            string[] portsBinding = app.ExposedPorts ?? new string[]{ };

            JObject result = new()
            {
                new JProperty("name", feature.FeatureName),
                new JProperty("release", new JObject
                {
                    new JProperty("id", app.AppId),
                    new JProperty("name", app.Repo + ":" + app.Tag)
                }),
                new JProperty("releaseDate", feature.ReleaseDate),
                new JProperty("description", feature.Description),
                new JProperty("distributionModels", modelsId),
                new JProperty("env_Variables", env_VARIABLES),
                new JProperty("volumesBinding", volumesBinding),
                new JProperty("portsBinding", portsBinding)
            };
            return Ok(result);
        }

        [HttpPut("modifyFeature")]
        public IActionResult ModifyFeature([FromBody] JObject featureInfo)
        {
            long? featureId = featureInfo["FeatureId"]?.Value<long>();
            // get feature to update
            Feature? feature = (from _feature in _context.Features
                               where _feature.FeatureId == featureId
                               select _feature).FirstOrDefault();
            if (feature == null)
                return NotFound();
            // get app related to feature
            App app = (from _app in _context.Apps
                       where _app.AppId == feature.AppId
                       select _app).First();

            long? linkedAppId = featureInfo["releaseId"]?.Value<long>();
            
            if(app.AppId != linkedAppId)
            {
                // the app has been changed
                // reset the linked application
                app.EnvVariables = null;
                app.ExposedPorts = null;
                app.Volumes = null;
                app.FeatureId = null;
                _context.SaveChanges();
                // link to new application
                app = (from _app in _context.Apps
                              where _app.AppId == linkedAppId
                              select _app).First();
                feature.AppId = app.AppId;
            }

            // configure new params for feature
            feature.FeatureName = featureInfo["appName"]?.Value<string>() ?? feature.FeatureName;
            feature.ReleaseDate = featureInfo["releaseTime"]?.Value<DateTime>() ?? feature.ReleaseDate;
            feature.Description = featureInfo["description"]?.Value<string>() ?? feature.Description;

            var newModels = featureInfo["modelId"]?.Values<long>() ?? new List<long>();

            List<ModelsFeature> modelFeatures = (from _ModelFeature in _context.ModelsFeatures
                                                 where _ModelFeature.FeatureId == feature.FeatureId
                                                 select _ModelFeature).ToList();

            List<ModelsFeature> modelsToDelete = (from _ModelFeature in modelFeatures
                                                  where (newModels.Contains(_ModelFeature.ModelId) == false)
                                                  select _ModelFeature).ToList();
            
            _context.ModelsFeatures.RemoveRange(modelsToDelete);

            List<long> modelFeaturesIds = (from _model in modelFeatures
                                           select _model.ModelId).ToList();

            var newModelsId = (from _model in newModels
                               where (modelFeaturesIds.Contains(_model) == false)
                               select _model).ToList();

            foreach(long newModelId in newModelsId)
            {
                _context.ModelsFeatures.Add(new ModelsFeature
                {
                    FeatureId = feature.FeatureId,
                    ModelId = newModelId
                });
            }

            // configure app paramters
            string env_str = featureInfo["env_vars"]?.Value<string>() ?? string.Empty; 
            string[] env_VARIABLES = env_str.Split("\n");
            string volumes_str = featureInfo["env_vars"]?.Value<string>() ?? string.Empty;
            string[] volumes_Binding = volumes_str.Split("\n");
            string ports_Bindings_str = featureInfo["ports"]?.Value<string>() ?? string.Empty;
            string[] ports_Bindings = ports_Bindings_str.Split("\n");
            app.EnvVariables = env_VARIABLES;
            app.Volumes = volumes_Binding;
            app.ExposedPorts = ports_Bindings;
            _context.SaveChanges();
            return Ok();
        }
    }
}
