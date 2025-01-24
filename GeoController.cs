using Dapper;
using GFAS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace GFAS.Controllers
{
    public class GeoController : Controller
    {
        private readonly IConfiguration configuration;

        public GeoController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private string GetConnectionString()
        {
            return this.configuration.GetConnectionString("GeoDB");
        }

        [Authorize]
        public IActionResult GeoFencing()
        {
            var session = HttpContext.Request.Cookies["Session"];
            var UserName = HttpContext.Request.Cookies["UserName"];

            //if (!string.IsNullOrEmpty(session) && !string.IsNullOrEmpty(UserName))
            //{
                var data = GetLocations();
                return View();
            //}
            //else
            //{
            //    return RedirectToAction("Login", "User");
            //}
            
        }


        public IActionResult GetLocations()
        {
            HardCodeData();
                var UserId = HttpContext.Request.Cookies["Session"];
                string connectionString = GetConnectionString();
                string query = @"select ps.Worksite from GEOFENCEDB.DBO.App_Position_Worksite as ps 
                                inner join GEOFENCEDB.DBO.App_Emp_position as es on es.position = ps.position 
                                where es.Pno = '" + UserId + "'";

                using (var connection = new SqlConnection(connectionString))
                {
                    string locations = connection.QuerySingleOrDefault<string>(query);

                    string s = locations;
                    s = "'" + s;

                    s = s.Replace(",", "','");
                    s = s + "'";

                    string query2 = @"select Longitude,Latitude,Range from GEOFENCEDB.DBO.App_LocationMaster 
                                      where work_site in (" + s + ")";

                    var locations2 = connection.Query<WorkLocation>(query2).ToList();
                    List<string> locations2List = new List<string>();

                    foreach (var arr2 in locations2)
                    {
                        locations2List.Add(arr2.Latitude.ToString());
                        locations2List.Add(arr2.Longitude.ToString());
                        locations2List.Add(arr2.Range.ToString());

                    }

                    return View(locations2List);
                }
            
           
        }


        //public bool IspointInPolygon(List<Location> poly, Location point)
        //{
        //    int i, j;
        //    bool c = false;
        //    for (i = 0; j = poly.Count - 1; i < poly.Count; j = i++){
        //        if ((((poly[i].Latitude <= point.Latitude) && (point.Latitude < poly[j].Latitude)) 
        //            || ((poly[j].Latitude <= point.Latitude) &&( point.Latitude < poly[i].Latitude)))
        //            && (point.Longitude< (poly[j].Longitude - poly[i].Longitude) * (point.Latitude - poly[i].Latitude)
        //                / (poly[j].Latitude - poly[i].Latitude) + poly[i].Longitude))
        //        {
        //            c = !c;
        //        }
        //    }
        //    return c;
        //}


        public IActionResult HardCodeData()
        {

            //22.796434, 86.183922
            //    22.796862, 86.183684
            //    22.796869, 86.184239


            var polygons = new List<List<Dictionary<string, double>>>
            {
                new List<Dictionary<string, double>>
                {
                    //new Dictionary<string, double>{{"latitude", 22.798088 }, { "longitude", 86.184976 } },
                    //new Dictionary<string, double>{{"latitude", 22.796404 }, { "longitude", 86.184595 } },
                    //new Dictionary<string, double>{{"latitude", 22.796364 }, { "longitude", 86.183014 } },
                    //new Dictionary<string, double>{{"latitude", 22.797673 }, { "longitude", 86.182989 } },
                    //new Dictionary<string, double>{{"latitude", 22.797986 }, { "longitude", 86.183358 } }

                    new Dictionary<string, double>{{"latitude", 22.804431 }, { "longitude", 86.183302 } },
                    new Dictionary<string, double>{{"latitude", 22.804145 }, { "longitude", 86.183529 } },
                    new Dictionary<string, double>{{"latitude", 22.804407 }, { "longitude", 86.183932 } },
                    new Dictionary<string, double>{{"latitude", 22.804583 }, { "longitude", 86.183633 } }
                    

                }
            };
            ViewBag.PolyData = polygons;
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Session");
            return RedirectToAction("Login", "User");

        }




        [HttpPost]
        public IActionResult DataSaved()
        {
            return RedirectToAction("Login", "User");
        }

    }
}
