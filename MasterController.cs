using GFAS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GFAS.Controllers
{
    public class MasterController : Controller

    {
        private readonly GEOFENCEDBContext context;

        public MasterController(GEOFENCEDBContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> LocationMaster(Guid? id, AppLocationMaster locationMaster, int page = 1, string searchString = "", string wsite = "")
        {
           
                var UserId = HttpContext.Request.Cookies["Session"];

                if (!string.IsNullOrEmpty(UserId))
                {
                    if (UserId != "842015" && UserId != "151514")
                    {
                        return RedirectToAction("AccessDenied", "Innovation");
                    }


                    ViewBag.Data = UserId;



                    int pageSize = 5;
                    var query = context.AppLocationMasters.AsQueryable();


                    if (!string.IsNullOrEmpty(wsite))
                    {
                        query = query.Where(p => p.WorkSite.Contains(wsite));
                    }

                    var data = query.ToList();




                var sortedData = data.OrderBy(p =>
                     int.Parse(p.LCode.Substring(6))
                 ).ToList();


                var pagedData = sortedData.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    var totalCount = sortedData.Count();

                    ViewBag.SearchLcode = wsite;

                    ViewBag.pList = pagedData;
                    ViewBag.CurrentPage = page;
                    ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                    ViewBag.SearchString = searchString;

                    if (id.HasValue)
                    {
                        var model = await context.AppLocationMasters.FindAsync(id.Value);
                        if (model == null)
                        {
                            return NotFound();
                        }

                        return Json(new
                        {
                            id = model.Id,
                            lcode = model.LCode,
                            worksite = model.WorkSite,
                            longitude = model.Longitude,
                            latitude = model.Latitude,
                            range = model.Range,
                            createdby = UserId,
                            createdon = model.CreatedOn
                        });
                    }

                    return View(new AppLocationMaster());
                }
               
            
            else
            {
                return RedirectToAction("Login", "User");
            }
    
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LocationMaster(List<AppLocationMaster> appLocations, string actionType)
        {
            if (string.IsNullOrEmpty(actionType))
            {
                return BadRequest("No action specified.");
            }

            //if (!ModelState.IsValid)
            //{
            //    foreach (var state in ModelState)
            //    {
            //        foreach (var error in state.Value.Errors)
            //        {
            //            Console.WriteLine($"Key:{state.Key},Error:{error.ErrorMessage}");
            //        }
            //    }
            //}

            if (actionType == "Submit")
            {
                if (ModelState.IsValid)
                {

                    foreach (var appLocation in appLocations)
                    {
                        var existingParameter = await context.AppLocationMasters.FindAsync(appLocation.Id);
                        if (existingParameter != null)
                        {
                            context.Entry(existingParameter).CurrentValues.SetValues(appLocation);
                        }
                        else
                        {
                            await context.AppLocationMasters.AddAsync(appLocation);

                        }

                    }
                    await context.SaveChangesAsync();
                    TempData["msg"] = "Location Saved Successfully!";
                    return RedirectToAction("LocationMaster");
                }
            }

            else if (actionType == "Delete")
            {
                foreach (var appLocation in appLocations)
                {
                    var existingParameter = await context.AppLocationMasters.FindAsync(appLocation.Id);
                    if (existingParameter != null)
                    {
                        context.AppLocationMasters.Remove(existingParameter);
                    }
                    
                       
                }
                await context.SaveChangesAsync();
                TempData["Dltmsg"] = "Location Deleted Successfully!";
                return RedirectToAction("LocationMaster");
            }
            return View(appLocations);

        }

        public async Task<IActionResult> PositionMaster(Guid? id, AppPositionWorksite appPosition, int page = 1, string searchString = "", string position = "")
        {
            
                var UserId = HttpContext.Request.Cookies["Session"];

                if (!string.IsNullOrEmpty(UserId))
                {
                    //var user = HttpContext.Session.GetString("Session");

                    if (UserId != "842015" && UserId != "151514")
                    {
                        return RedirectToAction("Login", "User");
                    }


                    int pageSize = 5;
                    var query = context.AppPositionWorksites.AsQueryable();


                    if (!string.IsNullOrEmpty(position))
                    {
                        query = query.Where(p => p.Position.ToString().Contains(position));
                    }

                    var data = query.ToList();




                var sortedData = data.OrderBy(p =>
                   (p.Position)
                ).ToList();



                var pagedData = sortedData.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    var totalCount = sortedData.Count();

                    ViewBag.Searchposition = position;

                    ViewBag.pList = pagedData;
                    ViewBag.CurrentPage = page;
                    ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                    ViewBag.SearchString = searchString;

                    var WorksiteList = context.AppLocationMasters
                        .Select(x => new SelectListItem
                        {
                            Value = x.WorkSite,
                            Text = x.WorkSite
                        }).ToList();

                    ViewBag.WorksiteDDList = WorksiteList;

                    var WorksiteList2 = context.AppEmpPositions
                       .Select(x => new SelectListItem
                       {
                           Value = x.Position.ToString(),
                           Text = x.Position.ToString()
                       }).ToList();

                    ViewBag.PositionDDList = WorksiteList2;

                    if (id.HasValue)
                    {
                        var model = await context.AppPositionWorksites.FindAsync(id.Value);
                        if (model == null)
                        {
                            return NotFound();
                        }

                        return Json(new
                        {
                            id = model.Id,
                            position = model.Position,
                            worksite = model.Worksite,
                            createdby = UserId,
                            createdon = model.CreatedOn,
                        });
                    }




                    return View(new AppPositionWorksite());
                }
              
            
            else
            {
                return RedirectToAction("Login", "User");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PositionMaster(AppPositionWorksite appPosition, string actionType)
        {
            if (string.IsNullOrEmpty(actionType))
            {
                return BadRequest("No action specified.");
            }

            var existingParameter = await context.AppPositionWorksites.FindAsync(appPosition.Id);
           

            if (actionType == "Submit")
            {
                if (!ModelState.IsValid)
                {
                    foreach (var state in ModelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            Console.WriteLine($"Key:{state.Key},Error:{error.ErrorMessage}");
                        }
                    }
                }


                if (ModelState.IsValid)
                {


                    if (existingParameter != null)
                    {

                        context.Entry(existingParameter).CurrentValues.SetValues(appPosition);
                        await context.SaveChangesAsync();
                        TempData["Updatedmsg"] = "Position Updated Successfully!";
                        return RedirectToAction("PositionMaster");
                    }
                    else
                    {


                        await context.AppPositionWorksites.AddAsync(appPosition);
                        await context.SaveChangesAsync();
                        TempData["msg"] = "Position Added Successfully!";
                        return RedirectToAction("PositionMaster");
                    }
                }
            }
            else if (actionType == "Delete")
            {
                if (existingParameter != null)
                {
                    context.AppPositionWorksites.Remove(existingParameter);
                    await context.SaveChangesAsync();
                    TempData["Dltmsg"] = "Position Deleted Successfully!";
                }
            }

            return RedirectToAction("PositionMaster");
        }
    }
}
